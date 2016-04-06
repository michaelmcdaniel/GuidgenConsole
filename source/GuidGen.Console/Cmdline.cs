using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GuidGen
{
	/// <summary>
	/// Class that helps process the command line arguments
	/// </summary>
	public static class Cmdline
	{
		private static List<string> _Options = new List<string>();
		private static List<Argument> _ArgumentsByIndex = new List<Argument>();
		private static Dictionary<string, Argument> _ArgumentsBySwitch = new Dictionary<string,Argument>(StringComparer.InvariantCultureIgnoreCase);

		/// <summary>
		/// Static constructor that preloads all the arguments from the command line.
		/// </summary>
		static Cmdline()
		{
			string[] args = System.Environment.GetCommandLineArgs();
			string cmdline = System.Environment.CommandLine;

			// start arguments need no key
			int index = 1;
			Argument empty = Argument.Empty;
			for (; index < args.Length; index++)
			{
				if (args[index][0] == '-' || args[index][0] == '/' || args[index][0] == '\\') break;
				_Options.Add(args[index]);
				empty.Values.Add(args[index]);
				_ArgumentsByIndex.Add(new Argument("", args[index]));
			}
			_ArgumentsBySwitch.Add(empty.Key, empty);

			Argument last = null;
			for (; index < args.Length; index++)
			{
				if (args[index][0] == '-' || args[index][0] == '/' || args[index][0] == '\\')
				{
					if (last != null)
					{
						_ArgumentsByIndex.Add(last);
						_ArgumentsBySwitch[last.Key] = last;
						last = null;
					}

					last = new Argument(args[index].Substring(1));
				}
				else if (last != null)
				{
					last.Values.Add(args[index]);
				}
			}
			if (last != null)
			{
				_ArgumentsByIndex.Add(last);
				_ArgumentsBySwitch[last.Key] = last;
				last = null;
			}
		}

		public static Argument Get(string name, params string[] defaultValue)
		{
			Argument retVal = null;
			if (!_ArgumentsBySwitch.TryGetValue(name, out retVal))
			{
				retVal = new Argument(name);
				if (defaultValue != null && defaultValue.Length > 0)
				{
					retVal.Values.AddRange(defaultValue);
				}
			}
			return retVal;
		}

		public static Argument Get(int index, params string[] defaultValue)
		{
			if (index >= 0 && index < _ArgumentsByIndex.Count) return _ArgumentsByIndex[index];
			return new Argument("", defaultValue);
		}

		public static bool Has(string name)
		{
			return _ArgumentsBySwitch.ContainsKey(name);
		}

		public static bool Has(int index)
		{
			return (index > 0 && index < _ArgumentsByIndex.Count);
		}

		/// <summary>
		/// Command line argument
		/// </summary>
		public class Argument
		{
			private string _Key = "";
			private List<string> _Values;

			public Argument()
			{
				_Values = new List<string>();
			}

			public Argument(string key, params string[] values)
			{
				_Key = key;
				_Values = new List<string>(values);
			}

			/// <summary>
			/// Gets whether or not a key is present
			/// </summary>
			public bool IsSwitch
			{
				get { return !string.IsNullOrEmpty(_Key); }
			}

			/// <summary>
			/// Get/set the key for the Argument
			/// </summary>
			public string Key
			{
				get { return _Key; }
				set { _Key = value; }
			}

			/// <summary>
			/// Generates hashcode from key
			/// </summary>
			/// <returns></returns>
			public override int GetHashCode()
			{
				return (Key??"").GetHashCode();
			}

			/// <summary>
			/// 
			/// </summary>
			/// <returns>Key=[comma separated values]</returns>
			public override string ToString()
			{
				return _Key + "=" + string.Join(", ", _Values.ToArray());
			}

			/// <summary>
			/// Get/set a list of values
			/// </summary>
			public List<string> Values
			{
				get { return _Values; }
				set { _Values = value; }
			}

			/// <summary>
			/// Gets the value of the Argument
			/// </summary>
			/// <remarks>
			/// At a minimum will return a empty string.  If the value is an array, will return arguments as space separated list.
			/// </remarks>
			public string Value
			{
				get
				{
					if (_Values.Count == 0) return "";
					if (_Values.Count == 1) return _Values[0]??"";
					return string.Join(" ", _Values.ToArray());
				}
			}

			/// <summary>
			/// Get a new empty argument
			/// </summary>
			public static Argument Empty
			{
				get { return new Argument(); }
			}
		}
	}
}
