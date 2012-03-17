using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ExplodingImages;
using Flannel.Contracts;
using Flannel.ORM;

namespace Flannel.Utilities
{
	public class MessagePusher : IPushMessages
	{
		private readonly Pubnub _pubnub;
		private const string Channel = "flannel";

		public MessagePusher()
		{
			_pubnub = new Pubnub("pub-31de6313-9900-484b-8dc8-cccfc74a9256", "sub-f39da011-2eaf-11e1-973f-831eb7090ee5", "");
		}

		public void NotifyUsersAboutNewSubmission(Submission submission)
		{
			_pubnub.Publish(Channel, Serialize(new PubNubMessage("PostChannel", submission)));
		}


		public void NotifyUsersAboutNewMode(Settings.AppMode newMode)
		{
			_pubnub.Publish(Channel, Serialize(new PubNubMessage("ModeChannel", (byte)newMode)));
		}

		private string Serialize(object obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
		}
	}

	public class PubNubMessage
	{
		public PubNubMessage(string channel, object message)
		{
			Channel = channel;
			Message = message;
		}

		public string Channel { get; set; }
		public object Message { get; set; }
	}
}