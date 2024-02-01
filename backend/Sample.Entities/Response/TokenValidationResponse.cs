namespace BE.LocalAccountabilitySystem.Entities.Response
{
    public class TokenValidationResponse
    {
        #region Constructors
        public TokenValidationResponse() { }
        #endregion

        #region Public Properties
        public bool IsValid { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        #endregion
    }
}
