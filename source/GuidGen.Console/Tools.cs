using System;
using System.Runtime.InteropServices;

namespace GuidGen
{
	/// <summary>
	/// Class Helper
	/// </summary>
	public static class Tools
	{
		/// <summary>
		/// Generic convert function 
		/// </summary>
		/// <typeparam name="T">To Type</typeparam>
		/// <typeparam name="F">From Type</typeparam>
		/// <param name="val">Input value</param>
		/// <param name="defaultValue">Default value if it can't convert.</param>
		/// <param name="defaultOnException">Return default value if exception. (EAT Exception)</param>
		/// <returns></returns>
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

		[DllImport("rpcrt4.dll", SetLastError = true)]
		private static extern int UuidCreateSequential(out Guid guid);

		/// <summary>
		/// Returns a System generated sequential Guid via rpcrt4::UuidCreateSequential
		/// </summary>
		/// <returns></returns>
		public static Guid NewSequentialGuid()
		{
			const int RPC_S_OK = 0;
			Guid g;
			return (UuidCreateSequential(out g) == RPC_S_OK) ? g : Guid.NewGuid();
		}

		/// <summary>
		/// Sets text to clipboard as unicode
		/// </summary>
		/// <remarks>Uses win32 (because clipboard missing in clrcore - version rc1)</remarks>
		/// <param name="text">The text to put on the clipboard</param>
		public static bool SetClipboard(string text)
		{
			System.Windows.Forms.Clipboard.SetText(text);
			return true;
		}

		/// <summary>
		/// gets text (unicode or ansi) from clipboard
		/// </summary>
		/// <remarks>Uses win32 (because clipboard missing in clrcore - version rc1)</remarks>
		/// <returns>text if string otherwise null</returns>
		public static string GetClipboard()
		{
			if (!System.Windows.Forms.Clipboard.ContainsText()) return null;
			return System.Windows.Forms.Clipboard.GetText();
		}

	}
}
