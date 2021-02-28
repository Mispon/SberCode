using System;
using System.ComponentModel;

namespace Utils.Extensions {
    public static class EnumExtensions {
        /// <summary>
        /// Возвращает описание перечисления по значению
        /// </summary>
        public static string GetDescription(this Enum value) 
        {
            var descriptionAttribute = GetAttribute<DescriptionAttribute>(value);
            return descriptionAttribute != null 
                ? descriptionAttribute.Description 
                : value.ToString();
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute {
            var fieldInfo = value.GetType().GetField(value.ToString());
            return (T) Attribute.GetCustomAttribute(fieldInfo, typeof(T));
        }
    }
}