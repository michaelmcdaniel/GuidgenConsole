using System;
using System.Collections.Generic;

namespace GuidGen
{
	public interface IGuidSearcher
	{
		string Key { get; }
		bool CanMatch { get; }
		bool IsMatch(string s);
		bool TryParse(string s, out Guid guid);

		IEnumerable<Found> Find(string s, int line=-1);

		string Replace(string input, Guider guider, IGuidFormatter formatter=null, bool upcase = false, Action<Replacement> onReplacement = null, int lineNumber = -1);
	}
}
