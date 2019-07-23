using System;
using System.Collections.Generic;
using System.Linq;
using SendGrid;
using SendGrid.Helpers.Mail;
using e = EmailService.Entities.Models;

namespace EmailService.Repositories
{
    public class SendGridRepository : IEmailRepository
    {
        private readonly SendGridClient client;
        public SendGridMessage Message;
        private bool bodyIsHtml = true;

        public SendGridRepository(string apiKey)
        {
            client = new SendGridClient(apiKey);
        }

        public SendGridRepository(string apiKey, SendGridMessage message)
        {
            client = new SendGridClient(apiKey);
            Message = message;
        }

        public IEmailRepository Attach(e.Attachment attachment)
        {
            if (attachment != null)
            {
                Message.AddAttachment(attachment.Name, Convert.ToBase64String(attachment.Content), type: attachment.ContentType);
            }

            return this;
        }

        public IEmailRepository Bcc(IEnumerable<string> bcc)
        {
            if (bcc != null)
            {
                Message.AddBccs(bcc.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

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

        public IEmailRepository BodyAsHtml()
        {
            bodyIsHtml = true;
            return this;
        }

        public IEmailRepository BodyAsPlainText()
        {
            bodyIsHtml = false;
            return this;
        }

        public IEmailRepository Cc(IEnumerable<string> cc)
        {
            if (cc != null)
            {
                Message.AddCcs(cc.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

        public IEmailRepository From(string from)
        {
            Message.From = new EmailAddress(from);

            return this;
        }

        public bool Send()
        {
            client.SendEmailAsync(Message);
            return true;
        }

        public IEmailRepository Subject(string subject)
        {
            Message.SetSubject(subject);
            return this;
        }

        public IEmailRepository To(IEnumerable<string> to)
        {
            if (to != null)
            {
                Message.AddTos(to.Select(x => new EmailAddress(x)).ToList());
            }

            return this;
        }

        public IEmailRepository HighPriority() => this;

        public IEmailRepository LowPriority() => this;
    }
}
