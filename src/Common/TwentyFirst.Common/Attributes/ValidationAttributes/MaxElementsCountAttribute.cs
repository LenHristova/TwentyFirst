namespace TwentyFirst.Common.Attributes.ValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    /// <inheritdoc />
    /// <summary>
    /// Specify maximum elements count in collection
    /// </summary>
    public class MaxElementsCountAttribute : ValidationAttribute
    {
        private readonly int maxElementsCount;
        private const string DefaultErrorMessage = "Max elements count is {0}.";

        public MaxElementsCountAttribute(int maxElementsCount)
        {
            this.maxElementsCount = maxElementsCount;
        }

        public override bool IsValid(object value)
            => value is ICollection enumerable && enumerable.Count <= this.maxElementsCount;

        public override string FormatErrorMessage(string name)
            => string.Format(this.ErrorMessage ?? DefaultErrorMessage, this.maxElementsCount);
    }
}
