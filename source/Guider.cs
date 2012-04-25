using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidGen
{
	public class Guider : IEnumerator<Guid>
	{


		private Func<Guid> _Creator = null;
		private int _Count = -1;
		private int _Index = 0;
		private Guid _Current = Guid.Empty;

		public Guider(int count = 1)
		{
			_Creator = delegate() { return _Current; };
			_Count = count;
		}

		public Guider(Func<Guid> creator, int count=1)
		{
			_Creator = creator;
			_Count = count;
		}


		public IEnumerator<Guid> GetEnumerator()
		{
			return this;
		}

		public int Count
		{
			get { return _Count; }
			set { _Count = value; }
		}

		public int Index
		{
			get { return _Index; }
		}

		public Guid Current
		{
			get { return _Creator(); }
		}

		public void Dispose()
		{
			Reset();
		}

		object System.Collections.IEnumerator.Current
		{
			get { return _Creator(); }
		}

		public bool MoveNext()
		{
			return MoveNext(Guid.Empty);
		}

		public bool MoveNext(Guid current)
		{
			_Current = current;
			bool retVal = _Count<0||_Index<_Count;
			_Index++;
			return retVal;
		}

		public void Reset()
		{
			_Index=0;
		}


		public static Guider NewGuid
		{
			get { return new Guider(delegate() { return Guid.NewGuid(); }, -1); }
		}

		public static Guider NewSequentialGuid
		{
			get { return new Guider(delegate() { return Tools.NewSequentialGuid(); }, -1); }
		}

		public static Guider NewZeroGuid
		{
			get { return new Guider(delegate() { return Guid.Empty; }, -1); }
		}

		public static Guider AsGuid(Guid guid)
		{
			return new Guider(delegate() { return guid; }, -1);
		}

		public static Guider AsCurrent()
		{
			return new Guider();
		}
	}
}
