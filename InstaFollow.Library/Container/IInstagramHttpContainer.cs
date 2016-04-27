namespace InstaFollow.Library.Container
{
	public interface IInstagramHttpContainer
	{
		string InstagramGet(string page, string referrer = null);
		string InstagramPost(string page, string csrfToken, string referrer = null, string postData = "", bool isCommentRequest = false);
		bool InstagramLogin(string userName, string password);
	}
}