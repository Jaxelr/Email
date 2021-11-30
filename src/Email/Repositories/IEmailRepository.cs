using System;
using System.Collections.Generic;
using Email.Models;

namespace Email.Repositories;

public interface IEmailRepository : IDisposable
{
    /// <summary>
    /// Add sender email address
    /// </summary>
    /// <param name="from"></param>
    IEmailRepository From(string from);

    /// <summary>
    /// Add To email recipients
    /// </summary>
    /// <param name="to"></param>
    IEmailRepository To(ICollection<string> to);

    /// <summary>
    /// Add Cc emails recipients
    /// </summary>
    /// <param name="cc"></param>
    IEmailRepository Cc(ICollection<string> cc);

    /// <summary>
    /// Add Bcc emails recipients
    /// </summary>
    /// <param name="bcc"></param>
    IEmailRepository Bcc(ICollection<string> bcc);

    /// <summary>
    /// Add email subject
    /// </summary>
    /// <param name="subject"></param>
    IEmailRepository Subject(string subject);

    /// <summary>
    /// Add email body
    /// </summary>
    /// <param name="body"></param>
    IEmailRepository Body(string body);

    /// <summary>
    /// Flag email as high priority
    /// </summary>
    IEmailRepository HighPriority();

    /// <summary>
    /// Flag email as low priority
    /// </summary>
    IEmailRepository LowPriority();

    /// <summary>
    /// Include an attachment to the email
    /// </summary>
    /// <param name="attachment"></param>
    IEmailRepository Attach(Attachment attachment);

    /// <summary>
    /// Add html flag on message
    /// </summary>
    IEmailRepository BodyAsHtml();

    /// <summary>
    /// Remove html flag on message
    /// </summary>
    IEmailRepository BodyAsPlainText();

    /// <summary>
    /// Send mail message
    /// </summary>
    bool Send();
}
