using System;
using System.Collections.Generic;

namespace GuidGen.Formats
{
	/// <summary>
	/// Abstract base class for formatting guids
	/// </summary>
	public abstract class BaseGuidFormat : IGuidFormatter
	{
#if DNX451
		public static readonly string DefaultFormat = "D";
		public static readonly Guid DefaultExample = new Guid("01020304-0506-0708-090a-0b0c0d0e0f10");
#else
		public static readonly string DefaultFormat = System.Configuration.ConfigurationManager.AppSettings["default:output:format"]??"D";
		public static readonly Guid DefaultExample = new Guid(System.Configuration.ConfigurationManager.AppSettings["default:example"]??"01020304-0506-0708-090a-0b0c0d0e0f10");
#endif 

		/// <summary>
		/// Get/Set the key for the using the format
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Get/set a description of the format
		/// </summary>
		public string Description { get; set; }
		
		/// <summary>
		/// Overridden: Gets a hashcode for the format
		/// </summary>
		/// <returns>hashcode of the key</returns>
		public override int GetHashCode()
		{
			return Key.GetHashCode();
		}

		/// <summary>
		/// Overriden: returns equality of Keys
		/// </summary>
		/// <param name="obj">object to match</param>
		/// <returns>true if matches</returns>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (obj is string) return Equals((string)obj);
			return base.Equals(obj);
		}

		/// <summary>
		/// Matches string against key (Invariant Culture - Ignores Case)
		/// </summary>
		/// <param name="s">String to match</param>
		/// <returns>true if matching keys</returns>
		public virtual bool Equals(string s)
		{
			if (string.IsNullOrEmpty(s)) return string.IsNullOrEmpty(Key);
			return s.Equals(Key, StringComparison.InvariantCultureIgnoreCase);
		}

		/// <summary>
		/// Get whether or not this format is the default format for the app.
		/// </summary>
		public bool IsDefault { get { return Equals(DefaultFormat); } }

		/// <summary>
		/// Overridden: returns help formatted text
		/// </summary>
		/// <returns>help format</returns>
		public override string ToString()
		{
			return string.Format(" {0}: {1} {2}\r\n    {3}", Key, Description, IsDefault?"(DEFAULT)":"", ToString(DefaultExample, false, true));
		}

		public abstract string ToString(Guid g, bool upcase=false, bool newline=false);

		/// <summary>
		/// Formats the given guid(s) in the output format using the byte order
		/// </summary>
		/// <param name="guider">Enumerator that returns guids to format</param>
		/// <param name="upcase">upper case output format</param>
		/// <param name="newline">append newline at the end of each formatted guid</param>
		/// <returns>formatted guids</returns>
		public virtual string ToString(IEnumerator<Guid> guider, bool upcase=false, bool newline=true)
		{
			System.Text.StringBuilder retVal = new System.Text.StringBuilder();
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
		/// <returns>formatted guids</returns>
		public virtual string ToString(IEnumerable<Guid> guids, bool upcase=false, bool newline=true)
		{
			return ToString(guids.GetEnumerator(), upcase, newline);
		}
	}
}
