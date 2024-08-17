using LMS.Domain.Ultilities;

namespace LMS.Domain.IService
{
    public interface IEmailSender
    {
        Task Send(MailInformation mailInformation);
    }
}
