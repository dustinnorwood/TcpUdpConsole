using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Net;
using System.Net.Sockets;

namespace TcpUdpConsole
{
    public class Utility
    {
        public static int GetTickCount() { return System.Environment.TickCount; }
        public static int GetElapsedTime(int pastTicks) { return (int)(0xFFFFFFFF - pastTicks + System.Environment.TickCount); }
        public static int GetElapsedTime(int ticks, int pastTicks) { return (int)(0xFFFFFFFF - pastTicks + ticks); }

        public static string GetVersion()
        {
            Assembly assem = Assembly.GetExecutingAssembly();
            AssemblyName aName = assem.GetName();
            return aName.Version.ToString();
        }

        public static string ConvertToAraibNumber(string s)
        {
            string arabic = "٠١٢٣٤۵٦٧٨٩";
            StringBuilder sb = new StringBuilder(s.Length);
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= '0' && c <= '9')
                {
                    sb.Append(arabic[c - '0']);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        public static string ConvertArabic(string s)
        {
#if WINCE
			string ret = "";
			for (int k = 0; k < s.Length; k++) {
				ret = InsertArabic(ret, s[k]);
			}
			return ret;
#else
            return s;
#endif
        }

        public static string InsertArabic(string s, char c)
        {
            string ret = s;
            if (c >= (char)0x0600 && c <= (char)0x06FF)
            {
                switch (c)
                {
                    case (char)0x622:
                        c = (char)0xFE81;
                        break;
                    case (char)0x623:
                        c = (char)0xFE83;
                        break;
                    case (char)0x624:
                        c = (char)0xFE85;
                        break;
                    case (char)0x625:
                        c = (char)0xFE87;
                        break;
                    case (char)0x626:
                        c = (char)0xFE89;
                        break;
                    case (char)0x627:
                        c = (char)0xFE8D;
                        break;
                    case (char)0x628:
                        c = (char)0xFE8F;
                        break;
                    case (char)0x629:
                        c = (char)0xFE93;
                        break;
                    case (char)0x62A:
                        c = (char)0xFE95;
                        break;
                    case (char)0x62B:
                        c = (char)0xFE99;
                        break;
                    case (char)0x62C:
                        c = (char)0xFE9D;
                        break;
                    case (char)0x62D:
                        c = (char)0xFEA1;
                        break;
                    case (char)0x62E:
                        c = (char)0xFEA5;
                        break;
                    case (char)0x62F:
                        c = (char)0xFEA9;
                        break;
                    case (char)0x630:
                        c = (char)0xFEAB;
                        break;
                    case (char)0x631:
                        c = (char)0xFEAD;
                        break;
                    case (char)0x632:
                        c = (char)0xFEAF;
                        break;
                    case (char)0x633:
                        c = (char)0xFEB1;
                        break;
                    case (char)0x634:
                        c = (char)0xFEB5;
                        break;
                    case (char)0x635:
                        c = (char)0xFEB9;
                        break;
                    case (char)0x636:
                        c = (char)0xFEBD;
                        break;
                    case (char)0x637:
                        c = (char)0xFEC1;
                        break;
                    case (char)0x638:
                        c = (char)0xFEC5;
                        break;
                    case (char)0x639:
                        c = (char)0xFEC9;
                        break;
                    case (char)0x63A:
                        c = (char)0xFECD;
                        break;
                    case (char)0x641:
                        c = (char)0xFED1;
                        break;
                    case (char)0x642:
                        c = (char)0xFED5;
                        break;
                    case (char)0x643:
                        c = (char)0xFED9;
                        break;
                    case (char)0x644:
                        c = (char)0xFEDD;
                        break;
                    case (char)0x645:
                        c = (char)0xFEE1;
                        break;
                    case (char)0x646:
                        c = (char)0xFEE5;
                        break;
                    case (char)0x647:
                        c = (char)0xFEE9;
                        break;
                    case (char)0x648:
                        c = (char)0xFEED;
                        break;
                    case (char)0x649:
                    case (char)0x6CC:
                        c = (char)0xFEEF;
                        break;
                    case (char)0x64A:
                        c = (char)0xFEF1;
                        break;
                    case (char)0x671:
                        c = (char)0xFB50;
                        break;
                    case (char)0x67A:
                        c = (char)0xFB5E;
                        break;
                    case (char)0x67B:
                        c = (char)0xFB52;
                        break;
                    case (char)0x67E:
                        c = (char)0xFB56;
                        break;
                    case (char)0x67F:
                        c = (char)0xFB62;
                        break;
                    case (char)0x680:
                        c = (char)0xFB5A;
                        break;
                    case (char)0x683:
                        c = (char)0xFB76;
                        break;
                    case (char)0x684:
                        c = (char)0xFB72;
                        break;
                    case (char)0x686:
                        c = (char)0xFB7A;
                        break;
                    case (char)0x687:
                        c = (char)0xFB7E;
                        break;
                    case (char)0x68A:
                        c = (char)0xFE91;
                        break;
                    case (char)0x68C:
                        c = (char)0xFB84;
                        break;
                    case (char)0x68D:
                        c = (char)0xFBFE;
                        break;
                    case (char)0x68E:
                        c = (char)0xFB86;
                        break;
                    case (char)0x698:
                        c = (char)0xFB8A;
                        break;
                    case (char)0x6A9:
                        c = (char)0xFB8E;
                        break;
                    case (char)0x6AF:
                        c = (char)0xFB92;
                        break;
                }
            }
            if (c == ' ' || (c >= (char)0xFB50 && c <= (char)0xFBFF) || (c >= (char)0xFE81 && c <= (char)0xFEFF))
            {
                int k = ret.Length - 1;
                //while (k >= 0 && (s[k] == ' ' || (s[k] >= (char)0xFB50 && s[k] <= (char)0xFBFF) || (s[k] >= (char)0xFE81 && s[k] <= (char)0xFEFF))) k--;
                k = -1;
                if (k == ret.Length - 1)
                    ret += c;
                else if (c == ' ')
                {
                    ret = ret.Insert(k + 1, string.Format("{0}", c));
                }
                else if (c == (char)0xfe8d && ret[k + 1] == (char)0xfedd)
                {
                    ret = ret.Remove(k + 1, 1);
                    ret = ret.Insert(k + 1, ((char)0xfefb).ToString());
                }
                else if (c == (char)0xfe8d && ret[k + 1] == (char)0xfede)
                {
                    ret = ret.Remove(k + 1, 1);
                    ret = ret.Insert(k + 1, ((char)0xfefc).ToString());
                }
                else if ((ret[k + 1] >= (char)0xFE95 && ret[k + 1] <= (char)0xFEA8 && (int)(ret[k + 1] - (char)0xFE95) % 4 == 0)
                    || (ret[k + 1] >= (char)0xFEB1 && ret[k + 1] <= (char)0xFEEC && (int)(ret[k + 1] - (char)0xFEB1) % 4 == 0)
                    || (ret[k + 1] >= (char)0xFB52 && ret[k + 1] <= (char)0xFB81 && (int)(ret[k + 1] - (char)0xFB52) % 4 == 0)
                    || (ret[k + 1] >= (char)0xFB8E && ret[k + 1] <= (char)0xFB9D && (int)(ret[k + 1] - (char)0xFB8E) % 4 == 0)
                    || ret[k + 1] == (char)0xFEEF || ret[k + 1] == (char)0xFEF1 || ret[k + 1] == (char)0xFE8F || ret[k + 1] == (char)0xFE89)
                {
                    char ins = (char)((int)ret[k + 1] + 2);
                    if (ins == (char)0xFEF1)
                        ins = (char)0xFEF3;
                    c = (char)((int)c + 1);
                    string cut = string.Format("{0}{1}", c, ins);
                    ret = ret.Remove(k + 1, 1);
                    ret = ret.Insert(k + 1, cut);
                }
                else if ((ret[k + 1] >= (char)0xFE95 && ret[k + 1] <= (char)0xFEA8 && (int)(ret[k + 1] - (char)0xFE95) % 4 == 1)
                    || (ret[k + 1] >= (char)0xFEB1 && ret[k + 1] <= (char)0xFEEC && (int)(ret[k + 1] - (char)0xFEB1) % 4 == 1)
                    || (ret[k + 1] >= (char)0xFB52 && ret[k + 1] <= (char)0xFB81 && (int)(ret[k + 1] - (char)0xFB52) % 4 == 1)
                    || (ret[k + 1] >= (char)0xFB8E && ret[k + 1] <= (char)0xFB9D && (int)(ret[k + 1] - (char)0xFB8E) % 4 == 1)
                    || ret[k + 1] == (char)0xFEF0 || ret[k + 1] == (char)0xFEF2 || ret[k + 1] == (char)0xFE90 || ret[k + 1] == (char)0xFE8A)
                {
                    char ins = (char)((int)ret[k + 1] + 2);
                    if (ins == (char)0xFEF2)
                        ins = (char)0xFEF4;
                    c = (char)((int)c + 1);
                    string cut = string.Format("{0}{1}", c, ins);
                    ret = ret.Remove(k + 1, 1);
                    ret = ret.Insert(k + 1, cut);
                }
                else ret = ret.Insert(k + 1, string.Format("{0}", c));
            }
            else
            {
                int k = 0;
                while (k < s.Length && !(s[k] >= (char)0xFB00 && s[k] <= (char)0xFEFF)) k++;
                ret = ret.Insert(k, string.Format("{0}", c));
            }
            return ret;
        }

        public static int HashString(string s, int radix, int shift)
        {
            int hash = 0;
            for (int i = 0; i < s.Length; i++)
            {
                hash <<= shift;
                hash += (int)s[i];
                hash %= radix;
            }
            return hash;
        }

        public static bool TryParse(string s, out int i)
        {
            try
            {
                i = int.Parse(s);
                return true;
            }
            catch
            {
                i = 0;
                return false;
            }
        }

        public static bool TryParse(string s, System.Globalization.NumberStyles style, out int i)
        {
            try
            {
                i = int.Parse(s, style);
                return true;
            }
            catch
            {
                i = 0;
                return false;
            }
        }
        //		public static void SetStatus(string text, WarningLevel level)
        //		{
        //		}
    }
}
