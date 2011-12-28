//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ls;
using NAttrArgs;

namespace TestMS.ls
{
	[TestClass]
	public class ListMainTest
	{
		private readonly string[] m_emptyArgs = new string[] { };

		[TestMethod]
		public void ListMainCons_CanConstruct_Yes()
		{
			Assert.IsNotNull(new ListMain(m_emptyArgs));
		}

		[TestMethod]
		public void ListMainCons_AllOptionsSet_Yes()
		{
			var ls = new ListMain(new string[] { "-R" });

			Assert.AreEqual(true, ls.IsListRecursive);
		}

		[TestMethod]
		[ExpectedException(typeof(NArgException))]
		public void ListMain_UsageException_AsExpected()
		{
			var ls = new ListMain(new string[] { "-BadArg" });
		}

		[TestMethod]
		public void ListMain_NoArgs_FileToListIsDot()
		{
			var ls = new ListMain(m_emptyArgs);

			Assert.AreEqual(1, ls.Files.Count());
			Assert.AreEqual(".", ls.Files.First());
		}

		[TestMethod]
		public void ListMain_When123SpecifiedToList_FilesAre123()
		{
			var ls = new ListMain(new string[] { "1", "2", "3"});

			Assert.AreEqual(3, ls.Files.Count());
			Assert.AreEqual("1", ls.Files.ElementAt(0));
			Assert.AreEqual("2", ls.Files.ElementAt(1));
			Assert.AreEqual("3", ls.Files.ElementAt(2));			
		}
	}
}
