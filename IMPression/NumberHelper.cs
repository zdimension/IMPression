using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMPression
{
    public class NumberHelper
    {
        public static string Superscript(string n, bool rp = false)
        {
            string ret = "";
            foreach (char c1 in n)
            {
                switch (c1)
                {
                    case '+':
                        if (!rp) ret += '\x207A';
                        continue;
                    case '-':
                        ret += '\x207B';
                        continue;
                }

                int c = int.Parse("" + c1);

                switch (c)
                {
                    case 0:
                        ret += '\x2070';
                        break;
                    case 1:
                        ret += '\x00B9';
                        break;
                    case 2:
                        ret += '\x00B2';
                        break;
                    case 3:
                        ret += '\x00B3';
                        break;
                    default:
                        ret += (char)((c - 4) + '\x2074');
                        break;
                }
            }
            return ret;
        }

        public static string Subscript(string expr)
        {
            return expr.Where(char.IsDigit).Aggregate("", (current, c) => current + (char)('₀' + int.Parse(c.ToString())));
        }
    }
}
