using System.Net;
using System.Net.Mail;

namespace CrystalApi.Services;

public class EmailService
{
    public bool Send(string toName, string toEmail, string subject, string body, string fromName = "Crystal SM", string fromEmail = "no-reply@crystalsm.com")
    {
        var smtpClient = new SmtpClient("smtp.mailtrap.io", 2525);
        smtpClient.Credentials = new NetworkCredential("44dc86844ed023", "7aeb6c48aaac1c");
        smtpClient.EnableSsl = true;

        var mail = new MailMessage();
        mail.From = new MailAddress(fromEmail, fromName);
        mail.To.Add(new MailAddress(toEmail, toName));
        mail.Subject = subject;
        mail.Body = body;
        mail.IsBodyHtml = true;

        try
        {
            smtpClient.Send(mail);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}