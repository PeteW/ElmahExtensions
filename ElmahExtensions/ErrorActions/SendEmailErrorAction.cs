using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Xml.Serialization;
using Elmah;
using ElmahExtensions.Utils;

namespace ElmahExtensions.ErrorActions
{
    [Serializable]
    public class SendEmailErrorAction:ErrorAction
    {
        private static ErrorMailHtmlFormatter _errorMailHtmlFormatter = new ErrorMailHtmlFormatter();

        [XmlAttribute]
        public string Recipients { get; set; }
        [XmlAttribute]
        public string CcRecipients { get; set; }
        [XmlAttribute]
        public string FormattedSubject { get; set; }
        [XmlAttribute]
        public string FormattedBody { get; set; }
        [XmlAttribute]
        public string From { get; set; }
        /// <summary>
        /// This is to compensate for a bug in system.net mail where you cannot set the configuration option for enableSsl
        /// </summary>
        [XmlAttribute]
        public string EnableSsl { get; set; }

        public bool IsTlsEnabled
        {
            get
            {
                try
                {
                    return bool.Parse(EnableSsl);
                }
                catch
                {
                    return false;
                }
            }
        }

        public override void Run(Error error)
        {
            var message = new MailMessage();
            Recipients.AssertNotNullOrEmpty("Recipients");
            FormattedSubject.AssertNotNullOrEmpty("FormattedSubject");
            FormattedBody.AssertNotNullOrEmpty("FormattedBody");
            From.AssertNotNullOrEmpty("From");
            var stringBuilder = new StringBuilder(FormatString(FormattedBody,error));
            using (var htmlTextWriter = new HtmlTextWriter(new StringWriter(stringBuilder)))
            {
                _errorMailHtmlFormatter.Format(htmlTextWriter,error);
            }
            message.From = new MailAddress(From);
            Recipients.Split(new char[]{','})
                .Where(x=>!string.IsNullOrEmpty(x))
                .ToList()
                .ForEach(x=>message.To.Add(x));
            (CcRecipients ?? "").Split(new char[] {','})
                                .Where(x => !string.IsNullOrEmpty(x))
                                .ToList()
                                .ForEach(x => message.To.Add(x));
            message.Subject = FormatString(FormattedSubject,error);
            message.Body = stringBuilder.ToString();
            message.IsBodyHtml = true;
            var client = new SmtpClient();
            client.EnableSsl = IsTlsEnabled;
            client.Send(message);
        }
    }
}