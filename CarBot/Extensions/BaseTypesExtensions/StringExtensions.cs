using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarBot.BaseTypesExtensions
{
	public static class StringExtensions
	{
		public static String Format(this String format, params object?[] args)
		{
			return string.Format(format, args);
		}

		public static String Format(this String format, object? arg0, object? arg1, object? arg2)
		{
			return string.Format(format, arg0, arg1, arg2);
		}

		public static String Format(this String format, object? arg0, object? arg1)
		{
			return string.Format(format, arg0, arg1);
		}

		public static String Format(this String format, IFormatProvider? provider, params object?[] args)
		{
			return string.Format(provider, format, args);
		}
	}
}
