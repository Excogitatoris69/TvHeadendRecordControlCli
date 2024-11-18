using System.Text.Json.Serialization;

namespace CliTvhRecControlDomain.Dto
{
    public class TvHDvrUpcomingDto
    {
        public string uuid { get; set; }
        public string channelName { get; set; }
        public string title { get; set; }
        public long startTime { get; set; }
        public long stoptime { get; set; }
        public long startTimeReal { get; set; }
        public long stopTimeReal { get; set; }
        public string owner { get; set; }

        //public int priority { get; set; }

        public string? dvrProfileUuid { get; set; }
        public string? channelUuid { get; set; }

        /// <summary>
        /// scheduled
        /// recording
        /// </summary>
        public string? scheduledStatus { get; set; }

        /// <summary>
        /// Scheduled for recording
        /// Running
        /// </summary>
        public string? status { get; set; }

        public string getStartTimeString()
        {
            return DateTimeOffset.FromUnixTimeSeconds(startTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        }
        public string getStopTimeString()
        {
            return DateTimeOffset.FromUnixTimeSeconds(stoptime).ToLocalTime().DateTime.ToString("HH:mm");
        }
        public override string? ToString()
        {
            return string.Format("[{0}]  {1} - {2} '{3}' {4}",
                channelName,
                getStartTimeString(), getStopTimeString(),
                title, uuid
                );
        }
    }

    public enum EDvrUpcomingScheduledStatus
    {
        scheduled,
        recording,
    }



}
