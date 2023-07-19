using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Front_Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            object value = builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            builder.Services.AddSingleton<HtmlEncoder>(
    HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic }));



            #region SSO Config
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
          
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                                                                                           

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, optins =>
            {
                optins.SignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //optins.SignInScheme = "Cookies";
                optins.Authority = "https://ssonicard.eniac-tech.com/";//SSO Server
                //optins.Authority = "https://localhost:7225/";//SSO Server
                optins.ClientId = "4BA800BA-061F-4E1D-BF40-3A746E6FA84C";
                optins.ClientSecret = "123456";
                optins.ResponseType = "code";
                optins.GetClaimsFromUserInfoEndpoint = true;
                optins.SaveTokens = true;
                optins.Scope.Add("profile");
                optins.Scope.Add("openid");
                optins.Scope.Add("FullAccess");
                optins.Scope.Add("roles");
                optins.Scope.Add("Infos");
                optins.Scope.Add("UserNames");

                optins.ClaimActions.MapAll();
                optins.ClaimActions.DeleteClaim("sid");
                optins.ClaimActions.DeleteClaim("idp");
                //optins.TokenValidationParameters = new TokenValidationParameters
                //{
                //    NameClaimType = "name",
                //    RoleClaimType = "role"
                //};

            });

            builder.Services.AddSingleton<HtmlEncoder>(
      HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin,
                    UnicodeRanges.Arabic }));

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                //options.ReturnUrlParameter = "RedirectUrl";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                //options.Cookie.Name = "Cookie.Auth";
            });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("ShopRole", policy =>
                {
                    policy.RequireRole("Shop");
                });
                options.AddPolicy("UserRole", policy =>
                {
                    policy.RequireRole("User");
                });
            });
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Identity", "/Shop", "ShopRole");
            });
            builder.Services.AddSession();


            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}