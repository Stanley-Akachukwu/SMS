namespace SMS.Web.Services
{
    public interface IUserAuthState
    {
        Task<string> GetCurrentUser();
        Task<bool> IsAuthenticated();
    }
}
