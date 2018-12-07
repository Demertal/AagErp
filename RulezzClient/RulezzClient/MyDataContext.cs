using System;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using RulezzClient.Annotations;

namespace RulezzClient
{
    public abstract class MyDataContext : DataContext
    {
        protected MyDataContext([NotNull] string fileOrServerOrConnection) : base(fileOrServerOrConnection)
        {
        }

        protected MyDataContext([NotNull] string fileOrServerOrConnection, [NotNull] MappingSource mapping) : base(fileOrServerOrConnection, mapping)
        {
        }

        protected MyDataContext([NotNull] IDbConnection connection) : base(connection)
        {
        }

        protected MyDataContext([NotNull] IDbConnection connection, [NotNull] MappingSource mapping) : base(connection, mapping)
        {
        }

        public abstract IQueryable<Type> Load(int id);
    }
}
