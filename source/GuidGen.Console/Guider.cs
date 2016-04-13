using System;
using System.Collections.Generic;

namespace GuidGen
{
	/// <summary>
	/// Class that generates Guids
	/// </summary>
	public class Guider : IEnumerator<Guid>
	{
#if NET2_0
		public delegate Guid CreatorFunc();
		private CreatorFunc _Creator = null;
#else
		private Func<Guid> _Creator = null;
#endif
		private int _Count = -1;
		private int _Index = 0;
		private Guid _Current = Guid.Empty;

		public Guider(int count = -1)
		{
			_Creator = delegate() { return _Current; };
			_Count = count;
		}

#if NET2_0
		public Guider(CreatorFunc creator, int count=-1)
		{
			_Creator = creator;
			_Count = count;
		}
#else
		public Guider(Func<Guid> creator, int count=1)
		{
			_Creator = creator;
			_Count = count;
		}
#endif

		public IEnumerator<Guid> GetEnumerator()
		{
			return this;
		}

		/// <summary>
		/// Get/set the number of Guids to generate
		/// </summary>
		public int Count
		{
			get { return _Count; }
			set { _Count = value; }
		}

		/// <summary>
		/// Get the current index
		/// </summary>
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

		public static Guider FromType(string type, Guider defaultValue)
		{
			Guider retVal = defaultValue;
			switch((type??"").ToLower())
			{
				case "z": retVal = Guider.NewZeroGuid; break;
				case "s": retVal = Guider.NewSequentialGuid; break;
				case "g": retVal = Guider.NewGuid; break;
			}
			return retVal;
		}
	}
}
