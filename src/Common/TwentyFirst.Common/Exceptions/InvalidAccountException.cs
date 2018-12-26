namespace TwentyFirst.Common.Exceptions
{
    public class InvalidAccountException : BaseTwentyFirstException
    {
        public override string Message => "Невалиден акаунт.";
    }
}
