using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace Flannel.ORM
{
	public class SubmissionMap : ClassMap<Submission>
	{
		public SubmissionMap()
		{
			Id(x => x.SubmissionId).GeneratedBy.Guid();
			Map(x => x.WhenGmt);
			Map(x => x.Href);
			Map(x => x.Score);
		}
	}

	public class SettingnMap : ClassMap<AppConfig>
	{
		public SettingnMap()
		{
			Id(x => x.id).GeneratedBy.Identity();
			Map(x => x.AppMode);
		}
	}
}