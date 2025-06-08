using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using SMS.Common.Dtos.Auths;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;

namespace SMS.Web.Authentication
{
    public class CustomAuthStateProvider(HttpClient httpClient,ProtectedLocalStorage localStorage ) : AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            try
            {
                var json = await localStorage.GetAsync<string>("sessionState");
                if (!string.IsNullOrWhiteSpace(json.Value))
                {
                    var sessionModel = JsonSerializer.Deserialize<TokenResponseDto>(json.Value);
                    identity = sessionModel == null ? new ClaimsIdentity() : GetClaimsIdentity(sessionModel.AccessToken);
                    user = new ClaimsPrincipal(identity);                   
                }
                return new AuthenticationState(user);
            }
            catch (Exception ex)
            {
                await MarkUserAsLoggedOut();
                return new AuthenticationState(user);
            }
        }

        public async Task MarkUserAsAuthenticated(TokenResponseDto model)
        {
            var json = JsonSerializer.Serialize(model);
            await localStorage.SetAsync("sessionState", json);
            var identity = GetClaimsIdentity(model.AccessToken);
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private ClaimsIdentity GetClaimsIdentity(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;
            return new ClaimsIdentity(claims, "jwt");
        }

        public async Task MarkUserAsLoggedOut()
        {
            await localStorage.DeleteAsync("sessionState");
            var identity = new ClaimsIdentity();
            var user = new ClaimsPrincipal(identity);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
