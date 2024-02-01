using HandlebarsDotNet;
using Microsoft.AspNetCore.Hosting;
using MimeKit;

namespace BE.LocalAccountabilitySystem.Business.Email.Base
{
    /// <summary>
    /// Provide basic email operations for inheriting emailers
    /// </summary>
    public class BaseEmailer
    {
        private readonly IWebHostEnvironment _environment;

        public BaseEmailer(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Take into account our current environment in order to find some template
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected string BuildPath(string fileName)
        {
            return $"{_environment.WebRootPath}{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}{fileName}";
        }

        /// <summary>
        /// Use Handlerbars to load the template and fill in any replacements
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        protected string BuildTemplate(string fileName, object replacements)
        {
            var templatePath = BuildPath(fileName);
            var unformattedTemplate = Handlebars.Compile(File.ReadAllText(templatePath));

            return unformattedTemplate(replacements);
        }

        /// <summary>
        /// Build an email message
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="to"></param>
        /// <param name="htmlBody"></param>
        /// <returns></returns>
        protected MimeMessage BuildMessage(string subject, IList<string> to, string htmlBody)
        {
            var message = new MimeMessage();

            foreach (var t in to)
                message.To.Add(new MailboxAddress(t, t));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = htmlBody
            };

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }
    }
}
