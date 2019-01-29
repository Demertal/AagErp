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
            UpdateProduct,
            AddStore,
            AddNomenclatureGroup,
            AddNomenclatureSubGroup,
            UpdateStore,
            UpdateNomenclatureGroup,
            UpdateNomenclatureSubGroup
        }

        public void ShowDialog(object view, object[] param, bool isModal, Action<bool?> closeAction)
        {
            UserControl control;
            Window wnd = new Window { SizeToContent = SizeToContent.WidthAndHeight };
            switch (view)
            {
                case ChoiceView.AddProduct:
                {
                    control = new Views.AddProduct();
                    wnd.Title = "Добавить товар";
                    Grid gr = (Grid) control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new ViewModels.AddProductViewModel();
                    break;
                }
                case ChoiceView.UpdateProduct:
                {
                    control = new Views.UpdateProduct();
                    wnd.Title = "Изменить товар";
                    Grid gr = (Grid) control.Content;
                    Border bo = (Border)gr.Children[gr.Children.Count - 1];
                    Views.AddProduct a = (Views.AddProduct)bo.Child;
                    Grid gr2 = (Grid)a.Content;
                    Button b = (Button)gr2.Children[gr2.Children.Count - 1];
                    b.Content = "Изменить";
                    gr2.Children[gr2.Children.Count - 1] = b;
                    a.Content = gr2;
                    bo.Child = a;
                    gr.Children[gr.Children.Count - 1] = bo;
                    control.Content = gr;
                    control.DataContext = new ViewModels.UpdateProductViewModel((ProductView_Result)param[0], (NomenclatureSubGroup)param[1], (NomenclatureGroup)param[2], (Store)param[3], wnd);
                    break;
                }
                case ChoiceView.AddStore:
                {
                    control = new Views.AddStore();
                    wnd.Title = "Добавить магазин";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new ViewModels.AddStoreVM(wnd);
                    break;
                }
                case ChoiceView.UpdateStore:
                {
                    control = new Views.UpdateStore();
                    wnd.Title = "Изменить магазин";
                    Grid gr = (Grid)control.Content;
                    Border bo = (Border)gr.Children[gr.Children.Count - 1];
                    Views.AddStore a = (Views.AddStore)bo.Child;
                    Grid gr2 = (Grid)a.Content;
                    Button b = (Button)gr2.Children[gr2.Children.Count - 1];
                    b.Content = "Изменить";
                    gr2.Children[gr2.Children.Count - 1] = b;
                    a.Content = gr2;
                    bo.Child = a;
                    gr.Children[gr.Children.Count - 1] = bo;
                    control.Content = gr;
                    control.DataContext = new ViewModels.UpdateStoreVM((Store)param[0], wnd);
                    break;
                }
                case ChoiceView.AddNomenclatureGroup:
                {
                    control = new Views.AddNomenclatureGroup();
                    wnd.Title = "Добавить номенклатурную группу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new ViewModels.AddNomenclatureGroupVM((Store)param[0], wnd);
                    break;
                }
                case ChoiceView.UpdateNomenclatureGroup:
                {
                    control = new Views.UpdateNomenclatureGroup();
                    wnd.Title = "Изменить номенклатурную группу";
                    Grid gr = (Grid)control.Content;
                    Border bo = (Border)gr.Children[gr.Children.Count - 1];
                    Views.AddNomenclatureGroup a = (Views.AddNomenclatureGroup)bo.Child;
                    Grid gr2 = (Grid)a.Content;
                    Button b = (Button)gr2.Children[gr2.Children.Count - 1];
                    b.Content = "Изменить";
                    gr2.Children[gr2.Children.Count - 1] = b;
                    a.Content = gr2;
                    bo.Child = a;
                    gr.Children[gr.Children.Count - 1] = bo;
                    control.Content = gr;
                    control.DataContext = new ViewModels.UpdateNomenclatureGroupVM((NomenclatureGroup)param[0], (Store)param[1], wnd);
                    break;
                }
                case ChoiceView.AddNomenclatureSubGroup:
                {
                    control = new Views.AddNomenclatureSubGroup();
                    wnd.Title = "Добавить номенклатурную подгруппу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new ViewModels.AddNomenclatureSubGroupVM((NomenclatureGroup)param[0], wnd);
                    break;
                }
                case ChoiceView.UpdateNomenclatureSubGroup:
                {
                    control = new Views.UpdateNomenclatureSubGroup();
                    wnd.Title = "Изменить номенклатурную подгруппу";
                    Grid gr = (Grid)control.Content;
                    Border bo = (Border)gr.Children[gr.Children.Count - 1];
                    Views.AddNomenclatureSubGroup a = (Views.AddNomenclatureSubGroup)bo.Child;
                    Grid gr2 = (Grid)a.Content;
                    Button b = (Button)gr2.Children[gr2.Children.Count - 1];
                    b.Content = "Изменить";
                    gr2.Children[gr2.Children.Count - 1] = b;
                    a.Content = gr2;
                    bo.Child = a;
                    gr.Children[gr.Children.Count - 1] = bo;
                    control.Content = gr;
                    control.DataContext = new ViewModels.UpdateNomenclatureSubGroupVM((NomenclatureSubGroup)param[0], (NomenclatureGroup)param[1], wnd);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(view), "Такого индекса View не существует");
            }
            StackPanel sp = new StackPanel();
            sp.Children.Add(control);
            wnd.Content = sp;
            wnd.Closed += (s, e) => closeAction(wnd.DialogResult);
            wnd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
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
