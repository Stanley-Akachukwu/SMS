using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
namespace SMS.Web.Services
{
    public class UserAuthState(AuthenticationStateProvider _authProvider) : IUserAuthState
    {
        public async Task<string> GetCurrentUser()
        {
            var authState = await _authProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            var currentUser = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "unknown user"; 
            return currentUser;
        }

        public async Task<bool> IsAuthenticated()
        {
           var authState = await _authProvider.GetAuthenticationStateAsync();
            var currentUser = authState.User;

            if (currentUser.Identity == null || !currentUser.Identity.IsAuthenticated)
                return false;
            return true;
        }
    }
}
