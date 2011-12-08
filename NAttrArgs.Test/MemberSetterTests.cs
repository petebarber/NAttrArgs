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
using System.Reflection;
using NUnit.Framework;

namespace NAttrArgs.Test
{
    class FieldSample<T>
    {
        private T _field;
        public T FieldProp { get { return _field; } }

        public T TestProp { get; set; }

		public void TestMethod(T value)
		{
			TestMethodProp = value;
		}

		private object _methodProp = null;

		public T TestMethodProp
		{
			get
			{
				if (_methodProp == null) throw new Exception("Not set");

				return (T)_methodProp;
			}

			set
			{
				_methodProp = value;
			}
		}

		// Method with no arguments
		public void TestMethodWithNoArgs()
		{
			HasTestMethodWithNoArgsExecuted = true;
		}

		public bool HasTestMethodWithNoArgsExecuted { get; private set; }

    	public string[] RemainingProp { get; set; }
		public IEnumerable<string> FooProp { get; set; }
    }

    class MemberSetterTests
    {
        MemberInfo GetSingleMemberInfo<T>(T t, string name)
        {
            MemberInfo[] members = t.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic);

            if (members.Length != 1) throw new Exception("Expected only a single member");

            return members[0];
        }

		// These test setting optional parameters without arguments

		// IsOptional (with no argument) tests
		[TestCase(true, (bool)true)]
		[TestCase(true, (byte)1)]
		[TestCase(true, (sbyte)1)]
		[TestCase(true, (char)1)]
		[TestCase(true, (double)1)]
		[TestCase(true, (float)1)]
		[TestCase(true, (int)1)]
		[TestCase(true, (uint)1)]
		[TestCase(true, (long)1)]
		[TestCase(true, (ulong)1)]
		[TestCase(true, (object)1)]
		[TestCase(true, (short)1)]
		[TestCase(true, (ushort)1)]
		[TestCase(true, (string)"True")]
		[TestCase(false, (bool)false)]
		[TestCase(false, (byte)0)]
		[TestCase(false, (sbyte)0)]
		[TestCase(false, (char)0)]
		[TestCase(false, (double)0)]
		[TestCase(false, (float)0)]
		[TestCase(false, (int)0)]
		[TestCase(false, (uint)0)]
		[TestCase(false, (long)0)]
		[TestCase(false, (ulong)0)]
		[TestCase(false, (object)0)]
		[TestCase(false, (short)0)]
		[TestCase(false, (ushort)0)]
		[TestCase(false, (string)"False")]
		public void SetWithArg_boolToMemberOfType_expected<T>(bool toSet, T expected)
		{
			FieldSample<T> sample = new FieldSample<T>();

			MemberAttribute fieldAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "_field"));
			MemberSetter.SetOptionalFlagArg(sample, fieldAttr, toSet);

			Assert.That(sample.FieldProp, Is.EqualTo(expected));

			MemberAttribute propAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestProp"));
			MemberSetter.SetOptionalFlagArg(sample, propAttr, toSet);

			Assert.That(sample.TestProp, Is.EqualTo(expected));

			MemberAttribute methodAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestMethod"));
			MemberSetter.SetOptionalFlagArg(sample, methodAttr, toSet);

			Assert.That(sample.TestMethodProp, Is.EqualTo(expected));

			MemberAttribute methodWithNoArgsAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestMethodWithNoArgs"));
			MemberSetter.SetOptionalFlagArg(sample, methodWithNoArgsAttr, toSet);

			Assert.That(sample.HasTestMethodWithNoArgsExecuted, Is.True);
		}

		[TestCase(true, 1)]
		[TestCase(false, 0)]
		public void SetWithArg_stringToMemberOfdecimal_expected(bool toSet, decimal expected)
		{
			SetWithArg_boolToMemberOfType_expected(toSet, expected);
		}


        [TestCase("true", (bool)true)]
		[TestCase("1", (byte)1)]
		[TestCase("1", (sbyte)1)]
		[TestCase("\x1", (char)1)]
		[TestCase("1", (double)1)]
		[TestCase("1", (float)1)]
		[TestCase("1", (int)1)]
		[TestCase("1", (uint)1)]
		[TestCase("1", (long)1)]
		[TestCase("1", (ulong)1)]
		[TestCase("1", (object)1)]
		[TestCase("1", (short)1)]
		[TestCase("1", (ushort)1)]
		[TestCase("1", (string)"1")]
        [TestCase("FALSE", (bool)false)]
		[TestCase("0", (byte)0)]
		[TestCase("0", (sbyte)0)]
		[TestCase("\x0", (char)0)]
		[TestCase("0", (double)0)]
		[TestCase("0", (float)0)]
		[TestCase("0", (int)0)]
		[TestCase("0", (uint)0)]
		[TestCase("0", (long)0)]
		[TestCase("0", (ulong)0)]
		[TestCase("0", (object)0)]
		[TestCase("0", (short)0)]
		[TestCase("0", (ushort)0)]
		[TestCase("0", (string)"0")]
		[TestCase("2", (byte)2)]
		[TestCase("2", (sbyte)2)]
		[TestCase("A", (char)65)]
		[TestCase("77.8", (double)77.8)]
		[TestCase("893.456", (float)893.456)]
		[TestCase("-1234567890", (int)-1234567890)]
        public void SetWithArg_stringToMemberOfType_expected<T>(string toSet, T expected)
        {
            FieldSample<T> sample = new FieldSample<T>();

            MemberAttribute fieldAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "_field"));
			MemberSetter.SetArg(sample, fieldAttr, toSet);

			MemberAttribute propAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestProp"));
			MemberSetter.SetArg(sample, propAttr, toSet);

			MemberAttribute methodAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestMethod"));
			MemberSetter.SetArg(sample, methodAttr, toSet);

			MemberAttribute methodWithNoArgsAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "TestMethodWithNoArgs"));
			MemberSetter.SetArg(sample, methodWithNoArgsAttr, toSet);

			Assert.That(sample.FieldProp, Is.EqualTo(expected));
			Assert.That(sample.TestProp, Is.EqualTo(expected));
			Assert.That(sample.TestMethodProp, Is.EqualTo(expected));
			Assert.That(sample.HasTestMethodWithNoArgsExecuted, Is.True);
        }

        [TestCase("1", 1)]
        [TestCase("0", 0)]
        public void SetWithArg_stringToMemberOfdecimal_expected(string toSet, decimal expected)
        {
			SetWithArg_stringToMemberOfType_expected(toSet, expected);   
        }

		[Test]
		public void SetRemainingArg_EmptyStringArrayToStringArray_DoesNotThrow()
		{
			var sample = new FieldSample<int>();

			string[] empty = new string[] {};

			MemberAttribute propAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "RemainingProp"));
			MemberSetter.SetRemainingArg(sample, propAttr, empty);
		}

		[Test]
		public void SetRemainingArg_EmptyStringArrayToIEnumerableOfSring_DoesNotThrow()
		{
			var sample = new FieldSample<int>();

			string[] empty = new string[] { };

			MemberAttribute propAttr = new MemberAttribute(new NArgAttribute(), GetSingleMemberInfo(sample, "FooProp"));
			MemberSetter.SetRemainingArg(sample, propAttr, empty);
		}
    }
}
