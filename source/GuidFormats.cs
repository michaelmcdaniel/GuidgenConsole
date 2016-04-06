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

		private static readonly byte[] c_StringByteOrder = new byte[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };
		private static readonly byte[] c_HexByteOrder = new byte[]    { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

		static GuidFormats()
		{
			s_Formats.Add(new GuidFormat() { Key="N", Description="32 digits", ByteOrder=c_StringByteOrder, OutputFormat="{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", Match="^[a-fA-F0-9]{32}$" });
			s_Formats.Add(new GuidFormat() { Key="D", Description="32 digits separated by hyphens", ByteOrder=c_StringByteOrder, OutputFormat="{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}", Match=@"^[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}$" });
			s_Formats.Add(new GuidFormat() { Key="P", Description="32 digits separated by hyphens, enclosed in (curly) braces", ByteOrder=c_StringByteOrder, OutputFormat="{{{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}}}", Match=@"^\{[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\}$" });
			s_Formats.Add(new GuidFormat() { Key="B", Description="32 digits separated by hyphens, enclosed in brackets", ByteOrder=c_StringByteOrder, OutputFormat="[{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}]", Match=@"^\[[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}\]$" });
			s_Formats.Add(new GuidFormat() { Key="C", Description="c format", ByteOrder=c_StringByteOrder, OutputFormat="0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}", Match=@"^0x[a-fA-F0-9]{8}(\s*,\s*0x[a-fA-F0-9]{4}){3}(\s*,\s*0x[a-fA-F0-9]{2}){6}$" });
			s_Formats.Add(new GuidFormat() { Key="CP", Description="c format, enclosed in (curly) braces", ByteOrder=c_StringByteOrder, OutputFormat="{{0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},{{0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}}}", Match=@"^\{\s*0x[a-fA-F0-9]{8}(\s*,\s*0x[a-fA-F0-9]{4}){3}\s*,\s*\{\s*0x[a-fA-F0-9]{2}(\s*,\s*0x[a-fA-F0-9]{2}){5}\s*\}$" });
			s_Formats.Add(new GuidFormat() { Key="GUID", Description="c format with const declaration", ByteOrder=c_StringByteOrder, OutputFormat="static const GUID <<name>> = 0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15};", Match=@"^static\s+const\s+GUID\s+(\<\<)?[a-zA-Z_][a-zA-Z0-9_]*(\>\>)?\s*=\s*\{\s*0x[a-fA-F0-9]{8}(\s*,\s*0x[a-fA-F0-9]{4}){3}\s*,\s*\{\s*0x[a-fA-F0-9]{2}(\s*,\s*0x[a-fA-F0-9]{2}){5}\s*\};$" });
			s_Formats.Add(new GuidFormat() { Key="OLECREATE", Description="c format with IMPLEMENT_OLECREATE declaration", ByteOrder=c_StringByteOrder, OutputFormat="IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15})", Match=@"^IMPLEMENT_OLECREATE\s*\(\s*(\<\<)?[a-zA-Z_][a-zA-Z0-9_]*(\>\>)?\s*,\s*(\<\<)?[a-zA-Z_][a-zA-Z0-9_]*(\>\>)?\s*,\s*0x[a-fA-F0-9]{8}(\s*,\s*0x[a-fA-F0-9]{4}){3}\s*,\s*\{\s*0x[a-fA-F0-9]{2}(\s*,\s*0x[a-fA-F0-9]{2}){5}\s*\)$" });
			s_Formats.Add(new GuidFormat() { Key="DEFINE_GUID", Description="c format with DEFINE_GUID declaration", ByteOrder=c_StringByteOrder, OutputFormat="DEFINE_GUID(<<name>>, 0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15})", Match=@"^DEFINE_GUID\s*\(\s*(\<\<)?[a-zA-Z_][a-zA-Z0-9_]*(\>\>)?\s*,\s*0x[a-fA-F0-9]{8}(\s*,\s*0x[a-fA-F0-9]{4}){3}\s*,\s*\{\s*0x[a-fA-F0-9]{2}(\s*,\s*0x[a-fA-F0-9]{2}){5}\s*\)$" });
			s_Formats.Add(new GuidFormat() { Key="H", Description="HEX byte array", ByteOrder=c_HexByteOrder, OutputFormat="{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", Match="^[a-fA-F0-9]{32}$" });
			s_Formats.Add(new GuidFormat() { Key="HC#", Description="CSharp HEX byte array", ByteOrder=c_HexByteOrder, OutputFormat="0x{0},0x{1},0x{2},0x{3},0x{4},0x{5},0x{6},0x{7},0x{8},0x{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}", Match=@"^0x[a-fA-F0-9]{2}(\s*,\s*0x[a-fA-F0-9]{2}){15}$" });
			s_Formats.Add(new GuidFormat() { Key="HVB", Description="VB HEX byte array", ByteOrder=c_HexByteOrder, OutputFormat="&H{0},&H{1},&H{2},&H{3},&H{4},&H{5},&H{6},&H{7},&H{8},&H{9},&H{10},&H{11},&H{12},&H{13},&H{14},&H{15}", Match=@"^\&H[a-fA-F0-9]{2}(\s*,\s*\&H[a-fA-F0-9]{2}){15}$" });
			s_Formats.Add(new GuidFormat() { Key="HLDAP", Description="LDAP HEX byte array", ByteOrder=c_HexByteOrder, OutputFormat=@"\\{0}\\{1}\\{2}\\{3}\\{4}\\{5}\\{6}\\{7}\\{8}\\{9}\\{10}\\{11}\\{12}\\{13}\\{14}\\{15}", Match=@"^(\\\\[a-fA-F0-9]{2}){16}$" });
			s_Formats.Add(new GuidFormat() { Key="ORACLE", Description="ORACLE raw format", ByteOrder=c_StringByteOrder, OutputFormat=@"{3}{2}{1}{0}-{5}{4}-{7}{6}-{8}{9}-{10}{11}{12}{13}{14}{15}", Match=@"^[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}$" });
			s_Formats.Add(new GuidFormat() { Key="ORACLE_HEXTORAW", Description="ORACLE raw format with HEXTORAW declaration", ByteOrder=c_StringByteOrder, OutputFormat=@"HEXTORAW('{3}{2}{1}{0}-{5}{4}-{7}{6}-{8}{9}-{10}{11}{12}{13}{14}{15}')", Match=@"^HEXTORAW\(\s*'[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}'\s*\)$" });
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
