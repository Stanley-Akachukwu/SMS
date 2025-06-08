using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Net.Http.Headers;
using SMS.Common.Dtos.Auths;
using SMS.Web.Authentication;
using System.Text.Json;
using SMS.Common.Dtos.Dashboards;
using SMS.Common.Dtos;
using SMS.Common.Dtos.Users;



namespace SMS.Web.Services
{
    
    public class SMSApiClient(HttpClient httpClient, ProtectedLocalStorage localStorage, NavigationManager navigationManager, AuthenticationStateProvider authStateProvider)
    {
        public async Task SetAuthorizeHeader(string? userId)
        {
            try
            {
                var json = await localStorage.GetAsync<string>("sessionState");
                var sessionState = JsonSerializer.Deserialize<TokenResponseDto>(json.Value);
                if(sessionState != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionState?.AccessToken);
                }
                //if (sessionState != null && !string.IsNullOrEmpty(sessionState.AccessToken))
                //{
                //    if (sessionState.TokenExpired.HasValue && sessionState.TokenExpired.Value.ToUnixTimeSeconds() < DateTimeOffset.UtcNow.ToUnixTimeSeconds())
                //    {
                //        await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                //        navigationManager.NavigateTo("/login");
                //    }
                //    else if (sessionState.TokenExpired.HasValue &&
                //        sessionState.TokenExpired.Value.ToUnixTimeSeconds() < DateTimeOffset.UtcNow.AddMinutes(5).ToUnixTimeSeconds())
                //    {
                //        var req = new RefreshTokenRequestDto { RefreshToken = sessionState.RefreshToken,UserId= userId };
                //        var rsp = await PostRefreshTokenFromJsonAsync<ApiResponse<TokenResponseDto?>, RefreshTokenRequestDto>("/api/auth/refresh-token", req);
                //        if (rsp != null)
                //        {
                //            await ((CustomAuthStateProvider)authStateProvider).MarkUserAsAuthenticated(rsp.Data);
                //            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rsp.Data.AccessToken);
                //        }
                //        else
                //        {
                //            await ((CustomAuthStateProvider)authStateProvider).MarkUserAsLoggedOut();
                //            navigationManager.NavigateTo("/login");
                //        }
                //    }
                //    else
                //    {
                //        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", sessionState.AccessToken);
                //    }
                //}
            }
            catch (Exception ex)
            {
                navigationManager.NavigateTo("/login");
            }
        }


        public async Task<T1> PostRefreshTokenFromJsonAsync<T1, T2>(string path, T2 postModel)
        {
            var res = await httpClient.PostAsJsonAsync(path, postModel);
            if (res != null && res.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync())!;
            }
            return default;
        }
        public async Task<T> GetFromJsonAsync<T>(string path, string? userId)
        {
            await SetAuthorizeHeader(userId);
            return await httpClient.GetFromJsonAsync<T>(path);
        }
        public async Task<T1> PostAsync<T1, T2>(string path, T2 postModel, string? userId="")
        {
            await SetAuthorizeHeader(userId);

            var res = await httpClient.PostAsJsonAsync(path, postModel);
            if (res != null && res.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync())!;
            }
            return default;
        }
        public async Task<T1> PutAsync<T1, T2>(string path, T2 postModel, string? userId)
        {
            //await SetAuthorizeHeader();
            var res = await httpClient.PutAsJsonAsync(path, postModel);
            if (res != null && res.IsSuccessStatusCode)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T1>(await res.Content.ReadAsStringAsync())!;
            }
            return default;
        }
        public async Task<T> DeleteAsync<T>(string path, string? userId)
        {
            //await SetAuthorizeHeader();
            return await httpClient.DeleteFromJsonAsync<T>(path);
        }
    }
}
