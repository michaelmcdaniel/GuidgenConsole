using System;
using System.Text.RegularExpressions;

namespace GuidGen.Formats
{
	public class IPAddressGuidFormat : BaseGuidSearchFormat
	{
		public static readonly Guid IPv4Loopback = ToGuid(System.Net.IPAddress.Loopback.GetAddressBytes());
		public static readonly Guid IPv6Loopback = ToGuid(System.Net.IPAddress.IPv6Loopback.GetAddressBytes());
		public static readonly Guid Empty = ToGuid(System.Net.IPAddress.None.GetAddressBytes());

		public IPAddressGuidFormat()
		{
			// pattern from: http://stackoverflow.com/questions/53497/regular-expression-that-matches-valid-ipv6-addresses
			string pattern = @"((([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])))|((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9]\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9]?[0-9]))";
			Matcher = new System.Text.RegularExpressions.Regex(pattern);
		}

		private static bool IsIPv4(byte[] bytes)
		{
			if (bytes.Length == 4) return true;
			if (bytes.Length != 16) throw new ArgumentOutOfRangeException("To many bytes");
			return 
				bytes[0] == 0 && bytes[1] == 0 && bytes[2] == 0 && bytes[3] == 0 && bytes[4] == 0 && bytes[5] == 0 &&
				bytes[6] == 0 && bytes[7] == 0 && bytes[8] == 0 && bytes[9] == 0 && bytes[10] == 0 && bytes[11] == 0;
		}

		public static bool IsIPv4(Guid g)
		{
			return IsIPv4(g.ToByteArray());
		}

		public static bool IsLoopBack(Guid g)
		{
			return g.Equals(IPv4Loopback) || g.Equals(IPv6Loopback);
		}

		public override string ToString(Guid g, bool upcase, bool newline)
		{
			if (IsLoopBack(g)) return "::1" + (newline?"\r\n":"");
			byte[] bytes = g.ToByteArray();
			if (IsIPv4(g)) return string.Format("{0}.{1}.{2}.{3}", bytes[12], bytes[13], bytes[14], bytes[15]) + (newline?"\r\n":"");
			return (new System.Net.IPAddress(bytes)).ToString() + (newline?"\r\n":"");
		}

		private static Guid ToGuid(byte[] bytes)
		{
			if (bytes.Length == 4) return new Guid(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, bytes[0], bytes[1], bytes[2], bytes[3] });
			if (bytes.Length != 16) throw new ArgumentOutOfRangeException("bytes", "buffer is not 16 bytes.");
			return new Guid(bytes);
		}

		public static System.Net.IPAddress ToIPAddress(Guid g)
		{
			byte[] bytes = g.ToByteArray();
			return new System.Net.IPAddress(!IsIPv4(bytes)? bytes : new byte[] { bytes[12], bytes[13], bytes[14], bytes[15] });
		}

		private static bool TryToGuid(byte[] bytes, out Guid guid)
		{
			if (bytes.Length == 4 || bytes.Length == 16)
			{
				guid = ToGuid(bytes);
				return true;
			}
			guid = Guid.Empty;
			return false;
		}

		public override bool TryParse(string s, out Guid guid)
		{
			System.Net.IPAddress ip;
			Guid tmp = Guid.Empty;
			bool retVal = (System.Net.IPAddress.TryParse(s, out ip) && TryToGuid(ip.GetAddressBytes(), out tmp));
			guid = tmp;
			return retVal;
		}

		protected override string Replace(string line, int offset, System.Text.RegularExpressions.Match m, Guid input, Guid output, bool upcase)
		{
			return line.Substring(0, m.Index - offset) + ToString(output, upcase, false) + line.Substring(m.Index - offset + m.Length);
		}

		protected override Guid MatchToGuid(Match m)
		{
			System.Net.IPAddress ip;
			Guid guid = Guid.Empty;
			if (System.Net.IPAddress.TryParse(m.Value, out ip)) TryToGuid(ip.GetAddressBytes(), out guid);
			return guid;
		}
	}
}
