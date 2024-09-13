using System.Collections.Generic;
using System.Threading.Tasks;
using google_calendar_api.Models;

namespace google_calendar_api.Services
{
  public interface IGoogleCalendarService
    {
        string GetAuthCode();

        Task<GoogleTokenResponse> GetTokens(string code);
        Task<string> AddToGoogleCalendar(GoogleCalendarReqDTO googleCalendarReqDTO);
        Task<List<GoogleCalendarListDto>> GetCalendarEvents(GoogleCalendarReqDTO googleCalendarReqDTO);
        Task<string> UpdateGoogleCalendar(GoogleCalendarListDto googleCalendarReqDTO);
    }
}
