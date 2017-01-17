using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Css.Extensions;

namespace Taclef.Authentication
{
    public static class StringExtensions
    {
        public static bool IsAllNumeric(this string s)
        {
            return s.All(char.IsDigit);
        }
    }
}