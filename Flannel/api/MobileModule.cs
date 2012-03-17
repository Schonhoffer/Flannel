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
	public class MobileModule : NancyModule
	{
		private readonly IModeDecider _modeDecider;

		public MobileModule(IModeDecider modeDecider) : base("/api")
		{
			_modeDecider = modeDecider;
			Get["/next"] = parameters => GetNext();
		}

		public MobileModule()
			: this(new ModeDecider())
		{
		}

		private Response GetNext()
		{
			return Response.AsJson(new
			{
				submission = GetNextSubmission(),
				mode = (byte)_modeDecider.GetMode(),
				NextSaturday = GetNextSaturday()
			});
		}

		private string GetNextSaturday()
		{
			var now = DateTime.Now;
			if (now.DayOfWeek == DayOfWeek.Saturday && now.Hour >= 5)
			{
				while (now.DayOfWeek != DayOfWeek.Saturday)
					now = now.AddDays(1);
			}
			now = new DateTime(now.Year, now.Month, now.Day, 17,0,0);
			return now.ToString("yyyy-MM-dd HH:mm:ss");
		}

		private Submission GetNextSubmission()
		{
			switch (_modeDecider.GetMode())
			{
				case AppMode.Countdown:
					return null;
				case AppMode.GoTime:
					return GetMostRecentSubmission();
				case AppMode.Hangover:
					return GetSubmissionForHangover();
			}
			
			throw new Exception("Invalid App Mode");
		}

		private Submission GetMostRecentSubmission()
		{
			Submission submission;

			using (var session = OrmHost.CreateSessionFactory().OpenSession())
			{
				submission = session.CreateCriteria<Submission>()
					.AddOrder(Order.Desc("WhenGmt"))
					.SetMaxResults(1)
					.List<Submission>()
					.FirstOrDefault();
			}
			return submission;
		}

		private Submission GetSubmissionForHangover()
		{
			Submission submission;

			using (var session = OrmHost.CreateSessionFactory().OpenSession())
			{
				var subs = session.CreateCriteria<Submission>().List<Submission>();

				submission = subs.Skip(new Random().Next(0, subs.Count-1)).Take(1).FirstOrDefault();
			}
			return submission;
		}

	}
}
