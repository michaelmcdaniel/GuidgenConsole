using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class Base64CombinedFormat : BaseGuidSearchFormat, IGuidSearcher
	{
		private static Regex s_B64Parser = new Regex("^([A-Za-z0-9+/]{64})*((?:[A-Za-z0-9+/]{22}==)|(?:[A-Za-z0-9+/]{43}=)|([A-Za-z0-9+/]{64}))$", RegexOptions.Singleline);
		private static Regex s_B64 = new Regex("[A-Za-z0-9\\s+/]+((=){1,2})?");
		private static Regex s_SpaceStripper = new Regex("\\s+", RegexOptions.Singleline);

		/// <summary>
		/// Constructor
		/// </summary>
		public Base64CombinedFormat()
		{
			Matcher = s_B64Parser;
		}
		
		public override IEnumerable<Found> Find(string s, int line=-1)
		{
			if (!string.IsNullOrEmpty(s))
			{
				foreach(Match m in s_B64.Matches(s))
				{
					string sm = s_SpaceStripper.Replace(m.Value, "");
					foreach(Guid g in ToGuid(sm)) yield return new Found() { Line=line, Column=m.Index, Guid = g, Match=g.ToString("D") };
				}
			}
		}


		public override string Replace(string input, Guider guider, IGuidFormatter formatter=null, bool upcase = false, Action<Replacement> onReplacement = null, int lineNumber = -1)
		{
			string output = (input != null)?input:"";
			return s_B64.Replace(input, (m)=> {
				string s = s_SpaceStripper.Replace(m.Value, "");
				StringBuilder replacementText = new StringBuilder();
				List<Guid> guids = new List<Guid>();
				foreach(Guid g in ToGuid(s))
				{
					if (guider.MoveNext(g))
					{
						Guid replacementGuid = guider.Current;
						guids.Add(replacementGuid);
						string currentText = (formatter != null)?formatter.ToString(replacementGuid, upcase, false):replacementGuid.ToString("D");
						if (formatter != null)
						{
							if (replacementText.Length > 0) replacementText.Append("\r\n");
							replacementText.Append(currentText);
						}
						if (onReplacement != null) onReplacement(new Replacement() { Line=lineNumber, Column = m.Index, FoundText=g.ToString("D"), FoundGuid=g,  ReplacedByText=currentText, ReplacedByGuid=replacementGuid });
					}
				}
				if (guids.Count > 0 && replacementText.Length == 0) replacementText.Append(ToString(guids, upcase, false));
				else if (guids.Count == 0) return m.Value;
				return replacementText.ToString();
			});
		}
		
		public static IEnumerable<Guid> ToGuid(string s)
		{
			byte[] bytes = null;
			try { bytes = Convert.FromBase64String(s); } catch (FormatException) { }
			if (bytes.Length % 16 == 0)
			{
				for(int i = 0; i < bytes.Length; i+=16)
				{
					byte[] guid = new byte[16];
					Buffer.BlockCopy(bytes, i, guid, 0, 16);
					yield return new Guid(guid);
				}
			}
		}

		protected override Guid MatchToGuid(Match m)
		{
			byte[] bytes = null;
			try { bytes = Convert.FromBase64String(m.Value); } catch (FormatException) { }
			if (bytes.Length == 16)
			{
				return new Guid(bytes);
			}
			throw new FormatException();
		}

		public override string ToString(Guid g, bool upcase = false, bool newline = false)
		{
			if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
			return Convert.ToBase64String(g.ToByteArray()) + (newline?"\r\n":"");
		}

		public override string ToString(IEnumerable<Guid> guids, bool upcase = false, bool newline = false)
		{
			if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
			List<byte> bytes = new List<byte>();
			foreach(var guid in guids)
			{
				bytes.AddRange(guid.ToByteArray());
			}
			return Convert.ToBase64String(bytes.ToArray()) + ((newline)?"\r\n":"");
		}

		public override string ToString(IEnumerator<Guid> guider, bool upcase = false, bool newline = false)
		{
			if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
			List<byte> bytes = new List<byte>();
			while (guider.MoveNext())
			{
				bytes.AddRange(guider.Current.ToByteArray());
			}
			return Convert.ToBase64String(bytes.ToArray()) + ((newline)?"\r\n":"");
		}
	}
}
