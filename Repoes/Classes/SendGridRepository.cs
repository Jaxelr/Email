using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repoes.Classes
{
    public class SendGridRepository : IEmailRepository
    {
        private SendGridClient _client;
        public SendGridMessage Message;
        private bool _bodyIsHtml = true;

        public SendGridRepository(string apiKey)
        {
            _client = new SendGridClient(apiKey);
        }

        public SendGridRepository(string apiKey, SendGridMessage message)
        {
            _client = new SendGridClient(apiKey);
            Message = message;
        }

        public IEmailRepository Attach(Api.Model.Email.Entities.Attachment attachment)
        {
            if (attachment != null)
            {
                Message.AddAttachment(attachment.Name, content: Convert.ToBase64String(attachment.Content), type: attachment.ContentType);
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
            if (_bodyIsHtml)
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
            _bodyIsHtml = true;
            return this;
        }

        public IEmailRepository BodyAsPlainText()
        {
            _bodyIsHtml = false;
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
            _client.SendEmailAsync(Message);
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
