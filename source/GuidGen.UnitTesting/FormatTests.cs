using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuidGen.UnitTesting
{
	[TestClass]
	public class FormatTests
	{
		[TestMethod]
		public void TestFormats()
		{
			Guid guid = new Guid("a388a92e-451d-4c29-a481-b61804cc7909");
			foreach(var format in GuidGen.GuidFormats.AvailableFormats)
			{
				string formatted = format.ToString(guid, false, false);
				Guid parsed;
				Assert.IsTrue(format.TryParse(formatted, out parsed));
				Assert.AreEqual(guid, parsed);
			}
		}
	}
}
