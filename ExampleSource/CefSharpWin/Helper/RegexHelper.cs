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
        public static List<string> GetMatchValue(string input,string format)
        {
            Regex reg = new Regex(format);
            MatchCollection mc = reg.Matches(input);
            List<string> stas = new List<string>();
            foreach (Match item in mc)
            {
                if (item.Groups.Count < 2)
                {
                    continue;
                }
                stas.Add(item.Groups[1].Value);
            }
            return stas;
        }
    }
}
