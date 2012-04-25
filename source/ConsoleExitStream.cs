using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidGen
{
	public class ConsoleExitStream : System.IO.TextReader
	{
		bool _Done = false;
		public ConsoleExitStream()
		{
		}

		public override void Close()
		{
		}

		public override System.Runtime.Remoting.ObjRef CreateObjRef(Type requestedType)
		{
			return base.CreateObjRef(requestedType);
		}

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
			//StringBuilder retVal = new StringBuilder();
			//string line;
			//while((line = ReadLine()) != null) retVal.AppendLine(line);
			//return retVal.ToString();
		}

		public override string ToString()
		{
			return ReadToEnd();
		}
	}
}
