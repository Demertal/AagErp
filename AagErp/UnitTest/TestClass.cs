using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ModelModul;
using ModelModul.Models;
using ModelModul.Repositories;
using ModelModul.Specifications;
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
            ConnectionTools.BuildConnectionString(configConnectionStringName: "AutomationAccountingGoodsContext");
            _transactionScope = new TransactionScope(TransactionScopeOption.Required);
        }

        [TearDown]
        public void TearDown()
        {
            _transactionScope.Dispose();
            //using (AutomationAccountingGoodsContext db = new AutomationAccountingGoodsContext(ConnectionTools.ConnectionString))
            //{
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (MovementGoodsInfos, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PriceProducts, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (InvoiceInfos, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (MoneyTransfers, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (RevaluationProduct, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Warranties, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (MovementGoods, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (SerialNumbers, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (WarrantyPeriods, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (UnitStorages, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PriceGroups, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (CountProducts, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PropertyProducts, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PropertyValues, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (PropertyNames, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Products, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Invoices, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Currencies, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Stores, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Counterparties, RESEED, 0)");
            //    db.Database.ExecuteSqlCommand("DBCC CHECKIDENT (Categories, RESEED, 0)");
            //    db.SaveChanges();
            //}
        }

        //[Test]
        //public void TestAddPurchaseGoods()
        //{
        //    InitAsync();

        //    SqlMovementGoodsReportRepository dbSetMovementGoodsReports = new SqlMovementGoodsReportRepository();

        //    PurchaseInfos purchaseInfo = new PurchaseInfos
        //    {
        //        Count = 1,
        //        IdCurrency = 1,
        //        IdProduct = 2,
        //        PurchasePrice = 500,
        //        SerialNumber =
        //            new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //    };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 1,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos
        //        {
        //            Count = 1,
        //            IdCurrency = 1,
        //            IdProduct = 2,
        //            PurchasePrice = 1000,
        //            SerialNumber =
        //                new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //        };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    using (AutomationAccountingGoodsContext db =
        //        new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
        //    {
        //        int result = db.SerialNumber.Count();
        //        Assert.AreEqual(result, 2, "Кол-во не равно");
        //        Assert.AreNotEqual(db.SerialNumber.Find(2), null, "Не добавлено");
        //    }
        //}

        //[Test]
        //public void TestAddSalesGoods()
        //{
        //    InitAsync();

        //    SqlMovementGoodsReportRepository dbSetMovementGoodsReports = new SqlMovementGoodsReportRepository();
        //    PurchaseInfos purchaseInfo;

        //    purchaseInfo =
        //        new PurchaseInfos
        //        {
        //            Count = 1,
        //            IdCurrency = 1,
        //            IdProduct = 2,
        //            PurchasePrice = 500,
        //            SerialNumber =
        //                new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //        };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 1,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos
        //        {
        //            Count = 1,
        //            IdCurrency = 1,
        //            IdProduct = 2,
        //            PurchasePrice = 1000,
        //            SerialNumber =
        //                new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //        };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos { Count = 5, IdCurrency = 2, IdProduct = 1, PurchasePrice = 5 };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    DbSetSalesGoods dbSetSalesGoods = new DbSetSalesGoods();
        //    SalesInfos salesInfo = new SalesInfos { Count = 1, IdProduct = 2, SellingPrice = 2500, SerialNumber = new SerialNumber { Id = 2 } };
        //    dbSetSalesGoods.AddAsync(new SalesReports
        //    {
        //        IdCounterparty = 2,
        //        StoreId = 1,
        //        SalesInfos = new List<SalesInfos> { salesInfo }
        //    });
        //    salesInfo = new SalesInfos { Count = 2, IdProduct = 1, SellingPrice = 150 };
        //    dbSetSalesGoods.AddAsync(new SalesReports
        //    {
        //        IdCounterparty = 2,
        //        StoreId = 1,
        //        SalesInfos = new List<SalesInfos> { salesInfo }
        //    });

        //    using (AutomationAccountingGoodsContext db =
        //        new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
        //    {
        //        Assert.AreEqual(db.SalesReports.Count(), 2, "Кол-во отчетов не равно");
        //        Assert.AreEqual(db.SalesInfos.Count(), 2, "Кол-во инфо не равно");
        //    }
        //}

        //[Test]
        //public void TestAddRevaluationProducts()
        //{
        //    InitAsync();

        //    SqlRevaluationProductRepository dbSetRevaluationProducts = new SqlRevaluationProductRepository();
        //    RevaluationProductsInfos revaluationProducts;

        //    revaluationProducts =
        //        new RevaluationProductsInfos
        //        {
        //            IdProduct = 2,
        //            NewSalesPrice = 150
        //        };
        //    dbSetRevaluationProducts.AddAsync(new RevaluationProduct
        //    {
        //        RevaluationProductsInfos = new List<RevaluationProductsInfos> { revaluationProducts }
        //    });

        //    using (AutomationAccountingGoodsContext db =
        //        new AutomationAccountingGoodsContext(AutomationAccountingGoodsContext.ConnectionString))
        //    {
        //        Assert.AreEqual(db.RevaluationProduct.Count(), 1, "Кол-во отчетов не равно");
        //        Assert.AreEqual(db.RevaluationProductsInfos.Count(), 1, "Кол-во инфо не равно");
        //    }
        //}

        //[Test]
        //public void TestReport()
        //{
        //    InitAsync();

        //    SqlMovementGoodsReportRepository dbSetMovementGoodsReports = new SqlMovementGoodsReportRepository();
        //    PurchaseInfos purchaseInfo;

        //    purchaseInfo = new PurchaseInfos { Count = 1, IdCurrency = 1, IdProduct = 1, PurchasePrice = 50 };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 1,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos { Count = 5, IdCurrency = 2, IdProduct = 1, PurchasePrice = 5 };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos
        //        {
        //            Count = 1,
        //            IdCurrency = 1,
        //            IdProduct = 2,
        //            PurchasePrice = 500,
        //            SerialNumber =
        //                new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //        };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    purchaseInfo =
        //        new PurchaseInfos
        //        {
        //            Count = 1,
        //            IdCurrency = 1,
        //            IdProduct = 2,
        //            PurchasePrice = 1000,
        //            SerialNumber =
        //                new List<SerialNumber> { new SerialNumber { IdCounterparty = 1, IdProduct = 2, Value = "123" } }
        //        };
        //    dbSetMovementGoodsReports.AddAsync(new PurchaseReports
        //    {
        //        IdCounterparty = 1,
        //        StoreId = 1,
        //        Cost = 20,
        //        PurchaseInfos = new List<PurchaseInfos> { purchaseInfo }
        //    });

        //    DbSetSalesGoods dbSetSalesGoods = new DbSetSalesGoods();
        //    SalesInfos salesInfo = new SalesInfos { Count = 1, IdProduct = 2, SellingPrice = 2500, SerialNumber = new SerialNumber { Id = 2 } };
        //    dbSetSalesGoods.AddAsync(new SalesReports
        //    {
        //        IdCounterparty = 2,
        //        StoreId = 1,
        //        SalesInfos = new List<SalesInfos> { salesInfo }
        //    });
        //    salesInfo = new SalesInfos { Count = 2, IdProduct = 1, SellingPrice = 150 };
        //    dbSetSalesGoods.AddAsync(new SalesReports
        //    {
        //        IdCounterparty = 2,
        //        StoreId = 1,
        //        SalesInfos = new List<SalesInfos> { salesInfo }
        //    });
        //    ;
        //    DbSetReports dbSetReports = new DbSetReports();
        //    var result = dbSetReports.GetFinalReport(DateTime.Today, DateTime.Today);

        //    Assert.AreEqual(result.Count, 2, "Кол-во не равно");
        //    Assert.AreEqual(result[1].FinalSum, 150, "Сумма не равна");
        //    Assert.AreEqual(result[0].FinalSum, 1500, "Сумма не равна");
        //}

        [Test]
        public async Task TestLoadGroups()
        {
            await InitAsync();
            IRepository<Category> groupRepository = new SqlCategoryRepository();
            IEnumerable<Category> result = await groupRepository.GetListAsync();
            Assert.AreEqual(result.Count(), 2, "Кол-во не равно");
        }

        [Test]
        public async Task TestDeleatGroups()
        {
            await InitAsync();
            IRepository<Category> groupRepository = new SqlCategoryRepository();
            await groupRepository.DeleteAsync(await groupRepository.GetItemAsync(1));
            IEnumerable<Category> result = await groupRepository.GetListAsync();
            Assert.AreEqual(result.Count(), 1, "Кол-во не равно");
        }

        [Test]
        public async Task TestLoadGroupsWhisSpecification()
        {
            await InitAsync();
            IRepository<Category> groupRepository = new SqlCategoryRepository();
            List<Category> result = new List<Category>(await groupRepository.GetListAsync(CategorySpecification.GetCategoriesByIdParent(null), new Dictionary<string, SortingTypes>{{"Id", SortingTypes.ASC}}, take:1));
            Assert.AreEqual(result.Count, 1, "Кол-во не равно");
            Assert.IsTrue(result.Any(obj => obj.Title == "Rulezz"), "Нет группы");
        }

        [TestCase("Товар2")]
        [TestCase("ven2")]
        [TestCase("8051412688784")]
        [Test]
        public async Task TestLoadProductWhisSpecification(string findString)
        {
            await InitAsync();
            IRepository<Product> productRepository = new SqlProductRepository();
            List<Product> result = new List<Product>(await productRepository.GetListAsync(ProductSpecification.GetProductsByFindString(findString)));
            Assert.AreEqual(result.Count, 1, "Кол-во не равно");
            Assert.IsTrue(result.Any(obj => obj.Title == "Товар2"), "Нет товара");
        }

        private async Task InitAsync()
        {
            IRepository<Category> repositoryGroup = new SqlCategoryRepository();
            await repositoryGroup.CreateAsync(new Category { Title = "Rulezz" });
            await repositoryGroup.CreateAsync(new Category { Title = "Craft" });

            IRepository<UnitStorage> repositoryUnitStorage = new SqlUnitStorageRepository();
            await repositoryUnitStorage.CreateAsync(new UnitStorage { Title = "шт" });

            IRepository<WarrantyPeriod> repositoryWarrantyPeriod = new SqlWarrantyPeriodRepository();
            await repositoryWarrantyPeriod.CreateAsync(new WarrantyPeriod { Period = "Нет" });
            await repositoryWarrantyPeriod.CreateAsync(new WarrantyPeriod { Period = "14 дней" });

            IRepository<Currency> dbSetExchangeRates = new SqlExchangeRateRepository();
            await dbSetExchangeRates.CreateAsync(new Currency { Title = "ГРН", Cost = 1, IsDefault = true});
            await dbSetExchangeRates.CreateAsync(new Currency { Title = "USD", Cost = 20 });

            IRepository<Counterparty> dbSetCounterparties = new SqlCounterpartyRepository();
            await dbSetCounterparties.CreateAsync(new Counterparty { Title = "Поставщик1", WhoIsIt = TypeCounterparties.Suppliers });
            await dbSetCounterparties.CreateAsync(new Counterparty { Title = "Покупатель1", WhoIsIt = TypeCounterparties.Buyers });

            //IRepository<PriceGroup> repositoryPriceGroup = new Sql

            //IRepository<Product> dbSetProducts = new SqlProductRepository();
            //await dbSetProducts.CreateAsync(new Product
            //{
            //    Title = "Товар1",
            //    Barcode = "1",
            //    IdCategory = 1,
            //    IdUnitStorage = 1,
            //    IdWarrantyPeriod = 1
            //});
            //await dbSetProducts.CreateAsync(new Product
            //{
            //    Title = "Товар2",
            //    VendorCode = "ven2",
            //    Barcode = "8051412688784",
            //    IdCategory = 1,
            //    IdUnitStorage = 1,
            //    IdWarrantyPeriod = 2
            //});
        }
    }
}
