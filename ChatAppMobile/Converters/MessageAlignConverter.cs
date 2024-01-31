using Microsoft.Maui.Controls.Shapes;
using Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppMobile.Converters
{

    public class MessageColorConverter : IValueConverter
    {


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isMe = (bool)value!;

            if (!isMe)
                return Color.FromHex("#d4e0f1");
            else
                return Color.FromHex("#f1eed4");
                
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageBorderConverter : IValueConverter
    {


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isMe = (bool)value!;

            if (!isMe)
                return new RoundRectangle
                {
                    CornerRadius = new CornerRadius(0, 20, 20, 20)
                };
            else
                return new RoundRectangle
                {
                    CornerRadius = new CornerRadius(20, 0, 20, 20)
                };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class MessageGridColumnConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var column = new ColumnDefinitionCollection();
            bool isMe = (bool)value!;

            if (!isMe)
            {
                column.Add(new ColumnDefinition(new GridLength(80, GridUnitType.Star)));
                column.Add(new ColumnDefinition(new GridLength(20, GridUnitType.Star)));
            }
            else
            {
                column.Add(new ColumnDefinition(new GridLength(20, GridUnitType.Star)));
                column.Add(new ColumnDefinition(new GridLength(80, GridUnitType.Star)));
            }
            return column;
        }


        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class MessageAlignConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            bool isMe = (bool)value!;
            return !isMe ? 0 : 1;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class MessageTypeConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            MessageType valueType = (MessageType)value!;
            MessageType paramType = (MessageType)parameter!;
            if (valueType == paramType)
                return true;
            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
