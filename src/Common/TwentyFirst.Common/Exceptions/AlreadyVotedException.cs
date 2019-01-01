namespace TwentyFirst.Common.Exceptions
{
    public class AlreadyVotedException : BaseTwentyFirstException
    {
        public override string Message => "Вече сте гласували за тази анкета.";
    }
}
