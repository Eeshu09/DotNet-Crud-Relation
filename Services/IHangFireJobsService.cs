using AutoMapper;
using Crud.Context;
using Crud.Interface;
using Crud.Model.Hangfire;
using MimeKit;

namespace Crud.Services
{
    public class IHangFireJobsService:IHangFireJobs
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IServiceScopeFactory _scopeFactory;
        //Dont inject Unitof works , mapper etc to avoid cyclic reference
        public IHangFireJobsService(EmailConfiguration emailConfiguration, IServiceScopeFactory serviceScopeFactory)
        {
            _emailConfig = emailConfiguration;
            _scopeFactory = serviceScopeFactory;


        }



        public async Task SendMail(string emails, string subject, string msg)
        {

            Message message = new Message(emails.Split(',').ToList(), subject, msg);

            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);


        }
        public async Task SendMailWithAttachments(string emails, string subject, string msg, string[] filepaths)
        {
            Message message = new Message(emails.Split(',').ToList(), subject, msg);

            var emailMessage = CreateEmailMessage(message, filepaths);

            Send(emailMessage, filepaths);

        }
        private MimeMessage CreateEmailMessage(Message message, string[] filepaths = null)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = message.Content;
            if (filepaths != null)
            {
                foreach (var filepath in filepaths)
                {
                    builder.Attachments.Add(filepath);
                }
            }
            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage, string[] attachementPath = null)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    string test = ex.Message;

                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                    if (attachementPath != null)
                    {
                        foreach (var item in attachementPath)
                        {
                            if (!string.IsNullOrEmpty(item))
                            {
                                if (System.IO.File.Exists(item))
                                {
                                    System.IO.File.Delete(item);
                                }
                            }
                        }
                    }
                }
            }
        }
        public async Task BackGroundJob()
        {

            using (var scope = _scopeFactory.CreateScope())
            {
                var uow = scope.ServiceProvider.GetRequiredService<UserContext>();
                var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

                //var dbdata = uow.Admins.FirstOrDefault();
                //if (dbdata != null)
                //{
                //    dbdata.ModifiedOn = DateTime.Now;
                //    uow.Admins.Update(dbdata);
                //    uow.SaveChanges();
                //}

            }
        }
    }
}
