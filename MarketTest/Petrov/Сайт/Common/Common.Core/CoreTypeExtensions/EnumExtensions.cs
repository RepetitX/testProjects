using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Common.Core
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum en)
        {
            var type = en.GetType();
            var memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                var attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }

            return en.ToString();
        }

        public static IReadOnlyList<T> ToList<T>()
        {
            var enumType = typeof(T);

            if (enumType.IsEnum)
                return Enum.GetValues(enumType).Cast<T>().ToList();

            return null;
        }
    }
}