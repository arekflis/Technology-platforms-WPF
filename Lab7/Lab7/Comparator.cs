using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab7
{
    [Serializable]
    public class Comparator : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if(x != null && y != null)
            {
                if (x.Length > y.Length) return 1;
                if (x.Length < y.Length) return -1;
                return x.CompareTo(y);
                
            }
            if (x == null && y == null) return 0;
            if (x == null) return -1;
            return 1;
        }
    }
}

