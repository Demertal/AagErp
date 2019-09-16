using System;
using Microsoft.EntityFrameworkCore;
using ModelModul.Configurations;
using ModelModul.Models;

namespace ModelModul
{
    public class AutomationAccountingGoodsContext : DbContext
    {
        //private readonly string _connectionString;

        public AutomationAccountingGoodsContext()
        {
            Database.EnsureCreated();
        }

        public AutomationAccountingGoodsContext(string connectionString)
        {
            //_connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(_connectionString);
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Counterparty> Counterparties { get; set; }
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<InvoiceInfo> InvoiceInfos { get; set; }
        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<MoneyTransfer> MoneyTransfers { get; set; }
        public virtual DbSet<MoneyTransferType> MoneyTransferTypes { get; set; }
        public virtual DbSet<MovementGoods> MovementGoods { get; set; }
        public virtual DbSet<MovementGoodsInfo> MovementGoodsInfos { get; set; }
        public virtual DbSet<MovmentGoodType> MovmentGoodTypes { get; set; }
        public virtual DbSet<PaymentType> PaymentTypes { get; set; }
        public virtual DbSet<PriceGroup> PriceGroups { get; set; }
        public virtual DbSet<PriceProduct> PriceProducts { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<PropertyName> PropertyNames { get; set; }
        public virtual DbSet<PropertyProduct> PropertyProducts { get; set; }
        public virtual DbSet<PropertyValue> PropertyValues { get; set; }
        public virtual DbSet<RevaluationProduct> RevaluationProducts { get; set; }
        public virtual DbSet<SerialNumberLog> SerialNumberLogs { get; set; }
        public virtual DbSet<SerialNumber> SerialNumbers { get; set; }
        public virtual DbSet<Store> Stores { get; set; }
        public virtual DbSet<UnitStorage> UnitStorages { get; set; }
        public virtual DbSet<Warranty> Warranties { get; set; }
        public virtual DbSet<WarrantyPeriod> WarrantyPeriods { get; set; }
        public virtual DbQuery<CountsProduct> CountsProducts { get; set; }
        public virtual DbQuery<EquivalentCostForExistingProduct> EquivalentCostFor≈xistingProducts { get; set; }
        public virtual DbQuery<ProductWithCountAndPrice> ProductWithCountAndPrice { get; set; }
        public virtual DbQuery<PropertyForProduct> PropertyForProduct { get; set; }

        public static decimal GetCurrentPrice(long idProduct) => throw new NotSupportedException();

        public static bool CheckProperty(int idProperty, int? idCategory, string title) => throw new NotSupportedException();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new CounterpartyConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration(new InvoiceInfoConfiguration());
            modelBuilder.ApplyConfiguration(new MoneyTransferConfiguration());
            modelBuilder.ApplyConfiguration(new MoneyTransferTypeConfiguration());
            modelBuilder.ApplyConfiguration(new MovementGoodsConfiguration());
            modelBuilder.ApplyConfiguration(new MovementGoodsInfoConfiguration());
            modelBuilder.ApplyConfiguration(new MovmentGoodTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PriceGroupConfiguration());
            modelBuilder.ApplyConfiguration(new PriceProductConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyNameConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyValueConfiguration());
            modelBuilder.ApplyConfiguration(new RevaluationProductConfiguration());
            modelBuilder.ApplyConfiguration(new SerialNumberConfiguration());
            modelBuilder.ApplyConfiguration(new SerialNumberLogConfiguration());
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new UnitStorageConfiguration());
            modelBuilder.ApplyConfiguration(new WarrantyConfiguration());
            modelBuilder.ApplyConfiguration(new WarrantyPeriodConfiguration());
            modelBuilder.HasDbFunction(() => GetCurrentPrice(default(long)));
            modelBuilder.HasDbFunction(() => CheckProperty(default(int), default(int?), default(string)));
            modelBuilder.Query<ProductWithCountAndPrice>().ToView("productsWithCountAndPrice");
        }
    }
}
