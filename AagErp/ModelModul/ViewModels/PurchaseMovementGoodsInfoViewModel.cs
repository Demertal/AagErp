using System.Linq;
using ModelModul.Models;

namespace ModelModul.ViewModels
{
    public class PurchaseMovementGoodsInfoViewModel : MovementGoodsInfo
    {
        private decimal _count;
        public override decimal Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
                OnCountChanged();
                OnPropertyChanged(nameof(IsValid));
            }
        }

        private void OnCountChanged()
        {
            if (Product == null || !Product.KeepTrackSerialNumbers) return;
            int count = (int)Count - Product.SerialNumbersCollection.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Product.SerialNumbersCollection.Add(new SerialNumber());
                }
            }
            else if (count < 0)
            {
                for (int i = 0; i > count; i--)
                {
                    var temp = Product.SerialNumbersCollection.FirstOrDefault(s =>
                        string.IsNullOrEmpty(s.Value));
                    Product.SerialNumbersCollection.Remove(temp ?? Product.SerialNumbersCollection.Last());
                }
            }
        }
    }
}
