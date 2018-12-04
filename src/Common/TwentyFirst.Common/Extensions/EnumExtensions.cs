namespace TwentyFirst.Common.Extensions
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var member = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First();

            var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();
            if (displayAttribute == null)
            {
                return member.Name;
            }

            return displayAttribute.GetName();
        }
    }
}
