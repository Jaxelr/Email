using Api.Model.Email.Entities;
using System.Collections.Generic;

namespace Repoes
{
    public interface IEmailRepository
    {
        IEmailRepository From(string from);

        IEmailRepository To(IEnumerable<string> to);

        IEmailRepository Cc(IEnumerable<string> cc);

        IEmailRepository Bcc(IEnumerable<string> bcc);

        IEmailRepository Subject(string subject);

        IEmailRepository Body(string body);

        IEmailRepository HighPriority();

        IEmailRepository LowPriority();

        IEmailRepository Attach(Attachment attachment);

        IEmailRepository BodyAsHtml();

        IEmailRepository BodyAsPlainText();

        bool Send();
    }
}