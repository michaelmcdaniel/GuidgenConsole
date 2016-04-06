using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	/// <summary>
	/// Abstract base class for formatting guids
	/// </summary>
	public abstract class BaseGuidFormat : IGuidFormatter
	{
		public readonly string DefaultFormat = System.Configuration.ConfigurationManager.AppSettings["default:output:format"]??"D";
		public readonly Guid DefaultExample = new Guid(System.Configuration.ConfigurationManager.AppSettings["default:example"]??"01020304-0506-0708-090a-0b0c0d0e0f10");

		/// <summary>
		/// Get the regular expression
		/// </summary>
		public virtual Regex Matcher { get; protected set; }
		
		/// <summary>
		/// Get whether or not the format can match a given string
		/// </summary>
		public bool CanMatch { get { return Matcher != null; } }

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
		/// Matches given string against matching format.  If no pattern exists, return false.
		/// </summary>
		/// <param name="s">string to match against</param>
		/// <returns>true if matching format</returns>
		public virtual bool IsMatch(string s)
		{
			if (Matcher == null) return false;
			return Matcher.IsMatch(s);
		}

		public virtual bool TryParse(string s, out Guid guid)
		{
			if (string.IsNullOrEmpty(s)) { guid = Guid.Empty;  return false; }
			Match m = Matcher.Match(s);
			if (m.Success)
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
				guid = new Guid(bytes);
			}
			else
			{
				guid = Guid.Empty;
			}
			return m.Success;
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
		public abstract string ToString(IEnumerator<Guid> guider, bool upcase=false, bool newline=true);
		public abstract string ToString(IEnumerable<Guid> guids, bool upcase=false, bool newline=true);
	}
}
