//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;

namespace NAttrArgs
{
	class CustomConvert
	{
		public static object ChangeType<T>(T value, Type targetType)
		{
			if (IsConvertingFromBoolToChar(value, targetType))
				return MakeCharFromBool((bool)(object)value);
			else if (targetType.IsAssignableFrom(typeof(T)))
				return value;
			else
				return Convert.ChangeType(value, targetType);
		}

		public static char MakeCharFromBool(bool value)
		{
			return value ? '\x1' : '\x0';
		}

		public static bool IsConvertingFromBoolToChar<T>(T value, Type targetType)
		{
			return targetType == typeof(char) && value is bool;
		}
	}
}
