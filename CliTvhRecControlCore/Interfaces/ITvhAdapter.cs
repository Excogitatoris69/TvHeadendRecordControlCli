using CliTvhRecControlDomain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliTvhRecControlCore.Interfaces
{
    public interface ITvhAdapter
    {
        public TvHConnectionDataDto? connectionData { get; set; }
        public TvHServerInfoDto getServerInfo();
        public List<TvHChannelDto> getChannellist();
        public List<TvHChanneltagDto> getChanneltaglist();
        public List<TvHDvrProfileDto> getDvrProfileList();
        public List<TvHDvrUpcomingDto> getDvrUpcominglist();
        public TvHResponseDto addEpgToDvr(TvHDvrAddEntryDto tvHDvrAddEntryDto);
        public TvHResponseDto removeFromDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto);
        public TvHResponseDto stopRunningDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto);
        public bool isExistChannelname(string channelname);
        public TvHChannelDto getChannelByNameOrUuid(string nameOrUuid);
        public TvHDvrProfileDto getDvrConfigByNameOrUuid(string nameOrUuid);
        public string getStreamUrlOfChannel(string channelUuid);

        public void getDvrEntryClass();

        //future todos:
        //public TvHResponseDto cancelRunningDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto);
        //public List<TvHDvrUpcomingDto> getFinishedRecordingslist();
    }
}
