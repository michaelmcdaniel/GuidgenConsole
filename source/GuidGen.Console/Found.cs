using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
		public Guid[] Guids { get; set; }

		/// <summary>
		/// Get/set the line number of the found Guids
		/// </summary>
		public int Line { get; set; }
	}
}
