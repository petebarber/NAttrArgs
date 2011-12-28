//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ls;
using System.IO.Moles;
using Microsoft.Moles.Framework;
[assembly: MoledType(typeof(System.IO.File))]
[assembly: MoledType(typeof(System.IO.FileSystemInfo))]
[assembly: MoledType(typeof(System.IO.FileInfo))]
[assembly: MoledType(typeof(System.IO.DirectoryInfo))]

namespace TestMS.ls
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class ListFileSystemTest
	{

		[TestMethod]
		public void ListFileSystem_constructs_notnull()
		{
			var ls = new ListFileSystem(new StringWriter(), string.Empty);

			Assert.IsNotNull(ls);
		}

		[TestMethod]
		[HostType("Moles")]
		public void List_WhenFileDoesNotExist_DisplaysError()
		{
			var output = new StringWriter();

			const string targetFileName = @"L:\foo.txt";

			var ls = new ListFileSystem(output, targetFileName);

			MFile.GetAttributesString = (fileName) => { throw new FileNotFoundException(); };

			ls.Run();

			Assert.AreEqual("ls: cannot access L:\\foo.txt: No such file or directory\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void List_WhenPathIsTooLong_DisplaysError()
		{
			var output = new StringWriter();

			const string targetFileName = @"L:\thisistoolong";

			var ls = new ListFileSystem(output, targetFileName);

			MFile.GetAttributesString = (fileName) => { throw new PathTooLongException(); };

			ls.Run();

			Assert.AreEqual("ls: cannot access L:\\thisistoolong: No such file or directory\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void Run_WhenRelPathIsFile_JustNameIsListed()
		{
			var output = new StringWriter();

			var ls = new ListFileSystem(output, @"foo.txt");

			MFile.GetAttributesString = (fileName) => FileAttributes.Normal;

			ls.Run();

			Assert.AreEqual("foo.txt\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void Run_WhenAbsPathIsFile_AbsNameIsListed()
		{
			var output = new StringWriter();

			var ls = new ListFileSystem(output, @"L:\foo.txt");

			MFile.GetAttributesString = (fileName) => FileAttributes.Normal;

			ls.Run();

			Assert.AreEqual("L:\\foo.txt\r\n", output.ToString());
		}


		[TestMethod]
		[HostType("Moles")]
		public void Run_WhenRelPathUsingDotIsFile_RelNameIsListed()
		{
			var output = new StringWriter();

			var ls = new ListFileSystem(output, @".\foo.txt");

			MFile.GetAttributesString = (fileName) => FileAttributes.Normal;

			ls.Run();

			Assert.AreEqual(".\\foo.txt\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void List_WhenAbsDirDoesNotExist_DisplaysError()
		{
			StringWriter output = new StringWriter();

			const string targetFileName = @"L:\bar";

			ListFileSystem ls = new ListFileSystem(output, targetFileName);

			MFile.GetAttributesString = (fileName) => { throw new DirectoryNotFoundException(); };

			ls.Run();

			Assert.AreEqual("ls: cannot access L:\\bar: No such file or directory\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void List_WhenRelDirDoesNotExist_DisplaysError()
		{
			StringWriter output = new StringWriter();

			const string targetFileName = @"bar";

			ListFileSystem ls = new ListFileSystem(output, targetFileName);

			MFile.GetAttributesString = (fileName) => { throw new DirectoryNotFoundException(); };

			ls.Run();

			Assert.AreEqual("ls: cannot access bar: No such file or directory\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void ListDir_WhenAbsNonEmptyNonDirectory_OnlyContentsListedNoDir()
		{
			ListDir_NonEmptyDirectory_ContentsListed("baz");
		}

		[TestMethod]
		[HostType("Moles")]
		public void ListDir_WhenRelNonEmptyDirectory_onlyContentsListedNoDir()
		{
			ListDir_NonEmptyDirectory_ContentsListed(@"L:\baz");
		}

		public void ListDir_NonEmptyDirectory_ContentsListed(string dirPath)
		{
			var output = new StringWriter();

			var ls = new TestableListFileSystem(output, string.Empty);

			var pathLBazStub = new MDirectoryInfo();
			new MFileSystemInfo(pathLBazStub) { FullNameGet = () => dirPath };

			var pathLBazStubContents = new FileSystemInfo[] 
			{ 
				new MFileInfo() { NameGet = () => @"a.txt" },
				new MFileInfo() { NameGet = () => @"b.txt" },
				new MFileInfo() { NameGet = () => @"c.txt" }
			};

			pathLBazStub.GetFileSystemInfos = () => pathLBazStubContents;

			ls.TestListDir(pathLBazStub);

			Assert.AreEqual("a.txt\r\nb.txt\r\nc.txt\r\n", output.ToString());
		}

		[TestMethod]
		[HostType("Moles")]
		public void ListDir_WhenAbsNonEmptyDirIsListedRecursive_ContentsAndAbsDirShownAndRecursionContinues()
		{
			var pathLBazStub = new MDirectoryInfo();
			new MFileSystemInfo(pathLBazStub) { FullNameGet = () => @"L:\baz" };

			var pathLBazFooStubContents = new FileSystemInfo[] 
			{ 
			};


			var pathLBazStubContents = new FileSystemInfo[] 
			{ 
				new MFileInfo() { NameGet = () => @"a.txt" },
				new MFileInfo() { NameGet = () => @"b.txt" },
				new MFileInfo() { NameGet = () => @"c.txt" },
				new MDirectoryInfo() { NameGet = () => @"foo", GetFileSystemInfos = () => pathLBazFooStubContents }
			};

			new MFileSystemInfo(pathLBazStubContents[3]) { AttributesGet = () => FileAttributes.Directory, FullNameGet = () => @"L:\baz\foo" };

			new MFileSystemInfo(pathLBazStubContents[0]) { AttributesGet = () => FileAttributes.Normal };
			new MFileSystemInfo(pathLBazStubContents[1]) { AttributesGet = () => FileAttributes.Normal };
			new MFileSystemInfo(pathLBazStubContents[2]) { AttributesGet = () => FileAttributes.Normal };

			MDirectoryInfo.AllInstances.GetFileSystemInfos = (x) => new FileSystemInfo[0];

			pathLBazStub.GetFileSystemInfos = () => pathLBazStubContents;

			var outputString = new StringWriter();
			new TestableListFileSystem(output: outputString, dirSuffix: @"L:\baz") { ShouldRecurseDirs = true }.TestListDir(pathLBazStub);

			Assert.AreEqual("a.txt\r\nb.txt\r\nc.txt\r\nfoo\r\n\r\nL:\\baz\\foo:\r\n", outputString.ToString());
		}

		[TestMethod]
		public void ObtainSuffix_DriveWithTrailingPathSep_Drive()
		{
			Assert.AreEqual(@"L:", new TestableListFileSystem().ObtainDirSuffix(@"L:\"));
		}

		[TestMethod]
		public void ObtainSuffix_DriveWithNoPathSep_Drive()
		{
			Assert.AreEqual(@"L:", new TestableListFileSystem().ObtainDirSuffix(@"L:"));
		}

		[TestMethod]
		public void ObtainSuffix_RelPathWithTrailingSep_RelPathWithoutSep()
		{
			Assert.AreEqual(@"..\..\foo", new TestableListFileSystem().ObtainDirSuffix(@"..\..\foo\"));
		}

		[TestMethod]
		public void ObtainSuffix_RelPathWithoutTrailingSep_RelPathWithoutSep()
		{
			Assert.AreEqual(@"..\..\foo", new TestableListFileSystem().ObtainDirSuffix(@"..\..\foo"));
		}

		[TestMethod]
		[HostType("Moles")]
		public void ListDirFromArg_WhenPrintRootDirNameTrue_PrintsRootDir()
		{
			MDirectoryInfo.AllInstances.GetFileSystemInfos = x => new FileSystemInfo[] {};

			var output = new StringWriter();

			var ls = new TestableListFileSystem(output, @"L:\baz", string.Empty) { ShouldPrintRootDirName = true };

			ls.TestListDirFromArg();

			Assert.AreEqual("L:\\baz:\r\n", output.ToString());			
		}
	}
}
