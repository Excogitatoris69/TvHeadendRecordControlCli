using CliTvhRecControlCore.Interfaces;
using CliTvhRecControlDomain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CliTvhRecControlCore.Services
{
    public class TvhService
    {
        private ITvhAdapter _tvhAdapter = null;

        public TvhService(ITvhAdapter tvhAdapter)
        {
                _tvhAdapter = tvhAdapter;
        }

        public TvHServerInfoDto getServerInfo()
        {
            return _tvhAdapter.getServerInfo();
        }

        public string getStreamUrlOfChannel(string channelNameOrUuid)
        {
            TvHChannelDto foundChannel = _tvhAdapter.getChannelByNameOrUuid(channelNameOrUuid);
            if (foundChannel != null)
            {
                return _tvhAdapter.getStreamUrlOfChannel(foundChannel.uuid);
            }
            else
            {
                return null;
            }
        }

        public List<TvHChannelDto> getChannellist()
        {
            List<TvHChannelDto> list = _tvhAdapter.getChannellist();
            if (list!=null)
                list.Sort((s1, s2) => s1.name.CompareTo(s2.name));
            return list;
        }

        public List<TvHDvrUpcomingDto> getDvrUpcominglist()
        {
            return _tvhAdapter.getDvrUpcominglist();
        }

        public List<TvHDvrProfileDto> getDvrProfileList()
        {
            List<TvHDvrProfileDto> list = _tvhAdapter.getDvrProfileList();
            if (list != null)
                list.Sort((s1, s2) => s1.name.CompareTo(s2.name));
            return list;
        }

        public TvHResponseDto addEpgToDvr(TvHDvrAddEntryDto tvHDvrAddEntryDto)
        {
            TvHResponseDto result = new TvHResponseDto();
            result.successful = true;
            //checkdata
            TvHChannelDto foundChannel = _tvhAdapter.getChannelByNameOrUuid(tvHDvrAddEntryDto.channelName);
            if (foundChannel != null)
            {
                //set name
                tvHDvrAddEntryDto.channelName = foundChannel.name;
            }
            else
            {
                result.successful = false;
                result.errorMessage = "Channel not found: " + tvHDvrAddEntryDto.channelName;
            }
            //dvrconfig
            if (!string.IsNullOrEmpty(tvHDvrAddEntryDto.dvrProfileName))//mit namen
            {
                TvHDvrProfileDto foundProfile = _tvhAdapter.getDvrConfigByNameOrUuid(tvHDvrAddEntryDto.dvrProfileName);
                if (foundProfile != null)
                {
                    tvHDvrAddEntryDto.dvrProfileUuid = foundProfile.uuid;
                }
                else
                {
                    result.successful = false;
                    result.errorMessage = string.Format("DVR-Profilename not found: '{0}'", tvHDvrAddEntryDto.dvrProfileName);
                }
            } else // mit uuid
            {
                TvHDvrProfileDto foundProfile = _tvhAdapter.getDvrConfigByNameOrUuid(tvHDvrAddEntryDto.dvrProfileUuid);
                if (foundProfile==null)
                {
                    result.successful = false;
                    result.errorMessage = "DVR-ProfileUuid not found: " + tvHDvrAddEntryDto.dvrProfileUuid;
                }
            }
            //starttime  endtime
            // start >= now && end>start
            long nowUnix = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            //if (tvHDvrAddEntryDto.startTime < nowUnix || tvHDvrAddEntryDto.stopTime <= tvHDvrAddEntryDto.startTime)

            // not necessary:
            //if (tvHDvrAddEntryDto.startTime < (nowUnix-30) || tvHDvrAddEntryDto.stopTime <= tvHDvrAddEntryDto.startTime)
            //{
            //    result.successful = false;
            //    string starttimeString = DateTimeOffset.FromUnixTimeSeconds(tvHDvrAddEntryDto.startTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            //    string stoptimeString = DateTimeOffset.FromUnixTimeSeconds(tvHDvrAddEntryDto.stopTime).DateTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
            //    result.errorMessage = string.Format("StartTime or Stoptime outside the valid time. {0} - {1}", starttimeString, stoptimeString);
            //}
            if (result.successful == true)
            {
                result = _tvhAdapter.addEpgToDvr(tvHDvrAddEntryDto);
            }
            return result; 

        }

        public TvHResponseDto removeUpcomingDvrEntry(string searchUuid)
        {
            TvHResponseDto result = new TvHResponseDto();
            result.successful = true;

            List<TvHDvrUpcomingDto> list = _tvhAdapter.getDvrUpcominglist();
            if (list == null || list.Count == 0)
            {
                result.successful = false;
                result.errorMessage = "Upcoming list is empty.";
            }

            bool found = false;
            TvHDvrUpcomingDto foundTvHDvrUpcomingDto = null;
            foreach (TvHDvrUpcomingDto item in list)
            {
                if (item.uuid == searchUuid)
                {
                    found = true;
                    foundTvHDvrUpcomingDto = item;
                    break;
                }
            }
            if (found == true)
            {
                if (foundTvHDvrUpcomingDto.scheduledStatus.Equals(EDvrUpcomingScheduledStatus.scheduled.ToString()))
                {
                    TvHResponseDto r1 = _tvhAdapter.removeFromDvr(foundTvHDvrUpcomingDto);
                    result.successful = r1.successful;
                }
                else if (foundTvHDvrUpcomingDto.scheduledStatus.Equals(EDvrUpcomingScheduledStatus.recording.ToString()))
                {
                    TvHResponseDto r1 = _tvhAdapter.stopRunningDvr(foundTvHDvrUpcomingDto);
                    result.successful = r1.successful;
                    //result.successful = false;
                    //result.errorMessage = "DvrEntry is currently being recorded.";
                }
                else
                {
                    result.successful = false;
                    result.errorMessage = "Removing DvrEntry is not possible. Status: " + foundTvHDvrUpcomingDto.scheduledStatus;
                }
            }
            else
            {
                result.successful = false;
                result.errorMessage = "DvrEntry not found.";
            }
            //remove
            return result;
        }

        

    }
}
