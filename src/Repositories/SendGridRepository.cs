﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using SendGrid;
using SendGrid.Helpers.Mail;
using Model = Email.Models;

namespace Email.Repositories;

public class SendGridRepository : IEmailRepository
{
    private readonly SendGridClient? client;
    private readonly ILogger<SendGridRepository> logger;
    private readonly RetryPolicy policy;
    public SendGridMessage? Message;
    private bool bodyIsHtml = true;

    /// <summary>
    /// Initializes the SendgridRepository with logging and the ApiKey
    /// </summary>
    public SendGridRepository(Model.AppSettings settings, ILogger<SendGridRepository> logger, RetryPolicy policy) : this(settings, new SendGridMessage(), logger, policy)
    {
    }

    /// <summary>
    /// Initialize a SendGridRepository class with a mail message
    /// </summary>
    public SendGridRepository(Model.AppSettings settings, SendGridMessage message, ILogger<SendGridRepository> logger, RetryPolicy policy)
    {
        this.logger = logger;
        this.policy = policy;

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
    public IEmailRepository Attach(Model.Attachment? attachment)
    {
        if (attachment is { })
        {
            Message?.AddAttachment(attachment.Name, Convert.ToBase64String(attachment.Content!), type: attachment.ContentType);
        }

        return this;
    }

    /// <summary>
    /// Add Bcc emails recipients
    /// </summary>
    public IEmailRepository Bcc(ICollection<string> bcc)
    {
        if (bcc?.Count > 0)
        {
            Message?.AddBccs(bcc.Select(x => new EmailAddress(x)).ToList());
        }

        return this;
    }

    /// <summary>
    /// Add email body
    /// </summary>
    public IEmailRepository Body(string body)
    {
        if (Message is { } && bodyIsHtml)
            Message.HtmlContent = body;

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
    public IEmailRepository Cc(ICollection<string> cc)
    {
        if (cc?.Count > 0)
        {
            Message?.AddCcs(cc.Select(x => new EmailAddress(x)).ToList());
        }

        return this;
    }

    /// <summary>
    /// Add sender email address
    /// </summary>
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
            await policy.Execute(async () => await client?.SendEmailAsync(Message)!);
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
    public IEmailRepository Subject(string subject)
    {
        Message?.SetSubject(subject);
        return this;
    }

    /// <summary>
    /// Add To email recipients
    /// </summary>
    public IEmailRepository To(ICollection<string> to)
    {
        if (to?.Count > 0)
        {
            Message?.AddTos(to.Select(x => new EmailAddress(x)).ToList());
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
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Message = null;
    }
}
