using System;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class Int128OutputFormat : BaseGuidSearchFormat
	{
		public Int128OutputFormat()
		{
			string int128Pattern = @"-?\d+";
			string pattern = @"(?'i1'" + int128Pattern + @")";
			Matcher = new System.Text.RegularExpressions.Regex(pattern);
		}
		public static Guid ToGuid(System.Numerics.BigInteger bigInt)
		{
			return new Guid(bigInt.ToByteArray());
		}

		protected override Guid MatchToGuid(Match m)
		{
			return ToGuid(System.Numerics.BigInteger.Parse(m.Groups["i1"].Value));
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			return (new System.Numerics.BigInteger(g.ToByteArray())).ToString() + (newline ? "\r\n" : "");
		}
	}
}
