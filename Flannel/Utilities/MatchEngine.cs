using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Flannel.Contracts;
using Flannel.Transport;
using Newtonsoft.Json;

namespace Flannel.Utilities
{
	public class MatchEngine : IPixMatch
	{
		const double duplicatescore = 60.0;

		readonly string baseApiURL;
		readonly string userName;
		readonly string password;
		string apiSearchUrl { get { return baseApiURL + "search/"; }}
		string apiAddUrl { get { return baseApiURL + "add/"; } }

		public MatchEngine()
			: this("http://mobileengine.tineye.com/hackdays/rest/", string.Empty, string.Empty)
		{
		}

		public MatchEngine(string baseApi, string username, string password)
		{
			this.password = password;
			this.userName = username;
			this.baseApiURL = baseApi;
		}

		public void Add(Stream image)
		{
			var apiResponse = HttpUploadFile(image, string.Format("{0}.jpg", Guid.NewGuid()), apiAddUrl, "images[0]", "image/jpg", null);
			
			// Do things with response
			var results = JsonConvert.DeserializeObject<MatchEngineResponse>(apiResponse);
			if (!results.status.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
				throw new ApplicationException(string.Format("Add method broken: {0}", results.error.First()));
		}

		/// <summary>
		/// <para>Call the API to do a search for the image specified by the <paramref name="image"/></para>
		/// </summary>
		/// <param name="image"> Image we're looking for</param>
		/// <returns>The API search results as a string (will be JSON).</returns>
		public SearchResult[] Search(Stream image)
		{
			var apiParams = new NameValueCollection
			{
				{"method", "search"}, 
				{"offset", "0"}, 
				{"limit", "50"}, 
				{"min_score", "0"}, 
				{"check_horizontal_flip", "false"}
			};

			//var searchStreams = new[] { image };

			var responseString = HttpUploadFile(image, string.Format("{0}.jpg", Guid.NewGuid()), apiSearchUrl, "images[0]", "image/jpg", apiParams);

			// Do things with reponse
			var results = JsonConvert.DeserializeObject<MatchEngineResponse>(responseString);

			if (results.status.Equals("ok", StringComparison.InvariantCultureIgnoreCase))
				return results.result;

			return null;
		}


		/// <summary>
		/// Decides if an image is a duplicate using teh Search method
		/// </summary>
		/// <param name="image">The image we're checking</param>
		/// <returns>True if duplicate, false if not.</returns>
		public bool IsDuplicate(Stream image)
		{
			var results = Search(image);
			return (results != null && results.Any(r => r.score >= duplicatescore));
		}


		public static string HttpUploadFile(Stream fileStream, string fileName, string url, string paramName, string contentType, NameValueCollection nvc = null)
		{
			string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

			HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
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
	}
}
