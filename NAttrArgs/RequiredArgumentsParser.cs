//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;
using System.Collections.Generic;

namespace NAttrArgs
{
	class RequiredArgumentsParser<T> where T: class
	{
		private readonly IEnumerable<MemberAttribute> _required;
		private readonly T _t = null;
		private readonly ArgIterator _argIt = null;

		public RequiredArgumentsParser(T t, IEnumerable<MemberAttribute> required, ArgIterator argIt)
		{
			_t = t;
			_required = required;
			_argIt = argIt;
		}

		public void Parse()
		{
			foreach (MemberAttribute arg in _required)
				if (_argIt.MoveNext())
					MemberSetter.SetArg(_t, arg, _argIt.Current);
				else
					throw new Exception("Number of required arguments and actual arguments don't match");
		}

	}
}
