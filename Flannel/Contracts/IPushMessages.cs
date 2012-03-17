using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flannel.ORM;
using Flannel.Settings;

namespace Flannel.Contracts
{
	public interface IPushMessages
	{
		void NotifyUsersAboutNewSubmission(Submission submission);

		void NotifyUsersAboutNewMode(AppMode newMode);
	}
}
