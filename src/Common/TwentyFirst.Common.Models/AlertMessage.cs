namespace TwentyFirst.Common.Models
{
    using Enums;

    public class AlertMessage
    {
        public AlertMessage(AlertMessageLevel alertMessageLevel, string message)
        {
            this.Level = alertMessageLevel;
            this.Message = message;
        }
        public AlertMessageLevel Level { get; }

        public string MessageLevelToShow
            => this.Level == AlertMessageLevel.Success
                ? "alert-success"
                : "alert-danger";

        public string Message { get; }
    }
}
