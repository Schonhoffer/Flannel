using System;
using Flannel.ORM;
using Nancy;

namespace Flannel.api
{
	public class VoteModule : NancyModule
	{
		public VoteModule() : base("/api")
		{
			Post["/voteup"] = x => Vote(1);
			Post["/votedown"] = x => Vote(-1);
		}

		private Response Vote(double change)
		{
			try
			{
				if (!Request.Form.SubmissionID.HasValue())
					return Response.AsJson("No ID", HttpStatusCode.BadRequest);

				var id = Guid.Parse(Request.Form.SubmissionID.Value);

				using (var session = OrmHost.CreateSessionFactory().OpenSession())
				{
					using (var transaction = session.BeginTransaction())
					{
						transaction.Begin();

						var submission = session.Get<Submission>(id);
						if (submission != null)
						{
							submission.Score += change;
							session.SaveOrUpdate(submission);
						}

						transaction.Commit();
					}
				}

				return Response.AsJson("OK", HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return Response.AsJson(ex.Message);
			}
		}
	}
}