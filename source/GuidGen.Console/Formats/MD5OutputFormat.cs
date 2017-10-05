using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace GuidGen.Formats
{
	public class MD5OutputFormat : BaseGuidSearchFormat, IGuidSearcher
	{
		private MD5 _Hasher = MD5.Create();

		public MD5OutputFormat()
		{
			Matcher = new System.Text.RegularExpressions.Regex("^.*$", System.Text.RegularExpressions.RegexOptions.None); // match entire line.
		}

		public IGuidFormatter OutputFormat { get; set; } = null;

		protected override Guid MatchToGuid(Match m)
		{
			return new Guid(_Hasher.ComputeHash(Encoding.Default.GetBytes(m.Value)));
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			if (OutputFormat == null) throw new ApplicationException("MD5OutputFormat requires OutputFormat");
			return OutputFormat.ToString(g, upcase, newline);
		}
	}
}
