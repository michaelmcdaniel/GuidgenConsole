using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	public static class GuidSearcher
	{
		private static Dictionary<string, Func<System.IO.TextReader, List<Found>, int>> s_Search = new Dictionary<string, Func<System.IO.TextReader, List<Found>, int>>(StringComparer.InvariantCultureIgnoreCase);
		private static Dictionary<string, Func<System.IO.TextReader, List<Found>, int>> s_Replace = new Dictionary<string, Func<System.IO.TextReader, List<Found>, int>>(StringComparer.InvariantCultureIgnoreCase);
		private static readonly byte[] c_StringByteOrder = new byte[] { 4, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };
		private static readonly byte[] c_HexByteOrder = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

		private static Regex s_B64Parser = new Regex("(^|(?![A-Za-z0-9+/]))([A-Za-z0-9+/]{64})*((?:[A-Za-z0-9+/]{22}==)|(?:[A-Za-z0-9+/]{43}=)|([A-Za-z0-9+/]{64}))$");
		private static Regex s_B64Parser_Single = new Regex("(^|(?![A-Za-z0-9+/]))(?:[A-Za-z0-9+/]{22}==)");
		private static Regex s_isB64 = new Regex("^[A-Za-z0-9+/]+((=){1,2})?$");

		static GuidSearcher()
		{
			s_Search["N"] = GetParser("([a-f0-9]{2}){16}", c_StringByteOrder);
			s_Search["ORACLE"] = GetParser("([a-f0-9]{2}){4}(?:-([a-f0-9]{2}){2}){3}-([a-f0-9]{2}){6}", c_HexByteOrder);
			s_Search["D"] = GetParser("([a-f0-9]{2}){4}(?:-([a-f0-9]{2}){2}){3}-([a-f0-9]{2}){6}", c_StringByteOrder);
			s_Search["P"] = GetParser("\\{([a-f0-9]{2}){4}(?:-([a-f0-9]{2}){2}){3}-([a-f0-9]{2}){6}\\}", c_StringByteOrder);
			s_Search["B"] = GetParser("\\[([a-f0-9]{2}){4}(?:-([a-f0-9]{2}){2}){3}-([a-f0-9]{2}){6}\\]", c_StringByteOrder);
			s_Search["C"] = GetParser("0x([a-f0-9]{2}){4}(?:\\s*,\\s*0x([a-f0-9]{2}){2}){3}(?:\\s*,\\s*0x([a-f0-9]{2})){6}", c_StringByteOrder);
			s_Search["CP"] = GetParser("\\{\\s*0x([a-f0-9]{2}){4}(?:\\s*,\\s*0x([a-f0-9]{2}){2}){3}\\s*\\{\\s*(?:\\s*,\\s*0x([a-f0-9]{2})){6}\\s*\\}\\s*\\}", c_StringByteOrder);
			s_Search["H"] = GetParser("([a-f0-9]{2}){16}", c_HexByteOrder);
			s_Search["HC#"] = GetParser("0x([a-f0-9]{2})(?:\\s*,\\s*0x([a-f0-9]{2})){15}", c_HexByteOrder);
			s_Search["HLDAP"] = GetParser("(?:\\\\([a-f0-9]{2})){16}", c_HexByteOrder);
			s_Search["HVB"] = GetParser("&H([a-f0-9]{2})(?:\\s*,\\s*&H([a-f0-9]{2})){15}", c_HexByteOrder);
			s_Search["BASE64"] = SearchB64;
			s_Search["BASE64C"] = SearchB64Combined;
			s_Search[""] = DefaultSearch.Search;
		}

		public static int Search(string type, System.IO.TextReader reader, List<Found> found)
		{
			Func<System.IO.TextReader, List<Found>, int> f;
			if (s_Search.TryGetValue(type, out f))
			{
				return f(reader, found);
			}
			return -1;
		}

		private static Func<System.IO.TextReader, List<Found>, int> GetParser(string parser, byte[] byteOrder)
		{
			Regex re = new Regex(parser, RegexOptions.IgnoreCase);
			return delegate(System.IO.TextReader reader, List<Found> found)
			{
				int startcount = found.Count;
				int lineCount = 0;
				string line;
				while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
				{
					List<Guid> retVal = new List<Guid>();
					MatchCollection mc = re.Matches(line);
					foreach (Match m in mc)
					{
						CaptureCollection cs = m.Captures;
						if (cs.Count == 17)
						{
							byte[] bytes = new byte[16];
							for (int i = 0; i < 16; i++)
							{
								bytes[byteOrder[i]] = (byte)Convert.ToInt32(cs[byteOrder[i]].Value, 16);
							}
							found.Add(new Found() { Line = lineCount, Match = m.Value, Guids = new Guid[] { new Guid(bytes) } });
						}
					}
					lineCount++;
				}
				return found.Count - startcount;
			};
		}

		public static int SearchB64(System.IO.TextReader reader, List<Found> found)
		{
			int startcount = found.Count;
			int lineCount = 0;
			string line;
			while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
			{
				MatchCollection mc = s_B64Parser_Single.Matches(line);
				foreach (Match m in mc)
				{
					found.Add(new Found() { Line = lineCount, Match = m.Value, Guids = new Guid[] { new Guid(Convert.FromBase64String(m.Value)) } });
				}
				lineCount++;
			}
			return found.Count - startcount;
		}

		public static int SearchB64Combined(System.IO.TextReader reader, List<Found> found)
		{
			int startCount = found.Count;
			int lineCount = 0;
			int startLine = 0;
			string line;
			System.Text.StringBuilder concatd = new StringBuilder();
			while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
			{
				if (s_isB64.IsMatch(line))
				{
					concatd.Append(line);
					Match m = s_B64Parser.Match(concatd.ToString());
					if (m.Success)
					{
						Found f = new Found() { Line = startLine, Match = m.Value };
						AddGuidsFromBytes(Convert.FromBase64String(m.Value), f);
						found.Add(f);
						startLine = lineCount+1;
						concatd.Clear();
					}
				}
				else 
				{
					concatd.Clear();
					startLine = lineCount+1;
				}
				lineCount++;

			}
			return found.Count - startCount;
		}

		private static void AddGuidsFromBytes(byte[] bytes, Found guids)
		{
			if (bytes.Length == 16)
			{
				guids.Guids = new Guid[] { new Guid(bytes) };
			}
			else if (bytes.Length % 16 == 0)
			{
				guids.Guids = new Guid[bytes.Length / 16];
				int index = 0;
				while (index * 16 < bytes.Length)
				{
					byte[] dest = new byte[16];
					Array.Copy(bytes, index * 16, dest, 0, 16);
					guids.Guids[index] = new Guid(dest);
					index++;
				}
			}
		}


	}
}
