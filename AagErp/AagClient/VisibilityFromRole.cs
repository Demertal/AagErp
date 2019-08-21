using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using AagClient.ViewModels;

namespace AagClient
{
    public class VisibilityFromRole: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Roles role)) return Visibility.Visible;
            if(role == Roles.Seller) return Visibility.Collapsed;
            if (!(parameter is string par)) return Visibility.Visible;
            if (par == "CashierWorkplace")
            {
                return Visibility.Collapsed;
            }
            switch (role)
            {
                case Roles.OldestSalesman:
                    switch (par)
                    {
                        case "ShowProduct":
                            return Visibility.Collapsed;
                        case "Directories":
                            return Visibility.Collapsed;
                        case "Reports":
                            return Visibility.Collapsed;
                    }

                    break;
                case Roles.Admin:
                    switch (par)
                    {
                        case "WorkProduct":
                            return Visibility.Collapsed;
                        case "ShowWarranties":
                            return Visibility.Collapsed;
                    }

                    break;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
