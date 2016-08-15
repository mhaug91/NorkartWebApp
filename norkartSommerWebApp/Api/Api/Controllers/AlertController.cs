using System.Web.Http;
using Api.Models;
using Newtonsoft.Json.Linq;

namespace norkartSommerWebApp.Api.Controllers
{
    public class AlertController : ApiController
    {
        // POST: api/Alert
        public void Alert([FromBody]JObject s)
        {
            SendSmsModule.SendSms(s.ToString());
        }

    }
}
