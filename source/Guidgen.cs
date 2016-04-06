using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GuidGen
{
	class Guidgen
	{

		[STAThread]
		static void Main(string[] args)
		{
			try
			{
				// This is really for testing, but pause before startup
				int timeout = 0;
				if (Cmdline.Has("sleep") && (timeout = Tools.Convert(Cmdline.Get("sleep").Value, 1000)) > 0) {  System.Threading.Thread.Sleep(timeout); }

				// Write out help documentation and end
				if (Cmdline.Has("?") || Cmdline.Has("help")) { WriteHelp(); return; }

				// get whether to uppercase output
				bool upcase = false;
				if (Cmdline.Has("u")) upcase=true;

				// get the number of guids to output
				int count = -1;
				if (Cmdline.Has("n")) count = Tools.Convert(Cmdline.Get("n").Value, 1);
				else if (Cmdline.Has("count")) count = Tools.Convert(Cmdline.Get("count").Value, 1);

				// get the format of guids to output
				string format = null;
				if (!Cmdline.Get(0).IsSwitch) format = Cmdline.Get(0).Value;

				// get the type of guids to output
				string type = null;
				if (Cmdline.Has("g")) type = "g";
				else if (Cmdline.Has("s")) type = "s";
				else if (Cmdline.Has("z")) type = "z";

				// process the requested action
				if (Cmdline.Has("find")) FindGuids(format, upcase);
				else if (Cmdline.Has("replace")) ReplaceGuids(false, type, format, count, upcase);
				else if (Cmdline.Has("replacebyline")) ReplaceGuids(true, type, format, count, upcase);
				else WriteGuids(type, format, count, upcase);

			}
			catch (Exception ex)
			{
				WriteHelp("OOPS! - an error occured: " + ex.Message);
			}
		}

		/// <summary>
		/// Writes guids out to console window and adds them to the clipboard
		/// </summary>
		/// <param name="type">The output format</param>
		/// <param name="count">The number of guids to output</param>
		/// <param name="upcase">Whether to uppercase the output values</param>
		private static void WriteGuids(string type, string format, int count, bool upcase)
		{
			if (type == null) type = System.Configuration.ConfigurationManager.AppSettings["default:guid:type"]??"g";
			if (format == null) format = System.Configuration.ConfigurationManager.AppSettings["default:output:format"]??"D";

			// validate the given type of guids to output
			if (!GuidFormats.IsValid(format)) { WriteHelp("Format Not Found: " + type); return; }
				
			Guider guider = Guider.FromType(type, Guider.NewGuid);
			guider.Count = Math.Max(1, count);
			string output = GuidFormats.Format(format, guider, upcase, count>1);
			if (!Cmdline.Has("nocopy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, output);
			System.Diagnostics.Debug.WriteLine(output);
			Console.WriteLine(output);
		}


		/// <summary>
		/// For finding and replacing, this gets data from the requested input stream
		/// </summary>
		/// <returns>Input stream</returns>
		private static System.IO.TextReader GetInputStream()
		{
			System.IO.TextReader retVal;
			if (Cmdline.Has("guid")) // user given guid on commandline
			{
				retVal = new System.IO.StringReader(string.Join("\r\n", Cmdline.Get("guid").Values));
			}
			else if (Cmdline.Has("clipboard")) // gets guids from clipboard
			{
				retVal = new System.IO.StringReader(System.Windows.Forms.Clipboard.GetText());
			}
			else if (ConsoleEx.InputRedirected && Console.In.Peek() != -1) // gets data via pipe syntax
			{
				retVal = Console.In;
			}
			else // reformatter - waits for input from user typing/pasting
			{
				Console.Write("Type \"quit\" to quit");
				retVal = new ConsoleExitStream();
			}
			return retVal;
		}

		/// <summary>
		/// Find Guids from input type
		/// </summary>
		/// <param name="format">The output format</param>
		/// <param name="upcase">whether to uppercase the output format</param>
		private static void FindGuids(string format, bool upcase)
		{
			List<Found> items = new List<Found>();
			GuidSearcher.Search(Cmdline.Get("find").Value, GetInputStream(), items);
			List<Guid> guids = new List<Guid>();
			foreach(Found item in items)
			{
				guids.AddRange(item.Guids);
				Console.WriteLine(((Cmdline.Has("l"))?((item.Line+1).ToString()+". "):"") + item.Match);
				if (format != null)
				{
					foreach (Guid g in item.Guids)
					{
						string formatted = GuidFormats.Format(format, g, upcase, true);
						if (formatted != item.Match) Console.WriteLine("\t" + formatted);
					}
				}
			}
			if (Cmdline.Has("copy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, GuidFormats.Format(format, guids, upcase, true));
		}

		/// <summary>
		/// Replace Guids
		/// </summary>
		/// <param name="byLine">replace per line</param>
		/// <param name="type">Type of guid to replace with</param>
		/// <param name="count">The number of guids to replace</param>
		/// <param name="upcase">Whether to upper case the replacement</param>
		private static void ReplaceGuids(bool byLine, string type, string format, int count, bool upcase)
		{
			// validate the given type of guids to output
			if (type != null && !GuidFormats.IsValid(type)) { WriteHelp("Format Not Found: " + type); return; }

			Guider guider = Guider.FromType(type, Guider.AsCurrent());
			guider.Count = count;
			Dictionary<Guid, Guid> replacements = new Dictionary<Guid,Guid>();
			if (byLine)
			{
				// This method is used as a 
				Console.WriteLine(".");
				DefaultSearch.Replace(GetInputStream(), Console.Out, guider, replacements, format, upcase, Cmdline.Has("copy"));
			}
			else
			{
				System.Text.StringBuilder output = new System.Text.StringBuilder();
				using (System.IO.TextWriter tw = new System.IO.StringWriter(output))
				{
					DefaultSearch.Replace(GetInputStream(), tw, guider, replacements, format, upcase);
				}

				Console.WriteLine(output.ToString());
				if (Cmdline.Has("copy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text,output.ToString());
			}
		}


		private static void WriteHelp(string error="")
		{
			string type = (System.Configuration.ConfigurationManager.AppSettings["default:guid:type"]??"g").ToUpper();
			string format = (System.Configuration.ConfigurationManager.AppSettings["default:output:format"]??"D").ToUpper();

			string output = string.IsNullOrEmpty(error)?"\r\n":(error+"\r\n");
			output += "usage: GuidGen.exe [N|D|P|B|C|CP|H|HC#|HVB|HLDAP|BASE64|BASE64C] [/G|/S|/Z] [/nocopy] [/n (number)] [/u]\r\n";
			output += " Output Formats:\r\n";
			output += "  N: 32 digits " + (format=="N"?"(DEFAULT)":"") + "\r\n";
			output += "   87654321dcbafe1054326789abcdef01\r\n";
			output += "  D: 32 digits separated by hyphens " + (format=="D"?"(DEFAULT)":"") + "\r\n";
			output += "   00000000-0000-0000-0000-000000000000\r\n";
			output += "  P: 32 digits separated by hyphens, enclosed in parentheses " + (format=="P"?"(DEFAULT)":"") + "\r\n";
			output += "   {00000000-0000-0000-0000-000000000000}\r\n";
			output += "  B: 32 digits separated by hyphens, enclosed in brackets " + (format=="B"?"(DEFAULT)":"") + "\r\n";
			output += "   [00000000-0000-0000-0000-000000000000]\r\n";
			output += "  C: c format " + (format=="C"?"(DEFAULT)":"") + "\r\n";
			output += "   0x00000000,0x0000,0x0000,0x0000,0x00,0x00,0x00,0x00,0x00,0x00\r\n";
			output += "  CP: c format, enclosed in parentheses " + (format=="CP"?"(DEFAULT)":"") + "\r\n";
			output += "   {0x00000000,0x0000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00}}\r\n";
			output += "  GUID: c format, enclosed in parentheses " + (format=="GUID"?"(DEFAULT)":"") + "\r\n";
			output += "   static const GUID <<name>> = [CP FORMAT];\r\n";
			output += "  DEFINE_GUID: c format, enclosed in parentheses " + (format=="DEFINE_GUID"?"(DEFAULT)":"") + "\r\n";
			output += "   DEFINE_GUID(<<name>>, [C FORMAT])\r\n";
			output += "  OLECREATE: c format, enclosed in parentheses " + (format=="OLECREATE"?"(DEFAULT)":"") + "\r\n";
			output += "   IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, [C FORMAT])\r\n";
			output += "  H: HEX byte array " + (format=="H"?"(DEFAULT)":"") + "\r\n";
			output += "   0123456789abcdef0123456789abcdef\r\n";
			output += "  HC#: CSharp Hex byte array " + (format=="HC#"?"(DEFAULT)":"") + "\r\n";
			output += "   0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef,0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef\r\n";
			output += "  HVB: VB Hex byte array " + (format=="HVB"?"(DEFAULT)":"") + "\r\n";
			output += "   &H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef,&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef\r\n";
			output += "  HLDAP: Hex byte array in ldap query form " + (format=="HLDAP"?"(DEFAULT)":"") + "\r\n";
			output += "   \\\\01\\\\23\\\\45\\\\67\\\\89\\\\ab\\\\cd\\\\ef\\\\01\\\\23\\\\45\\\\67\\\\89\\\\ab\\\\cd\\\\ef\r\n";
			output += "  ORACLE: ORACLE Hex byte array " + (format=="ORACLE"?"(DEFAULT)":"") + "\r\n";
			output += "   8967452301cdab01ef23456789abcdef\r\n";
			output += "  ORACLE_HEXTORAW: ORACLE Hex byte array with declaration " + (format=="ORACLE_HEXTORAW"?"(DEFAULT)":"") + "\r\n";
			output += "   HEXTORAW('8967452301cdab01ef23456789abcdef')\r\n";
			output += "  BASE64:  " + (format=="BASE64"?"(DEFAULT)":"") + "\r\n";
			output += "   AAAAAAAAAAAAAAAAAAAAAA==\r\n";
			output += "  BASE64C: combine bytes to single base64 string " + (format=="BASE64C"?"(DEFAULT)":"") + "\r\n";
			output += "   AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=\r\n";
			output += " Type of GUID to create\r\n";
			output += "  G: New Guid " + (type=="G"?"(DEFAULT)":"") + "\r\n";
			output += "  Z: Zero Guid" + (type=="Z"?"(DEFAULT)":"") + "\r\n";
			output += "  S: Sequential Guid" + (type=="S"?"(DEFAULT)":"") + "\r\n";
			output += "\r\n";
			output += " Additional Arguments\r\n";
			output += "  /u: returns format in uppercase (unless base64)\r\n";
			output += "  /count (number): will generate the given number of guids\r\n";
			output += "  /n (number): same as /count\r\n";
			output += "  /Find (format): Finds guids in format (no copy)\r\n";
			output += "  /l: shows line number for found guids \r\n";
			output += "  /copy: forces copy to clipboard \r\n";
			output += "  /nocopy: does not copy to clipboard \r\n";
			output += "  /Replace: replaces guid with (/Z|/S|/G) or same guid to specified output format (nocopy) (no-BASE64C)\r\n";
			output += "  /Replace [format]: replaces specified format with same guid or new guid if (/Z|/S|/G) is specified to specified output format (nocopy)\r\n";
			output += "  /ReplaceByLine: like replace, but does everything per input line. (see above)\r\n";
			output += "  /ReplaceByLine [format]: like replace, but does everything per input line. (see above)\r\n";
			output += "  /guid (GUID): uses specified (GUID) as input for find and replace.\r\n";
			output += "  /clipboard: uses clipboard for find and replace\r\n";
			output += " Notes:\r\n";
			output += "  if find or replace is used and data is not piped in (ex: more find.txt | guidgen /find) then enter guids and then type \"quit\" to find/replace and end.";
			Console.Write(output);		
		}
	}
}
