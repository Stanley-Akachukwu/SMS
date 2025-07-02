

using System.ComponentModel.DataAnnotations;

namespace SMS.Common.Enums
{
    public enum WorkflowType
    {
        None = 0,
        Entrance = 1,
        Staffing = 2,
        [Display(Name = "Result Computation")]
        ResultComputation = 3
    }
}
