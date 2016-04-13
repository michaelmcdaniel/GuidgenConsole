using System;

namespace GuidGen
{
	/// <summary>
	/// Stream reader to look for exit.
	/// </summary>
	public class ConsoleExitStream : System.IO.TextReader
	{
		bool _Done = false;
		public ConsoleExitStream()
		{
		}
#if NET2_0 || NET3_5 || NET4_0 || NET4_5 || NET4_6
		public override void Close()
		{
		}

		public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
		{
			return base.CreateObjRef(requestedType);
		}
#endif
		protected override void Dispose(bool disposing)
		{
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override int Peek()
		{
			if (!_Done) return 0;
			return -1;
		}

		public override int Read()
		{
			throw new NotImplementedException();
		}

		public override int Read(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		public override int ReadBlock(char[] buffer, int index, int count)
		{
			throw new NotImplementedException();
		}

		public override string ReadLine()
		{
			string line = Console.ReadLine();
			if (_Done = (line.Equals("exit", StringComparison.InvariantCultureIgnoreCase) || line.Equals("quit", StringComparison.InvariantCultureIgnoreCase))) return null;
			return line;
		}

		public override string ReadToEnd()
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return _Done.ToString();
		}
	}
}
