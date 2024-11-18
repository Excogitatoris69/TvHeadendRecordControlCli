using System.Text.Json.Serialization;

namespace CliTvhRecControlDomain.Dto
{
    public class TvHEpgEntryDto
    {
        public string? channelName { get; set; }
        public string? channelUuid { get; set; }
        public string? title { get; set; }
        public string? subtitle { get; set; }
        public string? description { get; set; }
        public long startTime { get; set; }
        public long stopTime { get; set; }
        public string? dvrUuid { get; set; }
        public string? dvrState { get; set; }
        public string? summary { get; set; }
        public int eventId { get; set; }
        public int nextEventId { get; set; }


        public string getStartTimeString()
        {
            return DateTimeOffset.FromUnixTimeSeconds(startTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        }
        public string getStopTimeString()
        {
            return DateTimeOffset.FromUnixTimeSeconds(stopTime).DateTime.ToLocalTime().ToString("HH:mm");
        }

        public int getDuration()
        {
            return (int)(stopTime - startTime)/60;
        }

        public override string? ToString()
        {
            return string.Format("[{0}]  {1} - {2} ({3}) '{4}'",
                channelName,
                getStartTimeString(), getStopTimeString(),
                getDuration(),
                title 
                //subtitle
                );
        }

        
    }

}
