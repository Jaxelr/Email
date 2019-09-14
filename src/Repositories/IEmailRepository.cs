using System.Collections.Generic;
using EmailService.Entities.Models;

namespace EmailService.Repositories
{
    public interface IEmailRepository
    {

        /// <summary>
        /// Add sender email address
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        IEmailRepository From(string from);

        /// <summary>
        /// Add To email recipients
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        IEmailRepository To(IEnumerable<string> to);

        /// <summary>
        /// Add Cc emails recipients
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        IEmailRepository Cc(IEnumerable<string> cc);

        /// <summary>
        /// Add Bcc emails recipients
        /// </summary>
        /// <param name="bcc"></param>
        /// <returns></returns>
        IEmailRepository Bcc(IEnumerable<string> bcc);

        /// <summary>
        /// Add email subject
        /// </summary>
        /// <param name="subject"></param>
        /// <returns></returns>
        IEmailRepository Subject(string subject);

        /// <summary>
        /// Add email body
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        IEmailRepository Body(string body);

        /// <summary>
        /// Flag email as high priority
        /// </summary>
        /// <returns></returns>
        IEmailRepository HighPriority();

        /// <summary>
        /// Flag email as low priority
        /// </summary>
        /// <returns></returns>
        IEmailRepository LowPriority();

        /// <summary>
        /// Include an attachment to the email
        /// </summary>
        /// <param name="attachment"></param>
        /// <returns></returns>
        IEmailRepository Attach(Attachment attachment);

        /// <summary>
        /// Add html flag on message
        /// </summary>
        /// <returns></returns>
        IEmailRepository BodyAsHtml();

        /// <summary>
        /// Remove html flag on message
        /// </summary>
        /// <returns></returns>
        IEmailRepository BodyAsPlainText();

        /// <summary>
        /// Send mail message
        /// </summary>
        /// <returns></returns>
        bool Send();
    }
}
