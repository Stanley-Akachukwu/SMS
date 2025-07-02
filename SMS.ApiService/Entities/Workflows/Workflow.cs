using SMS.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SMS.ApiService.Entities.Workflows
{
    public class Workflow : BaseEntity<string>
    {
        [MaxLength(400)]
        public string? RegLink { get; set; }
        [MaxLength(128)]
        public string? Name { get; set; }
        [MaxLength(128)]
        public string? Session { get; set; }
       
        public WorkflowType WorkflowType { get; set; } = WorkflowType.None;
    }
}
