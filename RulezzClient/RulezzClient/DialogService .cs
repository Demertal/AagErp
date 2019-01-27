using System;
using System.Windows;
using System.Windows.Controls;

namespace RulezzClient
{
    class DialogService: IUiDialogService
    {
        public enum ChoiceView : byte
        {
            AddProduct,
            UpdateProduct
        }

        public void ShowDialog(object view, object dataContext, bool isModal, Action<bool?> closeAction)
        {
            UserControl control;
            Window wnd = new Window { SizeToContent = SizeToContent.WidthAndHeight };
            switch (view)
            {
                case ChoiceView.AddProduct:
                    control = new Views.AddProduct();
                    wnd.Title = "Добавить товар";
                    break;
                case ChoiceView.UpdateProduct:
                    control = new Views.UpdateProduct();
                    wnd.Title = "Изменить товар";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(view), "Такого индекса View не существует");
            }

            control.DataContext = dataContext;
            StackPanel sp = new StackPanel();
            sp.Children.Add(control);
            wnd.Content = sp;
            wnd.Closed += (s, e) => closeAction(wnd.DialogResult);
            if (isModal) wnd.ShowDialog();
            else wnd.Show();
        }

        public MessageBoxResult ShowMessageBox(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon)
        {
            return MessageBox.Show(messageBoxText, caption, button, icon);
        }

        public void ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon,
            Action<MessageBoxResult> responseResult)
        {
            var result = ShowMessageBox(messageBoxText, caption, button, icon);
            responseResult?.Invoke(result);
        }
    }
}
