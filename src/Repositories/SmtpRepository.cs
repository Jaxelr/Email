using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using EmailService.Extensions;
using model = EmailService.Models;

namespace EmailService.Repositories
{
    public class SmtpRepository : IEmailRepository
    {
        private readonly SmtpClient client;
        public MailMessage Message;
        private bool bodyIsHtml = true;

        /// <summary>
        /// Initialize an SmtpRepository class
        /// </summary>
        /// <param name="smtpServer"></param>
        public SmtpRepository(string smtpServer)
        {
            client = new SmtpClient(smtpServer);
            Message = new MailMessage();
        }

        /// <summary>
        /// Initialize an SmtpRepository class with a mail nessage
        /// </summary>
        /// <param name="smtpServer"></param>
        /// <param name="message"></param>
        public SmtpRepository(string smtpServer, MailMessage message)
        {
            client = new SmtpClient(smtpServer);
            Message = message;
        }

        /// <summary>
        /// Add To email recipients
        /// </summary>
        /// <param name="to"></param>
        public IEmailRepository To(IEnumerable<string> to)
        {
            to?.ForEach(i => Message.To.Add(i));
            return this;
        }

        /// <summary>
        /// Add Cc emails recipients
        /// </summary>
        /// <param name="cc"></param>
        public IEmailRepository Cc(IEnumerable<string> cc)
        {
            cc?.ForEach(i => Message.CC.Add(i));

            return this;
        }

        /// <summary>
        /// Add Bcc emails recipients
        /// </summary>
        /// <param name="bcc"></param>
        public IEmailRepository Bcc(IEnumerable<string> bcc)
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
            Message.IsBodyHtml = bodyIsHtml;
            client.Send(Message);
            return true;
        }

        /// <summary>
        /// Dispose of the client and mail message
        /// </summary>
        public void Dispose()
        {
            if (client != null)
            {
                client?.Dispose();
            }

            if (Message != null)
            {
                Message?.Dispose();
            }
        }
    }
}
