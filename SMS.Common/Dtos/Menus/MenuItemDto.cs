using Microsoft.FluentUI.AspNetCore.Components;

namespace SMS.Common.Dtos.Menus
{
    public class MenuItemDto
    {
        public string Title { get; set; } = "";
        public string Href { get; set; } = "";
        public Icon? Icon { get; set; }  
        public Color IconColor { get; set; } = Color.Accent;
    }
}
