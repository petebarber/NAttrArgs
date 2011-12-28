//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using NAttrArgs;

namespace Test
{
	class Program
	{
		[NArg(IsOptional = true, Rank=9, AltName = "OptFlag")]
		private bool m_optionalFlagPrivateVariable;

		[NArg(Rank = 1, AltName = "First")] 
		private string m_first;

		[NArg(Rank = 2)]
		public int Second { get; private set; }

		[NArg(Rank = 2, AltName = "ThirdAllowed", AllowedValues = new string[] {"foo", "bar", "baz"})] 
		private string m_third;

		[NArg(IsOptional = true, Rank = 0, AltName = "SomeInt", AllowedValues = new string[] {"7", "8", "9"})]
		private int m_someOptionalInt;

		[NArg(IsOptional = true, OptionalArgName = "Square")]
		private void SquareOptionalValue(double value)
		{
			m_squaredValue = value*value;
		}

		private double m_squaredValue = 0;

		[NArg(IsConsumeRemaining = true)] private string[] m_theRest;


		static void Main(string[] args)
		{
			try
			{
				new Program().Run(args);
			}
			catch (NArgException e)
			{
				Console.Error.WriteLine(e.Message);
			}
			catch  (Exception e)
			{
				Console.Error.WriteLine("Whoops:{0}", e.Message);
			}
		}

		private void Run(string[] args)
		{
			new ArgParser<Program>("Test").Parse(this, args);

			Console.WriteLine("m_optionalFlagPrivateVariable:{0}", m_optionalFlagPrivateVariable);
			Console.WriteLine("m_first:{0}", m_first);
			Console.WriteLine("Second:{0}", Second);
			Console.WriteLine("m_third:{0}", m_third);
			Console.WriteLine("m_someOptionalInt:{0}", m_someOptionalInt);
			Console.WriteLine("m_squaredValue:{0}", m_squaredValue);
			Console.Write("m_theRest:");
			if (m_theRest.Length == 0)
				Console.WriteLine("None");
			else
			{
				Console.Write(m_theRest[0]);

				for (int i = 1; i < m_theRest.Length; ++i)
					Console.Write(", {0}", m_theRest[i]);

				Console.WriteLine();
			}
				
		}
	}
}
