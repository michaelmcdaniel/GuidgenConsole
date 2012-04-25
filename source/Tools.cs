using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuidGen
{
	public static class Tools
	{
		public static T Convert<T, F>(F val, T defaultValue, bool defaultOnException=true)
		{
			if (val == null) return defaultValue;
			System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
			if (converter.CanConvertFrom(typeof(F)))
			{
				try
				{
					return (T)converter.ConvertFrom(val);
				}
				catch (Exception) 
				{ 
					if (defaultOnException)	return defaultValue; 
					throw;
				}
			}
			converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(F));
			if (converter.CanConvertTo(typeof(T)))
			{
				try
				{
					return (T)converter.ConvertTo(val, typeof(T));
				}
				catch (Exception) 
				{
					if (defaultOnException) return defaultValue;
					throw;
				}
			}

			return defaultValue;
		}

		[System.Runtime.InteropServices.DllImport("rpcrt4.dll", SetLastError = true)]
		private static extern int UuidCreateSequential(out Guid guid);
		public static Guid NewSequentialGuid()
		{
			const int RPC_S_OK = 0;
			Guid g;
			return (UuidCreateSequential(out g) == RPC_S_OK) ? g : Guid.NewGuid();
		}
	}
}
