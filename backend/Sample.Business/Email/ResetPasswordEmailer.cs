using BE.LocalAccountabilitySystem.Business.Email.Adapters;
using BE.LocalAccountabilitySystem.Business.Email.Base;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace BE.LocalAccountabilitySystem.Business.Email
{
    public interface IResetPasswordEmailer
    {
        void SendResetPassword(string to, string token);
    }
    public class ResetPasswordEmailer : BaseEmailer, IResetPasswordEmailer
    {
        #region Variables
        private const string FORGOT_PASSWORD_TEMPLATE_KEY = "Notifications:TemplateDirectories:ForgotPassword";
        private const string BASE_URL = "FrontendUrl";
        private readonly IMailKitAdapter _mailKitAdapter;
        private readonly IConfiguration _config;
        #endregion

        public ResetPasswordEmailer(IMailKitAdapter mailKitAdapter, IConfiguration config, IWebHostEnvironment environment) : base(environment)
        {
            _mailKitAdapter = mailKitAdapter ?? throw new ArgumentNullException(nameof(mailKitAdapter)); 
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public void SendResetPassword(string toEmail, string token) 
        {
            var body = base.BuildTemplate(_config[FORGOT_PASSWORD_TEMPLATE_KEY], new { EmailAddress = toEmail, BaseUrl = _config[BASE_URL], Token = token });

            var message = base.BuildMessage("Reset your password", new List<string>() { toEmail }, body);

            _mailKitAdapter.Send(message);
        }
    }
}
