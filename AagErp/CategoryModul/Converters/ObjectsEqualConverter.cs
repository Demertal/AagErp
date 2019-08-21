using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace GroupModul.Converters
{
    class ObjectsEqualConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type tt, object p, CultureInfo ci) => values[0] == values[1] ? true : false;

        public object[] ConvertBack(object value, Type[] tt, object p, CultureInfo ci) => throw new NotImplementedException();

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
