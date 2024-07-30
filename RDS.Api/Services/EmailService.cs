namespace RDS.Api.Services;

public class EmailService
{
    public bool Send(string toName,
        string toEmail,
        string subject,
        string body,
        string fromName = "Equipe RDS Devs.",
        string fromEmail = "roberto.rn@gmail.com")
    {
        var smtpClient = new SmtpClient(ApiConfiguration.Smtp.Host, ApiConfiguration.Smtp.Port);

        smtpClient.Credentials = new NetworkCredential(ApiConfiguration.Smtp.UserName, ApiConfiguration.Smtp.Password);
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
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
        catch
        {
            return false;
        }
    }
}