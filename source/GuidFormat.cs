using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	/// <summary>
	/// Represents an available guid format
	/// </summary>
	public class GuidFormat : BaseGuidFormat
	{
		private string _MatchPattern = null;

		/// <summary>
		/// Get/set the format string that is applied to the byteorder
		/// </summary>
		public string OutputFormat { get; set; }

		/// <summary>
		/// The byte order of the Guid that is applied to the format string
		/// </summary>
		public byte[] ByteOrder { get; set; }

		/// <summary>
		/// Get/set the regular expression for matching
		/// </summary>
		public string Match
		{
			get { return _MatchPattern; }
			set
			{
				_MatchPattern = value;
				Matcher = new Regex(_MatchPattern, RegexOptions.IgnoreCase);
			}
		}

		/// <summary>
		/// Formats the given guid in the output format using the byte order
		/// </summary>
		/// <param name="g">Guid to format</param>
		/// <param name="upcase">upper case output format</param>
		/// <param name="newline">append newline at the end</param>
		/// <returns></returns>
		public override string ToString(Guid g, bool upcase=false, bool newline=false)
		{
			string[] items = new string[17]; 
			byte[] bytes = g.ToByteArray();
			for(int i = 0; i < 16; i++) 
			{
				items[ByteOrder[i]] = string.Format((upcase)?"{0:X2}":"{0:x2}", bytes[i]);
			}
			items[16] = newline?"\r\n":"";
			return string.Format(OutputFormat+"{16}", items);
		}

		/// <summary>
		/// Formats the given guid(s) in the output format using the byte order
		/// </summary>
		/// <param name="guider">Enumerator that returns guids to format</param>
		/// <param name="upcase">upper case output format</param>
		/// <param name="newline">append newline at the end of each formatted guid</param>
		/// <returns></returns>
		public override string ToString(IEnumerator<Guid> guider, bool upcase=false, bool newline=true)
		{
			StringBuilder retVal = new StringBuilder();
			while(guider.MoveNext())
			{
				retVal.Append(ToString(guider.Current, upcase, newline));
			}
			return retVal.ToString();
		}

		/// <summary>
		/// Formats the given guid(s) in the output format using the byte order
		/// </summary>
		/// <param name="guider">list of guids to format</param>
		/// <param name="upcase">upper case output format</param>
		/// <param name="newline">append newline at the end of each formatted guid</param>
		/// <returns></returns>
		public override string ToString(IEnumerable<Guid> guids, bool upcase=false, bool newline=true)
		{
			StringBuilder retVal = new StringBuilder();
			foreach(var g in guids)
			{
				retVal.Append(ToString(g, upcase, newline));
			}
			return retVal.ToString();
		}
	}
}
