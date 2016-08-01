using System.Web.Http;
using System.Threading.Tasks;
using norkartSommerWebApp.Models;

namespace norkartSommerWebApp.Api.Controllers
{
    public class IotController : ApiController
    {


        // POST: api/Iot
        public async Task PostCloudToDeviceMsg([FromBody]string s)
        {
            await IotHub.Main(s);
            return;
        }


    }
}
