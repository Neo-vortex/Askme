using System.Linq;

namespace VaderSharp
{
    internal static class Extensions
    {
        public static bool IsUpper(this string word)
        {
            return !word.Any(char.IsLower);
        }
        public static string RemovePunctuation(this string word)
        {
            return new string(word.Where(c => !char.IsPunctuation(c)).ToArray());
        }
    }
}
