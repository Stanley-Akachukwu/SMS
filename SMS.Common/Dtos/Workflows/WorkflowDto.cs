using SMS.Common.Enums;

namespace SMS.Common.Dtos.Workflows
{
    
    public class WorkflowDto  
    {
        public string? Id { get; set; }
        public string? RegLink { get; set; }
        public string? Name { get; set; }
        public string? Session { get; set; }
        public string? Description { get; set; }
        public string? CreatedByUserId { get; set; }
        public SMSCoreAction Action { get; set; }
        public DateTimeOffset DateCreated { get; set; } = DateTime.Now;
        public WorkflowType WorkflowType { get; set; } = WorkflowType.None;
    }
}
