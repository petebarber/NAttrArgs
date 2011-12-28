//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.Reflection;
using System.Linq;

namespace NAttrArgs
{
	public class MemberSetter
	{
		public static void SetOptionalFlagArg<T>(T t, MemberAttribute arg, bool value)
		{
			SetImpl(t, arg, value);
		}

		public static void SetRemainingArg<T>(T t, MemberAttribute arg, string[] value)
		{
			SetImpl(t, arg, value);
		}

		public static void SetArg<T>(T t, MemberAttribute arg, string value)
		{
			if (arg.AllowedValues != null)
				if (arg.AllowedValues.Contains(value) == false)
					throw new FormatException(string.Format("Not an allowed value:{0}", value));

			SetImpl(t, arg, value);
		}

		public static void SetImpl<T, U>(T t, MemberAttribute arg, U value)
		{
			if (arg.MemberInfo is FieldInfo)
				SetFieldWithArg((FieldInfo)arg.MemberInfo, t, value);
			else if (arg.MemberInfo is PropertyInfo)
				SetPropertyWithArg((PropertyInfo)arg.MemberInfo, t, value);
			else if (arg.MemberInfo is MethodInfo)
				SetMethodWithArg((MethodInfo)arg.MemberInfo, t, value);
			else
				throw new NotImplementedException("Unsupported member type");
		}

		public static void SetFieldWithArg<T, U>(FieldInfo foundMember, T t, U value)
        {
			foundMember.SetValue(t, CustomConvert.ChangeType(value, foundMember.FieldType));
        }

		public static void SetPropertyWithArg<T, U>(PropertyInfo foundMember, T t, U value)
        {
			foundMember.SetValue(t, CustomConvert.ChangeType(value, foundMember.PropertyType), null);
        }

		public static void SetMethodWithArg<T, U>(MethodInfo foundMember, T t, U value)
		{
			ParameterInfo[] paramInfo = foundMember.GetParameters();

			if (paramInfo.Length == 0)
				foundMember.Invoke(t, new object[] { });
			else
				foundMember.Invoke(t, new object[] { CustomConvert.ChangeType(value, paramInfo[0].ParameterType) });
		}
	}
}
