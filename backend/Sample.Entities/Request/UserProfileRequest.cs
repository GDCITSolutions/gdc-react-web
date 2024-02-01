namespace BE.LocalAccountabilitySystem.Entities.Request
{
    public class UserProfileRequest
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


        public string EmailAddress { get; set; }


        public string Password { get; set; }


        public string NewPassword { get; set; }
    }
}
