using System.Collections.Generic;
using System.Linq;

namespace HR_Management.Extensions
{
    static public class RelateElementOfList
    {
        public static List<T> RemoveDuplicate<T>(this List<T> source)
        {
            HashSet<T> tempHashSet = new HashSet<T>(source);
            return tempHashSet.ToList();
        }
    }
}
