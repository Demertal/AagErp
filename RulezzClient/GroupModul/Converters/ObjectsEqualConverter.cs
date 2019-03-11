﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
