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
	class ArgParserTests : SamplesSetup
	{
		List<MemberAttribute> _empty = new List<MemberAttribute>();

		[Test]
		public void Parse_SingleRequiredIntArgOf7_Is7()
		{
			string[] args = new string[] { "7" };

			_parserForTestClassWithIntSampleArgument.Parse(_testClassWithIntSampleArgument, args);

			Assert.That(_testClassWithIntSampleArgument.SampleArgumentValue, Is.EqualTo(7));
		}

        [Test]
        public void Parse_OptionalWhenNoArgsSupplied_DoesNotThrow()
        {
            Assert.That(() => _parserForTestClassWithOneOptional.Parse(_testClassWithOneOptional, new string[] { }), Throws.Nothing);
        }


		[Test]
		public void Parse_OptionalWithArgumentWhenNoArgsSupplied_DoesNotThrow()
		{
			Assert.That(() => _parserForTestClassWithOneOptionalWithArgument.Parse(_testClassWithOneOptionalWithArgument, new string[]{}), Throws.Nothing);
		}

        [Test]
        public void Parse_OptionalWithArgumentWhenOnlyOptionSupplied_DoesThrow()
        {
            Assert.That(() => _parserForTestClassWithOneOptionalWithArgument.Parse(_testClassWithOneOptionalWithArgument, new string[] { "-SetSomething" }),
                            Throws.TypeOf<NArgException>().With.Message.EqualTo("Usage: TestProg [-SetSomething <test>]"));
        }

		[Test]
		public void Parse_Optional_IsSet()
		{
            _parserForTestClassWithOneOptional.Parse(_testClassWithOneOptional, new string[] { "-SetSomething" });

            Assert.That(_testClassWithOneOptional.SetSomethingValue, Is.True);
		}
		[Test]
		public void Parse_OptionalWithIntArgOf9_Is9()
		{
            _parserForTestClassWithOneOptionalWithArgument.Parse(_testClassWithOneOptionalWithArgument, new string[] { "-SetSomething", "9" });

            Assert.That(_testClassWithOneOptionalWithArgument.SetSomethingValue, Is.EqualTo(9));
		}

		[Test]
		public void Parse_WhenOneArgumentIsExpectedAndNoneAreSupplied_ThrowsWithCorrectInnerException()
		{
			Assert.That(() => _parserForTestClassWithIntSampleArgument.Parse(_testClassWithIntSampleArgument, _emptyArgs),
				Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Number of required arguments and actual arguments don't match"));
		}

		[Test]
		public void Parse_UnexpectedMandatoryArgument_Throws()
		{
			var args = new string[] { "-ZOptRank2", "-POptRank1", "-QOptRank3", "-COptRank4", "foo" };

			Assert.That(() => _parserForTestClassWithOptionalRankedArguments.Parse(_testClassWithOptionalRankedArguments, args),
				Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Unexpected arguments"));
		}

		[Test]
		public void Parse_UnexpectedArgumentAfterTwoOptionalWithArguments_Throws()
		{
			var args = new string[] { "-SetSomethingElse", "1", "-SetSomething", "2", "foo" };

			Assert.That(() => _parserForTestClassWithTwoOptionalWithArgument.Parse(_testClassWithTwoOptionalWithArgument, args),
				Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Unexpected arguments"));
		}

		[Test]
		public void Parse_IncorrectOptionalArgumentWhenOnlyOneAllowed_Throws()
		{
			Assert.That(() => _parserForTestClassWithOneOptional.Parse(_testClassWithOneOptional, new string[] { "-foo" }),
							Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("No matching argument for: foo"));
		}

		[Test]
		public void Parse_IncorrectOptionalArgumentWhenMultipleOptionsAllow()
		{
			Assert.That(() => _parserForTestClassWithOptionalRankedArguments.Parse(_testClassWithOptionalRankedArguments, new string[] { "-POptRank1", "-foo", "-QOptRank3" }),
							Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("No matching argument for: foo"));

		}

		[Test]
		public void Parse_WhenOneArgumentIsExpectedAndTwoAreSupplied_ThrowsWithThrowsWithCorrectInnerException()
		{
			string[] args = new string[] { "7", "unexpected" };

			Assert.That(() => _parserForTestClassWithIntSampleArgument.Parse(_testClassWithIntSampleArgument, args),
				Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Unexpected arguments"));
		}

		[Test]
		public void Parse_TooManyRequiredArgs_Throws()
		{
			string[] args = new string[] { "1", "2", "3", "4", "5" };

			Assert.That(() => _parserForTestClassWithRequiredRankedArguments.Parse(_testClassWithRequiredRankedArguments, args),
						Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Unexpected arguments"));
		}

		[Test]
		public void Parse_TooManyRequiredArgsEvenThoughOptionalArgsAllowed_Throws()
		{
			string[] args = new string[] { "1", "2", "3", "4", "5" };

			Assert.That(() => _parserForTestClassWithRankedRequiredAndOptionalArguments.Parse(_testClassWithRankedRequiredAndOptionalArguments, args),
						Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Unexpected arguments"));
		}

		[Test]
		public void Parse_UsingNonAllowedValues_Throws()
		{
			string[] args = new string[] { "4" };

			Assert.That(() => _parserForTestClassWithIntArgumentWithAllowedValues.Parse(_testClassWithIntArgumentWithAllowedValues, args),
						Throws.TypeOf<NArgException>().With.InnerException.Message.EqualTo("Not an allowed value:4"));
		}

		[Test]
		public void Parse_WhenConsumeRemaingSetButNothingLeft_TheRestIsNullOrEmpty()
		{
			_parserForTestClassWithIntArgAndConsumeRemaining.Parse(_testClassWithIntArgAndConsumeRemaining, new string[] {"1"});

			Assert.That(_testClassWithIntArgAndConsumeRemaining.SampleArgumentValue, Is.EqualTo(1));
			Assert.That(_testClassWithIntArgAndConsumeRemaining.TheRest, Is.Empty);
		}

		[Test]
		public void Parse_WhenConsumeRemaingSetWithArgs1234_TheRestIs234()
		{
			_parserForTestClassWithIntArgAndConsumeRemaining.Parse(_testClassWithIntArgAndConsumeRemaining, new string[] { "1", "2", "3", "4" });

			Assert.That(_testClassWithIntArgAndConsumeRemaining.SampleArgumentValue, Is.EqualTo(1));
			Assert.That(_testClassWithIntArgAndConsumeRemaining.TheRest, Is.EqualTo(new string[] { "2", "3", "4"}));
		}

		[Test]
		public void Parse_WhenOnlyConsumeRemaingSetWithArgs1234_TheRestIs1234()
		{
			_parserForTestClassWithOnlyConsumeRemaining.Parse(_testClassWithOnlyConsumeRemaining, new string[] { "1", "2", "3", "4" });

			Assert.That(_testClassWithOnlyConsumeRemaining.TheRest, Is.EqualTo(new string[] { "1", "2", "3", "4" }));
		}
	}
}
