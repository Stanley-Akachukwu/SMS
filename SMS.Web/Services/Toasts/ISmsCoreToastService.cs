using Microsoft.FluentUI.AspNetCore.Components;

namespace SMS.Web.Services.Toasts
{
    public interface ISmsCoreToastService
    {
        void ShowToastMessage(string title, string toastParam, Color color, ToastIntent intent);
    }
}
