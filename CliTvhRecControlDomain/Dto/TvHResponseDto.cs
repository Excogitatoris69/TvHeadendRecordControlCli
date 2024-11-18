using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliTvhRecControlDomain.Dto
{
    public class TvHResponseDto
    {
        public bool successful { get; set; }
        public string uuid { get; set; }
        public string errorMessage { get; set; }
    }
}
