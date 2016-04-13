using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	/// <summary>
	/// Represents an available guid format
	/// </summary>
	public class GuidFormat : BaseGuidSearchFormat
	{
		private static Regex s_AutoIndexMatcher = new Regex(@"\{(?'byte_index'\d+)\}");
		private static Regex s_AutoMemberMatcher = new Regex(@"\<\<.*?\>\>");
		private static Regex s_EscaperReplacer = new Regex(@"[\(\)\[\]\.\?\^\$\!]");
		
		private string _OutputFormat = null;

		/// <summary>
		/// Get/set the format string that is applied to the byteorder
		/// </summary>
		public string OutputFormat
		{
			get { return _OutputFormat; }
			set
			{
				_OutputFormat = value.Replace("\\s", "");
				string pattern = value.Replace("\\", "\\\\").Replace("\\\\s", "\\s*").Replace(" ", "\\s+").Replace("{{", "\\{").Replace("}}}", "}\\}").Replace("}}", "\\}");
				pattern = s_EscaperReplacer.Replace(pattern, (m)=> { return "\\" + m.Value; });
				pattern = s_AutoIndexMatcher.Replace(pattern, (m)=> { return "(?'b" + m.Groups["byte_index"].Value + "'[a-zA-Z0-9]{2})"; });
				pattern = s_AutoMemberMatcher.Replace(pattern, @"(\<\<)?[a-zA-Z_][a-zA-Z0-9_]*(\>\>)?");
				Matcher = new Regex(pattern);
			}
		}


		/// <summary>
		/// The byte order of the Guid that is applied to the format string
		/// </summary>
		public byte[] ByteOrder { get; set; } = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

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

	}
}
