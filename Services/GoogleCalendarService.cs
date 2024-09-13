using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using google_calendar_api.Models;

namespace google_calendar_api.Services
{
  public class GoogleCalendarService : IGoogleCalendarService
  {

    private readonly HttpClient _httpClient;
    public GoogleCalendarService()
    {
      _httpClient = new HttpClient();
    }

    public string GetAuthCode()
    {
      try
      {
        string scopeURL1 = "https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&prompt={1}&response_type={2}&client_id={3}&scope={4}&access_type={5}";
        var redirectURL = "https://localhost:44318/auth/callback";
        string prompt = "consent";
        string response_type = "code";
        string clientID = "371265639394-sc66fjppb4v9795heuhjl23d904ipuh2.apps.googleusercontent.com";
        string scope = "https://www.googleapis.com/auth/calendar";
        string access_type = "offline";
        string redirect_uri_encode = Method.urlEncodeForGoogle(redirectURL);
        var mainURL = string.Format(scopeURL1, redirect_uri_encode, prompt, response_type, clientID, scope, access_type);

        return mainURL;
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }

    public async Task<GoogleTokenResponse> GetTokens(string code)
    {

      var clientId = "371265639394-sc66fjppb4v9795heuhjl23d904ipuh2.apps.googleusercontent.com";
      string clientSecret = "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";
      var redirectURL = "https://localhost:44318/auth/callback";
      var tokenEndpoint = "https://accounts.google.com/o/oauth2/token";
      var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectURL)}&client_id={clientId}&client_secret={clientSecret}&grant_type=authorization_code", Encoding.UTF8, "application/x-www-form-urlencoded");

