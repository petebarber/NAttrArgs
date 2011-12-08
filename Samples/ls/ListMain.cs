//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.Collections.Generic;
using System.Linq;
using NAttrArgs;

namespace ls
{
	public class ListMain
	{
		[NArg(IsOptional = true, AltName = "R")]
		public bool IsListRecursive { get; private set; }

		[NArg(IsConsumeRemaining = true, AltName = "File")]
		public IEnumerable<string> Files { get; set; }

		static void Main(string[] args)
		{
			try
			{
				new ListMain(args).Run();
			}
			catch (NArgException e)
			{
				Console.Error.WriteLine(e.Message);
			}
			catch (Exception e)
			{
				Console.Error.WriteLine("Whoops:{0}", e.Message);
			}			
		}

		public ListMain(string[] args)
		{
			new NAttrArgs.ArgParser<ListMain>("ls").Parse(this, args);

			if (Files.Count() == 0) Files = new string[] {"."};
		}

		public void Run()
		{
			foreach (var rootDir in Files)
				new ListFileSystem(Console.Out, rootDir) { ShouldRecurseDirs = IsListRecursive, ShouldPrintRootDirName = Files.Count() > 1 }.Run();
		}
	}

}
