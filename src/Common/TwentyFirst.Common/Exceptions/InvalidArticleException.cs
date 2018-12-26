namespace TwentyFirst.Common.Exceptions
{
    public class InvalidArticleException : BaseTwentyFirstException
    {
        public override string Message => "Невалидна новина.";
    }
}
