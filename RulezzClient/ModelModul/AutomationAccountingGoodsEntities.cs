
namespace ModelModul
{
    public partial class AutomationAccountingGoodsEntities
    {
        private static AutomationAccountingGoodsEntities _instance;

        protected AutomationAccountingGoodsEntities(string connectionString) : base(connectionString) {}

        public static AutomationAccountingGoodsEntities GetInstance(string connectionString = null)
        {
            if (_instance == null && !string.IsNullOrEmpty(connectionString))
                _instance = new AutomationAccountingGoodsEntities(connectionString);
            return _instance;
        }
    }
}
