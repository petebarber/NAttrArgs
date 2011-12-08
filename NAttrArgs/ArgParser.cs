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
using System.Linq;
using System.Reflection;

namespace NAttrArgs
{
	public class ArgParser<T> where T : class
	{
		private readonly string _progName;
		private readonly IEnumerable<MemberAttribute> _optionalArgumentAttrs;
		private readonly IEnumerable<MemberAttribute> _requiredArgumentAttrs;
		private readonly MemberAttribute _remainingArgumentAttr;

		public ArgParser(string progName)
		{
			_progName = progName;

			var attrs = GetAttributes();

			_optionalArgumentAttrs = from member in attrs where member.IsOptional == true && member.IsConsumeRemaining == false orderby member.Rank select member;
			_requiredArgumentAttrs = from member in attrs where member.IsOptional == false orderby member.Rank select member;
			IEnumerable<MemberAttribute> remainingArgumentAttrs = from member in attrs where member.IsOptional == true && member.IsConsumeRemaining == true orderby member.Rank select member;

			if (remainingArgumentAttrs.Count() > 0)
				_remainingArgumentAttr = remainingArgumentAttrs.First();
		}

		public void Parse(T t, string[] args)
		{
			try
			{
				var argIt = new ArgIterator(args);

				new OptionalArgumentsParser<T>(t, _optionalArgumentAttrs, argIt).Parse();
				new RequiredArgumentsParser<T>(t, _requiredArgumentAttrs, argIt).Parse();
				new RemainingArgumentsParser<T>(t, _remainingArgumentAttr, argIt).Parse();

				if (argIt.MoveNext() == true)
					throw new Exception("Unexpected arguments");
			}
			catch (Exception e)
			{
				throw new NArgException(Usage.GetUsageString(_progName, _optionalArgumentAttrs, _requiredArgumentAttrs, _remainingArgumentAttr), e);
			}
		}

		private IEnumerable<MemberAttribute> GetAttributes()
		{
			return from member in typeof(T).GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)
				   let attr = (NArgAttribute)Attribute.GetCustomAttribute(member, typeof(NArgAttribute))
				   where attr != null
				   select new MemberAttribute(attr, member);
		}
	}
}