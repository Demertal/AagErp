using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;
using ModelModul;
using ModelModul.Counterparty;
using ModelModul.ExchangeRate;
using ModelModul.Group;
using ModelModul.Product;
using ModelModul.PurchaseGoods;
using ModelModul.Report;
using ModelModul.RevaluationProduct;
using ModelModul.SalesGoods;
using ModelModul.SerialNumber;
using ModelModul.UnitStorage;
using ModelModul.WarrantyPeriod;
using NUnit.Framework;

namespace UnitTest
{
    [TestFixture]
    public class TestClass
    {
        private TransactionScope _transactionScope;

        [SetUp]
        public void Setup()
        {
            SqlConnectionStringBuilder sqlConnection = new SqlConnectionStringBuilder
            {
                DataSource = "(localdb)\\MSSQLLocalDB",
                InitialCatalog = "AutomationAccountingGoods",
                ApplicationName = "EntityFramework",
                IntegratedSecurity = true
            };
            EntityConnectionStringBuilder connectionString = new EntityConnectionStringBuilder
            {
                Metadata =
                    "res://*/AutomationAccountingGoodsModel.csdl|res://*/AutomationAccountingGoodsModel.ssdl|res://*/AutomationAccountingGoodsModel.msl",
                Provider = "System.Data.SqlClient",
                ProviderConnectionString = sqlConnection.ConnectionString
            };
            AutomationAccountingGoodsEntities.ConnectionString = connectionString.ConnectionString;
            _transactionScope = new TransactionScope(TransactionScopeOption.Required);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (UnitStorages, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Groups, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (WarrantyPeriods, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Products, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (ExchangeRates, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Counterparties, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PurchaseReports, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PurchaseInfos, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (SalesReports, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (SalesInfos, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Stores, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (CountProducts, RESEED, 0)");
                db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (SerialNumbers, RESEED, 0)");
                db.SaveChanges();
                db.Database.Connection.Close();
            }
        }

        [Test]
        public void TestAddPurchaseGoods()
        {
            Init();

            DbSetPurchaseGoods dbSetPurchaseGoods = new DbSetPurchaseGoods();

            PurchaseInfos purchaseInfo = new PurchaseInfos
            {
                Count = 1,
                IdExchangeRate = 1,
                IdProduct = 2,
                PurchasePrice = 500,
                SerialNumbers =
                    new List<SerialNumbers> { new SerialNumbers { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
            };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 1,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos
                {
                    Count = 1,
                    IdExchangeRate = 1,
                    IdProduct = 2,
                    PurchasePrice = 1000,
                    SerialNumbers =
                        new List<SerialNumbers> { new SerialNumbers { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
                };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                int result = db.SerialNumbers.Count();
                Assert.AreEqual(result, 2, "Кол-во не равно");
                Assert.AreNotEqual(db.SerialNumbers.Find(2), null, "Не добавлено");
            }
        }

        [Test]
        public void TestAddSalesGoods()
        {
            Init();

            DbSetPurchaseGoods dbSetPurchaseGoods = new DbSetPurchaseGoods();
            PurchaseInfos purchaseInfo;

            purchaseInfo =
                new PurchaseInfos
                {
                    Count = 1,
                    IdExchangeRate = 1,
                    IdProduct = 2,
                    PurchasePrice = 500,
                    SerialNumbers =
                        new List<SerialNumbers> { new SerialNumbers { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
                };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 1,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos
                {
                    Count = 1,
                    IdExchangeRate = 1,
                    IdProduct = 2,
                    PurchasePrice = 1000,
                    SerialNumbers =
                        new List<SerialNumbers> { new SerialNumbers { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
                };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos { Count = 5, IdExchangeRate = 2, IdProduct = 1, PurchasePrice = 5 };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            DbSetSalesGoods dbSetSalesGoods = new DbSetSalesGoods();
            SalesInfos salesInfo = new SalesInfos { Count = 1, IdProduct = 2, SellingPrice = 2500, SerialNumbers = new SerialNumbers{Id = 2} };
            dbSetSalesGoods.Add(new SalesReports
            {
                IdCounterparty = 2,
                IdStore = 1,
                SalesInfos = new List<SalesInfos> { salesInfo }
            });
            salesInfo = new SalesInfos { Count = 2, IdProduct = 1, SellingPrice = 150 };
            dbSetSalesGoods.Add(new SalesReports
            {
                IdCounterparty = 2,
                IdStore = 1,
                SalesInfos = new List<SalesInfos> { salesInfo }
            });

            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                Assert.AreEqual(db.SalesReports.Count(), 2, "Кол-во отчетов не равно");
                Assert.AreEqual(db.SalesInfos.Count(), 2, "Кол-во инфо не равно");
            }
        }

        [Test]
        public void TestAddRevaluationProducts()
        {
            Init();

            DbSetRevaluationProducts dbSetRevaluationProducts = new DbSetRevaluationProducts();
            RevaluationProductsInfos revaluationProducts;

            revaluationProducts =
                new RevaluationProductsInfos
                {
                    IdProduct = 2,
                    NewSalesPrice = 150
                };
            dbSetRevaluationProducts.Add(new RevaluationProductsReports
            {
                RevaluationProductsInfos = new List<RevaluationProductsInfos> {revaluationProducts}
            });

            using (AutomationAccountingGoodsEntities db =
                new AutomationAccountingGoodsEntities(AutomationAccountingGoodsEntities.ConnectionString))
            {
                Assert.AreEqual(db.RevaluationProductsReports.Count(), 1, "Кол-во отчетов не равно");
                Assert.AreEqual(db.RevaluationProductsInfos.Count(), 1, "Кол-во инфо не равно");
            }
        }

        [Test]
        public void TestReport()
        {
            Init();

            DbSetPurchaseGoods dbSetPurchaseGoods = new DbSetPurchaseGoods();
            PurchaseInfos purchaseInfo;

            purchaseInfo = new PurchaseInfos { Count = 1, IdExchangeRate = 1, IdProduct = 1, PurchasePrice = 50 };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 1,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos { Count = 5, IdExchangeRate = 2, IdProduct = 1, PurchasePrice = 5 };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos
                {
                    Count = 1,
                    IdExchangeRate = 1,
                    IdProduct = 2,
                    PurchasePrice = 500,
                    SerialNumbers =
                        new List<SerialNumbers> { new SerialNumbers { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
                };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            purchaseInfo =
                new PurchaseInfos
                {
                    Count = 1,
                    IdExchangeRate = 1,
                    IdProduct = 2,
                    PurchasePrice = 1000,
                    SerialNumbers =
                        new List<SerialNumbers> {new SerialNumbers {IdCounterparty = 1, IdProduct = 2, Value = "123"}}
                };
            dbSetPurchaseGoods.Add(new PurchaseReports
            {
                IdCounterparty = 1,
                IdStore = 1,
                Course = 20,
                PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
            });

            DbSetSalesGoods dbSetSalesGoods = new DbSetSalesGoods();
            SalesInfos salesInfo = new SalesInfos { Count = 1, IdProduct = 2, SellingPrice = 2500, SerialNumbers = new SerialNumbers{Id = 2}};
            dbSetSalesGoods.Add(new SalesReports
            {
                IdCounterparty = 2,
                IdStore = 1,
                SalesInfos = new List<SalesInfos> { salesInfo }
            });
            salesInfo = new SalesInfos { Count = 2, IdProduct = 1, SellingPrice = 150 };
            dbSetSalesGoods.Add(new SalesReports
            {
                IdCounterparty = 2,
                IdStore = 1,
                SalesInfos = new List<SalesInfos> { salesInfo }
            });
            ;
            DbSetReports dbSetReports = new DbSetReports();
            var result = dbSetReports.GetFinalReport(DateTime.Today, DateTime.Today);

            Assert.AreEqual(result.Count, 2, "Кол-во не равно");
            Assert.AreEqual(result[1].FinalSum, 150, "Сумма не равна");
            Assert.AreEqual(result[0].FinalSum, 1500, "Сумма не равна");
        }

        private void Init()
        {
            DbSetGroups dbSetGroups = new DbSetGroups();
            dbSetGroups.Add(new Groups { Title = "Группа1" });

            DbSetUnitStorages dbSetUnitStorages = new DbSetUnitStorages();
            dbSetUnitStorages.Add(new UnitStorages { Title = "шт" });

            DbSetWarrantyPeriods dbSetWarrantyPeriods = new DbSetWarrantyPeriods();
            dbSetWarrantyPeriods.Add(new WarrantyPeriods { Period = "Нет" });
            dbSetWarrantyPeriods.Add(new WarrantyPeriods { Period = "14 дней" });

            DbSetProducts dbSetProducts = new DbSetProducts();
            dbSetProducts.Add(new Products
            {
                Title = "Товар1",
                Barcode = "1",
                IdGroup = 1,
                IdUnitStorage = 1,
                IdWarrantyPeriod = 1
            });
            dbSetProducts.Add(new Products
            {
                Title = "Товар2",
                Barcode = "2",
                IdGroup = 1,
                IdUnitStorage = 1,
                IdWarrantyPeriod = 2
            });

            DbSetExchangeRates dbSetExchangeRates = new DbSetExchangeRates();
            dbSetExchangeRates.Add(new ExchangeRates { Title = "ГРН", Course = 1 });
            dbSetExchangeRates.Add(new ExchangeRates { Title = "USD", Course = 20 });

            DbSetCounterparties dbSetCounterparties = new DbSetCounterparties();
            dbSetCounterparties.Add(new Counterparties { Title = "Поставщик1", WhoIsIt = false });
            dbSetCounterparties.Add(new Counterparties { Title = "Покупатель1", WhoIsIt = true });
        }
    }
}
