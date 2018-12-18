namespace TwentyFirst.Common.Exceptions
{
    public class InvalidCategoryIdException : InvalidIdException
    {
        private const string DefaultMessage = "Не беше намерена категория с ID: \"{0}\".";

        public InvalidCategoryIdException(string id) : base(id, DefaultMessage)
        { }
    }
}
