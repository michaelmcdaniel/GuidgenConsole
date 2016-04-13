using System;
using System.Runtime.InteropServices;

namespace GuidGen
{

	/// <summary>
	/// Helper class to help deal with input/output/error redirection from console
	/// </summary>
	public static class ConsoleEx 
	{    
		public static bool OutputRedirected { get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdout)); }}    
		public static bool InputRedirected { get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdin)); }}    
		public static bool ErrorRedirected { get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stderr)); }}    
		
		// P/Invoke:    
		private enum FileType { Unknown, Disk, Char, Pipe };    
		private enum StdHandle { Stdin = -10, Stdout = -11, Stderr = -12 };    
		[DllImport("kernel32.dll")]    
		private static extern FileType GetFileType(IntPtr hdl);    
		[DllImport("kernel32.dll")]    
		private static extern IntPtr GetStdHandle(StdHandle std);
	}
}
