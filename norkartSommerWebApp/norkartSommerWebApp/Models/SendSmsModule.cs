using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using KomTek.Sms2.Web.Api.Client.ClientImpl;
using KomTek.Sms2.Web.Api.Dto.SendSms;

namespace norkartSommerWebApp.Models
{
    public class SendSmsModule
    {
        /// <summary>
        /// I prosjektet er 'KomTek.Sms2.Web.Api.Dto' lagt inn som referanse via NuGet (NorkartSource http://192.168.2.67/NONuget/nuget)
        /// Legg inn Newtonsoft.Json (min v8.0.0.0) som referanse i prosjektet med "copy local = true" 
        /// </summary>
        public static void SendSms(string value)
        {
            var bergenKundeId = new Guid("96C7DC3D-D9F4-4F11-8B4C-B584FCFB19B9");

            var smsDto = new SendSmsDto
            {
                Avsender = "Kongen",
                Tekst = "Hei Vilde, god tur til Hellas!",
                DbId = bergenKundeId,
                Toveis = false,
                Mottakere = new List<SendSmsMottakerDto>

                {
                    new SendSmsMottakerDto()
                    {
                        Mobilnummer = "90784845",
                        Atributter = new List<SmsAttributtDto>
                        {
                            //Unike egenskaper for meldingen tilknyttet denne mottaker    
                            //Legg til flere dersom dere ønsker det
                            new SmsAttributtDto
                            {
                                Navn = "Enhet",
                                Verdi = "TestEnhet"
                            }
                        }
                    }
                },
                Metadata = new SmsMetadataDto()
                {
                    BatchNavn = "Sommer16Batch",
                    Applikasjon = "sommer16"
                }
            };

            var sms2Client = new SmsClient("KOMTEK_Sms", "Sms_Funksjonstest", "X-KOMTEK-TOKEN AFF4F26E-B6A2-47A4-8135-C2FA2265C970");   //Henter URL for SMS API
            var sms = sms2Client.SendSmser(smsDto, false);                                                                               //Laster over SMS'er som skal sendes

            try
            {
                sms2Client.StartSending(sms.FramWebBatchId, bergenKundeId);                                                             //Trigger sending av sms'er
                Debug.Print("SMS sendt ok via SendSmser/StartSending ");
            }
            catch (Exception ex)
            {
                var info = GetFeilmelding(ex);
                var sb = new StringBuilder();
                sb.AppendLine("=====================");
                sb.Append("FEIL VED SENDING AV SMS:");
                sb.AppendLine(info);
                sb.AppendLine("=====================");
                Debug.Print(sb.ToString());
            }
        }




        /// <summary>
        /// Henter ut feilmelding (inkl.InnerException.Message)
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static string GetFeilmelding(Exception exception)
        {
            if (exception == null) return "";
            return Environment.NewLine + ">>> " + exception.Message + GetFeilmelding(exception.InnerException);
        }
    }
}