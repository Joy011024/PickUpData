using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace CefSharpWin
{
    public class RegexHelper
    {
        public static string GetMatchValue(string input,string format)
        {
            Regex reg = new Regex(format);
            MatchCollection mc = reg.Matches(input);
            return string.Empty;
        }
    }
}
