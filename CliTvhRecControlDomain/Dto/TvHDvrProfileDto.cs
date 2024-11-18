using System.Text.Json.Serialization;

namespace CliTvhRecControlDomain.Dto
{
    public class TvHDvrProfileDto
    {
        public string uuid { get; set; }
        public string name { get; set; }
        public int preExtraTime { get; set; }
        public int postExtraTime { get; set; }

        public override string? ToString()
        {
            return string.Format("{0} - {1}", name, uuid);
        }
    }



}
