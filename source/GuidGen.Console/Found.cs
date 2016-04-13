using System;

namespace GuidGen
{
	/// <summary>
	/// Model class to hold found guids
	/// </summary>
	public class Found
	{
		/// <summary>
		/// Get/Set the matching line
		/// </summary>
		public string Match { get; set; }

		/// <summary>
		/// The Guids that were found
		/// </summary>
		public Guid Guid { get; set; }

		/// <summary>
		/// Get/set the line number of the found Guids
		/// </summary>
		public int Line { get; set; }
		
		/// <summary>
		/// Get/set the original column that the match was found
		/// </summary>
		public int Column { get; set; }

		public override string ToString()
		{
			return Match;
		}

		public string ToString(bool includeLine, IGuidFormatter formatter = null, bool upcase = false)
		{
			if (includeLine && formatter != null) return string.Format("Ln: {0} Col: {1}\t{2}\t{3}", Line, Column, formatter.ToString(Guid, upcase, false), Match);
			else if (includeLine) return string.Format("Ln: {0} Col: {1}", Line, Column, Match);
			else if (formatter != null) return string.Format("{0}\t{1}", formatter.ToString(Guid, upcase, false), Match);
			return Match;
		}
	}
}
