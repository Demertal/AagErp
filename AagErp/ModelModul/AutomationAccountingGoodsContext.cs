using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ModelModul.Configurations;
using ModelModul.Models;

namespace ModelModul
{
    public class AutomationAccountingGoodsContext : DbContext
    {
        private readonly string _connectionString;

        public AutomationAccountingGoodsContext()
        {
            Database.EnsureCreated();
        }

        public AutomationAccountingGoodsContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
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
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyNameConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyProductConfiguration());
            modelBuilder.ApplyConfiguration(new PropertyValueConfiguration());
            modelBuilder.ApplyConfiguration(new RevaluationProductConfiguration());
            modelBuilder.ApplyConfiguration(new SerialNumberConfiguration());
            modelBuilder.ApplyConfiguration(new SerialNumberLogConfiguration());
            modelBuilder.ApplyConfiguration(new StoreConfiguration());
            modelBuilder.ApplyConfiguration(new UnitStorageConfiguration());
            modelBuilder.ApplyConfiguration(new WarrantyConfiguration());
            modelBuilder.ApplyConfiguration(new WarrantyPeriodConfiguration());
        }
    }
}
