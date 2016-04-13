using System;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class Int32OutputFormat : BaseGuidSearchFormat
	{
		public Int32OutputFormat()
		{
			string int32Pattern = @"-?\d{1,9}|-?1\d{9}|-?20\d{8}|-?21[0-3]\d{7}|-?214[0-6]\d{6}|-?2147[0-3]\d{5}|-?21474[0-7]\d{4}|-?214748[012]\d{4}|-?2147483[0-5]\d{3}|-?21474836[0-3]\d{2}|214748364[0-7]|-214748364[0-8]";
			string pattern = @"(?'i1'" + int32Pattern + @"),\s(?'i2'" + int32Pattern + @"),\s(?'i3'" + int32Pattern + @"),\s(?'i4'" + int32Pattern + @")";
			Matcher = new System.Text.RegularExpressions.Regex(pattern);
		}
		public static Guid ToGuid(Int32 i1, Int32 i2, Int32 i3, Int32 i4)
		{
			byte[] bytes = new byte[16];
			Buffer.BlockCopy(BitConverter.GetBytes(i1), 0, bytes, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(i2), 0, bytes, 4, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(i3), 0, bytes, 8, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(i4), 0, bytes, 12, 4);
			return new Guid(bytes);
		}

		protected override Guid MatchToGuid(Match m)
		{
			return ToGuid(int.Parse(m.Groups["i1"].Value), int.Parse(m.Groups["i2"].Value), int.Parse(m.Groups["i3"].Value), int.Parse(m.Groups["i4"].Value));
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			byte[] bytes = g.ToByteArray();
			return string.Format("{0}, {1}, {2}, {3}", BitConverter.ToInt32(bytes,0), BitConverter.ToInt32(bytes,4), BitConverter.ToInt32(bytes,8), BitConverter.ToInt32(bytes,12)) + (newline?"\r\n":"");
		}
	}
}
