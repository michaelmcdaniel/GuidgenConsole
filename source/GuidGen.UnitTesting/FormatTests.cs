using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuidGen.UnitTesting
{
	[TestClass]
	public class FormatTests
	{
		[TestMethod]
		public void TestFormats()
		{
			TestFormats(new Guid("a388a92e-451d-4c29-a481-b61804cc7909"));
			TestFormats(new Guid("00000002-0003-0000-0400-000001000000"));
			TestFormats(Guid.Empty);
			TestFormats(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
		}

		[TestMethod]
		public void TestMD5()
		{
			IEnumerable<Formats.MD5OutputFormat> formatters = GuidGen.GuidFormats.AvailableFormats.Where(f => f.Key.StartsWith("MD5")).Cast<Formats.MD5OutputFormat>();
			foreach (var format in formatters)
			{
				Guid guid1, guid2;
				format.TryParse("Just for fun", out guid1);
				format.TryParse("Just for fun", out guid2);
				Assert.AreEqual(guid1, guid2);
				IGuidFormatter internalFormat = GuidGen.GuidFormats.GetFormatter(format.Key.Substring(4));
				if (!(internalFormat is Formats.VersionGuidFormat)) // if I had the time to find a string that MD5s to a valid version, then we'd use it.  
				{
					Assert.AreEqual(internalFormat.ToString(guid1, false, false), format.ToString(guid1, false, false));
				}
				format.TryParse("Not for fun", out guid2);
				Assert.AreNotEqual(guid1, guid2);

			}
		}

		private void TestFormats(Guid guid)
		{
			int defaults = 0;
			foreach(var format in GuidGen.GuidFormats.AvailableFormats.Where(f=>!f.Key.StartsWith("MD5")))
			{
				Assert.IsFalse(string.IsNullOrEmpty(format.Key));
				Assert.IsFalse(string.IsNullOrEmpty(format.Description));
				Assert.IsTrue(format.Equals(format.Key));
				Assert.IsFalse(format.Equals(null));
				Assert.IsFalse(format.Equals((object)"BOO!!!"));
				Assert.IsTrue(format.Equals((object)format.Key));
				Assert.AreNotEqual(0, format.GetHashCode());
				if (format.IsDefault) defaults++;

				if (format is IValid && !((IValid)format).IsValid(guid)) { System.Diagnostics.Debug.WriteLine("Invalid Guid(\"" + guid.ToString("D") + "\") for format: " + format.Key); continue; }
				Assert.IsFalse(string.IsNullOrEmpty(format.ToString()));
				string formatted = format.ToString(guid, false, false);
				Assert.IsFalse(string.IsNullOrWhiteSpace(formatted));
				if (format is IGuidSearcher)
				{
					Guid parsed;
					Assert.IsTrue(((IGuidSearcher)format).TryParse(formatted, out parsed));
					Assert.AreEqual(guid, parsed);
				}
				List<Guid> guids = new List<Guid>(new Guid[] { guid });
				formatted = format.ToString(guids, false, false);
				if (format is IGuidSearcher)
				{
					Guid parsed;
					Assert.IsTrue(((IGuidSearcher)format).TryParse(formatted, out parsed));
					Assert.AreEqual(guid, parsed);
				}
				formatted = format.ToString(guids.GetEnumerator(), false, false);
				if (format is IGuidSearcher)
				{
					Guid parsed;
					Assert.IsTrue(((IGuidSearcher)format).TryParse(formatted, out parsed));
					Assert.AreEqual(guid, parsed);
				}

			}
			Assert.AreEqual(1, defaults);
		}

		[TestMethod]
		public void TestReplace()
		{
			TestReplace(new Guid("a388a92e-451d-4c29-a481-b61804cc7909"));
			TestReplace(new Guid("00000002-0003-0000-0400-000001000000"));
			TestReplace(Guid.Empty);
			TestReplace(new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff"));
		}

		private static void TestReplace(Guid guid)
		{
			List<IGuidFormatter> formatters = new List<IGuidFormatter>(GuidFormats.AvailableFormats.Where(f=>!f.Key.StartsWith("MD5")));
			Guider rgn = Guider.NewGuid;
			Guider rgs = Guider.NewSequentialGuid;
			Guider rgz = Guider.NewZeroGuid;
			for(int i = 0; i < formatters.Count; i++)
			{
				IGuidFormatter format = formatters[i];
				if (format is IValid && !((IValid)format).IsValid(guid)) { System.Diagnostics.Debug.WriteLine("Invalid Guid(\"" + guid.ToString("D") + "\") for format: " + format.Key); continue; }
				string expected = format.ToString(guid, false, false);
				foreach(IGuidSearcher searcher in from f in formatters where f is IGuidSearcher select f)
				{
					Assert.IsTrue(searcher.CanMatch);
					if (searcher is IValid && !((IValid)searcher).IsValid(guid)) { System.Diagnostics.Debug.WriteLine("Invalid Guid(\"" + guid.ToString("D") + "\") for format: " + format.Key); continue; }
					string input = ((IGuidFormatter)searcher).ToString(guid, false, false);
					Assert.IsTrue(searcher.IsMatch(input));
					string actual = searcher.Replace(input, Guider.AsCurrent(), format, false, (r)=> { });
					Assert.AreEqual(expected, actual);

					actual = searcher.Replace(input, Guider.AsCurrent(), null, false, (r)=> { });
					Assert.AreEqual(input, actual);

					try
					{
						actual = searcher.Replace(input, rgn, format, false, (r)=> { });
						Assert.AreNotEqual(input, actual);
					}
					catch(ArgumentOutOfRangeException)
					{
						if (!format.Equals("Version")) Assert.Fail();
					}

					try
					{
						actual = searcher.Replace(input, rgs, format, false, (r)=> { });
						Assert.AreNotEqual(input, actual);
					}
					catch(ArgumentOutOfRangeException)
					{
						if (!format.Equals("Version")) Assert.Fail();
					}

					actual = searcher.Replace(input, rgz, format, false, (r)=> { });
					if (rgz.Current == guid)
					{
						Assert.AreEqual(format.ToString(guid, false, false), actual);
					}
					else
					{
						Assert.AreNotEqual(input, actual);
					}

				}
			}
		}

		private string Split(string s, int at, string chars)
		{
			StringBuilder output = new StringBuilder(s.Length + ((s.Length/at)*chars.Length) + 16);
			output.Append(s);
			for(int i = at; i < output.Length; i += (at + chars.Length)) output.Insert(i, chars);
			return output.ToString();
		}

		[TestMethod]
		public void TestBase64Combined()
		{
			GuidGen.Formats.Base64CombinedFormat b64c = new Formats.Base64CombinedFormat();
			Guider g = Guider.NewSequentialGuid; g.MoveNext();
			List<Guid> guids = new List<Guid>();
			for(int i = 0; i < 20; i++, g.MoveNext()) { guids.Add(g.Current); }
			string text = b64c.ToString(guids);
			IEnumerable<Found> found = b64c.Find(text);
			int ci = 0;
			foreach(var f in found) Assert.AreEqual(guids[ci++], f.Guid);

			string split = Split(text, 78, "\r\n");
			Assert.AreNotEqual(text,split);

			found = b64c.Find(split);
			ci = 0;
			foreach(var f in found) Assert.AreEqual(guids[ci++], f.Guid);

			IGuidFormatter formatter = GuidGen.GuidFormats.GetFormatter("N");
			string toMatch = formatter.ToString(guids, false, true);
			List<Replacement> replaced = new List<Replacement>();
			string s = b64c.Replace(text + "\r\n", Guider.AsCurrent(), formatter, false, (r)=> { replaced.Add(r); });
			Assert.AreEqual(toMatch, s);

			s = b64c.Replace(split + "\r\n", Guider.AsCurrent(), formatter, false, (r)=> { replaced.Add(r); });
			Assert.AreEqual(toMatch, s);

		}

		[TestMethod]
		public void TestIP()
		{
			string iptext = "127.0.0.1";
			Guid g = new Guid("00000000-0000-0000-0000-00007F000001");
			Guid g2 = new Guid("00000000-0000-0000-0000-000000000001");

			GuidGen.Formats.IPAddressGuidFormat f = new Formats.IPAddressGuidFormat();
			Guid og;
			Assert.IsTrue(f.TryParse(iptext, out og));
			Assert.AreEqual(g, og);
			Assert.AreEqual("::1", f.ToString(og, false, false));
			Assert.IsTrue(f.TryParse("::1", out og));
			Assert.AreNotEqual(g, og);
			Assert.AreEqual(g2, og);

			iptext = "192.168.0.1";
			g = new Guid("00000000-0000-0000-0000-0000c0a80001");

			Assert.IsTrue(f.TryParse(iptext, out og));
			Assert.AreEqual(g, og);
			Assert.AreEqual("192.168.0.1", f.ToString(og, false, false));
			Assert.AreEqual((new Version("192.168.0.1")).ToString(), GuidGen.Formats.IPAddressGuidFormat.ToIPAddress(g).ToString());
		}

		
		[TestMethod]
		public void TestVersion()
		{
			string text = "2.3.4.1";
			Guid g = new Guid("00000002-0003-0000-0400-000001000000");
			Guid invalid = new Guid("a388a92e-451d-4c29-a481-b61804cc7909");

			GuidGen.Formats.VersionGuidFormat f = new Formats.VersionGuidFormat();
			Assert.IsTrue(f.IsValid(g));
			Assert.IsFalse(f.IsValid(invalid));
			Guid og;
			Assert.IsTrue(f.TryParse(text, out og));
			Assert.AreEqual(g, og);
			Assert.AreEqual("2.3.4.1", f.ToString(og, false, false));
			
		}


		[TestMethod]
		public void TestSearchFormat()
		{
			GuidGen.Formats.SearchFormat sf = new Formats.SearchFormat();
			Guid g = new Guid("4745435A-B95C-4A8A-AC12-BB5983C2110A");
			string s = GuidGen.GuidFormats.Format("N", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("D", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("P", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("B", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = "0x" + GuidGen.GuidFormats.Format("H", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("HVB", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("HC#", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("HLDAP", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("C", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
			s = GuidGen.GuidFormats.Format("CP", g, false, false);
			Assert.AreEqual(s, sf.Find(s).First().Match);
		}
	}
}
