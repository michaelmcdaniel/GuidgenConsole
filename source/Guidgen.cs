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
			if (Cmdline.Has("sleep"))
			{
				int timeout = Tools.Convert(Cmdline.Get("sleep").Value, 1000);
				if (timeout>0) System.Threading.Thread.Sleep(timeout);
			}
			if (Cmdline.Has("?") || Cmdline.Has("help"))
			{
				WriteHelp();
				return;
			}

			string type = null;
			if (!Cmdline.Get(0).IsSwitch)
			{
				type = Cmdline.Get(0).Value;
				if (!GuidFormats.IsValid(type))
				{
					WriteHelp("Format Not Found: " + type);
				}
			}

			bool upcase = false;
			if (Cmdline.Has("u")) upcase=true;

			int count = -1;
			if (Cmdline.Has("n")) count = Tools.Convert(Cmdline.Get("n").Value, 1);
			else if (Cmdline.Has("count")) count = Tools.Convert(Cmdline.Get("count").Value, 1);

			Guider guider = null;
			if (Cmdline.Has("z")) guider = Guider.NewZeroGuid;
			else if (Cmdline.Has("s")) guider = Guider.NewSequentialGuid;
			else if (Cmdline.Has("g")) guider = Guider.NewGuid;

			bool isFindOrReplace = Cmdline.Has("find") || Cmdline.Has("replace") || Cmdline.Has("replacebyline");

			if (!isFindOrReplace)
			{
				if (type==null) type="D";
				if (guider == null) guider = Guider.NewGuid;
				if (count < 0) count = 1;
				guider.Count = count;
				string output = GuidFormats.Format(type, guider, upcase, count>1);
				if (!Cmdline.Has("nocopy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, output);
				Console.WriteLine(output);
			}
			else if (isFindOrReplace)
			{
				if (guider == null) guider = Guider.AsCurrent();
				guider.Count = count;
				System.IO.TextReader from;
				if (Cmdline.Has("guid"))
				{
					from = new System.IO.StringReader(string.Join("\r\n", Cmdline.Get("guid").Values));
				}
				else if (Cmdline.Has("clipboard"))
				{
					from = new System.IO.StringReader(System.Windows.Forms.Clipboard.GetText());
				}
				else if (ConsoleEx.InputRedirected && Console.In.Peek() != -1)
				{
					from = Console.In;
				}
				else
				{
					Console.Write("Type \"quit\" to quit");
					from = new ConsoleExitStream();
				}
				if (Cmdline.Has("find"))
				{
					List<Found> items = new List<Found>();
					GuidSearcher.Search(Cmdline.Get("find").Value, from, items);
					List<Guid> guids = new List<Guid>();
					foreach(Found item in items)
					{
						guids.AddRange(item.Guids);
						Console.WriteLine(((Cmdline.Has("l"))?((item.Line+1).ToString()+". "):"") + item.Match);
						if (type != null)
						{
							foreach (Guid g in item.Guids)
							{
								string formatted = GuidFormats.Format(type, g, upcase, true);
								if (formatted != item.Match) Console.WriteLine("\t" + formatted);
							}
						}
					}
					if (Cmdline.Has("copy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, GuidFormats.Format(type, guids, upcase, true));
				}
				else // Cmdline.Has("replace")
				{
					Dictionary<Guid, Guid> replacements = new Dictionary<Guid,Guid>();
					if (Cmdline.Has("replacebyline"))
					{
						Console.WriteLine(".");
						DefaultSearch.Replace(from, Console.Out, guider, replacements, type, upcase, Cmdline.Has("copy"));
					}
					else
					{
						System.Text.StringBuilder output = new System.Text.StringBuilder();
						using (System.IO.TextWriter tw = new System.IO.StringWriter(output))
						{
							DefaultSearch.Replace(from, tw, guider, replacements, type, upcase);
						}

						Console.WriteLine(output.ToString());
						if (Cmdline.Has("copy")) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text,output.ToString());
					}
				}
			}

			}
			catch (Exception ex)
			{
				WriteHelp("OOPS! - an error occured: " + ex.Message);
			}
		}

		private static void WriteHelp(string error="")
		{

			string output = string.IsNullOrEmpty(error)?"\r\n":(error+"\r\n");
			output += "usage: GuidGen.exe [N|D|P|B|C|CP|H|HC#|HVB|HLDAP|BASE64|BASE64C] [/G|/S|/Z] [/nocopy] [/n (number)] [/u]\r\n";
			output += " Output Formats:\r\n";
			output += "  N: 32 digits \r\n";
			output += "   87654321dcbafe1054326789abcdef01\r\n";
			output += "  D: 32 digits separated by hyphens (DEFAULT)\r\n";
			output += "   00000000-0000-0000-0000-000000000000\r\n";
			output += "  P: 32 digits separated by hyphens, enclosed in parentheses\r\n";
			output += "   {00000000-0000-0000-0000-000000000000}\r\n";
			output += "  B: 32 digits separated by hyphens, enclosed in brackets\r\n";
			output += "   [00000000-0000-0000-0000-000000000000]\r\n";
			output += "  C: c format\r\n";
			output += "   0x00000000,0x0000,0x0000,0x0000,0x00,0x00,0x00,0x00,0x00,0x00\r\n";
			output += "  CP: c format, enclosed in parentheses\r\n";
			output += "   {0x00000000,0x0000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00}}\r\n";
			output += "  GUID: c format, enclosed in parentheses\r\n";
			output += "   static const GUID <<name>> = [CP FORMAT];\r\n";
			output += "  DEFINE_GUID: c format, enclosed in parentheses\r\n";
			output += "   DEFINE_GUID(<<name>>, [C FORMAT])\r\n";
			output += "  OLECREATE: c format, enclosed in parentheses\r\n";
			output += "   IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, [C FORMAT])\r\n";
			output += "  H: HEX byte array\r\n";
			output += "   0123456789abcdef0123456789abcdef\r\n";
			output += "  HC#: CSharp Hex byte array\r\n";
			output += "   0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef,0x01,0x23,0x45,0x67,0x89,0xab,0xcd,0xef\r\n";
			output += "  HVB: VB Hex byte array\r\n";
			output += "   &H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef,&H01,&H23,&H45,&H67,&H89,&Hab,&Hcd,&Hef\r\n";
			output += "  HLDAP: Hex byte array in ldap query form\r\n";
			output += "   \\\\01\\\\23\\\\45\\\\67\\\\89\\\\ab\\\\cd\\\\ef\\\\01\\\\23\\\\45\\\\67\\\\89\\\\ab\\\\cd\\\\ef\r\n";
			output += "  BASE64: \r\n";
			output += "   AAAAAAAAAAAAAAAAAAAAAA==\r\n";
			output += "  BASE64C: combine bytes to single base64 string\r\n";
			output += "   AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA=\r\n";
			output += " Type of GUID to create\r\n";
			output += "  G: New Guid (default)\r\n";
			output += "  Z: Zero Guid\r\n";
			output += "  S: Sequential Guid\r\n";
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
