using System;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class VersionGuidFormat : BaseGuidSearchFormat, IValid
	{

		public VersionGuidFormat()
		{
			// only allow positive version numbers.
			string subpattern = @"(\d{1,9}|1\d{9}|20\d{8}|21[0-3]\d{7}|214[0-6]\d{6}|2147[0-3]\d{5}|21474[0-7]\d{4}|214748[012]\d{4}|2147483[0-5]\d{3}|21474836[0-3]\d{2}|214748364[0-7])";
			string pattern = @"(?'major'" + subpattern + @")\.(?'minor'" + subpattern + @")\.(?'build'" + subpattern + @")\.(?'revision'" + subpattern + @")";
			Matcher = new System.Text.RegularExpressions.Regex(pattern);
		}

		public override string ToString(Guid g, bool upcase, bool newline)
		{
			byte[] bytes = g.ToByteArray();
			if (!(BitConverter.ToInt32(bytes, 0) >= 0 && BitConverter.ToInt32(bytes, 4) >= 0 && BitConverter.ToInt32(bytes, 8) >= 0 && BitConverter.ToInt32(bytes, 12) >= 0)) throw new ArgumentOutOfRangeException("Guid results in invalid version format. (negative value)");
			return string.Format("{0}.{1}.{2}.{3}", BitConverter.ToInt32(bytes, 0), BitConverter.ToInt32(bytes, 4), BitConverter.ToInt32(bytes, 8), BitConverter.ToInt32(bytes, 12)) + (newline?"\r\n":"");
		}

		public static System.Version ToVersion(Guid g)
		{
			byte[] bytes = g.ToByteArray();
			return new System.Version(BitConverter.ToInt32(bytes, 0), BitConverter.ToInt32(bytes, 4), BitConverter.ToInt32(bytes, 8), BitConverter.ToInt32(bytes, 12));
		}

		public bool IsValid(Guid g)
		{
			byte[] bytes = g.ToByteArray();
			return BitConverter.ToInt32(bytes, 0) >= 0 && BitConverter.ToInt32(bytes, 4) >= 0 && BitConverter.ToInt32(bytes, 8) >= 0 && BitConverter.ToInt32(bytes, 12) >= 0;
		}

		public static Guid ToGuid(Version v)
		{
			byte[] bytes = new byte[16];
			Buffer.BlockCopy(BitConverter.GetBytes(v.Major), 0, bytes, 0, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(v.Minor), 0, bytes, 4, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(v.Build), 0, bytes, 8, 4);
			Buffer.BlockCopy(BitConverter.GetBytes(v.Revision), 0, bytes, 12, 4);
			return new Guid(bytes);
		}

		public override bool TryParse(string s, out Guid guid)
		{
			bool retVal = false;
			Guid tmp = Guid.Empty;
#if NET2_0 || NET3_5
			try { tmp = ToGuid(new System.Version(s)); retVal = true; } catch(Exception) { }
#else
			System.Version version;
			if ((retVal = System.Version.TryParse(s, out version))) tmp = ToGuid(version);
#endif
			guid = tmp;
			return retVal;
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		protected override Guid MatchToGuid(Match m)
		{
			return ToGuid(new Version(int.Parse(m.Groups["major"].Value), int.Parse(m.Groups["minor"].Value), int.Parse(m.Groups["build"].Value), int.Parse(m.Groups["revision"].Value)));
		}
	}
}
