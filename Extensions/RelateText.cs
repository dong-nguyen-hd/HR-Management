using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace HR_Management.Extensions
{
    public static class RelateText
    {
        public static string RemoveSpaceCharacter(this string source)
        {
            source = Regex.Replace(source.Trim(), @"\s{2,}", " ");

            return source;
        }

        public static string ConcatenateWithComma(this List<int> source)
        {
            int countOfList = source.Count;
            StringBuilder text = new StringBuilder();

            for (int i = 0; i < countOfList; i++)
            {
                if (i == countOfList - 1)
                {
                    text.Append($"{source[i]}");
                    break;
                }

                text.Append($"{source[i]}, ");
            }

            return text.ToString();
        }
    }
}
