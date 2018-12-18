namespace TwentyFirst.Common
{
    using System;

    public static class CoreValidator
    {
        public static void ThrowIfNull(object obj, Exception exception = null)
        {
            if (obj == null)
            {
                throw exception ?? new ArgumentNullException(nameof(obj));
            }
        }
    }
}
