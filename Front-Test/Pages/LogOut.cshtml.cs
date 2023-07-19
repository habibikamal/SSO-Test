using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Front_Test.Pages
{
    public class LogOutModel : PageModel
    {
        public async void OnGet()
        {


            try
            {
                await HttpContext.SignOutAsync();
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
                await HttpContext.SignOutAsync("Cookies");
                //foreach (var cookie in Request.Cookies.Keys)
                //{
                //    Response.Cookies.Delete(cookie);
                //}

            }
            catch (Exception)
            {

                RedirectToPage("Index");
            }

            //RedirectToPage("Index");
            //return RedirectToPage("Index");
        }
    }
}
