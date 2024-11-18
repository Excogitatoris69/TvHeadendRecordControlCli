namespace CliTvhRecControlDomain.Dto
{
    public class TvHConnectionDataDto
    {
        public string serverUrl { get; set; }
        //public int port { get; set; }
        public TvHCredentialsDto credentials { get; set; }
    }
}
