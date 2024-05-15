using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using System;
using System.IO;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        string[] scopes = { CalendarService.Scope.Calendar };
        string credPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "credentials.json");

        var clientSecrets = await GoogleClientSecrets.FromFileAsync(credPath);
        var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
            clientSecrets.Secrets,
            scopes,
            "user",
            CancellationToken.None);

        var service = new CalendarService(new BaseClientService.Initializer()
        {
            HttpClientInitializer = credential
        });

        //Có thể lược bỏ từ dòng này cho đến dòng 38 nếu không muốn tạo 1 calendar mới hoặc sử dụng calendar đã có sẵn
        // Tạo một lịch mới
        //var newCalendar = new Google.Apis.Calendar.v3.Data.Calendar();
        //newCalendar.Summary = "Test Calendar";
        //var createdCalendar = await service.Calendars.Insert(newCalendar).ExecuteAsync();

        // Thêm lịch mới vào danh sách lịch
        //var calendarListEntry = new Google.Apis.Calendar.v3.Data.CalendarListEntry();
        //calendarListEntry.Id = createdCalendar.Id;
        //await service.CalendarList.Insert(calendarListEntry).ExecuteAsync();

        Event calendarEvent = new Event()
        {
            Summary = "Họp mặt trực tuyến 10h37",
            Location = "Google Meet",
            Description = "Cuộc họp trực tuyến qua Google Meet",
            Start = new EventDateTime()
            {
                DateTime = DateTime.Now,
                TimeZone = "Asia/Ho_Chi_Minh"
            },
            End = new EventDateTime()
            {
                DateTime = DateTime.Now.AddHours(1),
                TimeZone = "Asia/Ho_Chi_Minh"
            },
            ConferenceData = new ConferenceData()
            {
                CreateRequest = new CreateConferenceRequest()
                {
                    RequestId = "1234abcdef",
                    ConferenceSolutionKey = new ConferenceSolutionKey()
                    {
                        Type = "hangoutsMeet"
                    }
                }
            },
            GuestsCanInviteOthers = false,
            GuestsCanModify = false,
            GuestsCanSeeOtherGuests = false,
            
            Attendees = new List<EventAttendee>()
            {
                new EventAttendee() { Email = "7tavapcohoipen@gmail.com" },
                new EventAttendee() { Email = "riconduongcung@gmail.com" }
            }
        };
        //event với lịch được tạo ở dòng 30-37
        //EventsResource.InsertRequest request = service.Events.Insert(calendarEvent, createdCalendar.Id);
        //event với lịch có ID sẵn
        EventsResource.InsertRequest request = service.Events.Insert(calendarEvent, "your-calendar-id");
        request.ConferenceDataVersion = 1;
        Event createdEvent = await request.ExecuteAsync();

        Console.WriteLine("Event created: {0}", createdEvent.HtmlLink);
        Console.WriteLine("Meet link created: {0}", createdEvent.HangoutLink);
    }
}
