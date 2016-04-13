using System;

namespace GuidGen
{
	public struct Replacement
	{
		/// <summary>
		/// Get/Set the matching text
		/// </summary>
		public string FoundText { get; set; }

		/// <summary>
		/// get/set the text that was replaced
		/// </summary>
		public string ReplacedByText { get; set; }

		/// <summary>
		/// get/set the guid that was found
		/// </summary>
		public Guid FoundGuid { get; set; }

		/// <summary>
		/// get/set the guid that replaced the original
		/// </summary>
		public Guid ReplacedByGuid { get; set; }

		/// <summary>
		/// Get/set the line number of the found Guids
		/// </summary>
		public int Line { get; set; }

		/// <summary>
		/// Get/set the original column that the match was found
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Get whether or not the found value was reformatted
		/// </summary>
		public bool WasReformat { get { return ReplacedByText != FoundText; } }

		/// <summary>
		/// Get whether or not the found guid was replaced by a new guid
		/// </summary>
		public bool WasReplacement { get { return FoundGuid == ReplacedByGuid; } }

		public override string ToString()
		{
			if (WasReplacement || WasReformat) return string.Format("{0} -> {1}", FoundText, ReplacedByText);
			return string.Format("{0}", FoundText);
		}
	}

}
