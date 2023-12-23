using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Email.Extensions;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Model = Email.Models;

namespace Email.Repositories;

public class SmtpRepository : IEmailRepository
{
    private readonly SmtpClient? client;
    private readonly ILogger<SmtpRepository> logger;
    private readonly RetryPolicy policy;
    public MailMessage? Message;
    private bool bodyIsHtml = true;

    /// <summary>
    /// Initializes the SmtpRepository with the server
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public SmtpRepository(Model.AppSettings settings, ILogger<SmtpRepository> logger, RetryPolicy policy) : this(settings, new MailMessage(), logger, policy)
    {
    }

    /// <summary>
    /// Initialize an SmtpRepository class with a mail message
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="message"></param>
    /// <param name="logger"></param>
    public SmtpRepository(Model.AppSettings settings, MailMessage message, ILogger<SmtpRepository> logger, RetryPolicy policy)
    {
        this.logger = logger;
        this.policy = policy;

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
        to?.ForEach(i => Message?.To.Add(i));
        return this;
    }

    /// <summary>
    /// Add Cc emails recipients
    /// </summary>
    /// <param name="cc"></param>
    public IEmailRepository Cc(ICollection<string> cc)
    {
        cc?.ForEach(i => Message?.CC.Add(i));
        return this;
    }

    /// <summary>
    /// Add Bcc emails recipients
    /// </summary>
    /// <param name="bcc"></param>
    public IEmailRepository Bcc(ICollection<string> bcc)
    {
        bcc?.ForEach(i => Message?.Bcc.Add(i));
        return this;
    }

    /// <summary>
    /// Add sender email address
    /// </summary>
    /// <param name="from"></param>
    public IEmailRepository From(string from)
    {
        Message = new MailMessage
        {
            From = new MailAddress(from)
        };

        return this;
    }

    public MailMessage GetMessage() => Message!;

    /// <summary>
    /// Add email subject
    /// </summary>
    /// <param name="subject"></param>
    public IEmailRepository Subject(string subject)
    {
        if (Message is { })
            Message.Subject = subject;
        return this;
    }

    /// <summary>
    /// Add email body
    /// </summary>
    /// <param name="body"></param>
    public IEmailRepository Body(string body)
    {
        if (Message is { })
            Message.Body = body;
        return this;
    }

    /// <summary>
    /// Flag email as high priority
    /// </summary>
    public IEmailRepository HighPriority()
    {
        if (Message is { })
            Message.Priority = MailPriority.High;
        return this;
    }

    /// <summary>
    /// Flag email as low priority
    /// </summary>
    public IEmailRepository LowPriority()
    {
        if (Message is { })
            Message.Priority = MailPriority.Low;
        return this;
    }

    /// <summary>
    /// Include an attachment to the email
    /// </summary>
    /// <param name="attachment"></param>
    public IEmailRepository Attach(Model.Attachment? attachment)
    {
        if (attachment is { })
        {
            var _attachment = new Attachment(new MemoryStream(attachment.Content), attachment.Name, attachment.ContentType);
            if (!Message!.Attachments.Contains(_attachment))
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
    public async Task<bool> SendAsync()
    {
        try
        {
            if (Message is { })
                Message.IsBodyHtml = bodyIsHtml;
            await policy.Execute(async () => await client?.SendMailAsync(Message!)!);
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
