using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	/// <summary>
	/// Guid formatter that outputs Guid byte array as base64
	/// </summary>
	public class Base64GuidFormat : BaseGuidFormat
	{
		private static Regex s_B64Parser_Single = new Regex("(^|(?![A-Za-z0-9+/]))(?:[A-Za-z0-9+/]{22}==)");
		private static Regex s_B64Parser = new Regex("^([A-Za-z0-9+/]{64})*((?:[A-Za-z0-9+/]{22}==)|(?:[A-Za-z0-9+/]{43}=)|([A-Za-z0-9+/]{64}))$");

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="combine">Combine bytes before base64 encoding</param>
		public Base64GuidFormat(bool combine = false)
		{
			CombineOutput = combine;
		}

		/// <summary>
		/// Combine multiple GUIDS into single byte array before base64 encoding
		/// </summary>
		public bool CombineOutput { get; set; }

		public override Regex Matcher
		{
			get
			{
				return ((CombineOutput)?s_B64Parser:s_B64Parser_Single);
			}

			protected set
			{
				throw new NotSupportedException();
			}
		}

		public override string ToString(Guid g, bool upcase, bool newline)
		{
			if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
			return Convert.ToBase64String(g.ToByteArray()) + (newline?"\r\n":"");
		}

		public override string ToString(IEnumerable<Guid> guids, bool upcase, bool newline)
		{
			if (CombineOutput)
			{
				if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
				List<byte> bytes = new List<byte>();
				foreach(var guid in guids)
				{
					bytes.AddRange(guid.ToByteArray());
				}
				return Convert.ToBase64String(bytes.ToArray()) + ((newline)?"\r\n":"");
			}
			else
			{
				StringBuilder retVal = new StringBuilder();
				foreach(var guid in guids)
				{
					retVal.Append(ToString(guid, upcase, newline));
				}
				return retVal.ToString();
			}
		}

		public override string ToString(IEnumerator<Guid> guider, bool upcase, bool newline)
		{
			if (CombineOutput)
			{
				if (upcase == true) throw new ArgumentOutOfRangeException("upcase", "Base64 does not support upper case.");
				List<byte> bytes = new List<byte>();
				while (guider.MoveNext())
				{
					bytes.AddRange(guider.Current.ToByteArray());
				}
				return Convert.ToBase64String(bytes.ToArray()) + ((newline)?"\r\n":"");
			}
			else
			{
				StringBuilder retVal = new StringBuilder();
				while (guider.MoveNext())
				{
					retVal.Append(ToString(guider.Current, upcase, newline));
				}
				return retVal.ToString();
			}
		}
	}
}
