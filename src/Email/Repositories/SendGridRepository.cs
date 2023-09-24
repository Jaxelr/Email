using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using Model = Email.Models;

namespace Email.Repositories;

public class SendGridRepository : IEmailRepository
{
    private readonly SendGridClient client;
    private readonly ILogger<SendGridRepository> logger;
    public SendGridMessage Message;
    private bool bodyIsHtml = true;

    /// <summary>
    /// Initializes the SendgridRepository with logging and the ApiKey
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="logger"></param>
    public SendGridRepository(Model.AppSettings settings, ILogger<SendGridRepository> logger) : this(settings, new SendGridMessage(), logger)
    {
    }

    /// <summary>
    /// Initialize a SendGridRepository class with a mail message
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="message"></param>
    /// <param name="logger"></param>
    public SendGridRepository(Model.AppSettings settings, SendGridMessage message, ILogger<SendGridRepository> logger)
    {
        this.logger = logger;
        try
        {
            client = new SendGridClient(settings.ApiKey);
            Message = message;
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "Error configuring the Sendgrid Client");
        }
    }

    /// <summary>
    /// Include an attachment to the email
    /// </summary>
    /// <param name="attachment"></param>
    public IEmailRepository Attach(Model.Attachment attachment)
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
    public IEmailRepository Bcc(ICollection<string> bcc)
    {
        if (bcc?.Count > 0)
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
    public IEmailRepository Cc(ICollection<string> cc)
    {
        if (cc?.Count > 0)
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
        Message = new SendGridMessage
        {
            From = new EmailAddress(from)
        };

        return this;
    }

    /// <summary>
    /// Send mail message
    /// </summary>
    public async Task<bool> SendAsync()
    {
        try
        {
            await IEmailRepository.Retry(3, TimeSpan.FromSeconds(1), async () => await client.SendEmailAsync(Message));
            Message = null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending email");
            return false;
        }

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
    public IEmailRepository To(ICollection<string> to)
    {
        if (to?.Count > 0)
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
    public void Dispose()
    {
        Message = null;
        GC.SuppressFinalize(this);
    }
}
