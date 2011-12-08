//
// NAttrArgs
//
// Copyright (c) 2011 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Reflection;

namespace NAttrArgs
{
	public class MemberAttribute : NArgAttribute
	{
		private readonly MemberInfo _memberInfo;

		public MemberAttribute(NArgAttribute attr, MemberInfo memberInfo) : base(attr)
		{
			_memberInfo = memberInfo;
		}

		public string ArgName
		{
			get { return AltName ?? _memberInfo.Name; }
		}

		public MemberInfo MemberInfo
		{
			get { return _memberInfo; }
		}
	}
}