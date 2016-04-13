using System;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class Int64OutputFormat : BaseGuidSearchFormat
	{
		public Int64OutputFormat()
		{
			string int32Pattern = @"-?\d+";
			string pattern = @"(?'i1'" + int32Pattern + @"),\s(?'i2'" + int32Pattern + @")";
			Matcher = new System.Text.RegularExpressions.Regex(pattern);
		}
		public static Guid ToGuid(Int64 i1, Int64 i2)
		{
			byte[] bytes = new byte[16];
			Buffer.BlockCopy(BitConverter.GetBytes(i1), 0, bytes, 0, 8);
			Buffer.BlockCopy(BitConverter.GetBytes(i2), 0, bytes, 8, 8);
			return new Guid(bytes);
		}

		protected override Guid MatchToGuid(Match m)
		{
			return ToGuid(Int64.Parse(m.Groups["i1"].Value), Int64.Parse(m.Groups["i2"].Value));
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			byte[] bytes = g.ToByteArray();
			return string.Format("{0}, {1}", BitConverter.ToInt64(bytes,0), BitConverter.ToInt64(bytes,8)) + (newline?"\r\n":"");
		}
	}
}
