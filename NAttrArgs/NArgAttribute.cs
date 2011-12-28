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
	public class NArgAttribute : Attribute
	{
		private bool _isOptional = false;

		public NArgAttribute() { /* Needed to grant public access */ }

		protected NArgAttribute(NArgAttribute attr)
		{
			Rank = attr.Rank;
            IsOptional = attr.IsOptional;
			OptionalArgName = attr.OptionalArgName;
			AltName = attr.AltName;
			AllowedValues = attr.AllowedValues;
			IsConsumeRemaining = attr.IsConsumeRemaining;
		}

		public uint Rank { get; set; }
		public string OptionalArgName { get; set;  }
		
		public bool IsOptional
		{
			get { return _isOptional || OptionalArgName != null || IsConsumeRemaining; } 
		
			set { _isOptional = value; }
		}

		public bool HasOptionalArgument
		{
			get { return IsOptional == true && (OptionalArgName != null || AllowedValues != null); }
		}

		public string AltName { get; set; }
		public string[] AllowedValues { get; set; }
		public bool IsConsumeRemaining { get; set; }
	}
}
