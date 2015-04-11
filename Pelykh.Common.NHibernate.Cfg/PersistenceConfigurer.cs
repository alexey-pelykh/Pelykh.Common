using FluentNHibernate.Cfg.Db;
using System;
using System.Configuration;

namespace Pelykh.Common.NHibernate.Cfg
{
    public static class PersistenceConfigurer
    {
        public static IPersistenceConfigurer CreateConfigurationFromConnectionString(ConnectionStringSettings connectionString)
        {
            connectionString.ThrowIfNull("connectionString");

            if (connectionString.ProviderName == "System.Data.SqlClient")
            {
                return MsSqlConfiguration
                    .MsSql2012
                    .ConnectionString(connectionString.ConnectionString);
            }
            else if (connectionString.ProviderName == "MySql.Data.MySqlClient")
            {
                return MySQLConfiguration
                    .Standard
                    .ConnectionString(connectionString.ConnectionString);
            }

            throw new Exception(string.Format("'{0}' is not supported database", connectionString.ProviderName));
        }
    }
}
