using SendReportService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SendReportService.ReportSamples
{
    public static class CommomReport
    {
        public static MailData SendReport(List<Ratings> data, MailData mailData)
        {
            mailData.subject = "ТОП 10 самых высокооцениваемых заведений";
            var mostPopular = data.OrderByDescending(item => item.rate).First();
            StringBuilder body = new StringBuilder();
            var text =  $"Самое популярное место {mostPopular.name} с рейтингом {mostPopular.rate} по адресу {mostPopular.adress}";
            body.Append(text);
            body.Append("<table border=1><tr>" +
                "<th> Место </th>" +
                "<th> Организация </th>" +
                "<th > Адрес </th >" +
                "<th> Рейтинг </th>" +
                "</tr>");
            var counter = 1;
            foreach(var item in data)
            {
                body.Append("<tr><td>" + counter + "</td>" +
                    "<td>" + item.name + "</td>" +
                    "<td>" + item.adress + "</td>" +
                    "<td>" + item.rate + "</td> </tr>");
                counter++;
            }
            body.Append("</table>");
            mailData.body = body.ToString();
            return mailData;
        }
    }
}
