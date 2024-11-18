namespace CliTvhRecControlDomain.Dto
{
    public class TvHDvrEntryDto
    {
        public bool enabled { get; set; }
        public string channelName { get; set; }
        public long startTime { get; set; }
        public long stopTime { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string comment { get; set; }
        public string dvrProfileUuid { get; set; }
        //public int priority { get; set; }
    }



}
