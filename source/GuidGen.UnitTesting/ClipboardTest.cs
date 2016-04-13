using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GuidGen.UnitTesting
{
	[TestClass]
	public class ClipboardTest
	{
		[TestMethod]
		public void TestClipboard()
		{
			string s = Guid.NewGuid().ToString("D");
			Tools.SetClipboard(s);
			Assert.AreEqual(s, Tools.GetClipboard());
		}
	}
}
