//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.IO;
using ls;

namespace TestMS.ls
{
	public class TestableListFileSystem : ListFileSystem
	{
		public TestableListFileSystem(TextWriter output, string root)
			: base(output, root)
		{

		}

		public TestableListFileSystem(TextWriter output = null, string root = null, string dirSuffix = null)
			: base(output, root, dirSuffix)
		{

		}

		public void TestListDirFromArg()
		{
			base.ListDirFromArg();
		}

		public void TestListDir(DirectoryInfo dirPath)
		{
			base.ListDir(dirPath);
		}
	}
}
