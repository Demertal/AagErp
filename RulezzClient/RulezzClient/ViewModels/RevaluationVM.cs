﻿using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;

namespace RulezzClient.ViewModels
{
    class RevaluationVM : BindableBase
    {
        private readonly IUiDialogService _dialogService = new DialogService();

        private bool _bringPurchasePrice;

        private readonly ReadOnlyObservableCollection<RevaluationProductModel> _rproducts;

        public ReadOnlyObservableCollection<RevaluationProductModel> Products => _rproducts;

        public RevaluationVM(Window wnd)
        {
            _rproducts = new ReadOnlyObservableCollection<RevaluationProductModel>(AllProduct);
            AddProductComand = new DelegateCommand(() =>
            {
                object []param = new object[1];
                param[0] = this;
                _dialogService.ShowDialog(DialogService.ChoiceView.ProductSelection, param, true, b => { });
            });
            RevaluationComand = new DelegateCommand(() =>
            {
                bool sucsuccess = false;
                using (StoreEntities db = new StoreEntities())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in AllProduct)
                            {
                                Product product = db.Product.Find(item.Id);
                                product.SalesPrice = item.SalesPrice;
                                db.Entry(product).State = EntityState.Modified;
                                RevaluationProduct rev = new RevaluationProduct
                                {
                                    IdProduct = product.Id,
                                    Date = DateTime.Today,
                                    NewSalesPrice = item.SalesPrice,
                                    OldSalesPrice = item.OldSalesPrice
                                };
                                db.RevaluationProduct.Add(rev);
                            }
                            db.SaveChanges();
                            transaction.Commit();
                            sucsuccess = true;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        if (!sucsuccess) return;
                        MessageBox.Show("Товар переоценен", "Успех", MessageBoxButton.OK);
                        wnd.Close();
                    }
                }
            });
        }

        public ObservableCollection<RevaluationProductModel> AllProduct { get; } = new ObservableCollection<RevaluationProductModel>();

        public bool BringPurchasePrice
        {
            get => _bringPurchasePrice;
            set
            {
                _bringPurchasePrice = value;
                RaisePropertyChanged();
                RaisePropertyChanged("Products");
            }
        }

        public DelegateCommand AddProductComand { get; }
        public DelegateCommand RevaluationComand { get; }
    }
}