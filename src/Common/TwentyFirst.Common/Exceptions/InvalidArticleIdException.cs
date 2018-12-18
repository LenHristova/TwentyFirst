namespace TwentyFirst.Common.Exceptions
{
    public class InvalidArticleIdException : InvalidIdException
    {
        private const string DefaultMessage = "Не беше намерена новина с ID: \"{0}\".";

        public InvalidArticleIdException(string id) : base(id, DefaultMessage)
        { }
    }
}
