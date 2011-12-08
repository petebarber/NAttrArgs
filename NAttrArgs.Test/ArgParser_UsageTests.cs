//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using NUnit.Framework;

namespace NAttrArgs.Test
{
	public class ArgParser_UsageTests : SamplesSetup
	{
		[Test]
		public void Parse_NoNArgAttributes_DoesNotThrow()
		{
			Assert.That(() => _parserForTestClassWithNoNArgAttributes.Parse(_testClassWithNoNArgAttributes, _emptyArgs), Throws.Nothing);
		}

		[Test]
		public void Parse_OneRankedArgumentOfIntWithNoArgsSupplied_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithIntSampleArgument.Parse(_testClassWithIntSampleArgument, _emptyArgs), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg <SampleArgument>"));
		}

		[Test]
		public void Parse_OneOptionalArgWithNoArgsSupplied_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithOneOptional.Parse(_testClassWithOneOptional, new string[] { "-BadOption" }), 
						Throws.Exception.With.Message.EqualTo("Usage: TestProg [-SetSomething]"));
		}

		[Test]
        public void Parse_OneOptionalWithArgument_ThrowsTheCorrectUsageException()
		{
            Assert.That(() => _parserForTestClassWithOneOptionalWithArgument.Parse(_testClassWithOneOptionalWithArgument, new string[] { "ShouldntMatch" }), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-SetSomething <test>]"));
		}

		[Test]
        public void Parse_OneMandatoryAndOneOptional_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithMandatoryAndFlagArguments.Parse(_testClassWithMandatoryAndFlagArguments, _emptyArgs), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-SetSomething] <SampleArgument>"));

		}

		[Test]
        public void Parse_RankedSetOfOptionalArguments_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithOptionalRankedArguments.Parse(_testClassWithOptionalRankedArguments, new string[] { "-NotAnOptions" }), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-POptRank1] [-ZOptRank2] [-QOptRank3] [-COptRank4]"));
		}

		[Test]
        public void Parse_RankedSetOfRequiredArguments_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithRequiredRankedArguments.Parse(_testClassWithRequiredRankedArguments, _emptyArgs), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg <PRank1> <ZRank2> <QRank3> <CRank4>"));
		}

		[Test]
        public void Parse_RankedSetOfOptionalAndRequiredArguments_ThrowsTheCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithRankedRequiredAndOptionalArguments.Parse(_testClassWithRankedRequiredAndOptionalArguments, _emptyArgs), 
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-POptRank1] [-ZOptRank2] [-QOptRank3] [-COptRank4] <PRank1> <ZRank2> <QRank3> <CRank4>"));
		}

		[Test]
		public void Parse_RequiredArgumentWithAllowedValues_ThrowsCorrectUsageException()
		{
			Assert.That(() => _parserForTestClassWithIntArgumentWithAllowedValues.Parse(_testClassWithIntArgumentWithAllowedValues, _emptyArgs),
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg <1|2|3>"));
		}

		[Test]
		public void Parse_RequiredArgumentWithAllowedValues_ThrowsCorrectUsageExceptionAndAllowedValuesAreIgnored()
		{
			Assert.That(() => _parserFortestClassWithOneOptionalWithAllowedValues.Parse(_testClassWithOneOptionalWithAllowedValues, new string[] {"-badOption" }),
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-SetSomething <1|2|3>]"));
		}

		[Test]
		public void Parse_WhenConsumeRemainingSpecified_ThatIsDisplayedLastInUsage()
		{
			Assert.That(()=> _parserForTestClassWithIntArgAndConsumeRemaining.Parse(_testClassWithIntArgAndConsumeRemaining, new string[] { "-badOption" }),
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg <SampleArgument> [...]"));
		}

		[Test]
		public void Parse_WhenConsumeRemainingWithAltNameSpecified_ThatIsDisplayedLastInUsageWithAltName()
		{
			Assert.That(() => _parserForTestClassWithIntArgAndConsumeRemainingWithAltName.Parse(_testClassWithIntArgAndConsumeRemainingWithAltName, new string[] { "-badOption" }),
						Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg <SampleArgument> [<File> ...]"));
		}
	}
}
