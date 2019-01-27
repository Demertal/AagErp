using System;
using System.Windows;

namespace RulezzClient
{
    public interface IUiDialogService
    {
        void ShowDialog(object view, object dataContext, bool isModal, Action<bool?> closeAction);

        MessageBoxResult ShowMessageBox(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon);

        void ShowMessageBox(
            string messageBoxText,
            string caption,
            MessageBoxButton button,
            MessageBoxImage icon,
            Action<MessageBoxResult> responseResult);
    }
}
