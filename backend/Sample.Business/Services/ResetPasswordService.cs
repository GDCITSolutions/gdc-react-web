using BE.LocalAccountabilitySystem.Common.Security;
using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Schema;
using BE.LocalAccountabilitySystem.Business.Email;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using BE.LocalAccountabilitySystem.Entities.Response;

namespace BE.LocalAccountabilitySystem.Business.Services
{
    /// <summary>
    /// Handle password reset functionality
    /// </summary>
    public interface IResetPasswordService
    {
        /// <summary>
        /// Checks the database for the provided email address.
        /// </summary>
        /// <param name="userEmailAddress">Provided email address of user.</param>
        /// <returns></returns>
        Task<bool> SendEmail(string userEmailAddress);

        /// <summary>
        /// Checks to see if a token that has not expired is being passed.
        /// </summary>
        /// <param name="token">Token being passed</param>
        /// <returns></returns>
        Task<TokenValidationResponse> ValidateToken(string token);

        /// <summary>
        /// Hash/salt and resets new password. Uses reset token.
        /// </summary>
        /// <param name="newPassword">New Password being passed in</param>
        /// <param name="resetToken">Reset Token that is being used</param>
        /// <returns></returns>
        Task<bool> ResetPassword(string newPassword, string resetToken);
    }

    public class ResetPasswordService : IResetPasswordService
    {
        #region Variables
        private readonly SampleContext _lasContext;
        private readonly IResetPasswordEmailer _resetPasswordEmailer;
        #endregion

        #region Constructors
        public ResetPasswordService(SampleContext lasContext, IResetPasswordEmailer resetPasswordEmailer)
        {
            _lasContext = lasContext;
            _resetPasswordEmailer = resetPasswordEmailer;
        }
        #endregion

        #region Priavte Methods
        private bool IsStrongPassword(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            bool hasLetter = Regex.IsMatch(password, "[a-zA-Z]");
            bool hasNumber = Regex.IsMatch(password, "\\d");
            bool hasSpecialChar = Regex.IsMatch(password, "[@%^$!*]");

            return hasLetter && hasNumber && hasSpecialChar;
        }
        #endregion

        #region Public Methods
        public async Task<bool> SendEmail(string userEmailAddress)
        {
            var user = await _lasContext.User.FirstOrDefaultAsync(e => e.EmailAddress == userEmailAddress);

            if (user == null)
            {
                return false;
            }
            else
            {
                var tokenBytes = SecurityUtil.GenerateSalt();
                string resetTokenString = Convert.ToBase64String(tokenBytes);
                resetTokenString = resetTokenString.Replace("+", "");
                var hashedToken = SecurityUtil.HashToken(resetTokenString);

                var passwordResetToken = new PasswordResetToken
                {
                    UserId = user.Id,
                    Token = hashedToken,
                    IsUsed = false,
                    ExpirationDate = DateTime.UtcNow.AddMinutes(30),
                    SystemStatusId = user.SystemStatusId,
                    User = user,
                };

                _lasContext.PasswordResetToken.Add(passwordResetToken);

                await _lasContext.SaveChangesAsync();

                _resetPasswordEmailer.SendResetPassword(userEmailAddress, resetTokenString);

                return true;
            }
        }

        public async Task<TokenValidationResponse> ValidateToken(string token)
        {
            TokenValidationResponse validationResponse = new TokenValidationResponse();

            if (string.IsNullOrEmpty(token))
            {
                validationResponse.IsValid = false;
                validationResponse.Email = null;
                validationResponse.Message = "Token is null or empty.";
                return validationResponse;
            }

            byte[] hashedToken = SecurityUtil.HashToken(token);

            var latestPasswordResetToken = await _lasContext.PasswordResetToken
                .Where(t => t.Token.SequenceEqual(hashedToken) &&
                t.ExpirationDate >= DateTime.UtcNow &&
                t.IsUsed == false) 
                .OrderByDescending(t => t.CreatedDate)
                .Include(u => u.User)
                .FirstOrDefaultAsync();


            if (latestPasswordResetToken == null)
            {
                validationResponse.IsValid = false;
                validationResponse.Email = null;
                validationResponse.Message = "Token validation failed.";
            }
            else
            {
                validationResponse.IsValid = true;
                validationResponse.Email = latestPasswordResetToken.User.EmailAddress;
                validationResponse.Message = "Token is valid.";
            }

            return validationResponse;
        }

        public async Task<bool> ResetPassword(string newPassword, string resetToken)
        {
            if (!IsStrongPassword(newPassword))
            {
                return false;
            }

            var passwordBytes = SecurityUtil.GenerateSalt();

            var newHashedPassword = SecurityUtil.HashPassword(newPassword, passwordBytes);

            byte[] tokenBytes = SecurityUtil.HashToken(resetToken);
            var token = await _lasContext.PasswordResetToken
                .Where(t => t.Token.SequenceEqual(tokenBytes))
                .Include(t => t.User)
                .FirstOrDefaultAsync();

            token.User.Salt = passwordBytes;
            token.User.Password = newHashedPassword;
            token.IsUsed = true;

            await _lasContext.SaveChangesAsync();

            return true;
        }
        #endregion
    }
}
