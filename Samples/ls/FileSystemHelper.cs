//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.IO;

namespace ls
{
	public interface IFileSystemHelper
	{
		bool IsFile(string path);
	}

	class FileSystemHelper : IFileSystemHelper
	{
		public bool IsFile(string path)
		{
			if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
				return false;
			else
				return true;
		}
	}
}
