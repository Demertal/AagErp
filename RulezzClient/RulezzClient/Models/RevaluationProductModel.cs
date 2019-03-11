//using System;
//using RulezzClient.ViewModels;

//namespace RulezzClient.Models
//{
////    class RevaluationProductModel : Products, IEquatable<RevaluationProductModel>
////    {
////        public RevaluationProductModel()
////        {
////            OldSalesPrice = 0;
////        }

////        public RevaluationProductModel(Products obj)
////        {
////            Id = obj.Id;
////            Title = obj.Title;
////            VendorCode = obj.VendorCode;
////            Barcode = obj.Barcode;
////            PurchasePrice = obj.PurchasePrice;
////            OldPurchasePrice = obj.PurchasePrice;
////            OldSalesPrice = obj.SalesPrice;
////            SalesPrice = obj.SalesPrice;
////            IdUnitStorage = obj.IdUnitStorage;
////            IdExchangeRate = obj.IdExchangeRate;
////            IdWarrantyPeriod = obj.IdWarrantyPeriod;
////        }

////        public decimal OldSalesPrice { get; set; }

////        public decimal OldPurchasePrice { get; set; }

////        public bool Equals(RevaluationProductModel obj)
////        {
////            return obj != null && Id == obj.Id;
////        }

////        public static explicit operator RevaluationProductModel(PurchaseInvoiceProductVM obj)
////        {
////            return new RevaluationProductModel
////            {
////                Id = obj.Id,
////                Title = obj.Title,
////                VendorCode = obj.VendorCode,
////                Barcode = obj.Barcode,
////                PurchasePrice = obj.PurchasePrice,
////                OldPurchasePrice = obj.OldPurchasePrice,
////                OldSalesPrice = obj.SalesPrice,
////                SalesPrice = obj.SalesPrice,
////                IdUnitStorage = obj.IdUnitStorage,
////                IdExchangeRate = obj.IdExchangeRate,
////                IdWarrantyPeriod = obj.IdWarrantyPeriod
////            };
////        }
////    }
//}
