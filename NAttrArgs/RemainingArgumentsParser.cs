//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Collections.Generic;

namespace NAttrArgs
{
	class RemainingArgumentsParser<T> where T: class 
	{
		private readonly MemberAttribute _remaining;
		private readonly T _t = null;
		private readonly ArgIterator _argIt = null;

		public RemainingArgumentsParser(T t, MemberAttribute remaining, ArgIterator argIt)
		{
			_t = t;
			_remaining = remaining;
			_argIt = argIt;
		}

		public void Parse()
		{
			if (_remaining == null) return;

			MemberSetter.SetRemainingArg(_t, _remaining, MakeStringArrayFromRemainingArgs());
		}

		string[] MakeStringArrayFromRemainingArgs()
		{
			List<string> remaining = new List<string>();

			while (_argIt.MoveNext())
				remaining.Add(_argIt.Current);

			return remaining.ToArray();
		}

	}
}
