using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class SearchFormat : BaseGuidSearchFormat
	{
		private static Regex s_AllMatcher = null;
		private static Regex s_Matcher = null;
		private static Regex s_isB64 = new Regex("^[A-Za-z0-9+/]+((=){1,2})?$");

		static SearchFormat()
		{
			List<string> formats = new List<string>( new string[] {
				"(0x(?'b0'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b3'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])(?'b5'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])(?'b7'[a-f0-9][a-f0-9])(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				//"(\\[(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])-(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])-(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])-(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])-(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9])\\])",
				//"((?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])-(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])-(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])-(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])-(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				//"((?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9]))",
				"([\\{]?(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])\\-?(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])\\-?(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])\\-?(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])\\-?(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9])[\\}]?)",
				"(\\[?(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])\\-?(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])\\-?(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])\\-?(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])\\-?(?'b10'[a-f0-9][a-f0-9])(?'b11'[a-f0-9][a-f0-9])(?'b12'[a-f0-9][a-f0-9])(?'b13'[a-f0-9][a-f0-9])(?'b14'[a-f0-9][a-f0-9])(?'b15'[a-f0-9][a-f0-9])\\]?)",
				"((0x|\\\\\\\\|&H)(?'b0'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b1'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b2'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b3'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b4'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b5'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b6'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b7'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b8'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b9'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b10'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b11'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b12'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b13'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b14'[a-f0-9][a-f0-9])(\\s*,\\s*0x|\\s*\\\\\\\\|\\s*,\\s*&H)(?'b15'[a-f0-9][a-f0-9]))",
				"(\\{?\\s*0x(?'b3'[a-f0-9][a-f0-9])(?'b2'[a-f0-9][a-f0-9])(?'b1'[a-f0-9][a-f0-9])(?'b0'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b5'[a-f0-9][a-f0-9])(?'b4'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b7'[a-f0-9][a-f0-9])(?'b6'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b8'[a-f0-9][a-f0-9])(?'b9'[a-f0-9][a-f0-9])\\s*,\\s*\\{?\\s*0x(?'b10'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b11'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b12'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b13'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b14'[a-f0-9][a-f0-9])\\s*,\\s*0x(?'b15'[a-f0-9][a-f0-9])\\s*\\}?\\s*\\}?)",
				
			});
			s_Matcher = new Regex(string.Join("|", formats.ToArray()), RegexOptions.IgnoreCase);
			formats.Add("^[A-Za-z0-9+/]+((=){1,2})?$");
			s_AllMatcher = new Regex(string.Join("|", formats.ToArray()), RegexOptions.IgnoreCase);
		}

		public SearchFormat()
		{
			Matcher = s_AllMatcher;
		}

		protected override Guid MatchToGuid(Match m)
		{
			if (s_isB64.IsMatch(m.Value)) return Base64GuidFormat.ToGuid(m);
			return base.MatchToGuid(m);
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			throw new NotImplementedException();
		}
	}
}
