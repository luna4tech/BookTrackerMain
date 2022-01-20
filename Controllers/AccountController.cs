using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;

namespace BookTrackerMain.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult LoginWithFacebook(string returnUrl = "/")
		{
			var props = new AuthenticationProperties
			{
				RedirectUri = returnUrl
			};
			return Challenge(props, FacebookDefaults.AuthenticationScheme);
		}
	}
}
