namespace TwentyFirst.Common.Exceptions
{
    public class InvalidCategoryOrderException : BaseTwentyFirstException
    {
        public override string Message => "Невалидна подредба на категории.";
    }
}
