namespace TwentyFirst.Common.Exceptions
{
    using System;

    public class InvalidIdException : Exception, ITwentyFirstException
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
