namespace BE.LocalAccountabilitySystem.Entities.Response
{
    public class UserManagementResponse
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public int SystemStatusId { get; set; }

        public string SystemStatusName { get; set; }

        public int? LockedByUserId { get; set; }

        public IList<UserManagementRoleResponse> Roles { get; set; }
    }

    public class UserManagementRoleResponse 
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class UserManagementStudentIdentifierResponse
    {
        public int Id { get; set; }

        public string Value { get; set; }
    }
}
