namespace Crud.Interface
{
    public interface IHangFireJobs
    {
        Task SendMail(string tomail, string subject, string body);
        Task SendMailWithAttachments(string tomail, string subject, string body, string[] filepaths);
        Task BackGroundJob();
    }
}
