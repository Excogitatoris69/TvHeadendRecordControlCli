using CliTvhRecControlCore.Interfaces;
using CliTvhRecControlDomain.Dto;
using System.Text.Json;
using System.Web;

namespace TvhAdapterSimulate
{
    /// <summary>
    /// TvhAdapter implementation simulate Tvh for offline test.
    /// </summary>
    public class TvHAdapterSimulateImpl : ITvhAdapter
    {
        private TvHConnectionDataDto? _connectionData;
        private string APIPATH_LIVESTREAM = "/stream/channel/";
        //----- Datenstrukturen fuer offlineMode ---------------
        private List<TvHChannelDto> _channelList;
        private List<TvHChanneltagDto> _channeltagList;
        private List<TvHDvrProfileDto> _dvrProfileList;
        private List<TvHDvrUpcomingDto> _upcomingDvrList;
        private TvHServerInfoDto? _serverInfo;

        public TvHAdapterSimulateImpl()
        {
            generateDataForOfflineMode();
        }

        public TvHConnectionDataDto? connectionData
        {
            get
            {
                return _connectionData;
            }
            set
            {
                _connectionData = value;
            }
        }

        /// <summary>
        /// Fügt eine neue Aufnahme hinzu.
        /// </summary>
        /// <param name="tvHDvrAddEntryDto"></param>
        /// <returns></returns>
        public TvHResponseDto addEpgToDvr(TvHDvrAddEntryDto tvHDvrAddEntryDto)
        {
            TvHResponseDto tvHResponseDto = new TvHResponseDto();
            tvHResponseDto.successful = false;
                TvHDvrUpcomingDto aTvHDvrUpcomingDto = new TvHDvrUpcomingDto();
                aTvHDvrUpcomingDto.dvrProfileUuid = tvHDvrAddEntryDto.dvrProfileUuid;
                aTvHDvrUpcomingDto.channelName = tvHDvrAddEntryDto.channelName;
                aTvHDvrUpcomingDto.channelUuid = tvHDvrAddEntryDto.channelName;
                aTvHDvrUpcomingDto.title = tvHDvrAddEntryDto.title;
                aTvHDvrUpcomingDto.status = "Scheduled for recording";
                aTvHDvrUpcomingDto.startTime = tvHDvrAddEntryDto.startTime;
                aTvHDvrUpcomingDto.stoptime = tvHDvrAddEntryDto.stopTime;
                aTvHDvrUpcomingDto.scheduledStatus = "scheduled";
                aTvHDvrUpcomingDto.owner = "tvuser";
                aTvHDvrUpcomingDto.uuid = getGeneratedUuid();
                _upcomingDvrList.Add(aTvHDvrUpcomingDto);
                return new TvHResponseDto { successful = true, uuid = aTvHDvrUpcomingDto.uuid, };
        }

        public List<TvHChannelDto> getChannellist()
        {
            List<TvHChannelDto> channelList = new List<TvHChannelDto>(100);
            return _channelList;
        }

        public List<TvHChanneltagDto> getChanneltaglist()
        {
            return _channeltagList;
        }

        public List<TvHDvrProfileDto> getDvrProfileList()
        {
            return _dvrProfileList;
        }

        public List<TvHDvrUpcomingDto> getDvrUpcominglist()
        {
            return _upcomingDvrList;
        }

        /// <summary>
        /// Fordert die Serverinfos wie API- und Programmversion an.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TvHServerInfoDto getServerInfo()
        {
            return _serverInfo;
        }

        /// <summary>
        /// Liefert Stream-Url eines Channel
        /// </summary>
        /// <param name="channelUuid"></param>
        /// <returns></returns>
        public string getStreamUrlOfChannel(string channelUuid)
        {
            string streamUrl = null;
            // http://ptvheadend:9981 --> http://user:pw@ptvheadend:9981/stream/channel/uuid
            string[] urlTokens = _connectionData.serverUrl.Split(":");

            streamUrl = string.Format("{0}://{3}:{4}@{1}:{2}{5}{6}"
                , urlTokens[0]  //http
                , urlTokens[1].Substring(2)  //server
                , urlTokens[2]  //port
                , _connectionData.credentials.username
                , _connectionData.credentials.password
                , APIPATH_LIVESTREAM
                , channelUuid
                );
            return streamUrl;
        }


        public TvHResponseDto removeFromDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto)
        {
            
            TvHDvrUpcomingDto foundTvHDvrAddEntryDto = null;
            foreach (TvHDvrUpcomingDto item in _upcomingDvrList)
            {
                if (item.uuid.Equals(tvHDvrUpcomingDto.uuid))
                {
                    foundTvHDvrAddEntryDto = item;
                    break;
                }
            }
            if (foundTvHDvrAddEntryDto != null)
                _upcomingDvrList.Remove(foundTvHDvrAddEntryDto);
            return new TvHResponseDto { successful = true };
        }

