using System;
using System.Collections.Generic;
#if !NET2_0
using System.Linq;
#endif
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	/// <summary>
	/// Guid formatter that outputs Guid byte array as base64
	/// </summary>
	public class Base64GuidFormat : BaseGuidSearchFormat
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Base64GuidFormat()
		{
			Matcher = new Regex("(^|(?![A-Za-z0-9+/]))(?:[A-Za-z0-9+/]{22}==)");
		}

		public static Guid ToGuid(Match m)
		{
			try
			{
				byte[] bytes = Convert.FromBase64String(m.Value);
				if (bytes.Length == 16)
				{
					return new Guid(bytes);
				}
			}
			catch (FormatException) { }

			return Guid.Empty;
		}

		protected override Guid MatchToGuid(Match m)
		{
			return ToGuid(m);
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		public override string ToString(Guid g, bool upcase, bool newline)
		{
			if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
			return Convert.ToBase64String(g.ToByteArray()) + (newline?"\r\n":"");
		}
	}
}
