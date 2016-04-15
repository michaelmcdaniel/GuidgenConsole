using System;
using System.Collections.Generic;
using System.Text;
using GuidGen.Formats;

namespace GuidGen
{
	public class GuidFormats
	{
		private static List<IGuidFormatter> s_Formats = new List<IGuidFormatter>();
		private static Dictionary<string, IGuidFormatter> s_FormatsByKey = new Dictionary<string, IGuidFormatter>(StringComparer.InvariantCultureIgnoreCase);
		private static Dictionary<string, IGuidSearcher> s_SearchesByKey = new Dictionary<string, IGuidSearcher>(StringComparer.InvariantCultureIgnoreCase);

		static GuidFormats()
		{
			s_Formats.Add(new GuidFormat() { Key ="N", Description ="32 digits", OutputFormat ="{3}{2}{1}{0}{5}{4}{7}{6}{8}{9}{10}{11}{12}{13}{14}{15}"});
			s_Formats.Add(new GuidFormat() { Key="D", Description="32 digits separated by hyphens",OutputFormat ="{3}{2}{1}{0}-{5}{4}-{7}{6}-{8}{9}-{10}{11}{12}{13}{14}{15}"});
			s_Formats.Add(new GuidFormat() { Key="P", Description="32 digits separated by hyphens, enclosed in (curly) braces", OutputFormat="{{{3}{2}{1}{0}-{5}{4}-{7}{6}-{8}{9}-{10}{11}{12}{13}{14}{15}}}" });
			s_Formats.Add(new GuidFormat() { Key="B", Description="32 digits separated by hyphens, enclosed in brackets", OutputFormat="[{3}{2}{1}{0}-{5}{4}-{7}{6}-{8}{9}-{10}{11}{12}{13}{14}{15}]" });
			s_Formats.Add(new GuidFormat() { Key="C", Description="c format", OutputFormat=@"0x{3}{2}{1}{0},\s0x{5}{4},\s0x{7}{6},\s0x{8}{9},\s0x{10},\s0x{11},\s0x{12},\s0x{13},\s0x{14},\s0x{15}" });
			s_Formats.Add(new GuidFormat() { Key="CP", Description="c format, enclosed in (curly) braces", OutputFormat=@"{{\s0x{3}{2}{1}{0},\s0x{5}{4},\s0x{7}{6},\s0x{8}{9},\s{{\s0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}\s}}\s}}" });
			s_Formats.Add(new GuidFormat() { Key="GUID", Description="c format with const declaration", OutputFormat=@"static const GUID <<name>> = 0x{3}{2}{1}{0},\s0x{5}{4},\s0x{7}{6},\s0x{8}{9},\s0x{10},\s0x{11},\s0x{12},\s0x{13},\s0x{14},\s0x{15}\s;" });
			s_Formats.Add(new GuidFormat() { Key="OLECREATE", Description="c format with IMPLEMENT_OLECREATE declaration", OutputFormat=@"IMPLEMENT_OLECREATE\s(\s<<class>>,\s<<external_name>>,\s0x{3}{2}{1}{0},\s0x{5}{4},\s0x{7}{6},\s0x{8}{9},\s0x{10},\s0x{11},\s0x{12},\s0x{13},\s0x{14},\s0x{15}\s)" });
			s_Formats.Add(new GuidFormat() { Key="DEFINE_GUID", Description="c format with DEFINE_GUID declaration", OutputFormat=@"DEFINE_GUID\s(\s<<name>>,\s0x{3}{2}{1}{0},\s0x{5}{4},\s0x{7}{6},\s0x{8}{9},\s0x{10},\s0x{11},\s0x{12},\s0x{13},\s0x{14},\s0x{15}\s)" });
			s_Formats.Add(new GuidFormat() { Key="H", Description="HEX byte array", OutputFormat="{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}" });
			s_Formats.Add(new GuidFormat() { Key="HC#", Description="CSharp HEX byte array", OutputFormat=@"0x{0},\s0x{1},\s0x{2},\s0x{3},\s0x{4},\s0x{5},\s0x{6},\s0x{7},\s0x{8},\s0x{9},\s0x{10},\s0x{11},\s0x{12},\s0x{13},\s0x{14},\s0x{15}" });
			s_Formats.Add(new GuidFormat() { Key="HVB", Description="VB HEX byte array", OutputFormat=@"&H{0},\s&H{1},\s&H{2},\s&H{3},\s&H{4},\s&H{5},\s&H{6},\s&H{7},\s&H{8},\s&H{9},\s&H{10},\s&H{11},\s&H{12},\s&H{13},\s&H{14},\s&H{15}" });
			s_Formats.Add(new GuidFormat() { Key="HLDAP", Description="LDAP HEX byte array", OutputFormat=@"\\{0}\s\\{1}\s\\{2}\s\\{3}\s\\{4}\s\\{5}\s\\{6}\s\\{7}\s\\{8}\s\\{9}\s\\{10}\s\\{11}\s\\{12}\s\\{13}\s\\{14}\s\\{15}"});
			s_Formats.Add(new GuidFormat() { Key="ORACLE", Description="ORACLE raw format", OutputFormat=@"{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}" });
			s_Formats.Add(new GuidFormat() { Key="ORACLE_HEXTORAW", Description="ORACLE raw format with HEXTORAW declaration", OutputFormat=@"HEXTORAW\s(\s'{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}'\s)" });
			s_Formats.Add(new IPAddressGuidFormat() { Key="IP", Description="IP Address format (IPv4/IPv6)" });
			s_Formats.Add(new VersionGuidFormat() { Key="Version", Description="Version format (Major.Minor.Build.Revision)" });
			s_Formats.Add(new Int32OutputFormat() { Key="Int32", Description="Int32 format (Int32, Int32, Int32, Int32)" });
			s_Formats.Add(new Int64OutputFormat() { Key="Int64", Description="Int64format (Int64, Int64)" });
			s_Formats.Add(new Base64GuidFormat() { Key="BASE64", Description="Base64 from bytes" });
			s_Formats.Add(new Base64CombinedFormat() { Key="BASE64C", Description="Combine bytes to single base64 string" });

			foreach(var format in s_Formats)
			{
				s_FormatsByKey.Add(format.Key, format);
				if (format is IGuidSearcher) { s_SearchesByKey.Add(format.Key, (IGuidSearcher)format); }
			}
			s_SearchesByKey.Add("", new SearchFormat());
		}

