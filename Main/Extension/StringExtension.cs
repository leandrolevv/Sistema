namespace Main.Extension
{
    public static class StringExtension
    {
        public static string CreateSlug(this string text)
        {
            return text.ToLower().Replace(" ", "_").Replace("@", "_").Replace(".", "_");
        }
    }
}
