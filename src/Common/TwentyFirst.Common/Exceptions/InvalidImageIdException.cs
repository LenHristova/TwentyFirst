namespace TwentyFirst.Common.Exceptions
{
    public class InvalidImageIdException : InvalidIdException
    {
        private const string DefaultMessage = "Не беше намерена снимка с ID: \"{0}\".";

        public InvalidImageIdException(string id) : base(id, DefaultMessage)
        {
        }
    }
}
