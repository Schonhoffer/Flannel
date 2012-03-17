using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Flannel.Contracts;
using RestSharp;

namespace Flannel.Utilities
{
	public class XqBlobUploader : IUploadBlobImages
	{
		private string _baseUrl = "https://xq.iqmetrix.net/api/";
		private string _sessionKey = null;
		private CreateSession _creds;

		public bool IsLoggedIn { get; private set; }

		public XqBlobUploader()
		{
			_creds = new CreateSession()
			         	{
			         		username = "hackathon1",
			         		password = "hackathon1"
			         	};
		}


		public string UploadBlob(System.IO.Stream imageStream)
		{
			if (!IsLoggedIn)
				Login();

			var result = UploadAsset(string.Format("{0}.jpg", Guid.NewGuid()), imageStream);

			return result.Href;
		}

		public void Login()
		{
			var request = new RestRequest {Resource = "/sessions", Method = Method.POST, RequestFormat = DataFormat.Json};
			request.AddBody(_creds);

			var response = Execute<SessionModel>(request);

			_sessionKey = Uri.EscapeDataString(response.Data.SessionKey);
			IsLoggedIn = true;
		}


		public RestResponse<T> Execute<T>(RestRequest request) where T : new()
		{
			string resourceUri = request.Resource;
			if (this.IsLoggedIn) request.Resource = _appendSessionId(request.Resource);

			var client = ClientFactory();
			var response = client.Execute<T>(request);
			//LastResponse = response;

			CheckResponseForError(response);

			return response;
		}

		public AssetModel UploadAsset(string assetName, Stream assetContents)
		{
			string response = HttpUploadFile(assetContents, assetName, _appendSessionId(string.Format("{0}/assets", _baseUrl)), "imageFile", "image/png", null);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<AssetModel>(response);
		}

		private string _appendSessionId(string resource)
		{
			var keystring = "api_key=" + _sessionKey;
			if (resource.Contains('?')) return resource + "&" + keystring;
			return resource + "?" + keystring;
		}

		public static string HttpUploadFile(Stream fileStream, string fileName, string url, string paramName, string contentType, NameValueCollection nvc = null)
		{
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

			HttpWebRequest wr = (HttpWebRequest) WebRequest.Create(url);
			wr.ContentType = "multipart/form-data; boundary=" + boundary;
			wr.Method = "POST";
			wr.KeepAlive = true;
			wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

			Stream rs = wr.GetRequestStream();

			string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
			nvc = nvc ?? new NameValueCollection();
			foreach (string key in nvc.Keys)
			{
				rs.Write(boundarybytes, 0, boundarybytes.Length);
				string formitem = string.Format(formdataTemplate, key, nvc[key]);
				byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
				rs.Write(formitembytes, 0, formitembytes.Length);
			}
			rs.Write(boundarybytes, 0, boundarybytes.Length);

			string headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
			string header = string.Format(headerTemplate, paramName, fileName, contentType);
			byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
			rs.Write(headerbytes, 0, headerbytes.Length);

			fileStream.Seek(0, SeekOrigin.Begin);
			fileStream.CopyTo(rs);

			byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
			rs.Write(trailer, 0, trailer.Length);
			rs.Close();

			WebResponse wresp = null;
			string response = string.Empty;
			try
			{
				wresp = wr.GetResponse();
				Stream stream2 = wresp.GetResponseStream();
				StreamReader resonseReader = new StreamReader(stream2);
				response = resonseReader.ReadToEnd();
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				if (wresp != null)
				{
					wresp.Close();
					wresp = null;
				}
				wr = null;
			}
			return response;
		}

		private RestClient ClientFactory()
		{
			var client = new RestClient {BaseUrl = _baseUrl};
			client.AddHandler("application/json", new JsonDeserializer());
			return client;
		}

		private void CheckResponseForError(IRestResponse response)
		{
			if ((response.ResponseStatus == ResponseStatus.Error)
			    || (response.StatusCode != HttpStatusCode.OK
			        && response.StatusCode != HttpStatusCode.Created
			        && response.StatusCode != HttpStatusCode.Accepted
			        && response.StatusCode != HttpStatusCode.NoContent))
			{
				if (response.StatusCode == HttpStatusCode.Unauthorized)
					throw new UnauthorizedAccessException(response.Content);
				else
					throw new Exception(string.Format("status code: {0} response status: {1} message: {2} content: {3}", response.StatusCode, response.ResponseStatus, response.ErrorMessage, response.Content));
			}
		}

	}

	[Serializable]
	public class CreateSession
	{
		public string username { get; set; }
		public string password { get; set; }
	}

	[Serializable]
	public class AssetModel
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string AssetType { get; set; }
		public string MimeType { get; set; }
		public string Href { get; set; }
		public bool IsHero { get; set; }
		public string ThumbnailHref { get; set; }
	}


	[Serializable]
	public class SessionModel
	{
		public SessionModel()
		{

			Roles = new string[0];
			Expires = new DateTime();
		}

		public string Id { get; set; }
		public string DisplayName { get; set; }
		public string SessionKey { get; set; }
		public Guid AccountId { get; set; }
		public string[] Roles { get; set; }
		public DateTime Expires { get; set; }
	}

	class JsonDeserializer : RestSharp.Deserializers.IDeserializer
	{
		public string DateFormat
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				//not needed
			}
		}

		public T Deserialize<T>(string response) where T : new()
		{
			return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(response);
		}

		public T Deserialize<T>(RestSharp.RestResponse response) where T : new()
		{
			return Deserialize<T>(response.Content);
		}

		public string Namespace
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				//not needed
			}
		}

		public string RootElement
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				//not needed
			}
		}
	}
}