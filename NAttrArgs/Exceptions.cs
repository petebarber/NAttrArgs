//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System;

namespace NAttrArgs
{
	public class NArgException : ApplicationException
	{
		public NArgException(string usage, Exception inner) : base(usage, inner) { /* Empty */ }
		public NArgException(string usage) : this(usage, null) { /* Empty */ }
	}
}
