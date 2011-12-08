//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.IO;
using System.Collections.Generic;

namespace ls
{
	public class ListFileSystem
	{
		readonly TextWriter _output;
		readonly string _root;
		public bool ShouldRecurseDirs { get; set; }
		public bool ShouldPrintRootDirName { get; set; }
		string _dirSuffix;

		public ListFileSystem(TextWriter output, string root)
		{
			if (output == null && string.IsNullOrEmpty(root))
				throw new ArgumentNullException();

			_output = output;
			_root = root;
		}

		protected ListFileSystem(TextWriter output, string root, string dirSuffix)
		{
			_output = output;
			_root = root;
			_dirSuffix = dirSuffix;
		}

		public void Run()
		{
			try
			{
				if (IsFile(_root))
					_output.WriteLine(_root);
				else
				{
					_dirSuffix = ObtainDirSuffix(_root);
					ListDirFromArg();
				}
			}
			catch (PathTooLongException)
			{
				_output.WriteLine("ls: cannot access {0}: No such file or directory", _root);
			}
			catch (FileNotFoundException)
			{
				_output.WriteLine("ls: cannot access {0}: No such file or directory", _root);
			}
			catch (DirectoryNotFoundException)
			{
				_output.WriteLine("ls: cannot access {0}: No such file or directory", _root);
			}
		}

		protected void ListDirFromArg()
		{
			var dir = new DirectoryInfo(_root);

			if (ShouldPrintRootDirName == true)
				PrintRootDirName();

			ListDir(dir);
		}

		protected void ListDir(DirectoryInfo dirPath)
		{
			FileSystemInfo[] entries = dirPath.GetFileSystemInfos();

			PrintContents(entries);

			if (ShouldRecurseDirs)
				RecursivelyListDirs(entries);
		}

		private void RecursivelyListDirs(IEnumerable<FileSystemInfo> entries)
		{
			if (entries == null) return;

			foreach (FileSystemInfo entry in entries)
				if ((entry.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
				{
					var entryAsDir = (DirectoryInfo) entry;
					PrintDirName(entryAsDir);
					ListDir(entryAsDir);
				}
		}

		private void PrintRootDirName()
		{
			_output.WriteLine("{0}:", _root);
		}

		private void PrintDirName(DirectoryInfo entry)
		{
			_output.WriteLine("\r\n{0}{1}{2}:", _dirSuffix, Path.DirectorySeparatorChar, entry.Name);
		}

		private void PrintContents(IEnumerable<FileSystemInfo> entries)
		{
			foreach (FileSystemInfo entry in entries)
				_output.WriteLine(entry.Name);
		}

		public bool IsFile(string path)
		{
			if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
				return false;
			else
				return true;
		}

		public string ObtainDirSuffix(string dir)
		{
			return dir.TrimEnd(Path.DirectorySeparatorChar);
		}

	}

	// NOTE: I think we need classes for:
	// 1. main class to obtain options
	// 2. special class to determine if initial path if a file or directory to then kick off file or directory listing
	// 3. a directory lister
	// 4. a file listser
	// 5. handle wild cards at the shell level, i.e. effectively seperate instances of the initial lister.
}