        public TvHResponseDto stopRunningDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto)
        {

            TvHDvrUpcomingDto foundTvHDvrAddEntryDto = null;
            foreach (TvHDvrUpcomingDto item in _upcomingDvrList)
            {
                if (item.uuid.Equals(tvHDvrUpcomingDto.uuid))
                {
                    foundTvHDvrAddEntryDto = item;
                    break;
                }
            }
            if (foundTvHDvrAddEntryDto != null)
                _upcomingDvrList.Remove(foundTvHDvrAddEntryDto);
            return new TvHResponseDto { successful = true };
        }


        public TvHChannelDto getChannelByNameOrUuid(string nameOrUuid)
        {
            TvHChannelDto result = null;
            foreach (TvHChannelDto item in getChannellist())
            {
                if (item.name.Equals(nameOrUuid, StringComparison.OrdinalIgnoreCase)
                    || item.uuid.Equals(nameOrUuid))
                {
                    result = item;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// True, wenn Channelname oder Channel-UUID existiert.
        /// </summary>
        /// <param name="channelname"></param>
        /// <returns></returns>
        public bool isExistChannelname(string channelname)
        {
            if (string.IsNullOrEmpty(channelname)) return false;
            bool result = false;
            foreach (TvHChannelDto item in getChannellist())
            {
                if (item.name.Equals(channelname, StringComparison.OrdinalIgnoreCase)
                    || item.uuid.Equals(channelname))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public TvHDvrProfileDto getDvrConfigByNameOrUuid(string nameOrUuid)
        {
            if (string.IsNullOrEmpty(nameOrUuid)) return null;
            TvHDvrProfileDto result = null;
            foreach (TvHDvrProfileDto item in getDvrProfileList())
            {
                if (item.name.Equals(nameOrUuid, StringComparison.OrdinalIgnoreCase)
                    || item.uuid.Equals(nameOrUuid, StringComparison.OrdinalIgnoreCase))
                {
                    result = item;
                    break;
                }
            }
            return result;
        }


        //-----------------------------------------------------
        /// <summary>
        /// 32 zeichen
        /// </summary>
        /// <returns></returns>
        private string getGeneratedUuid()
        {
            string newGuid = Guid.NewGuid().ToString("N");
            return newGuid;
        }


        /// <summary>
        /// Zum Testen ohne laufenden TvHeadendserver werden künnstliche Daten erstellt.
        /// </summary>
        private void generateDataForOfflineMode()
        {
            _serverInfo = new TvHServerInfoDto { versionApi = 1, versionTvhServerSoftware = "0.0 OfflineMode" };
            _upcomingDvrList = new List<TvHDvrUpcomingDto>(10);
            //channeltag
            TvHChanneltagDto channeltag = new TvHChanneltagDto { name = "Tag_01", uuid = getGeneratedUuid() };
            TvHChanneltagDto channeltag2 = new TvHChanneltagDto { name = "Tag_02", uuid = getGeneratedUuid() };
            _channeltagList = new List<TvHChanneltagDto>();
            _channeltagList.Add(channeltag);
            _channeltagList.Add(channeltag2);

            //dvrprofile
            _dvrProfileList = new List<TvHDvrProfileDto>(10);
            TvHDvrProfileDto tvHDvrProfileDto = new TvHDvrProfileDto
            {
                name = "withPadding",
                uuid = getGeneratedUuid(),
                postExtraTime = 5,
                preExtraTime = 15,
            };
            TvHDvrProfileDto tvHDvrProfileDto2 = new TvHDvrProfileDto
            {
                name = "withoutPadding",
                uuid = getGeneratedUuid(),
                postExtraTime = 1,
                preExtraTime = 1,
            };
            _dvrProfileList.Add(tvHDvrProfileDto);
            _dvrProfileList.Add(tvHDvrProfileDto2);


            //channel
            _channelList = new List<TvHChannelDto>();
            TvHChannelDto tvHChannelDto = null;
            for (int i = 1; i < 20; i++)
            {
                tvHChannelDto = new TvHChannelDto();
                tvHChannelDto.name = "Channel_" + i.ToString("00");
                tvHChannelDto.number = i + 1;
                tvHChannelDto.uuid = getGeneratedUuid();
                tvHChannelDto.tagUuidListList = new List<string>();
                tvHChannelDto.tagUuidListList.Add(channeltag.uuid);
                _channelList.Add(tvHChannelDto);
            }
            TvHChannelDto tvHChannelDto2 = new TvHChannelDto
            {
                name = "ZDF HD",
                number = 20,
                uuid = getGeneratedUuid(),
                tagUuidListList = new List<string>(),
            };
            tvHChannelDto2.tagUuidListList.Add(channeltag.uuid);
            _channelList.Add(tvHChannelDto2);
        }



    }
}
