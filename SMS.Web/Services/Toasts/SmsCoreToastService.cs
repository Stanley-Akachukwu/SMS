
using Microsoft.FluentUI.AspNetCore.Components;
using SMS.Web.Components.Shared;
using SMS.Web.Models;

namespace SMS.Web.Services.Toasts
{
    public class SmsCoreToastService : ISmsCoreToastService
    {
        private readonly IToastService _toastService;

        public SmsCoreToastService(IToastService toastService)
        {
            _toastService = toastService;
        }
        public void ShowToastMessage(string title, string toastParam, Color color, ToastIntent intent)
        {
            _toastService.ShowToast<SMSCoreToast, ToastData>(new ToastParameters<ToastData>()
            {
                Intent = intent,
                Title = title,
                Timeout = 6000,
                Icon = (new Microsoft.FluentUI.AspNetCore.Components.Icons.Filled.Size12.Backpack(), color),
                Content = new ToastData()
                {
                    ToastParam = toastParam,
                }
            });
        }
    }
}
