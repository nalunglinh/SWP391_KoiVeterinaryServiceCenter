using System.Net;
using System.Net.Mail;
using KoiServiceVetBooking.Models;
using Microsoft.Extensions.Configuration;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public void SendContactEmail(ContactViewModel model)
    {
        var smtpSettings = _config.GetSection("SmtpSettings");
        var fromAddress = smtpSettings["Username"];
        var toAddress = "koicareservice@gmail.com"; // Email nhận phản hồi

#pragma warning disable CS8604 // Possible null reference argument.
        var mailMessage = new MailMessage(fromAddress, toAddress)
        {
            Subject = "New Contact",
            Body = $"From: {model.FullName}\nEmail: {model.Email}\nPhone: {model.Phone}\nMessage: {model.Message}",
            IsBodyHtml = false
        };
#pragma warning restore CS8604 // Possible null reference argument.

        using (var smtpClient = new SmtpClient())
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            smtpClient.Host = smtpSettings["Host"];
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.
            smtpClient.Port = int.Parse(smtpSettings["Port"]);
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning disable CS8604 // Possible null reference argument.
            smtpClient.EnableSsl = bool.Parse(smtpSettings["EnableSSL"]);
#pragma warning restore CS8604 // Possible null reference argument.
            smtpClient.Credentials = new NetworkCredential(fromAddress, smtpSettings["Password"]);
            smtpClient.Send(mailMessage);
        }
    }


}
