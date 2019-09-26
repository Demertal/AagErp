using System;
using System.Data.SqlClient;
using ModelModul.Models;

namespace ModelModul
{
    public static class ConnectionTools
    {
        public static string ConnectionString;

        public static string Login;

        public static User CurrrentUser;

        public static void BuildConnectionString(
            string configConnectionStringName,
            string initialCatalog = "",
            string dataSource = "",
            string userId = "",
            string password = "",
            bool integratedSecuity = true)
        {
            if (string.IsNullOrEmpty(configConnectionStringName)) throw new ArgumentNullException(nameof(configConnectionStringName), "Параметр должен быть указан");

            var sqlCnxStringBuilder = new SqlConnectionStringBuilder
                (System.Configuration.ConfigurationManager
                .ConnectionStrings[configConnectionStringName].ConnectionString);

            if (!string.IsNullOrEmpty(initialCatalog))
                sqlCnxStringBuilder.InitialCatalog = initialCatalog;
            if (!string.IsNullOrEmpty(dataSource))
                sqlCnxStringBuilder.DataSource = dataSource;
            if (!string.IsNullOrEmpty(userId))
                sqlCnxStringBuilder.UserID = userId;
            if (!string.IsNullOrEmpty(password))
                sqlCnxStringBuilder.Password = password;

            sqlCnxStringBuilder.IntegratedSecurity = integratedSecuity;

            ConnectionString = sqlCnxStringBuilder.ConnectionString;
        }
    }
}
