using SMS.Common.Enums;
using System.Collections.Generic;

namespace SMS.Common.Dtos.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MenuSection MenuSectionMap { get; set; }
    }
}

