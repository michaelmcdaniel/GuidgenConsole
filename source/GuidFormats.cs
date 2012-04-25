using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	public class GuidFormats
	{
		private static Dictionary<string, Func<IEnumerator<Guid>, bool, bool, string>> s_Formatters = new Dictionary<string, Func<IEnumerator<Guid>, bool, bool, string>>(StringComparer.InvariantCultureIgnoreCase);
		private static Dictionary<string, Func<IEnumerator<Guid>, bool, bool, string>> s_Replacers = new Dictionary<string, Func<IEnumerator<Guid>, bool, bool, string>>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly byte[] c_StringByteOrder = new byte[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };
		private static readonly byte[] c_HexByteOrder = new byte[]    { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

		static GuidFormats()
		{
			s_Formatters["N"] = GetFormatter("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", c_StringByteOrder);
			s_Formatters["D"] = GetFormatter("{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}", c_StringByteOrder);
			s_Formatters["P"] = GetFormatter("{16}{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}{17}", c_StringByteOrder);
			s_Formatters["B"] = GetFormatter("[{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}]", c_StringByteOrder);
			s_Formatters["C"] = GetFormatter("0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}", c_StringByteOrder);
			s_Formatters["OLECREATE"] = GetFormatter("IMPLEMENT_OLECREATE(<<class>>, <<external_name>>, 0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15})", c_StringByteOrder);
			s_Formatters["DEFINE_GUID"] = GetFormatter("DEFINE_GUID(<<name>>, 0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15})", c_StringByteOrder);
			s_Formatters["CP"] = GetFormatter("{16}0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},{16}0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}{17}{17}", c_StringByteOrder);
			s_Formatters["GUID"] = GetFormatter("static const GUID <<name>> = {16}0x{0}{1}{2}{3},0x{4}{5},0x{6}{7},0x{8}{9},{16}0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}{17}{17};", c_StringByteOrder);
			s_Formatters["H"] = GetFormatter("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}{15}", c_HexByteOrder);
			s_Formatters["HC#"] = GetFormatter("0x{0},0x{1},0x{2},0x{3},0x{4},0x{5},0x{6},0x{7},0x{8},0x{9},0x{10},0x{11},0x{12},0x{13},0x{14},0x{15}", c_HexByteOrder);
			s_Formatters["HLDAP"] = GetFormatter("\\\\{0}\\\\{1}\\\\{2}\\\\{3}\\\\{4}\\\\{5}\\\\{6}\\\\{7}\\\\{8}\\\\{9}\\\\{10}\\\\{11}\\\\{12}\\\\{13}\\\\{14}\\\\{15}", c_HexByteOrder);
			s_Formatters["HVB"] = GetFormatter("&H{0},&H{1},&H{2},&H{3},&H{4},&H{5},&H{6},&H{7},&H{8},&H{9},&H{10},&H{11},&H{12},&H{13},&H{14},&H{15}", c_HexByteOrder);
			s_Formatters[""] = GetFormatter("{0}{1}{2}{3}-{4}{5}-{6}{7}-{8}{9}-{10}{11}{12}{13}{14}{15}", c_StringByteOrder);
			s_Formatters["BASE64"] = Base64;
			s_Formatters["BASE64C"] = Base64Combined;
		}

		public static string Format(string type, IEnumerator<Guid> guider, bool upcase, bool newline)
		{
			Func<IEnumerator<Guid>, bool, bool, string> retVal = null;
			if (!s_Formatters.TryGetValue(type, out retVal)) throw new ArgumentOutOfRangeException();
			return retVal(guider, upcase, newline);
		}

		public static string Format(string type, IEnumerable<Guid> guider, bool upcase, bool newline)
		{
			Func<IEnumerator<Guid>, bool, bool, string> retVal = null;
			if (!s_Formatters.TryGetValue(type, out retVal)) throw new ArgumentOutOfRangeException();
			return retVal(guider.GetEnumerator(), upcase, newline);
		}

		public static string Format(string type, Guid guid, bool upcase, bool newline)
		{
			return Format(type, new Guid[] { guid }, upcase, newline);
		}

		public static bool IsValid(string type)
		{
			return s_Formatters.ContainsKey(type);
		}

		private static string Base64(IEnumerator<Guid> guider, bool upcase, bool newline)
		{
			StringBuilder retVal = new StringBuilder(24);
			while (guider.MoveNext())
			{
				retVal.Append(Convert.ToBase64String(guider.Current.ToByteArray()));
				if (newline) retVal.Append("\r\n");
			}
			return retVal.ToString();
		}

		private static string Base64Combined(IEnumerator<Guid> guider, bool upcase, bool newline)
		{
			List<byte> bytes = new List<byte>();
			while (guider.MoveNext())
			{
				bytes.AddRange(guider.Current.ToByteArray());
			}
			return Convert.ToBase64String(bytes.ToArray()) + ((newline)?"\r\n":"");
		}

		private static Func<IEnumerator<Guid>, bool, bool, string> GetFormatter(string pattern, byte[] byteOrder)
		{
			return delegate(IEnumerator<Guid> guider, bool upcase, bool newline) {
				StringBuilder retVal = new StringBuilder(72);
				string format = (upcase)?"{0:X2}":"{0:x2}";

				while (guider.MoveNext())
				{
					List<string> items = new List<string>(new string[] {"","","","","","","","","","","","","","","",""});
					byte[] bytes = guider.Current.ToByteArray();
					for(int i = 0; i < 16; i++) 
					{
						items[byteOrder[i]] = string.Format(format, bytes[i]);
					}
					items.Add("{");
					items.Add("}");
					retVal.AppendFormat(pattern, items.ToArray());
					if (newline) retVal.Append("\r\n");
				}
				return retVal.ToString();
			};
			

		}
	}
}
