namespace TwentyFirst.Common.Attributes.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <inheritdoc />
    /// <summary>
    /// Specify minimum elements count in collection
    /// </summary>
    public class MinElementsCountAttribute : ValidationAttribute
    {
        private readonly int minElementsCount;
        private const string DefaultErrorMessage = "Min elements count is {0}.";

        public MinElementsCountAttribute(int minElementsCount)
        {
            this.minElementsCount = minElementsCount;
        }

        public override bool IsValid(object value)
            => value is ICollection enumerable && enumerable.Count >= this.minElementsCount;

        public override string FormatErrorMessage(string name)
            => string.Format(this.ErrorMessage ?? DefaultErrorMessage, this.minElementsCount);
    }
}
