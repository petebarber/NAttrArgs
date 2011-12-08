//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Collections.Generic;
using NUnit.Framework;

namespace NAttrArgs.Test
{
	internal class TestClassWithNoNArgAttributes
	{
	}

	internal class TestClassWithIntSampleArgument
	{
		[NArg(Rank = 1)]
		private int SampleArgument;

		public int SampleArgumentValue { get { return SampleArgument; } }
	}

	internal class TestClassWithIntArgumentWithRequiredValues
	{
		[NArg(Rank = 1, AllowedValues=new string[] { "1", "2", "3"})]
		private int SampleArgument;

		public int SampleArgumentValue { get { return SampleArgument; } }
	}

	internal class TestClassWithOneOptional
	{
		[NArg(IsOptional = true)]
		private bool SetSomething;

        public bool SetSomethingValue { get { return SetSomething; } }
	}

	internal class TestClassWithOneOptionalWithAllowedValues
	{
		// Not a valid combination
		[NArg(IsOptional = true, AllowedValues=new string[] { "1", "2", "3" })]
		private bool SetSomething;

		public bool SetSomethingValue { get { return SetSomething; } }
	}

	internal class TestClassWithOneOptionalWithArgument
	{
		[NArg(IsOptional = true, OptionalArgName = "test")]
		private int SetSomething;

		public int SetSomethingValue { get { return SetSomething; } }

	}

	internal class TestClassWithTwoOptionalWithArgument
	{
		[NArg(IsOptional = true, OptionalArgName = "test")]
		private int SetSomething;

		public int SetSomethingValue { get { return SetSomething; } }

		[NArg(IsOptional = true, OptionalArgName = "test1")]
		private int SetSomethingElse;

		public int SetSomethingElseValue { get { return SetSomethingElse; } }

	}

	internal class TestClassWithMandatoryAndFlagArguments
	{
		[NArg(IsOptional = true)]
		private bool SetSomething;

		[NArg]
		private int SampleArgument;
	}

	internal class TestClassWithRequiredRankedArguments
	{
		// Prefixed with non-order characters to avoid implicit sorting bias

		[NArg(Rank = 4)]
		private int CRank4;

		[NArg(Rank = 1)]
		private int PRank1;

		[NArg(Rank = 3)]
		private int QRank3;

		[NArg(Rank = 2)]
		private int ZRank2;
	}

	internal class TestClassWithOptionalRankedArguments
	{
		// Prefixed with non-order characters to avoid implicit sorting bias

		[NArg(Rank = 4, IsOptional = true)]
		private int COptRank4;

		[NArg(Rank = 1, IsOptional = true)]
		private int POptRank1;

		[NArg(Rank = 3, IsOptional = true)]
		private int QOptRank3;

		[NArg(Rank = 2, IsOptional = true)]
		private int ZOptRank2;
	}

	internal class TestClassWithRankedRequiredAndOptionalArguments
	{
		[NArg(Rank = 4)]
		private int CRank4;

		[NArg(Rank = 1)]
		private int PRank1;

		[NArg(Rank = 3)]
		private int QRank3;

		[NArg(Rank = 2)]
		private int ZRank2;

		[NArg(Rank = 4, IsOptional = true)]
		private int COptRank4;

		[NArg(Rank = 1, IsOptional = true)]
		private int POptRank1;

		[NArg(Rank = 3, IsOptional = true)]
		private int QOptRank3;

		[NArg(Rank = 2, IsOptional = true)]
		private int ZOptRank2;
	}

	internal class TestClassWithIntArgAndConsumeRemaining
	{
		[NArg(Rank = 1)]
		private int SampleArgument;

		[NArg(IsConsumeRemaining = true)] 
		private string[] m_theRest;

		public int SampleArgumentValue { get { return SampleArgument; } }
		public IEnumerable<string> TheRest { get { return m_theRest; } }
	}

	internal class TestClassWithIntArgAndConsumeRemainingWithAltName
	{
		[NArg(Rank = 1)]
		private int SampleArgument;

		[NArg(IsConsumeRemaining = true, AltName = "File")]
		private string[] m_theRest;

		public int SampleArgumentValue { get { return SampleArgument; } }
		public IEnumerable<string> TheRest { get { return m_theRest; } }
	}

	internal class TestClassWithOnlyConsumeRemaining
	{
		[NArg(IsConsumeRemaining = true)]
		private string[] m_theRest;

		public IEnumerable<string> TheRest { get { return m_theRest; } }
	}

	public class SamplesSetup
	{

		internal readonly string _testProg = "TestProg";
		internal readonly string[] _emptyArgs = new string[] { };

		internal TestClassWithNoNArgAttributes _testClassWithNoNArgAttributes;
		internal ArgParser<TestClassWithNoNArgAttributes> _parserForTestClassWithNoNArgAttributes;

		internal TestClassWithIntSampleArgument _testClassWithIntSampleArgument;
		internal ArgParser<TestClassWithIntSampleArgument> _parserForTestClassWithIntSampleArgument;

		internal TestClassWithIntArgumentWithRequiredValues _testClassWithIntArgumentWithAllowedValues;
		internal ArgParser<TestClassWithIntArgumentWithRequiredValues> _parserForTestClassWithIntArgumentWithAllowedValues;

        internal TestClassWithOneOptional _testClassWithOneOptional;
        internal ArgParser<TestClassWithOneOptional> _parserForTestClassWithOneOptional;

		internal TestClassWithOneOptionalWithAllowedValues _testClassWithOneOptionalWithAllowedValues;
		internal ArgParser<TestClassWithOneOptionalWithAllowedValues> _parserFortestClassWithOneOptionalWithAllowedValues;

