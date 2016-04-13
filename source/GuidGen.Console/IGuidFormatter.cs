using System;
using System.Collections.Generic;

namespace GuidGen
{
	public interface IGuidFormatter
	{
		string Key { get; set; }
		string Description { get; set; }
		string ToString(Guid g, bool upcase, bool newline);
		string ToString(IEnumerator<Guid> guider, bool upcase, bool newline);
		string ToString(IEnumerable<Guid> guids, bool upcase, bool newline);

		bool IsDefault { get; }

		int GetHashCode();
		bool Equals(string s);
	}
}
