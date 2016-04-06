using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuidGen
{
	public interface IGuidFormatter
	{
		string Key { get; set; }
		string Description { get; set; }
		bool CanMatch { get; }
		bool IsMatch(string s);
		string ToString(Guid g, bool upcase, bool newline);
		string ToString(IEnumerator<Guid> guider, bool upcase, bool newline);
		string ToString(IEnumerable<Guid> guids, bool upcase, bool newline);

		int GetHashCode();
		bool Equals(string s);

		bool TryParse(string s, out Guid guid);
	}
}