        internal TestClassWithOneOptionalWithArgument _testClassWithOneOptionalWithArgument;
        internal ArgParser<TestClassWithOneOptionalWithArgument> _parserForTestClassWithOneOptionalWithArgument;

		internal TestClassWithTwoOptionalWithArgument _testClassWithTwoOptionalWithArgument;
		internal ArgParser<TestClassWithTwoOptionalWithArgument> _parserForTestClassWithTwoOptionalWithArgument;

		internal TestClassWithMandatoryAndFlagArguments _testClassWithMandatoryAndFlagArguments;
		internal ArgParser<TestClassWithMandatoryAndFlagArguments> _parserForTestClassWithMandatoryAndFlagArguments;

		internal TestClassWithOptionalRankedArguments _testClassWithOptionalRankedArguments;
		internal ArgParser<TestClassWithOptionalRankedArguments> _parserForTestClassWithOptionalRankedArguments;

		internal TestClassWithRequiredRankedArguments _testClassWithRequiredRankedArguments;
		internal ArgParser<TestClassWithRequiredRankedArguments> _parserForTestClassWithRequiredRankedArguments;

		internal TestClassWithRankedRequiredAndOptionalArguments _testClassWithRankedRequiredAndOptionalArguments;
		internal ArgParser<TestClassWithRankedRequiredAndOptionalArguments> _parserForTestClassWithRankedRequiredAndOptionalArguments;

		internal TestClassWithIntArgAndConsumeRemaining _testClassWithIntArgAndConsumeRemaining;
		internal ArgParser<TestClassWithIntArgAndConsumeRemaining> _parserForTestClassWithIntArgAndConsumeRemaining;

		internal TestClassWithIntArgAndConsumeRemainingWithAltName _testClassWithIntArgAndConsumeRemainingWithAltName;
		internal ArgParser<TestClassWithIntArgAndConsumeRemainingWithAltName> _parserForTestClassWithIntArgAndConsumeRemainingWithAltName;

		internal TestClassWithOnlyConsumeRemaining _testClassWithOnlyConsumeRemaining;
		internal ArgParser<TestClassWithOnlyConsumeRemaining> _parserForTestClassWithOnlyConsumeRemaining;
			
		[SetUp]
		public void Setup()
		{
			_testClassWithNoNArgAttributes = new TestClassWithNoNArgAttributes();
			_parserForTestClassWithNoNArgAttributes = new ArgParser<TestClassWithNoNArgAttributes>(_testProg);

			_testClassWithIntSampleArgument = new TestClassWithIntSampleArgument();
			_parserForTestClassWithIntSampleArgument = new ArgParser<TestClassWithIntSampleArgument>(_testProg);

			_testClassWithIntArgumentWithAllowedValues = new TestClassWithIntArgumentWithRequiredValues();
			_parserForTestClassWithIntArgumentWithAllowedValues = new ArgParser<TestClassWithIntArgumentWithRequiredValues>(_testProg);

			_testClassWithOneOptional = new TestClassWithOneOptional();
            _parserForTestClassWithOneOptional = new ArgParser<TestClassWithOneOptional>(_testProg);

			_testClassWithOneOptionalWithAllowedValues = new TestClassWithOneOptionalWithAllowedValues();
			_parserFortestClassWithOneOptionalWithAllowedValues = new ArgParser<TestClassWithOneOptionalWithAllowedValues>(_testProg);

            _testClassWithOneOptionalWithArgument = new TestClassWithOneOptionalWithArgument();
            _parserForTestClassWithOneOptionalWithArgument = new ArgParser<TestClassWithOneOptionalWithArgument>(_testProg);

			_testClassWithTwoOptionalWithArgument = new TestClassWithTwoOptionalWithArgument();
			_parserForTestClassWithTwoOptionalWithArgument = new ArgParser<TestClassWithTwoOptionalWithArgument>(_testProg);

			_testClassWithMandatoryAndFlagArguments = new TestClassWithMandatoryAndFlagArguments();
			_parserForTestClassWithMandatoryAndFlagArguments = new ArgParser<TestClassWithMandatoryAndFlagArguments>(_testProg);

			_testClassWithOptionalRankedArguments = new TestClassWithOptionalRankedArguments();
			_parserForTestClassWithOptionalRankedArguments = new ArgParser<TestClassWithOptionalRankedArguments>(_testProg);

			_testClassWithRequiredRankedArguments = new TestClassWithRequiredRankedArguments();
			_parserForTestClassWithRequiredRankedArguments = new ArgParser<TestClassWithRequiredRankedArguments>(_testProg);

			_testClassWithRankedRequiredAndOptionalArguments = new TestClassWithRankedRequiredAndOptionalArguments();
			_parserForTestClassWithRankedRequiredAndOptionalArguments = new ArgParser<TestClassWithRankedRequiredAndOptionalArguments>(_testProg);

			_testClassWithIntArgAndConsumeRemaining = new TestClassWithIntArgAndConsumeRemaining();
			_parserForTestClassWithIntArgAndConsumeRemaining = new ArgParser<TestClassWithIntArgAndConsumeRemaining>(_testProg);

			_testClassWithIntArgAndConsumeRemainingWithAltName = new TestClassWithIntArgAndConsumeRemainingWithAltName();
			_parserForTestClassWithIntArgAndConsumeRemainingWithAltName = new ArgParser<TestClassWithIntArgAndConsumeRemainingWithAltName>(_testProg);

			_testClassWithOnlyConsumeRemaining = new TestClassWithOnlyConsumeRemaining();
			_parserForTestClassWithOnlyConsumeRemaining = new ArgParser<TestClassWithOnlyConsumeRemaining>(_testProg);
		}
	}
}
