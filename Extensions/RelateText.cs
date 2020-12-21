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
    }
}
