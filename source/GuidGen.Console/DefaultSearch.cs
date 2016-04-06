using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	public static class DefaultSearch
	{
		private static string[] s_Formats;
		private static Regex s_Parser;
		private static Regex s_B64Parser_Single = new Regex("(^|(?![A-Za-z0-9+/]))(?:[A-Za-z0-9+/]{22}==)");
		//private static Regex s_B64Parser_SingleStart = new Regex("^(?:[A-Za-z0-9+/]{22}==)");
		private static Regex s_B64Parser = new Regex("^([A-Za-z0-9+/]{64})*((?:[A-Za-z0-9+/]{22}==)|(?:[A-Za-z0-9+/]{43}=)|([A-Za-z0-9+/]{64}))$");
		private static Regex s_isB64 = new Regex("^[A-Za-z0-9+/]+((=){1,2})?$");

		static DefaultSearch()
		{
			s_Formats = new string[] {
				"(0x(?'b0'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b3'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])(?'b5'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])(?'b7'[a-f0-9][a-f0-9])(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				"(\\[(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])-(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])-(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])-(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])-(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9])\\])",
				"(\\{(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9])\\})",
				"((?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])-(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])-(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])-(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])-(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				"((?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				"((0x|\\\\|&H)(?'b0'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b1'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b2'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b3'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b4'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b5'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b6'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b7'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b8'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b9'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b10'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b11'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b12'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b13'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b14'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\|\\s*,\\s*&H)(?'b15'[a-f0-9][a-f0-9]))",
				"(\\{?\\s*0x(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])\\s*,\\s*\\{?\\s*0x(?'b8'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b9'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b10'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b11'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b12'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b13'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b14'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b15'[a-f0-9][a-f0-9])\\s*\\}?\\s*\\}?)"
			};
			s_Parser = new Regex(string.Join("|", s_Formats), RegexOptions.IgnoreCase);
		}

		public static int Replace(System.IO.TextReader reader, System.IO.TextWriter writer, Guider guider, Dictionary<Guid, Guid> replacements, string format, bool upcase, bool copyByLine = false)
		{
			int totalReplacements = 0;
			string ucformat = upcase ? "X2" : "x2";
			Guid replacement;
			string line;

			while ((line = reader.ReadLine()) != null)
			{
				int lc = 0;
				MatchCollection mc = s_Parser.Matches(line);
				foreach (Match m in mc)
				{
					lc++;
					byte[] bytes = new byte[16];
					for (int i = 0; i < 16; i++)
					{
						byte b;
						if (!byte.TryParse(m.Groups["b" + i.ToString()].Value, System.Globalization.NumberStyles.HexNumber, null, out b))
						{
							if (!m.Success) throw new ArgumentOutOfRangeException();
						}
						bytes[i] = b;
					}
					Guid active = new Guid(bytes);
					if (!replacements.TryGetValue(active, out replacement))
					{
						if (!guider.MoveNext(active)) continue;
						replacement = guider.Current;
						replacements[active] = replacement;
					}
					if (format == null)
					{
						byte[] rbytes = replacement.ToByteArray();
						for (int i = 0; i < 16; i++)
						{
							Group g = m.Groups["b" + i.ToString()];
							string hex = rbytes[i].ToString(ucformat);
							line = line.Substring(0, g.Index) + hex + line.Substring(g.Index + 2);
						}
					}
					else
					{
						string formattedGuid = GuidFormats.Format(format, replacement, upcase, false);
						//replace 
						line = line.Replace(m.Value, formattedGuid);
					}
					totalReplacements++;
				}

				mc = s_B64Parser_Single.Matches(line);
				foreach (Match m in mc)
				{
					lc++;
					Guid active = new Guid(Convert.FromBase64String(m.Value));
					if (!replacements.TryGetValue(active, out replacement))
					{
						if (!guider.MoveNext(active)) continue;
						replacement = guider.Current;
						replacements[active] = replacement;
					}
					if (format == null)
					{
						byte[] rbytes = replacement.ToByteArray();
						string b64Replacement = Convert.ToBase64String(rbytes);
						line = line.Substring(0, m.Index) + b64Replacement + line.Substring(m.Index + m.Length);
					}
					else
					{
						string formattedGuid = GuidFormats.Format(format, replacement, upcase, false);
						//replace 
						line = line.Replace(m.Value, formattedGuid);
					}
					totalReplacements++;
				}

				if (writer != null) writer.WriteLine(line);
				if (copyByLine) System.Windows.Forms.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, line);
			}

			return totalReplacements;
		}

		public static int Search(System.IO.TextReader reader, List<Found> found)
		{
			int startCount = found.Count;
			bool inBase64 = false;
			int lineCount = 0;
			string line;
			while (reader.Peek() != -1 && (line = reader.ReadLine()) != null)
			{

				if (s_isB64.IsMatch(line))
				{
					int len = line.Length;
					int beginLine = lineCount;
					System.Text.StringBuilder concatd = new StringBuilder(line);
					bool appendLast = false;
					List<Found> temp = new List<Found>();
					AddGuidsFromLine(lineCount, line, temp);
					if (!line.EndsWith("="))
					{
						while (!line.EndsWith("=") && (line = reader.ReadLine()) != null && (appendLast = s_isB64.IsMatch(line)) && line.Length == len)
						{
							lineCount++;
							AddGuidsFromLine(lineCount, line, temp);
							concatd.Append(line);
							appendLast = false;
						}
						if (line != null && (inBase64 = appendLast)) concatd.Append(line);
					}
					if (s_B64Parser.IsMatch(concatd.ToString()))
					{
						Found f = new Found() { Line = beginLine, Match = concatd.ToString() };
						AddGuidsFromBytes(Convert.FromBase64String(concatd.ToString()), f);
						found.Add(f);
					}
					else
					{
						found.AddRange(temp);
					}
				}
				else if (line != null)
				{
					AddGuidsFromLine(lineCount, line, found);
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

		private static void AddGuidsFromLine(int index, string line, List<Found> guids)
		{
			MatchCollection mc = s_Parser.Matches(line);
			foreach (Match m in mc)
			{
				byte[] bytes = new byte[16];
				for (int i = 0; i < 16; i++)
				{
					byte b;
					if (!byte.TryParse(m.Groups["b" + i.ToString()].Value, System.Globalization.NumberStyles.HexNumber, null, out b))
					{
						if (!m.Success) throw new ArgumentOutOfRangeException();
					}
					bytes[i] = b;
				}
				guids.Add(new Found() { Line = index, Match = m.Value, Guids = new Guid[] { new Guid(bytes) } });
			}

			mc = s_B64Parser_Single.Matches(line);
			foreach (Match m in mc)
			{
				Found f = new Found() { Line = index, Match = m.Value };
				AddGuidsFromBytes(Convert.FromBase64String(m.Value), f);
				guids.Add(f);
			}
		}

	}
}
