using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using InstaFollow.Core.Exceptions;
using log4net;

namespace InstaFollow.Core.Container
{
	public class InstagramInstagramHttpContainer : IInstagramHttpContainer
	{
		private readonly ILog log = LogManager.GetLogger(typeof(InstagramInstagramHttpContainer));

		private static IInstagramHttpContainer instance;

		private readonly CookieContainer cookies;

		/// <summary>
		/// Prevents a default instance of the <see cref="InstagramInstagramHttpContainer"/> class from being created.
		/// </summary>
		private InstagramInstagramHttpContainer()
		{
			this.cookies = new CookieContainer();
		}

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>
		/// The instance.
		/// </value>
		public static IInstagramHttpContainer Instance
		{
			get { return instance ?? (instance = new InstagramInstagramHttpContainer()); }
		}

		/// <summary>
		/// Sends a GET request to instagram
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="referrer">The referrer.</param>
		/// <returns>The response string.</returns>
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
				if (!ex.Message.Contains("404"))
				{
					throw;
				}
				
				return string.Empty;
			}
		}

		/// <summary>
		/// Sends a POST request to instagram
		/// </summary>
		/// <param name="page">The page.</param>
		/// <param name="csrfToken">The CSRF token.</param>
		/// <param name="referrer">The referrer.</param>
		/// <param name="postData">The post data.</param>
		/// <param name="isCommentRequest">if set to <c>true</c> is comment request.</param>
		/// <returns>The response string.</returns>
		/// <exception cref="InstagramCommentException">Error commenting! Will turn off this feature.</exception>
		public string InstagramPost(string page, string csrfToken, string referrer = null, string postData = "", bool isCommentRequest = false)
		{
			try
			{
				var postDataEncoded = HttpUtility.UrlEncode(postData);
				var bytes = Encoding.UTF8.GetBytes(postDataEncoded ?? string.Empty);

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

		/// <summary>
		/// Sends a login request to instagram
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">The password.</param>
		/// <returns>True on success, false otherwise.</returns>
		public bool InstagramLogin(string userName, string password)
		{
			try
			{
				this.log.Info("Logging in user " + userName);

				const string loginUrl = "https://www.instagram.com/accounts/login/";
				const string loginPostUrl = loginUrl + "ajax/";

				var loginPage = this.InstagramGet(loginUrl);

				if (loginPage == string.Empty)
				{
					throw new Exception("Something went wrong with login page!");
				}

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

				return authResponse.ToString().Replace(" ", string.Empty).Contains("\"authenticated\":true");
			}
			catch (Exception ex)
			{
				this.log.Error(ex.Message);
				throw;
			}
		}
	}
}