using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliTvhRecControlDomain.Dto
{
    public class EpgFilterDto
    {
        public EpgFilterDto()
        {
            channelName = null;
            nowMode = false;
            limit = 0;
            start = 0;
            durationMin = -1;
            durationMax = -1;
        }

        public string? channelName { get; set; }

        public bool nowMode { get; set; }

        public int durationMin { get; set; }

        public int durationMax { get; set; }

        public int start { get; set; }

        public int limit { get; set; }


    }
}
