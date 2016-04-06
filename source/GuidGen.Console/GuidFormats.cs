using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	public class GuidFormats
	{
		private static List<IGuidFormatter> s_Formats = new List<IGuidFormatter>();
		private static Dictionary<string, IGuidFormatter> s_FormatsByKey = new Dictionary<string, IGuidFormatter>(StringComparer.InvariantCultureIgnoreCase);

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
			s_Formats.Add(new GuidFormat() { Key="HLDAP", Description="LDAP HEX byte array", OutputFormat=@"\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\{7}\\{8}\\{9}\\{10}\\{11}\\{12}\\{13}\\{14}\\{15}"});
			s_Formats.Add(new GuidFormat() { Key="ORACLE", Description="ORACLE raw format", OutputFormat=@"{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}" });
			s_Formats.Add(new GuidFormat() { Key="ORACLE_HEXTORAW", Description="ORACLE raw format with HEXTORAW declaration", OutputFormat=@"HEXTORAW\s(\s'{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}'\s)" });
			s_Formats.Add(new Base64GuidFormat(false) { Key="BASE64", Description="Base64 from bytes" });
			s_Formats.Add(new Base64GuidFormat(true) { Key="BASE64C", Description="Combine bytes to single base64 string" });

			foreach(var format in s_Formats)
			{
				s_FormatsByKey.Add(format.Key, format);
			}
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
