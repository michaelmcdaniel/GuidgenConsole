using System;
using System.Text;

namespace GuidGen
{
	public class ConsoleClipboardOut : System.IO.TextWriter
	{
		public override Encoding Encoding
		{
			get
			{
				return Console.Out.Encoding;
			}
		}

		public override void WriteLine(string value)
		{
			Tools.SetClipboard(value);
			Console.Out.WriteLine(value);
		}

		public override void Write(string value)
		{
			//Tools.SetClipboard(value.EndsWith("\r\n")?value.Substring(0, value.Length-2):value);
			Console.Out.Write(value);
		}

		public override void Write(char value)
		{
			Console.Out.Write(value);
		}

		public override void Flush()
		{
			Console.Out.Flush();
		}
	}
}
