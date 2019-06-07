using System;
using ModelModul.Product;

namespace ModelModul.SalesGoods
{
    public class SalesInfosViewModel : ProductViewModel
    {
        private SalesInfos _salesInfo = new SalesInfos();

        public SalesInfos SalesInfo
        {
            get => _salesInfo;
            set => SetProperty(ref _salesInfo, value);
        }

        public override Products Product
        {
            get => SalesInfo.Products;
            set
            {
                SalesInfo.Products = value;
                RaisePropertyChanged();
            }
        }

        public override int Id
        {
            get => SalesInfo.Id;
            set
            {
                SalesInfo.Id = value;
                RaisePropertyChanged();
            }
        }
        public new int Count
        {
            get => SalesInfo.Count;
            set
            {
                SalesInfo.Count = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }
        public decimal SellingPrice
        {
            get => SalesInfo.SellingPrice;
            set
            {
                SalesInfo.SellingPrice = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }
        public int IdProduct
        {
            get => SalesInfo.IdProduct;
            set
            {
                SalesInfo.IdProduct = value;
                RaisePropertyChanged();
            }
        }
        public int IdSalesReport
        {
            get => SalesInfo.IdSalesReport;
            set
            {
                SalesInfo.IdSalesReport = value;
                RaisePropertyChanged();
            }
        }
        public Nullable<int> IdSerialNumber
        {
            get => SalesInfo.IdSerialNumber;
            set
            {
                SalesInfo.IdSerialNumber = value;
                RaisePropertyChanged();
                RaisePropertyChanged("IsValidate");
            }
        }

        public SerialNumbers SerialNumber
        {
            get => SalesInfo.SerialNumbers;
            set
            {
                SalesInfo.SerialNumbers = value;
                RaisePropertyChanged();
            }
        }

        public bool IsValidate => SellingPrice != 0 && Count > 0 && (SerialNumber.Id != 0 || WarrantyPeriod.Period == "Нет");
    }
}
