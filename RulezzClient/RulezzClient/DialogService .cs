using System;
using System.Windows;
using System.Windows.Controls;
using RulezzClient.ViewModels;
using RulezzClient.Views;

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
            UpdateNomenclatureSubGroup,
            ProductSelection,
            Revaluation,
            AddUnitStorage,
            PurchaseInvoice
        }

        public void ShowDialog(object view, object[] param, bool isModal, Action<bool?> closeAction)
        {
            if (!isModal)
            {
                foreach (Window w in Application.Current.Windows)
                {
                    if (w.Title == "Переоценка")
                    {
                        w.Activate();
                        return;
                    }
                }
            }
            UserControl control;
            Window wnd = new Window { SizeToContent = SizeToContent.WidthAndHeight };
            switch (view)
            {
                case ChoiceView.AddProduct:
                {
                    control = new AddProduct();
                    wnd.Title = "Добавить товар";
                    Grid gr = (Grid) control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new AddProductViewModel();
                    break;
                }
                case ChoiceView.UpdateProduct:
                {
                    control = new AddProduct();
                    wnd.Title = "Изменить товар";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Изменить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                        control.DataContext = new UpdateProductViewModel((ProductView)param[0], wnd);
                    break;
                }
                case ChoiceView.AddStore:
                {
                    control = new AddStore();
                    wnd.Title = "Добавить магазин";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new AddStoreVM(wnd);
                    break;
                }
                case ChoiceView.UpdateStore:
                {
                    control = new AddStore();
                    wnd.Title = "Изменить магазин";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Изменить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new UpdateStoreVM((Store)param[0], wnd);
                    break;
                }
                case ChoiceView.AddNomenclatureGroup:
                {
                    control = new AddNomenclatureGroup();
                    wnd.Title = "Добавить номенклатурную группу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new AddNomenclatureGroupVM((Store)param[0], wnd);
                    break;
                }
                case ChoiceView.UpdateNomenclatureGroup:
                {
                    control = new AddNomenclatureGroup();
                    wnd.Title = "Изменить номенклатурную группу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Изменить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new UpdateNomenclatureGroupVM((NomenclatureGroup)param[0], (Store)param[1], wnd);
                    break;
                }
                case ChoiceView.AddNomenclatureSubGroup:
                {
                    control = new AddNomenclatureSubGroup();
                    wnd.Title = "Добавить номенклатурную подгруппу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new AddNomenclatureSubGroupVM((NomenclatureGroup)param[0], wnd);
                    break;
                }
                case ChoiceView.UpdateNomenclatureSubGroup:
                {
                    control = new AddNomenclatureSubGroup();
                    wnd.Title = "Изменить номенклатурную подгруппу";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Изменить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new UpdateNomenclatureSubGroupVM((NomenclatureSubGroup)param[0], (NomenclatureGroup)param[1], wnd);
                    break;
                }
                case ChoiceView.ProductSelection:
                {
                    control = new ShowProduct();
                    wnd.Title = "Выбрать товар";
                    StackPanel s = (StackPanel)control.Content;
                    if (param[0].GetType().ToString() == "RulezzClient.ViewModels.RevaluationVM")
                    {
                        s.DataContext = new ShowProductViewModel((RevaluationVM) param[0], wnd);
                    }
                    else if (param[0].GetType().ToString() == "RulezzClient.ViewModels.PurchaseInvoiceVM")
                    {
                        s.DataContext = new ShowProductViewModel((PurchaseInvoiceVM)param[0], wnd);
                    }
                    control.Content = s;
                    break;
                }
                case ChoiceView.Revaluation:
                {
                    control = new Revaluation();
                    wnd.Title = "Переоценка";
                    StackPanel s = (StackPanel)control.Content;
                    s.DataContext = new RevaluationVM(wnd);
                    control.Content = s;
                    break;
                }
                case ChoiceView.AddUnitStorage:
                {
                    control = new AddUnitStorage();
                    wnd.Title = "Добавить ед. хранения";
                    Grid gr = (Grid)control.Content;
                    Button b = (Button)gr.Children[gr.Children.Count - 1];
                    b.Content = "Добавить";
                    gr.Children[gr.Children.Count - 1] = b;
                    control.Content = gr;
                    control.DataContext = new AddUnitStorageVM(wnd);
                    break;
                }
                case ChoiceView.PurchaseInvoice:
                {
                    control = new PurchaseInvoice();
                    wnd.Title = "Прием товара";
                    StackPanel s = (StackPanel)control.Content;
                    s.DataContext = new PurchaseInvoiceVM(wnd);
                    control.Content = s;
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
