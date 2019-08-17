namespace ModelModul
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<categories> categories { get; set; }
        public virtual DbSet<counterparties> counterparties { get; set; }
        public virtual DbSet<currencies> currencies { get; set; }
        public virtual DbSet<invoiceInfos> invoiceInfos { get; set; }
        public virtual DbSet<invoices> invoices { get; set; }
        public virtual DbSet<moneyTransfers> moneyTransfers { get; set; }
        public virtual DbSet<moneyTransferTypes> moneyTransferTypes { get; set; }
        public virtual DbSet<movementGoods> movementGoods { get; set; }
        public virtual DbSet<movementGoodsInfos> movementGoodsInfos { get; set; }
        public virtual DbSet<movmentGoodTypes> movmentGoodTypes { get; set; }
        public virtual DbSet<paymentTypes> paymentTypes { get; set; }
        public virtual DbSet<priceGroups> priceGroups { get; set; }
        public virtual DbSet<priceProducts> priceProducts { get; set; }
        public virtual DbSet<products> products { get; set; }
        public virtual DbSet<propertyNames> propertyNames { get; set; }
        public virtual DbSet<propertyProducts> propertyProducts { get; set; }
        public virtual DbSet<propertyValues> propertyValues { get; set; }
        public virtual DbSet<revaluationProducts> revaluationProducts { get; set; }
        public virtual DbSet<serialNumberLogs> serialNumberLogs { get; set; }
        public virtual DbSet<serialNumbers> serialNumbers { get; set; }
        public virtual DbSet<stores> stores { get; set; }
        public virtual DbSet<unitStorages> unitStorages { get; set; }
        public virtual DbSet<warranties> warranties { get; set; }
        public virtual DbSet<warrantyPeriods> warrantyPeriods { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<categories>()
                .HasMany(e => e.categories1)
                .WithOptional(e => e.categories2)
                .HasForeignKey(e => e.idParent);

            modelBuilder.Entity<categories>()
                .HasMany(e => e.products)
                .WithRequired(e => e.categories)
                .HasForeignKey(e => e.idCategory);

            modelBuilder.Entity<categories>()
                .HasMany(e => e.propertyNames)
                .WithOptional(e => e.categories)
                .HasForeignKey(e => e.idCategory);

            modelBuilder.Entity<counterparties>()
                .HasMany(e => e.moneyTransfers)
                .WithRequired(e => e.counterparties)
                .HasForeignKey(e => e.idCounterparty)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<counterparties>()
                .HasMany(e => e.movementGoods)
                .WithOptional(e => e.counterparties)
                .HasForeignKey(e => e.idCounterparty);

            modelBuilder.Entity<currencies>()
                .Property(e => e.cost)
                .HasPrecision(19, 4);

            modelBuilder.Entity<currencies>()
                .HasMany(e => e.movementGoods)
                .WithOptional(e => e.currencies)
                .HasForeignKey(e => e.idCurrency);

            modelBuilder.Entity<invoices>()
                .HasMany(e => e.invoiceInfos)
                .WithRequired(e => e.invoices)
                .HasForeignKey(e => e.idInvoice)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<moneyTransfers>()
                .Property(e => e.moneyAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<moneyTransferTypes>()
                .HasMany(e => e.moneyTransfers)
                .WithRequired(e => e.moneyTransferTypes)
                .HasForeignKey(e => e.idType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movementGoods>()
                .Property(e => e.rate)
                .HasPrecision(19, 4);

            modelBuilder.Entity<movementGoods>()
                .HasMany(e => e.moneyTransfers)
                .WithRequired(e => e.movementGoods)
                .HasForeignKey(e => e.idMovementGoods)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movementGoods>()
                .HasMany(e => e.movementGoodsInfos)
                .WithRequired(e => e.movementGoods)
                .HasForeignKey(e => e.idReport)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movementGoods>()
                .HasMany(e => e.serialNumberLogs)
                .WithRequired(e => e.movementGoods)
                .HasForeignKey(e => e.idMovmentGood)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<movementGoodsInfos>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<movmentGoodTypes>()
                .HasMany(e => e.movementGoods)
                .WithRequired(e => e.movmentGoodTypes)
                .HasForeignKey(e => e.idType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<paymentTypes>()
                .HasMany(e => e.counterparties)
                .WithOptional(e => e.paymentTypes)
                .HasForeignKey(e => e.idPaymentType);

            modelBuilder.Entity<priceGroups>()
                .HasMany(e => e.products)
                .WithOptional(e => e.priceGroups)
                .HasForeignKey(e => e.idPriceGroup);

            modelBuilder.Entity<priceProducts>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<products>()
                .HasMany(e => e.invoiceInfos)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.idProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.movementGoodsInfos)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.idProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.priceProducts)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.idProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<products>()
                .HasMany(e => e.propertyProducts)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.idProduct);

            modelBuilder.Entity<products>()
                .HasMany(e => e.serialNumbers)
                .WithRequired(e => e.products)
                .HasForeignKey(e => e.idProduct)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<propertyNames>()
                .HasMany(e => e.propertyProducts)
                .WithOptional(e => e.propertyNames)
                .HasForeignKey(e => e.idPropertyName);

            modelBuilder.Entity<propertyNames>()
                .HasMany(e => e.propertyValues)
                .WithRequired(e => e.propertyNames)
                .HasForeignKey(e => e.idPropertyName);

            modelBuilder.Entity<propertyValues>()
                .HasMany(e => e.propertyProducts)
                .WithOptional(e => e.propertyValues)
                .HasForeignKey(e => e.idPropertyValue)
                .WillCascadeOnDelete();

            modelBuilder.Entity<revaluationProducts>()
                .HasMany(e => e.priceProducts)
                .WithRequired(e => e.revaluationProducts)
                .HasForeignKey(e => e.idRevaluation)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<serialNumbers>()
                .Property(e => e.value)
                .IsUnicode(false);

            modelBuilder.Entity<serialNumbers>()
                .HasMany(e => e.serialNumberLogs)
                .WithRequired(e => e.serialNumbers)
                .HasForeignKey(e => e.idSerialNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<serialNumbers>()
                .HasMany(e => e.warranties)
                .WithRequired(e => e.serialNumbers)
                .HasForeignKey(e => e.idSerialNumber)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<serialNumbers>()
                .HasMany(e => e.warranties1)
                .WithOptional(e => e.serialNumbers1)
                .HasForeignKey(e => e.idSerialNumberСhange);

            modelBuilder.Entity<stores>()
                .HasMany(e => e.movementGoods)
                .WithOptional(e => e.stores)
                .HasForeignKey(e => e.idArrivalStore);

            modelBuilder.Entity<stores>()
                .HasMany(e => e.movementGoods1)
                .WithOptional(e => e.stores1)
                .HasForeignKey(e => e.idDisposalStore);

            modelBuilder.Entity<unitStorages>()
                .HasMany(e => e.products)
                .WithRequired(e => e.unitStorages)
                .HasForeignKey(e => e.idUnitStorage)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<warrantyPeriods>()
                .HasMany(e => e.products)
                .WithRequired(e => e.warrantyPeriods)
                .HasForeignKey(e => e.idWarrantyPeriod)
                .WillCascadeOnDelete(false);
        }
    }
}
