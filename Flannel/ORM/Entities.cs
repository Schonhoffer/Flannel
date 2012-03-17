using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentNHibernate.Mapping;

namespace Flannel.ORM
{
	public class Submission
	{
		public virtual Guid SubmissionId { get; set; }
		public virtual DateTime WhenGmt { get; set; }
		public virtual string Href { get; set; }
		public virtual double Score { get; set; }
	}

	public class AppConfig
	{
		public virtual int id { get; set; }
		public virtual byte AppMode { get; set; }
	}
	
}