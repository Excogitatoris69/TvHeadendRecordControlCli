namespace CliTvhRecControlDomain.Dto
{
    public class TvHChannelDto
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public int number { get; set; }
        public bool enabled { get; set; }
        public string iconPublicUrl { get; set; }
        public string icon { get; set; }
        public List<string> tagUuidListList { get; set; }
        public override string? ToString()
        {
            return string.Format("{0} ({1})", name, uuid);
        }
    }

}