      var response = await _httpClient.PostAsync(tokenEndpoint, content);
      var responseContent = await response.Content.ReadAsStringAsync();
      if (response.IsSuccessStatusCode)
      {
        var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);
        return tokenResponse;
      }
      else
      {
        // Handle the error case when authentication fails
        throw new Exception($"Failed to authenticate: {responseContent}");
      }
    }

    public async Task<string> AddToGoogleCalendar(GoogleCalendarReqDTO req)
    {
      try
      {
        var token = new TokenResponse
        {
          RefreshToken = req.refreshToken
        };
        var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
          new GoogleAuthorizationCodeFlow.Initializer
          {
            ClientSecrets = new ClientSecrets
            {
              ClientId = "371265639394-sc66fjppb4v9795heuhjl23d904ipuh2.apps.googleusercontent.com",
              ClientSecret = "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"
            }

          }), "user", token);

        var service = new CalendarService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credentials,
        });

        Event newEvent = new Event()
        {
          Summary = req.Summary,
          Description = req.Description,
          Start = new EventDateTime()
          {
            DateTime = req.StartTime,
            //TimeZone = Method.WindowsToIana();    //user's time zone
          },
          End = new EventDateTime()
          {
            DateTime = req.EndTime,
            //TimeZone = Method.WindowsToIana();    //user's time zone
          },
          Reminders = new Event.RemindersData()
          {
            UseDefault = false,
            Overrides = new EventReminder[] {

                new EventReminder() {
                    Method = "email", Minutes = 30
                  },

                  new EventReminder() {
                    Method = "popup", Minutes = 15
                  },

                  new EventReminder() {
                    Method = "popup", Minutes = 1
                  },
              }
          }

        };

        EventsResource.InsertRequest insertRequest = service.Events.Insert(newEvent, req.CalendarId);
        Event createdEvent = insertRequest.Execute();
        return createdEvent.Id;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return string.Empty;
      }
    }


    public async Task<string> UpdateGoogleCalendar(GoogleCalendarListDto req)
    {
      try
      {
        var token = new TokenResponse
        {
          RefreshToken = req.refreshToken
        };
        var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
          new GoogleAuthorizationCodeFlow.Initializer
          {
            ClientSecrets = new ClientSecrets
            {
              ClientId = "371265639394-sc66fjppb4v9795heuhjl23d904ipuh2.apps.googleusercontent.com",
              ClientSecret = "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"
            }

          }), "user", token);

        var service = new CalendarService(new BaseClientService.Initializer()
        {
          HttpClientInitializer = credentials,
        });

        Event updateEvent = new Event()
        {
          Id = req.Id,
          Summary = req.Summary,
          Description = req.Description,
          Start = new EventDateTime()
          {
            Date = req.Start
          },
          End = new EventDateTime()
          {
            Date = req.End
          }
        };

        EventsResource.UpdateRequest updateRequest = service.Events.Update(updateEvent, req.CalendarId, req.Id);
        Event updateRequestResult = updateRequest.Execute();
        return updateRequestResult.Id;
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return string.Empty;
      }
    }

    [Obsolete]
    public async Task<List<GoogleCalendarListDto>> GetCalendarEvents(GoogleCalendarReqDTO googleCalendarReqDTO)
    {
      // Define parameters of request.
      var token = new TokenResponse
      {
        RefreshToken = googleCalendarReqDTO.refreshToken
      };
      var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
        new GoogleAuthorizationCodeFlow.Initializer
        {
          ClientSecrets = new ClientSecrets
          {
            ClientId = "371265639394-sc66fjppb4v9795heuhjl23d904ipuh2.apps.googleusercontent.com",
            ClientSecret = "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"
          }

        }), "user", token);

      var service = new CalendarService(new BaseClientService.Initializer()
      {
        HttpClientInitializer = credentials,
      });

      EventsResource.ListRequest request = service.Events.List("primary");
      request.TimeMin = DateTime.Now.AddMonths(-2);
      request.ShowDeleted = false;
      request.SingleEvents = true;
      request.MaxResults = 10;
      request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

      string eventsValue = "";
      // List events.
      Events events = request.Execute();

      List<GoogleCalendarListDto> returnList = new List<GoogleCalendarListDto>(); //JsonConvert.DeserializeObject<List<GoogleCalendarListResponseDto>>(events.Items.ToString());

      foreach (Event item in events.Items)
      {
        GoogleCalendarListDto Item1 = new GoogleCalendarListDto()
        {
          AnyoneCanAddSelf = item.AnyoneCanAddSelf,
          Attachments = item.Attachments,
          Attendees = item.Attendees,
          AttendeesOmitted = item.AttendeesOmitted,
          ColorId = item.ColorId,
          ConferenceData = item.ConferenceData,
          Created = item.Created,
          CreatedDateTimeOffset = item.CreatedDateTimeOffset,
          CreatedRaw = item.CreatedRaw,
          Creator = item.Creator,
          Description = item.Description,
          End = string.Format("{0:dd.MM.yyyy}", item.End.Date),
          Start = string.Format("{0:dd.MM.yyyy}", item.Start.Date),
          EndTimeUnspecified = item.EndTimeUnspecified,
          ETag = item.ETag,
          EventType = item.EventType,
          ExtendedProperties = item.ExtendedProperties,
          FocusTimeProperties = item.FocusTimeProperties,
          Gadget = item.Gadget,
          GuestsCanInviteOthers = item.GuestsCanInviteOthers,
          GuestsCanModify = item.GuestsCanModify,
          GuestsCanSeeOtherGuests = item.GuestsCanSeeOtherGuests,
          HangoutLink = item.HangoutLink,
          HtmlLink = item.HtmlLink,
          ICalUID = item.ICalUID,
          Id = item.Id,
          Kind = item.Kind,
          Location = item.Location,
          Locked = item.Locked,
          Organizer = item.Organizer,
          OriginalStartTime = item.OriginalStartTime,
          OutOfOfficeProperties = item.OutOfOfficeProperties,
          PrivateCopy = item.PrivateCopy,
          Recurrence = item.Recurrence,
          RecurrencingEventId = item.RecurringEventId,
          Reminders = item.Reminders,
          Sequence = item.Sequence,
          Source = item.Source,

          Status = item.Status,
          Summary = item.Summary,
          Transparency = item.Transparency,
          Updated = item.Updated,
          UpdatedRaw = item.UpdatedRaw,
          UpdatedTimeOffset = item.UpdatedDateTimeOffset,
          Visibility = item.Visibility,
          WorkingLocationProperties = item.WorkingLocationProperties
        };
        returnList.Add(Item1);
      }
      return returnList;
    }
  }
}
