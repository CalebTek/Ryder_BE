using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ryder.Domain.Enums.Helper
{
     public class EnumHelper
    {
        public static string GetEnumDescription(OrderStatus value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }

            return value.ToString();
        }

    }
}
