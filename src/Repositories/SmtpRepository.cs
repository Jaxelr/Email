using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using EmailService.Extensions;
using Microsoft.Extensions.Logging;

using model = EmailService.Models;

namespace EmailService.Repositories
{
    public class SmtpRepository : IEmailRepository
    {
        private readonly SmtpClient client;
        private readonly ILogger<SmtpRepository> logger;
        public MailMessage Message;
        private bool bodyIsHtml = true;

        /// <summary>
        /// Initializes the SmtpRepository with the server
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        public SmtpRepository(model.AppSettings settings, ILogger<SmtpRepository> logger) : this(settings, new MailMessage(), logger)
        {
        }

        /// <summary>
        /// Initialize an SmtpRepository class with a mail message
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="message"></param>
        /// <param name="logger"></param>
        public SmtpRepository(model.AppSettings settings, MailMessage message, ILogger<SmtpRepository> logger)
        {
            this.logger = logger;

            try
            {
                client = new SmtpClient(settings.SmtpServer);
                Message = message;
            }
            catch (Exception ex)
            {
                logger.LogCritical(ex, "Error configuring Stmp Client");
            }
        }

        /// <summary>
        /// Add To email recipients
        /// </summary>
        /// <param name="to"></param>
        public IEmailRepository To(ICollection<string> to)
        {
            to?.ForEach(i => Message.To.Add(i));
            return this;
        }

        /// <summary>
        /// Add Cc emails recipients
        /// </summary>
        /// <param name="cc"></param>
        public IEmailRepository Cc(ICollection<string> cc)
        {
            cc?.ForEach(i => Message.CC.Add(i));

            return this;
        }

        /// <summary>
        /// Add Bcc emails recipients
        /// </summary>
        /// <param name="bcc"></param>
        public IEmailRepository Bcc(ICollection<string> bcc)
        {
            bcc?.ForEach(i => Message.Bcc.Add(i));

            return this;
        }

        /// <summary>
        /// Add sender email address
        /// </summary>
        /// <param name="from"></param>
        public IEmailRepository From(string from)
        {
            Message.From = new MailAddress(from);

            Message.Bcc.Clear();
            Message.CC.Clear();
            Message.To.Clear();

            return this;
        }

        /// <summary>
        /// Add email subject
        /// </summary>
        /// <param name="subject"></param>
        public IEmailRepository Subject(string subject)
        {
            Message.Subject = subject;
            return this;
        }

        /// <summary>
        /// Add email body
        /// </summary>
        /// <param name="body"></param>
        public IEmailRepository Body(string body)
        {
            Message.Body = body;
            return this;
        }

        /// <summary>
        /// Flag email as high priority
        /// </summary>
        public IEmailRepository HighPriority()
        {
            Message.Priority = MailPriority.High;
            return this;
        }

        /// <summary>
        /// Flag email as low priority
        /// </summary>
        public IEmailRepository LowPriority()
        {
            Message.Priority = MailPriority.Low;
            return this;
        }

        /// <summary>
        /// Include an attachment to the email
        /// </summary>
        /// <param name="attachment"></param>
        public IEmailRepository Attach(model.Attachment attachment)
        {
            if (attachment != null)
            {
                var _attachment = new Attachment(new MemoryStream(attachment.Content), attachment.Name, attachment.ContentType);
                if (!Message.Attachments.Contains(_attachment))
                    Message.Attachments.Add(_attachment);
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
        /// Send mail message
        /// </summary>
        public bool Send()
        {
            try
            {
                Message.IsBodyHtml = bodyIsHtml;
                client.SendMailAsync(Message);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error sending email");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Dispose of the client and mail message
        /// </summary>
        public void Dispose()
        {
            client?.Dispose();
            Message?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
