namespace CliTvhRecControlDomain.Dto
{
    public class TvHDvrAddEntryDto
    {
        private bool _enabled = true;
        private int _priority = 0;

        public TvHDvrAddEntryDto()
        {
            _priority = 2;
            _enabled = true;
        }

        public bool enable
        {
            get { return _enabled; }
        }
        public int priority
        {
            get { return _priority; }
        }
        public long startTime { get; set; }
        public long stopTime { get; set; }
        public string channelName { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string dispExtratext { get; set; }
        public string dispDescription { get; set; }
        public string dispSubtitle { get; set; }
        public string comment { get; set; }
        public string dvrProfileUuid { get; set; }
        public string dvrProfileName { get; set; }
        public string languageShort { get; set; }

        public void setStartTime(DateTime time)
        {
            startTime = ((DateTimeOffset)time).ToUnixTimeSeconds();
        }

        public void setStopTime(DateTime time)
        {
            stopTime = ((DateTimeOffset)time).ToUnixTimeSeconds();
        }

        public string startTimeHuman
        {
            get { 
                return DateTimeOffset.FromUnixTimeSeconds(startTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"); 
            }
        }
        public string stopTimeHuman
        {
            get
            {
                return DateTimeOffset.FromUnixTimeSeconds(stopTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            }
        }
    }

}
