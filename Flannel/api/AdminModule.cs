using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flannel.Contracts;
using Flannel.ORM;
using Flannel.Settings;
using Flannel.Utilities;
using NHibernate.Criterion;
using Nancy;

namespace Flannel.api
{
	public class AdminModule : NancyModule
	{
		private readonly IPushMessages _messagePusher;

		public AdminModule(IPushMessages messagePusher) : base("/api")
		{
			_messagePusher = messagePusher;

			Get["/schema"] = parameters => BuildSchema();
			Get["/countdown"] = parameters => ChangeMode((int)AppMode.Countdown);
			Get["/gotime"] = parameters => ChangeMode((int)AppMode.GoTime);
			Get["/hangover"] = parameters => ChangeMode((int)AppMode.Hangover);

			Get["/viewsubs"] = parameters => ViewSubs();
		}

		public AdminModule()
			: this(new MessagePusher())
		{
		}

		private Response BuildSchema()
		{
			//do anything against the database using the Orm Host Schema Builder to regenerate the DB Schema
			using (var session = OrmHost.SchemaBuilder().OpenSession())
			{
				var config = session.CreateCriteria<AppConfig>().UniqueResult<AppConfig>();
				if (config != null)
					;
			}

			ChangeMode((byte)AppMode.GoTime);

			return Response.AsJson("OK", HttpStatusCode.OK);
		}

		private Response ChangeMode(int newMode)
		{
			AppConfig config;
			bool modeChanged = false;
			using (var session = OrmHost.CreateSessionFactory().OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					transaction.Begin();

					config = session.CreateCriteria<AppConfig>().UniqueResult<AppConfig>();
					if (config != null)
					{
						modeChanged = config.AppMode != (byte) newMode;
						config.AppMode = (byte) newMode;
					}
					else
					{
						modeChanged = true;
						config = new AppConfig() { AppMode = (byte)newMode };
					}

					session.SaveOrUpdate(config);

					transaction.Commit();
				}
			}

			if(modeChanged)
				_messagePusher.NotifyUsersAboutNewMode((AppMode)newMode);

			return Response.AsJson(config, HttpStatusCode.OK);
		}

		private Response ViewSubs()
		{
			try
			{
				using (var session = OrmHost.CreateSessionFactory().OpenSession())
				{
					var subs = session.CreateCriteria<Submission>().AddOrder(Order.Desc("WhenGmt")).List<Submission>();

					return Response.AsJson(subs);
				}
			}
			catch (Exception ex)
			{
				return Response.AsJson(ex.Message);
			}
		}

	}
}
