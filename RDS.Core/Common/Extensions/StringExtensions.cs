namespace RDS.Core.Common.Extensions;



public static class StringExtensions
{
    public static string Capitalize(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        return textInfo.ToTitleCase(text.ToLower());
    }
    
    public static string CapitalizeFirstLetter(this string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            return text;
        }

        return char.ToUpper(text[0]) + text[1..].ToLower();
    }
}