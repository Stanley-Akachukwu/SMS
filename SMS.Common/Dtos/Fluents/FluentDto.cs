namespace SMS.Common.Dtos.Fluents
{
    public class FluentDto
    {
        public string IconName { get; set; } = default!;
        public string IconSet { get; set; } = "Filled"; // e.g., "Filled", "Regular"
        public string IconColor { get; set; } = "Success"; // e.g., "Success", "Warning", etc.
        public string IconSize { get; set; }          // e.g., "Size16", "Size20"
    }
}
