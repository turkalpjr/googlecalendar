using google_calendar_api.Models;
using google_calendar_api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace google_calendar_api.Controllers
{
    public class GoogleCalendarController : Controller
    {
        private IGoogleCalendarService _googleCalendarService;
        public GoogleCalendarController(IGoogleCalendarService googleCalendarService)

        {
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet]
        [Route("/googlecalendar/index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/auth/google")]
        public async Task<IActionResult> GoogleAuth()
        {
            return Redirect(_googleCalendarService.GetAuthCode());
        }

        [HttpGet]
        [Route("/auth/callback")]
        public async Task<IActionResult> Callback()
        {
            string code = HttpContext.Request.Query["code"];
            string scope = HttpContext.Request.Query["scope"];

            //get token method
            var token = await _googleCalendarService.GetTokens(code);
            return Ok(token);
        }

        [HttpPost]
        [Route("/googlecalendar/addcalendarevent")]
        public async Task<IActionResult> AddCalendarEvent([FromBody] GoogleCalendarReqDTO calendarEventReqDTO)
        {
            var data = await _googleCalendarService.AddToGoogleCalendar(calendarEventReqDTO);
            return Ok(data);
        }

        [HttpPost]
        [Route("/googlecalendar/updatecalendarevent")]
        public async Task<IActionResult> UpdateCalendarEvent([FromBody] GoogleCalendarListDto calendarEventReqDTO)
        {
            var data = await _googleCalendarService.UpdateGoogleCalendar(calendarEventReqDTO);
            return Ok(data);
        }

        [HttpPost]
        [Route("/googlecalendar/getlistcalendar")]
        public async Task<IActionResult> GetListCalendar([FromBody] GoogleCalendarReqDTO calendarEventReqDTO)
        {
            var data =  await _googleCalendarService.GetCalendarEvents(calendarEventReqDTO);
            return Ok(data);
        }
    }
}
