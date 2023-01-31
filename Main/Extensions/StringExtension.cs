using System.Globalization;
using System.Text.RegularExpressions;
using System.Text;

namespace Main.Extension
{
    public static class StringExtension
    {
        public static string CreateSlug(this string text)
        {
            return text.ToLower().ReplaceTabs().ReplaceSpecialCharacters();
        }

        public static string ReplaceSpecialCharacters(this string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return Regex.Replace(stringBuilder.ToString(), @"[^\w\s]", "").Trim();
        }

        public static string ReplaceTabs(this string text)
        {
            string pattern = @"[@.\s]+";
            string replacement = "_";
            return Regex.Replace(text, pattern, replacement);
        }
    }
}
