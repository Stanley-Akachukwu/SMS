using SMS.Common.Enums;

namespace SMS.Common.Authorization
{
    public static class UserRoleTypes
    {
        public static List<RoleType> GetUserRoleTypes()
        {
            return Enum.GetValues(typeof(UserRoleType))
                .Cast<UserRoleType>()
                .Where(p => p != UserRoleType.None)
                .Select(p => new RoleType
                {
                    Id = (int)p,
                    Name = p.ToString(),
                })
                .ToList();
        }
    }
}
