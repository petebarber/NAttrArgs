//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Reflection;
using NUnit.Framework;

namespace NAttrArgs.Test
{
	public class AttributeTests
	{
		private static class CustomAttributeHelper
		{
			// Helpers
			public static object[] GetCustomAttrPropertySetOnMethod<TAttr, TProp>(MethodBase method, string argName,
			                                                                      TProp expectedValue)
			{
				object[] customAttributes = method.GetCustomAttributes(typeof (TAttr), false);

				return customAttributes;
			}

			public static object GetCustomPropertiesFromMember<T, TAttr>(string memberName)
			{
				MemberInfo[] memberInfos = typeof (T).GetMember(memberName,
				                                                BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static |
				                                                BindingFlags.NonPublic);

				return memberInfos[0].GetCustomAttributes(typeof (TAttr), false)[0];
			}
		}

		private class BadInheritFromNArgAttribute : NArgAttribute
		{
			public BadInheritFromNArgAttribute() : base(null)
			{
			}
		}

		private class GoodInheritFromNArgAttribute : NArgAttribute
		{
			public GoodInheritFromNArgAttribute(NArgAttribute attr) : base(attr)
			{
			}
		}

		//
		// Initial
		//
		[Test]
		public void NArgAttribute_CanCreateInstanceOf()
		{
			Assert.That(new NArgAttribute(), Is.TypeOf<NArgAttribute>());
			Assert.That(new NArgAttribute(), Is.InstanceOf<NArgAttribute>());
		}

		[Test]
		public void NArgAttribute_ProtectedCopyConstructorWithNull_Throws()
		{
			Assert.That(() => { new BadInheritFromNArgAttribute(); }, Throws.Exception);
		}

		//
		// Test that protected copy constructor is working
		//
		[Test]
		[NArg(IsOptional = false, AltName = "somethingelse", Rank = 5, AllowedValues = new string[] {"1", "2", "3"},
			OptionalArgName = "avalue")]
		public void NArgAttribute_ProtectedCopyConstructorWorkWithValidValues_DoesNotThrow()
		{
			object nattr =
				CustomAttributeHelper.GetCustomPropertiesFromMember<AttributeTests, NArgAttribute>(
					"NArgAttribute_ProtectedCopyConstructorWorkWithValidValues_DoesNotThrow");

			GoodInheritFromNArgAttribute good = new GoodInheritFromNArgAttribute((NArgAttribute) nattr);

			Assert.That(good.AltName, Is.EqualTo("somethingelse"));
			Assert.That(good.IsOptional, Is.True);
			Assert.That(good.Rank, Is.EqualTo(5));
			Assert.That(good.AllowedValues, Is.EqualTo(new string[] {"1", "2", "3"}));
			Assert.That(good.OptionalArgName, Is.EqualTo("avalue"));
		}

