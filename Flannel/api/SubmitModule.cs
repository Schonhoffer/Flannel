using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Flannel.Contracts;
using Flannel.ORM;
using Flannel.Settings;
using Flannel.Utilities;
using Nancy;

namespace Flannel.api
{
	public class SubmitModule : NancyModule
	{
		private readonly IPixMatch _pixMatch;
		private readonly IPushMessages _messagePusher;
		private readonly IUploadBlobImages _blobUploader;
		private readonly IModeDecider _modeDecider;

		public SubmitModule(IPixMatch pixMatch, IPushMessages messagePusher, IUploadBlobImages blobUploader, IModeDecider modeDecider) : base("/api")
		{
			_pixMatch = pixMatch;
			_messagePusher = messagePusher;
			_blobUploader = blobUploader;
			_modeDecider = modeDecider;
			Get["/"] = parameters => HttpStatusCode.NotImplemented;
			Post["/submit"] = parameters => UploadImage();
		}

		public SubmitModule()
			:this(new MatchEngine(), new MessagePusher(), new XqBlobUploader(), new ModeDecider())
		{
		}

		public Response UploadImage()
		{
			try
			{
				var file = Request.Files.FirstOrDefault();

				if (file == null)
					return HttpStatusCode.ExpectationFailed;

				Stream imageStream = file.Value;

				if (_pixMatch.IsDuplicate(imageStream))
					return HttpStatusCode.NotAcceptable;

				//Add the file to pixmatch
				_pixMatch.Add(imageStream);

				//imageStream = imageStream.FixOrientation();

				string imageUrl = _blobUploader.UploadBlob(imageStream);

				var submission = CreateSubmission(imageUrl);

				if (_modeDecider.GetMode() == AppMode.GoTime)
					_messagePusher.NotifyUsersAboutNewSubmission(submission);


				return Response.AsJson(submission, HttpStatusCode.OK);
			}
			catch (Exception ex)
			{
				return Response.AsJson(ex.Message);
			}
		}


		private Submission CreateSubmission(string url)
		{
			var submission = new Submission()
			                 	{
			                 		Href = url,
			                 		WhenGmt = DateTime.UtcNow,
			                 		Score = 1
			                 	};

			using (var session = OrmHost.CreateSessionFactory().OpenSession())
			{
				using (var transaction = session.BeginTransaction())
				{
					transaction.Begin();

					session.SaveOrUpdate(submission);

					transaction.Commit();
				}
			}

			return submission;
		}
	}

}