		public static IGuidFormatter GetFormatter(string format)
		{
			IGuidFormatter retVal;
			s_FormatsByKey.TryGetValue(format, out retVal);
			return retVal;
		}

		public static string Format(string format, IEnumerator<Guid> guider, bool upcase, bool newline)
		{
			IGuidFormatter retVal = null;
			if (!s_FormatsByKey.TryGetValue(format, out retVal)) throw new ArgumentOutOfRangeException();
			return retVal.ToString(guider, upcase, newline);
		}

		public static string Format(string type, IEnumerable<Guid> guider, bool upcase, bool newline)
		{
			IGuidFormatter retVal = null;
			if (!s_FormatsByKey.TryGetValue(type, out retVal)) throw new ArgumentOutOfRangeException();
			return retVal.ToString(guider, upcase, newline);
		}

		public static string Format(string type, Guid guid, bool upcase, bool newline)
		{
			return Format(type, new Guid[] { guid }, upcase, newline);
		}

		public static IEnumerable<Found> Find(System.IO.TextReader reader, string type)
		{
			IGuidSearcher searcher;
			if (!s_SearchesByKey.TryGetValue(type, out searcher)) throw new ArgumentOutOfRangeException();
			int lineCount = 0;
			string line;
			while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
			{
				foreach(Found found in searcher.Find(line, lineCount++)) yield return found;
			}
		}

		public static IEnumerable<Replacement> Replace(System.IO.TextReader reader, System.IO.TextWriter writer, string inputFormat, string outputFormat, Guider guider, bool upcase, bool writeAllInput)
		{
			IGuidSearcher searcher;
			IGuidFormatter formatter = null;
			if (!s_SearchesByKey.TryGetValue(inputFormat??"", out searcher)) throw new ArgumentOutOfRangeException("Unrecognized search Type: " + outputFormat);
			if (!string.IsNullOrEmpty(outputFormat) && !s_FormatsByKey.TryGetValue(outputFormat, out formatter)) throw new ArgumentOutOfRangeException("Unrecognized output format: " + outputFormat);

			while (reader.Peek() != -1)
			{
				string line;		
				StringBuilder text = new StringBuilder();
				while(reader.Peek() != -1 && (line = reader.ReadLine()) != null) text.AppendLine(line);

				List<Replacement> replacements = new List<Replacement>();
				string output;
				try
				{
					output = searcher.Replace(text.ToString(), guider, formatter, upcase, (r)=> { replacements.Add(r); });
					if (writeAllInput || replacements.Count > 0)
					{
						writer.WriteLine(output);
					}
				}
				catch(ArgumentOutOfRangeException orex)
				{
					Console.WriteLine("Error: " + orex.Message);
				}

				for(int i = 0; i < replacements.Count; i++) yield return replacements[i];
			}
		}

		public static IEnumerable<Replacement> ReplaceByLine(System.IO.TextReader reader, System.IO.TextWriter writer, string inputFormat, string outputFormat, Guider guider, bool upcase, bool writeAllInput)
		{
			IGuidSearcher searcher;
			IGuidFormatter formatter = null;
			if (!s_SearchesByKey.TryGetValue(inputFormat??"", out searcher)) throw new ArgumentOutOfRangeException("Unrecognized search Type: " + outputFormat);
			if (!string.IsNullOrEmpty(outputFormat) && !s_FormatsByKey.TryGetValue(outputFormat, out formatter)) throw new ArgumentOutOfRangeException("Unrecognized output format: " + outputFormat);

			int lineCount = 0;
			string line;
			writer.Write("> ");
			while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
			{
				List<Replacement> replacements = new List<Replacement>();
				string output;
				try
				{
					output = searcher.Replace(line, guider, formatter, upcase, (r)=> { replacements.Add(r); }, lineCount);

					if (writeAllInput || replacements.Count > 0)
					{
						writer.WriteLine(output);
					}
				}
				catch(ArgumentOutOfRangeException orex)
				{
					Console.WriteLine("Error: " + orex.Message);
				}

				for(int i = 0; i < replacements.Count; i++) yield return replacements[i];
				lineCount++;
				writer.Write("> ");
			}
		}

		public static bool IsValid(string type)
		{
			return s_FormatsByKey.ContainsKey(type);
		}

		public static IEnumerable<IGuidFormatter> AvailableFormats
		{
			get { return s_Formats; }
		}
	}
}
