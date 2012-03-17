using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Flannel.Contracts;
using Flannel.ORM;
using Flannel.Settings;

namespace Flannel.Utilities
{
	public class ModeDecider : IModeDecider
	{
		public AppMode GetMode()
		{
			using (var session = OrmHost.CreateSessionFactory().OpenSession())
			{
				var config = session.CreateCriteria<AppConfig>().UniqueResult<AppConfig>();
				if (config != null)
					return (AppMode)config.AppMode;

				return AppMode.Countdown;
			}
		}
	}
}