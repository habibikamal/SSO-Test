using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace Front_Test.Pages
{
    [Authorize]
    public class ManageModel : PageModel
    {

        public List<string> BankList { get; set; }
        public void OnGet()
        {

        }

        public void OnPost() {
        }

        private async Task<string> GetAccessToken()
        {

            HttpClient client = new HttpClient();

            var discoveryDocument = await client.GetDiscoveryDocumentAsync("https://ssonicard.eniac-tech.com/"); //SSO
            var req = new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = "BluBankAPI",
                ClientSecret = "123456",
                Scope = "BluBankFullAccess",

            };
            var token = await client.RequestClientCredentialsTokenAsync(req);

            if (token.IsError)
            {
                throw new Exception(token.Error);
            }
            return token.AccessToken;
        }

        public async Task OnGetBankList() {
            //var token= await GetAccessToken();
            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7030/api/Bank");
            //request.Headers.Add("Authorization", $"Bearer {token}");
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //var result = await response.Content.ReadAsStringAsync();
            //Console.WriteLine(result);

            //var resultModel = JsonSerializer.Deserialize<List<string>>(result);

            //;

            //BankList = resultModel;
           
        }
    }
}
