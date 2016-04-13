using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public abstract class BaseGuidSearchFormat : BaseGuidFormat, IGuidSearcher
	{
		/// <summary>
		/// Get the regular expression
		/// </summary>
		public virtual Regex Matcher { get; protected set; }
		
		/// <summary>
		/// Get whether or not the format can match a given string
		/// </summary>
		public bool CanMatch { get { return Matcher != null; } }


		/// <summary>
		/// Matches given string against matching format.  If no pattern exists, return false.
		/// </summary>
		/// <param name="s">string to match against</param>
		/// <returns>true if matching format</returns>
		public virtual bool IsMatch(string s)
		{
			if (Matcher == null) return false;
			return Matcher.IsMatch(s);
		}

		public virtual IEnumerable<Found> Find(string s, int line=-1)
		{
			if (!string.IsNullOrEmpty(s))
			{
				foreach(Match m in Matcher.Matches(s))
				{
					yield return new Found() { Line=line, Column=m.Index, Guid = MatchToGuid(m), Match=m.Value };
				}
			}
		}


		protected virtual string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			string ucformat = upcase ? "X2" : "x2";
			byte[] rbytes = output.ToByteArray();
			string retVal = m.Value;
			for (int i = 0; i < 16; i++)
			{
				Group g = m.Groups["b" + i.ToString()];
				string hex = rbytes[i].ToString(ucformat);
				line = line.Substring(0, g.Index - offset) + hex + line.Substring(g.Index + 2 - offset);
			}
			return line;
		}

		protected virtual Guid MatchToGuid(Match m)
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
			return new Guid(bytes);
		}

		public virtual string Replace(string input, Guider guider, IGuidFormatter formatter=null, bool upcase = false, Action<Replacement> onReplacement = null, int lineNumber = -1)
		{
			if (!string.IsNullOrEmpty(input))
			{
				return Matcher.Replace(input, (m) => {
					Guid foundGuid = MatchToGuid(m);
					if (guider.MoveNext(foundGuid))
					{
						Guid replacementGuid = guider.Current;
						string replacementText = formatter==null?Replace(m.Value, m.Index, m, foundGuid, replacementGuid, upcase):formatter.ToString(replacementGuid, upcase, false);
						if (onReplacement != null) onReplacement(new Replacement() { Line=lineNumber, Column = m.Index, FoundText=m.Value, FoundGuid=foundGuid,  ReplacedByText=replacementText, ReplacedByGuid=replacementGuid });
						return replacementText;
					}
					return m.Value;
				});
			}
			return input;
		}

		public virtual bool TryParse(string s, out Guid guid)
		{
			if (string.IsNullOrEmpty(s)) { guid = Guid.Empty;  return false; }
			Match m = Matcher.Match(s);
			bool success = m.Success;
			guid = success?MatchToGuid(m):Guid.Empty;
			return success;
		}

	}
}
