using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BuildBoard.Hubs;
using BuildBoard.Models;
using BuildBoard.Services;
using Microsoft.AspNet.SignalR;

namespace BuildBoard.Controllers
{
    [RoutePrefix("api/board")]
    public class BoardController : ApiController
    {
        private readonly IBoardService _boardService;

        public BoardController(IBoardService boardService1)
        {
            _boardService = boardService1;
        }

        [Route("locations")]
        [HttpGet]
        public IEnumerable<Location> GetLocations()
        {
            return _boardService.GetLocations();
        }

        [Route("post")]
        [HttpPost]
        public async Task PostMessage(MessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            await _boardService.AddMessage(model.LocationId, model.Text);

            IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<MessageBroadcastHub>();
            hubContext.Clients.Groups(new List<string> { model.LocationId.ToString() }).broadcastMessage(model.Text);
        }

        [Route("test")]
        [HttpGet]
        public string Test()
        {
            return "Everything is working fine :)";
        }
    }
}
