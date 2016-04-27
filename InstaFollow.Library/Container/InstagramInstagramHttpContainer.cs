using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using InstaFollow.Library.Exceptions;
using log4net;

namespace InstaFollow.Library.Container
{
	public class InstagramInstagramHttpContainer : IInstagramHttpContainer
	{
		private readonly ILog log = LogManager.GetLogger(typeof(InstagramInstagramHttpContainer));

		private static IInstagramHttpContainer instance;

		private readonly CookieContainer cookies;

		private InstagramInstagramHttpContainer()
		{
			this.cookies = new CookieContainer();
		}

		public static IInstagramHttpContainer Instance
		{
			get { return instance ?? (instance = new InstagramInstagramHttpContainer()); }
		}

		public string InstagramGet(string page, string referrer = null)
		{
			try
			{
				var request = (HttpWebRequest) WebRequest.Create(page);
				request.Method = "GET";
				request.KeepAlive = true;
				request.CookieContainer = this.cookies;
				request.ContentType = "application/x-www-form-urlencoded";
				request.UserAgent =
					"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.75 Safari/537.36";
				request.Referer = referrer;
				request.AllowAutoRedirect = true;

				var response = (HttpWebResponse) request.GetResponse();
				cookies.Add(response.Cookies);

				var reader = new StreamReader(response.GetResponseStream());
				return reader.ReadToEnd();
			}
			catch (Exception ex)
			{
				this.log.Error(ex.Message);
				throw;
			}
		}

		public string InstagramPost(string page, string csrfToken, string referrer = null, string postData = "", bool isCommentRequest = false)
		{
			try
			{
				var bytes = Encoding.UTF8.GetBytes(postData);

				var request = (HttpWebRequest) WebRequest.Create(page);
				request.Method = "POST";
				request.KeepAlive = true;
				request.CookieContainer = this.cookies;
				request.ContentType = "application/x-www-form-urlencoded";
				request.Accept = "*/*";
				request.UserAgent =
					"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.75 Safari/537.36";
				request.Referer = referrer;
				request.AllowAutoRedirect = true;

				request.Headers.Add("X-Instagram-AJAX", "1");
				request.Headers.Add("X-CSRFToken", csrfToken);
				request.Headers.Add("X-Requested-With", "XMLHttpRequest");
				request.Headers.Add("Pragma", "no-cache");
				request.Headers.Add("Cache-Control", "no-cache");

				request.ContentLength = bytes.Length;

				var postStream = request.GetRequestStream();
				postStream.Write(bytes, 0, bytes.Length);
				postStream.Close();

				var response = (HttpWebResponse) request.GetResponse();
				this.cookies.Add(response.Cookies);

				var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
				object authResponse = reader.ReadToEnd();
				return authResponse.ToString();
			}
			catch (Exception ex)
			{
				this.log.Error(ex.Message);

				if (!isCommentRequest)
				{
					throw;
				}
				
				throw new InstagramCommentException("Error commenting! Will turn off this feature.");
			}
		}

		public bool InstagramLogin(string userName, string password)
		{
			try
			{
				this.log.Info("Logging in user " + userName);

				const string loginUrl = "https://www.instagram.com/accounts/login/";
				const string loginPostUrl = loginUrl + "ajax/";

				var loginPage = this.InstagramGet(loginUrl);
				var csrfToken = Regex.Match(loginPage, "\"csrf_token\":\"(\\w+)\"").Groups[1].Value;

				userName = WebUtility.UrlEncode(userName);
				password = WebUtility.UrlEncode(password);
				var post = string.Format("username={0}&password={1}", userName, password);
				var postData = Encoding.ASCII.GetBytes(post);

				var request = (HttpWebRequest) WebRequest.Create(loginPostUrl);
				request.Method = "POST";
				request.KeepAlive = true;
				request.CookieContainer = this.cookies;
				request.ContentType = "application/x-www-form-urlencoded";
				request.Accept = "*/*";
				request.UserAgent =
					"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/50.0.2661.75 Safari/537.36";
				request.ContentLength = postData.Length;
				request.Referer = loginUrl;
				request.AllowAutoRedirect = true;

				request.Headers.Add("X-Instagram-AJAX", "1");
				request.Headers.Add("X-CSRFToken", csrfToken);
				request.Headers.Add("X-Requested-With", "XMLHttpRequest");

				var postStream = request.GetRequestStream();
				postStream.Write(postData, 0, postData.Length);
				postStream.Close();

				var response = (HttpWebResponse) request.GetResponse();
				this.cookies.Add(response.Cookies);

				var reader = new StreamReader(response.GetResponseStream());
				object authResponse = reader.ReadToEnd();

				this.log.Info(authResponse.ToString());

				return authResponse.ToString().Contains("\"authenticated\":true");
			}
			catch (Exception ex)
			{
				this.log.Error(ex.Message);
				throw;
			}
		}
	}
}