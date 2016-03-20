using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using BuildBoard.Models;
using BuildBoard.Services;

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
        public async Task PostMessage(NewMessageModel model)
        {
            if (!ModelState.IsValid)
            {
                return;
            }

            await _boardService.AddMessage(model.LocationId, model.Text);
        }

        [HttpGet]
        [Route("list/{locationId}/{top}")]
        public IEnumerable<MessageModel> GetMessages(int locationId, int top)
        {
            var result = _boardService.GetMessages(locationId, top);
            return result.Select(m => new MessageModel
            {
                Date = m.Date,
                Message = m.Text
            });
        }

        [Route("test")]
        [HttpGet]
        public string Test()
        {
            return "Everything is working fine :)";
        }
    }
}
