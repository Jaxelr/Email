using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;

namespace Repoes
{
    public class SmtpRepository : IEmailRepository
    {
        private SmtpClient _client;
        public MailMessage Message;
        private bool _bodyIsHtml = true;

        public SmtpRepository(string smtpServer)
        {
            _client = new SmtpClient(smtpServer);
            Message = new MailMessage();
        }

        public SmtpRepository(string smtpServer, MailMessage message)
        {
            _client = new SmtpClient(smtpServer);
            Message = message;
        }

        public IEmailRepository To(IEnumerable<string> to)
        {
            if (to != null)
                to.ToList().ForEach(i => Message.To.Add(i));
            return this;
        }

        public IEmailRepository Cc(IEnumerable<string> cc)
        {
            if (cc != null)
            {
                cc.ToList().ForEach(i => Message.CC.Add(i));
            }

            return this;
        }

        public IEmailRepository Bcc(IEnumerable<string> bcc)
        {
            if (bcc != null)
            {
                bcc.ToList().ForEach(i => Message.Bcc.Add(i));
            }

            return this;
        }

        public IEmailRepository From(string from)
        {
            Message.From = new MailAddress(from);
            return this;
        }

        public IEmailRepository Subject(string subject)
        {
            Message.Subject = subject;
            return this;
        }

        public IEmailRepository Body(string body)
        {
            Message.Body = body;
            return this;
        }

        public IEmailRepository HighPriority()
        {
            Message.Priority = MailPriority.High;
            return this;
        }

        public IEmailRepository LowPriority()
        {
            Message.Priority = MailPriority.Low;
            return this;
        }

        public IEmailRepository Attach(Api.Model.Email.Entities.Attachment attachment)
        {
            if (attachment != null)
            {
                var _attachment = new Attachment(new MemoryStream(attachment.Content), attachment.Name, attachment.ContentType);
                if (!Message.Attachments.Contains(_attachment))
                    Message.Attachments.Add(_attachment);
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

        public bool Send()
        {
            Message.IsBodyHtml = _bodyIsHtml;
            _client.Send(Message);
            return true;
        }

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();

            if (Message != null)
                Message.Dispose();
        }
    }
}
