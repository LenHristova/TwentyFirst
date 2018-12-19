namespace TwentyFirst.Common.Exceptions
{
    public class InvalidInterviewIdException : InvalidIdException
    {
        private const string DefaultMessage = "Не беше намерено интервю с ID: \"{0}\".";

        public InvalidInterviewIdException(string id) : base(id, DefaultMessage)
        { }
    }
}
