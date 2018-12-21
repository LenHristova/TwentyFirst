namespace TwentyFirst.Common.Exceptions
{
    public class InvalidIdException : BaseTwentyFirstException
    {
        private readonly string givenId;
        protected readonly string message;

        public InvalidIdException(string id, string message)
        {
            this.givenId = id;
            this.message = message;
        }

        public override string Message => string.Format(this.message, this.givenId);
    }
}
