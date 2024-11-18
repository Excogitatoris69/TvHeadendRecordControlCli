namespace CliTvhRecControlDomain.Dto
{
    public class TvHConnectionIdDto
    {
        public TvHConnectionIdDto()
        {
            connectionId = Guid.NewGuid().ToString();
        }
        public string connectionId { get; }
    }
}
