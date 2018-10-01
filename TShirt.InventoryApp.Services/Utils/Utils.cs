using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace TShirt.InventoryApp.Services.Utils
{
  public class Utils
  {
    public static void SendEmail(string Title, string Body, string CorreoTO, bool IsHTML, byte[] Attachment = null, string FileName = "")
    {
      try
      {
        MailMessage msg = new MailMessage();
        msg.Subject = Title;

        Attachment logo = new Attachment("D:\\greenRetail\\inventariomovil\\TShirt.Inventory.Web\\Content\\Images\\Logo.png");
        logo.ContentId = "logo";

        msg.Attachments.Add(logo);

        msg.Body = Body;

        if (CorreoTO.IndexOf(';') > 0)
        {
          foreach (var item in CorreoTO.Split(';'))
          {
            msg.To.Add(item);
          }
        }
        else
          msg.To.Add(CorreoTO);
        msg.IsBodyHtml = IsHTML;
        if (Attachment != null)
          msg.Attachments.Add(new Attachment(new MemoryStream(Attachment), FileName, null));

        SmtpClient client = new SmtpClient();
        client.SendCompleted += new SendCompletedEventHandler(cliente_SendCompleted);
        client.Send(msg);

      }
      catch (Exception ex)
      {
        //log4net.LogManager.GetLogger("root").Error("Error enviando correo " + String.Format("Titulo: {0}, Para: {1}, Cuerpo: {2}", Titulo, CorreoTO, Cuerpo), ex);
      }
    }


    static void cliente_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
      /*  
      if (e.Error != null)
        log4net.LogManager.GetLogger("root").Error("Error enviado correo " + e.Error.Message + " " + e.UserState, e.Error);
      else
        log4net.LogManager.GetLogger("root").Info("Correo enviado exitosamente: " + e.UserState);
      */
    }
    

    public static string CreateSampleHtml(string order) {

      StringBuilder str = new StringBuilder();

      str.Append("<html>");
      str.Append("<head><title>Notificación solicitud de muestra</title></head>");
      str.Append("<body>");
      str.Append("<table><tr>");
      str.Append("<td><img src=cid:logo width='50px' height='50px' /></td>");
      str.Append("<td><strong>Notificación solicitud de muestra</strong></td>");
      str.Append("</tr><tr>");
      str.Append("<td width='50px'></td>");
      str.Append(string.Format("<td><span>Se ha realizado la solicitud <strong>SOL{0}</strong> con éxito</span></td>",order));
      str.Append("</tr>");
      str.Append("</table></body></html>");
      return str.ToString();
    }


  }
}
