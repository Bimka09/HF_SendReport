using System.Net;
using System.Net.Mail;
using SendReportService.Models;

namespace SendReportService
{
    public static class SendMail
    {
        public static void Send(MailData mailData)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("ncsharplearn@gmail.com", "HighFive");
            // кому отправляем
            MailAddress to = new MailAddress(mailData.mailAdress);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = mailData.subject;
            // текст письма
            m.Body = mailData.body;
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            //SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 465);
            // логин и пароль
            //smtp.Credentials = new NetworkCredential("profittraider", "qjyhdzxotumxssqs");
            smtp.Credentials = new NetworkCredential("ncsharplearn@gmail.com", "k1t2i3f4");
            smtp.EnableSsl = true;
            smtp.Send(m);
            
        }
    }
}
