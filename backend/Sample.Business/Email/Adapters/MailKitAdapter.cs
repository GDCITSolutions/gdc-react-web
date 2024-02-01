using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace BE.LocalAccountabilitySystem.Business.Email.Adapters
{
    /// <summary>
    /// Encapsulate mail kit for unit testing purposes
    /// </summary>
    public interface IMailKitAdapter
    {
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        void Send(MimeMessage message, CancellationToken cancellationToken = default);
    }

    public class MailKitAdapter : IMailKitAdapter
    {
        private const string PORT_KEY = "Notifications:Config:Port";
        private const string HOST_KEY = "Notifications:Config:Host";
        private const string USE_SSL_KEY = "Notifications:Config:UseSSL";
        private const string USERNAME_KEY = "Notifications:Config:Username";
        private const string PASSWORD_KEY = "Notifications:Config:Password";
        private const string FROM_KEY = "Notifications:Config:FromAddress";

        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public MailKitAdapter(ILogger<MailKitAdapter> logger, IConfiguration config)
        {
            _config = config;
            _logger = logger;
        }

        public void Send(MimeMessage message, CancellationToken cancellationToken = default)
        {
            try
            {
                int port = int.Parse(_config[PORT_KEY]);
                string host = _config[HOST_KEY];
                bool isSslEnabled = bool.Parse(_config[USE_SSL_KEY]);
                string userName = _config[USERNAME_KEY];
                string password = _config[PASSWORD_KEY];
                string from = _config[FROM_KEY];

                message.From.Add(new MailboxAddress(from, from));

                var socketOptions = isSslEnabled ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.None;

                using (var client = new SmtpClient())
                {
                    client.Connect(host, port, socketOptions, cancellationToken);

                    if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
                        client.Authenticate(userName, password, cancellationToken);

                    client.Send(message, cancellationToken);
                    client.Disconnect(true, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email", ex);
            }
        }
    }
}
