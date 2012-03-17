
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Flannel.ORM
{
	public class OrmHost
	{
		private static readonly string ConnectionStringConfigKey = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING_ALIAS"];

		public static ISessionFactory CreateSessionFactory()
		{
			return CreateConfig().BuildSessionFactory();
		}

		private static FluentConfiguration CreateConfig()
		{
			string conString = ConfigurationManager.ConnectionStrings[ConnectionStringConfigKey].ConnectionString;

			return Fluently.Configure().Database(
				MsSqlConfiguration.MsSql2008.ConnectionString(c => c.Is(conString)))
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<OrmHost>());
		}

		public static ISessionFactory SchemaBuilder()
		{
			return CreateConfig().ExposeConfiguration(c => new SchemaExport(c).Create(false, true)).BuildSessionFactory();
		}
	}
}