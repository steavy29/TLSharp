namespace Telegram.Net.SchemaGen
{
    public static class Extensions
    {
        public static string Capitalize(this string value)
        {
            if (value.Length == 0)
                return value;

            var chars = value.ToCharArray();
            chars[0] = char.ToUpperInvariant(chars[0]);

            return new string(chars);
        }
    }
}
