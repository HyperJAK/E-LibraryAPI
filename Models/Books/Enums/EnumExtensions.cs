namespace ELib_IDSFintech_Internship.Models.Books.Enums
{
    public static class EnumExtensions
    {

        public static string enumToString(this BookFormatType format) {

            return format.ToString();
        }

        public static BookFormatType stringToFormatType(string value)
        {
            if (Enum.TryParse(value, out BookFormatType result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid value: {value}", nameof(value));
            }
        }

        public static string enumToString(this BookType type)
        {

            return type.ToString();
        }

        public static BookType stringToBookType(string value)
        {
            if (Enum.TryParse(value, out BookType result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid value: {value}", nameof(value));
            }
        }

        public static string EnumToString(this BookTagType tag)
        {
            return tag.ToString();
        }

        public static BookTagType stringToTagType(string value)
        {
            if (Enum.TryParse(value, out BookTagType result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid value: {value}", nameof(value));
            }
        }

        public static string enumToString(this BookGenreType genre)
        {
            return genre.ToString();
        }

        public static BookGenreType stringToGenreType(string value)
        {
            if (Enum.TryParse(value, out BookGenreType result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException($"Invalid value: {value}", nameof(value));
            }
        }

    }
}
