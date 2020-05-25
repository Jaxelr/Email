using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using model = EmailService.Models;

namespace EmailService.Repositories
{
    public class SendGridRepository : IEmailRepository
    {
        private readonly SendGridClient client;
        public SendGridMessage Message;
        private bool bodyIsHtml = true;

        /// <summary>
        /// Initialize a SendGridRepository class
        /// </summary>
        /// <param name="apiKey"></param>
        public SendGridRepository(string apiKey)
        {
            client = new SendGridClient(apiKey);
        }

        /// <summary>
        /// Initialize a SendGridRepository class with a mail message
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="message"></param>
        public SendGridRepository(string apiKey, SendGridMessage message)
        {
            client = new SendGridClient(apiKey);
            Message = message;
        }

        /// <summary>
        /// Include an attachment to the email
        /// </summary>
        /// <param name="attachment"></param>
        public IEmailRepository Attach(model.Attachment attachment)
        {
            if (attachment != null)
            {
                Message.AddAttachment(attachment.Name, Convert.ToBase64String(attachment.Content), type: attachment.ContentType);
            }

            return this;
        }

        /// <summary>
        /// Add Bcc emails recipients
        /// </summary>
        /// <param name="bcc"></param>
        public IEmailRepository Bcc(IEnumerable<string> bcc)
        {
            if (bcc?.Count() > 0)
            {
                Message.AddBccs(bcc.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

        /// <summary>
        /// Add email body
        /// </summary>
        /// <param name="body"></param>
        public IEmailRepository Body(string body)
        {
            if (bodyIsHtml)
            {
                Message.HtmlContent = body;
            }
            else
            {
                Message.PlainTextContent = body;
            }

            return this;
        }

        /// <summary>
        /// Add html flag on message
        /// </summary>
        public IEmailRepository BodyAsHtml()
        {
            bodyIsHtml = true;
            return this;
        }

        /// <summary>
        /// Remove html flag on message
        /// </summary>
        public IEmailRepository BodyAsPlainText()
        {
            bodyIsHtml = false;
            return this;
        }

        /// <summary>
        /// Add Cc emails recipients
        /// </summary>
        /// <param name="cc"></param>
        public IEmailRepository Cc(IEnumerable<string> cc)
        {
            if (cc?.Count() > 0)
            {
                Message.AddCcs(cc.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

        /// <summary>
        /// Add sender email address
        /// </summary>
        /// <param name="from"></param>
        public IEmailRepository From(string from)
        {
            Message.From = new EmailAddress(from);

            return this;
        }

        /// <summary>
        /// Send mail message
        /// </summary>
        public bool Send()
        {
            client.SendEmailAsync(Message);
            return true;
        }

        /// <summary>
        /// Add email subject
        /// </summary>
        /// <param name="subject"></param>
        public IEmailRepository Subject(string subject)
        {
            Message.SetSubject(subject);
            return this;
        }

        /// <summary>
        /// Add To email recipients
        /// </summary>
        /// <param name="to"></param>
        public IEmailRepository To(IEnumerable<string> to)
        {
            if (to?.Count() > 0)
            {
                Message.AddTos(to.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

        /// <summary>
        /// Flag email as high priority
        /// </summary>
        public IEmailRepository HighPriority() => this;

        /// <summary>
        /// Flag email as low priority
        /// </summary>
        public IEmailRepository LowPriority() => this;

        /// <summary>
        /// Dispose of dependencies
        /// </summary>
        public void Dispose() => Message = null;
    }
}