		[Test]
		[NArg]
		public void NArgAttribute_CanObtain_DoesNotThrow()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>());
		}


		//
		// Defaults
		//

		[Test]
		[NArg]
		public void NArgAttribute_RankProperty_IsTypeOf_uint()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("Rank").TypeOf<uint>());
		}

		[Test]
		[NArg]
		public void NArgAttribute_RankProperty_HasDefault_0()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("Rank").EqualTo(0));
		}

		[Test]
		[NArg]
		public void NArgAttribute_OptionalProperty_IsTypeOf_bool()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsOptional").TypeOf<bool>());
		}

		[Test]
		[NArg]
		public void NArgAttribute_OptionalProperty_DefaultIs_False()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsOptional").False);
		}

		[Test]
		[NArg(OptionalArgName = "")]
		public void NArgAttribute_OptionalArgNameProperty_IsTypeOf_string()
		{
			// Ideally this would work without having explicitly set the string to empty.
			Assert.That(MethodBase.GetCurrentMethod(),
			            Has.Attribute<NArgAttribute>().Property("OptionalArgName").TypeOf<string>());
		}

		[Test]
		[NArg(OptionalArgName = "", IsOptional = false)]
		public void NArgAttribute_WhenOptionalArgNamePropertyIsSet_OptionalPropertyIsTrue()
		{
			// Ideally this would work without having explicitly set the string to empty.
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsOptional").True);
		}

		[Test]
		[NArg]
		public void NArgAttribute_WhenOptionalArgNameProperty_DefaultIs_null()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("OptionalArgName").Null);
		}

		[Test]
		[NArg(AltName="")]
        public void NArgAttribute_AltNameProperty_IsTypeOf_string()
		{
			// Ideally this would work without having explicitly set the string to empty.
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("AltName").TypeOf<string>());
		}

		[Test]
		[NArg]
        public void NArgAttribute_AltNameProperty_DefaultIs_Null()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("AltName").Null);
		}

		[Test]
		[NArg(AllowedValues = new string[]{})]
        public void NArgAttribute_AllowedValuesPropertyTypeIs_arrayOfstring()
		{
			// Ideally this would work without having explicitly set the object[] to null
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("AllowedValues").TypeOf<string[]>());
		}

		[Test]
		[NArg]
        public void NArgAttribute_ConsumeRemainingPropertyDefault_false()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsConsumeRemaining").False);
		}

		[Test]
		[NArg(AllowedValues = new string[] { })]
		public void NArgAttribute_ConsumeRemainingPropertyTypeIs_arrayOfstring()
		{
			// Ideally this would work without having explicitly set the object[] to null
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsConsumeRemaining").TypeOf<bool>());
		}

		[Test]
		[NArg(IsOptional = false, IsConsumeRemaining = true)]
		public void NArgAttribute_ConsumeRemainingPropertyIsSet_OptionalPropertyIsTrue()
		{
			// Ideally this would work without having explicitly set the string to empty.
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsOptional").True);
		}

		[Test]
		[NArg]
		public void NArgAttribute_AllowedValuesPropertyDefault_null()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("AllowedValues").Null);
		}

		//
		// Setting
		//

		[Test]
		[NArg(Rank=11)]
        public void NArgAttribute_CanSetRankProperty_Is11()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("Rank").EqualTo(11));
		}

        [Test]
        [NArg(IsOptional = true)]
        public void NArgAttribute_CanSetOptionalPropertyToFalse_IsTrue()
        {
            Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("IsOptional").True);
        }

		[Test]
		[NArg(OptionalArgName = "Something")]
        public void NArgAttribute_CanSetOptionalArgNameProperty_IsSomething()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("OptionalArgName").EqualTo("Something"));
		}

		[Test]
		[NArg(AltName="foo")]
        public void NArgAttribute_CanSetAltNameProperty_IsTrue()
		{
			Assert.That(MethodBase.GetCurrentMethod(), Has.Attribute<NArgAttribute>().Property("AltName").EqualTo("foo"));
		}

        [Test]
		[NArg(AllowedValues = new string[] { "1", "2", "3" })]
        public void NArgAttribute_CanSetAllowedValuesProperty_ToArrayOf123()
		{
			Assert.That(MethodBase.GetCurrentMethod(),
			            Has.Attribute<NArgAttribute>().Property("AllowedValues").EqualTo(new string[] {"1", "2", "3"}));
		}

		[Test]
		[NArg(AllowedValues = new string[] { "one", "two", "three" })]
        public void NArgAttribute_CanSetAllowedValuesProperty_ToArrayOfStringOneTwoThree()
		{
			Assert.That(MethodBase.GetCurrentMethod(),
						Has.Attribute<NArgAttribute>().Property("AllowedValues").EqualTo(new string[] { "one", "two", "three" }));
		}

		//
		// Basic set test on a data member
		//
		private class Sample
		{
			[NArg(Rank = 7)]
			private int _one;

			[NArg(AltName = "Bar")]
			public string Foo { get; set; }

			[NArg(IsOptional = true, AllowedValues = new string[] {"1", "2", "3"})] 
			private int _limitedValues;
		}

		[Test]
        public void NArgAttribute_CanSetRankPropertyOnADataMember_Is7()
		{
			Assert.That(CustomAttributeHelper.GetCustomPropertiesFromMember<Sample, NArgAttribute>("_one"), Has.Property("Rank").EqualTo(7));
		}

		//
		// Basic set test on a property member
		//
		[Test]
        public void NArgAttribute_CanSetAltNamePropertyOnAPropertyMember_IsBar()
		{
			Assert.That(CustomAttributeHelper.GetCustomPropertiesFromMember<Sample, NArgAttribute>("Foo"), Has.Property("AltName").EqualTo("Bar"));
		}

		[Test]
		public void MemberAttribute_WhenOptionalSetWithAllowedValuesAndOptionalArgNameNotSet_HasOptionalArgumentIsTrue()
		{
			NArgAttribute nattr = (NArgAttribute)CustomAttributeHelper.GetCustomPropertiesFromMember<Sample, NArgAttribute>("_limitedValues");

			Assert.That(nattr.HasOptionalArgument, Is.True);
		}
	}
}
