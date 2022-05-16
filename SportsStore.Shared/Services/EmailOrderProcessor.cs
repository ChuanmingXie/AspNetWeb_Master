/*****************************************************************************
*项目名称:SportsStore.Shared.Services
*项目描述:
*类 名 称:EmailOrderProcessor
*类 描 述:
*创 建 人:Chuanmingxie
*创建时间:2022/5/16 9:42:56
*修 改 人:
*修改时间:
*作用描述:<FUNCTION>
*Copyright @ chuanming 2022. All rights reserved
******************************************************************************/
using System.Net;
using System.Net.Mail;
using System.Text;
using SportsStore.Shared.Common;
using SportsStore.Shared.DataInterface;
using SportsStore.Shared.Entities;

namespace SportsStore.Shared.Services
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;
        }

        public void ProcessOrder(ShopCart shopCart, ShoppingDetails details)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);
                if (emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl=false;
                }
                StringBuilder body = new StringBuilder()
                    .AppendLine("一个新的订单被提交")
                    .AppendLine("------------------")
                    .AppendLine("商品项：");
                foreach (var line in shopCart.Lines)
                {
                    var subTotal = line.Product.Price * line.Quantity;
                    body.AppendFormat($"{line.Quantity} x {line.Product.Name}(小计:{subTotal:c})");
                }
                body.AppendFormat($"订单共计:{shopCart.ComputeTotalValue():c}")
                    .AppendLine("-------------------")
                    .AppendLine("寄送至:")
                    .AppendLine(details.Name)
                    .AppendLine(details.Phone)
                    .AppendLine(details.Line1)
                    .AppendLine(details.Community ?? "")
                    .AppendLine(details.BuldingRoom ?? "")
                    .AppendLine(details.City)
                    .AppendLine(details.Province)
                    .AppendLine(details.Country)
                    .AppendLine(details.Zip)
                    .AppendLine("-------------------")
                    .AppendFormat("礼盒打包:{0}", details.GiftWrap ? "是" : "否");
                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAdress,
                    emailSettings.MailToAdress,
                    "新提交的订单",
                    body.ToString());
                if (emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
