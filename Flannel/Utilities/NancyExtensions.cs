using System.IO;
using Newtonsoft.Json;

namespace Flannel.Utilities
{
	public static class NancyExtensions
	{
		public static T FromJson<T>(this Stream body)
		{
			var reader = new StreamReader(body, true);
			body.Position = 0;
			var value = reader.ReadToEnd();
			return JsonConvert.DeserializeObject<T>(value, new JsonSerializerSettings
			{
				NullValueHandling = NullValueHandling.Ignore,
				MissingMemberHandling = MissingMemberHandling.Ignore
			});
		}	 
	}
}