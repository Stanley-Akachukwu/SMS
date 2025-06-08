using Microsoft.FluentUI.AspNetCore.Components;
using SMS.Common.Enums;

namespace SMS.Common.Dtos.Menus
{
    public class MenuGroupDto
    {
        public MenuSection Section { get; set; }
        public string DisplayTitle { get; set; } = "";
        public string Tooltip { get; set; } = "";
        public Icon? Icon { get; set; }
        public List<MenuItemDto> Items { get; set; } = new();
    }
}
