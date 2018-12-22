namespace TwentyFirst.Common.Exceptions
{
    public class InvalidAccountIdException : InvalidIdException
    {
        private const string DefaultMessage = "Не беше намерен акаунт с ID: \"{0}\".";

        public InvalidAccountIdException(string id) : base(id, DefaultMessage)
        { }
    }
}
