//
// NAttrArgs
//
// Copyright (c) 2012 Pete Barber
//
// Licensed under the The Code Project Open License (CPOL.html)
// http://www.codeproject.com/info/cpol10.aspx 
//
using System.Collections.Generic;
using System.Text;

namespace NAttrArgs
{
	public static class Usage
	{
		private static string GetAllowedValues(IEnumerable<object> allowedValues)
		{
			var usage = new StringBuilder();

			if (allowedValues != null)
				foreach (object o in allowedValues)
					if (usage.Length > 0)
						usage.AppendFormat("|{0}", o);
					else
						usage.Append(o);

			return usage.ToString();
		}

		public static string GetUsageString(string progName, IEnumerable<MemberAttribute> optional, IEnumerable<MemberAttribute> required, MemberAttribute consumeRemaining)
		{
			var usage = new StringBuilder("Usage: ");

			usage.Append(progName);

			MakeOptionalUsageString(optional, usage);
			MakeRequiredUsageString(required, usage);
			MakeRemainingUsageString(consumeRemaining, usage);

			return usage.ToString();
		}

		private static void MakeRemainingUsageString(MemberAttribute consumeRemaining, StringBuilder usage)
		{
			if (consumeRemaining != null)
				if (consumeRemaining.AltName != null)
					usage.AppendFormat(" [<{0}> ...]", consumeRemaining.AltName);
				else
					usage.Append(" [...]");
		}

		private static void MakeRequiredUsageString(IEnumerable<MemberAttribute> required, StringBuilder usage)
		{
			foreach (MemberAttribute arg in required)
				usage.AppendFormat(" <{0}>", arg.AllowedValues == null ? arg.ArgName : GetAllowedValues(arg.AllowedValues));
		}

		private static void MakeOptionalUsageString(IEnumerable<MemberAttribute> optional, StringBuilder usage)
		{
			foreach (MemberAttribute arg in optional)
				if (arg.OptionalArgName == null && arg.AllowedValues == null)
					usage.AppendFormat(" [-{0}]", arg.ArgName);
				else if (arg.AllowedValues == null)
					usage.AppendFormat(" [-{0} <{1}>]", arg.ArgName, arg.OptionalArgName);
				else
					usage.AppendFormat(" [-{0} <{1}>]", arg.ArgName, GetAllowedValues(arg.AllowedValues));
		}
	}
}
