using SMS.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SMS.Common.Helpers
{
    public static class EnumHelper
    {
        public static List<EnumDto> GetEnumList<T>() where T : Enum
        {
            var workflowTypes = Enum.GetValues(typeof(WorkflowType))
    .Cast<WorkflowType>()
    .Select(e => new
    {
        Id = (int)e,
        Name = e.GetType()
               .GetMember(e.ToString())
               .First()
               .GetCustomAttribute<DisplayAttribute>()?.Name
               ?? e.ToString()
    })
    .ToList();
            return workflowTypes.Select(e => new EnumDto { Id = e.Id, Name = e.Name }).ToList();
        }

        
    }
}
