using System;
using System.Collections.Generic;
using System.Data.OracleClient;
using Google.Apis.Calendar.v3.Data;


namespace google_calendar_api.Models
{
  public class GoogleCalendarListDto
  {
    public string Id { get; set; }
    public string CalendarId { get; set; }
    public string refreshToken { get; set; }
    public DateTime? Created { get; set; }
    public string Summary { get; set; }
    public string Description { get; set; }
    public EventDateTime Start { get; set; }
    public EventDateTime End { get; set; }
    public IEnumerable<EventAttendee> Attendees { get; set; }
    public IEnumerable<EventAttachment> Attachments { get; set; }
    public bool? AnyoneCanAddSelf { get; set; }
    public bool? AttendeesOmitted { get; set; }
    public string? ColorId { get; set; }
    public ConferenceData ConferenceData { get; set; }
    public System.DateTimeOffset? CreatedDateTimeOffset { get; set; }
    public string CreatedRaw { get; set; }
    public Event.CreatorData Creator { get; set; }
    public string? ETag { get; set; }
    public bool? EndTimeUnspecified { get; set; }
    public string EventType { get; set; }
    public Event.ExtendedPropertiesData ExtendedProperties { get; set; }
    public EventFocusTimeProperties FocusTimeProperties { get; set; }
    public Event.GadgetData Gadget { get; set; }
    public bool? GuestsCanInviteOthers { get; set; }
    public bool? GuestsCanModify { get; set; }
    public bool? GuestsCanSeeOtherGuests { get; set; }
    public string? HangoutLink { get; set; }
    public string HtmlLink { get; set; }
    public string? ICalUID { get; set; }
    public string Kind { get; set; }
    public string? Location { get; set; }
    public bool? Locked { get; set; }
    public Event.OrganizerData Organizer { get; set; }
    public EventDateTime OriginalStartTime { get; set; }
    public EventOutOfOfficeProperties OutOfOfficeProperties { get; set; }
    public bool? PrivateCopy { get; set; }
    public IEnumerable<string> Recurrence { get; set; }
    public string? RecurrencingEventId { get; set; }
    public Event.RemindersData Reminders { get; set; }
    public int? Sequence { get; set; }
    public Event.SourceData Source { get; set; }
    public string Status { get; set; }
    public string? Transparency { get; set; }
    public DateTime? Updated { get; set; }
    public DateTimeOffset? UpdatedTimeOffset { get; set; }
    public string? UpdatedRaw { get; set; }
    public string? Visibility { get; set; }
    public EventWorkingLocationProperties WorkingLocationProperties { get; set; }
  }
}
