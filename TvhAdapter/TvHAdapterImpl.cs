using CliTvhRecControlCore.Interfaces;
using CliTvhRecControlDomain.Dto;
using System.Net.Http.Headers;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TvhAdapter.JsonEntities;
using TvhAdapter.Mapper;
using TvHeadendAdapter.Mapper;
using System.Web;
using System.Net.Sockets;

namespace TvhAdapter
{
    public class TvHAdapterImpl : ITvhAdapter
    {

        //-------------------------------------------
        private HttpClient tvHeadendHttpclient = null;
        private ServerInfoMapper serverInfoMapper = new ServerInfoMapper();
        private ChannelMapper channelMapper = new ChannelMapper();
        private ChanneltagMapper channeltagMapper = new ChanneltagMapper();
        private EpgEntryMapper epgEntryMapper = new EpgEntryMapper();
        private DvrProfileMapper dvrProfileMapper = new DvrProfileMapper();
        private DvrAddEntitylMapper dvrAddEntitylMapper = new DvrAddEntitylMapper();
        private DvrUpcomingMapper dvrUpcomingMapper = new DvrUpcomingMapper();
        //---------------------------------------------
        private string APIPATH_SERVERINFO = "/api/serverinfo";
        private string APIPATH_CHANNELLIST = "/api/channel/list";
        private string APIPATH_CHANNELGRIDLIST = "/api/channel/grid";
        private string APIPATH_CHANNELTAGGRIDLIST = "/api/channeltag/grid";
        private string APIPATH_LANGUAGELIST = "/api/language/list";
        private string APIPATH_GRIDUPCOMING = "/api/dvr/entry/grid_upcoming";
        private string APIPATH_DVRCANCEL = "/api/dvr/entry/cancel";
        private string APIPATH_DVRSTOP = "/api/dvr/entry/stop";
        private string APIPATH_DVRCREATE = "/api/dvr/entry/create";
        private string APIPATH_DVRCONFIGGRID = "/api/dvr/config/grid";
        private string APIPATH_EPGEVENTSGRID = "/api/epg/events/grid";
        private string APIPATH_LIVESTREAM = "/stream/channel/";
        //---------------------------------------------
        private JsonSerializerOptions jsonSerializerOptions = null;
        private TvHConnectionDataDto? _connectionData;
        

        public TvHConnectionDataDto? connectionData
        {
            get
            {
                return _connectionData;
            }
            set
            {
                _connectionData = value;
                tvHeadendHttpclient = null;//sorgt für eine neue HttpClient-Instance mit den aktuellen credentials
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
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            
            //prepair apipath
            DvrAddEntity dvrAddEntity = dvrAddEntitylMapper.mapToEntity(tvHDvrAddEntryDto);
            if (jsonSerializerOptions == null)
            {
                jsonSerializerOptions = new JsonSerializerOptions();
                jsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                jsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }
            string jsonDvrRequestString = JsonSerializer.Serialize(dvrAddEntity, jsonSerializerOptions);
            string encodedUrl = HttpUtility.UrlEncode(jsonDvrRequestString);
            
            //send
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_DVRCREATE + "?conf=" + encodedUrl,
            };
            Task<ApiResponseDto<TvHResponseData>> taskData = sendMessage<TvHResponseData>(requestData);
            ApiResponseDto<TvHResponseData> apiResponseDto = taskData.Result;
            if (apiResponseDto.success)
            {
                tvHResponseDto.successful = true;
                tvHResponseDto.uuid = apiResponseDto.responseData.Uuid;
            }
            else
            {
                tvHResponseDto.errorMessage = apiResponseDto.errorMsg;
            }
            return tvHResponseDto;
        }

        public List<TvHChannelDto> getChannellist()
        {
            List<TvHChannelDto> channelList = new List<TvHChannelDto>(100);
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_CHANNELGRIDLIST,
            };

            Task<ApiResponseDto<ChannelEntryList>> taskData = sendMessage<ChannelEntryList>(requestData);
            ApiResponseDto<ChannelEntryList> apiResponseDto = taskData.Result;

            if (apiResponseDto.responseData != null && apiResponseDto.responseData.entries!=null && apiResponseDto.responseData.entries.Count > 0)
            {
                foreach (ChannelEntity item in apiResponseDto.responseData.entries)
                {
                    channelList.Add(channelMapper.mapToDto(item));
                }
            }
            return channelList;
        }

        public List<TvHChanneltagDto> getChanneltaglist()
        {
            List<TvHChanneltagDto> channelList = new List<TvHChanneltagDto>(100);
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_CHANNELTAGGRIDLIST,
            };
            Task<ApiResponseDto<ChanneltagEntryList>> taskData = sendMessage<ChanneltagEntryList>(requestData);
            ApiResponseDto<ChanneltagEntryList> apiResponseDto = taskData.Result;
            if (apiResponseDto.responseData != null && apiResponseDto.responseData.entries != null && apiResponseDto.responseData.entries.Count > 0)
            {
                foreach (ChanneltagEntity item in apiResponseDto.responseData.entries)
                {
                    channelList.Add(channeltagMapper.mapToDto(item));
                }
            }
            return channelList;
        }

        public List<TvHDvrProfileDto> getDvrProfileList()
        {
            List<TvHDvrProfileDto> profileList = new List<TvHDvrProfileDto>(20);
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_DVRCONFIGGRID,
            };
            Task<ApiResponseDto<DvrProfileEntityList>> taskData = sendMessage<DvrProfileEntityList>(requestData);
            ApiResponseDto<DvrProfileEntityList> apiResponseDto = taskData.Result;
            if(apiResponseDto.responseData !=null && apiResponseDto.responseData.entityList!=null && apiResponseDto.responseData.entityList.Count > 0)
            {
                foreach (DvrProfileEntity item in apiResponseDto.responseData.entityList)
                {
                    profileList.Add(dvrProfileMapper.mapToDto(item));
                }
            }
            return profileList;
        }

        public List<TvHDvrUpcomingDto> getDvrUpcominglist()
        {
            List<TvHDvrUpcomingDto> upcommingList = new List<TvHDvrUpcomingDto>(20);
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_GRIDUPCOMING,
            };
            Task<ApiResponseDto<DvrUpcomingEntryList>> taskData = sendMessage<DvrUpcomingEntryList>(requestData);
            ApiResponseDto<DvrUpcomingEntryList> apiResponseDto = taskData.Result;
            if (apiResponseDto.responseData != null && apiResponseDto.responseData.dvrUpcomingEntityList!=null && apiResponseDto.responseData.dvrUpcomingEntityList.Count > 0)
            {
                foreach (DvrUpcomingEntity item in apiResponseDto.responseData.dvrUpcomingEntityList)
                {
                    upcommingList.Add(dvrUpcomingMapper.mapToDto(item));
                }
            }
            return upcommingList;
        }

        /// <summary>
        /// Fordert die Serverinfos wie API- und Programmversion an.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TvHServerInfoDto getServerInfo()
        {
            TvHServerInfoDto result = null;
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_SERVERINFO,
            };
            Task<ApiResponseDto<ServerInfoEntity>> taskData = sendMessage<ServerInfoEntity>(requestData);
            ApiResponseDto<ServerInfoEntity> apiResponseDto = taskData.Result;
            if (apiResponseDto.success)
            {
                result = serverInfoMapper.mapToDto(apiResponseDto.responseData);
            }
            else
            {
                throw new Exception(apiResponseDto.errorMsg);
            }
            return result;
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
            TvHResponseDto tvHResponseDto = new TvHResponseDto();
            tvHResponseDto.successful = false;
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);
            
            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_DVRCANCEL + "?uuid=" + tvHDvrUpcomingDto.uuid,
                ignoreResponseData = true,
            };

            Task<ApiResponseDto<string>> taskData = sendMessage<string>(requestData);
            ApiResponseDto<string> apiResponseDto = taskData.Result;
            if (apiResponseDto.success)
            {
                tvHResponseDto.successful = true;
            }
            else
            {
                tvHResponseDto.errorMessage = apiResponseDto.errorMsg;
            }
            return tvHResponseDto;
        }

        public TvHResponseDto stopRunningDvr(TvHDvrUpcomingDto tvHDvrUpcomingDto)
        {
            TvHResponseDto tvHResponseDto = new TvHResponseDto();
            tvHResponseDto.successful = false;
            if (_connectionData is null)
                throw new Exception(Messages.MESSAGE_NO_CONNECTION_DATA_SET);

            RequestData requestData = new RequestData
            {
                apiPath = APIPATH_DVRSTOP + "?uuid=" + tvHDvrUpcomingDto.uuid,
                ignoreResponseData = true,
            };

            Task<ApiResponseDto<string>> taskData = sendMessage<string>(requestData);
            ApiResponseDto<string> apiResponseDto = taskData.Result;
            if (apiResponseDto.success)
            {
                tvHResponseDto.successful = true;
            }
            else
            {
                tvHResponseDto.errorMessage = apiResponseDto.errorMsg;
            }
            return tvHResponseDto;
        }


        private async Task<ApiResponseDto<R>> sendMessage<R>(RequestData requestData)
        {
            HttpResponseMessage? httpResponseMessage = null;
            ApiResponseDto<R> apiResponseDto = new ApiResponseDto<R>();
            apiResponseDto.success = false;
            string? jsonResultString = null;
            try
            {
                if (tvHeadendHttpclient == null)
                    tvHeadendHttpclient = createHttpClient(false);//Basic auth
                httpResponseMessage = await tvHeadendHttpclient.GetAsync(_connectionData.serverUrl + requestData.apiPath);
                if (httpResponseMessage != null && httpResponseMessage.IsSuccessStatusCode)
                {
                    HttpContent responseContent = httpResponseMessage.Content;
                    jsonResultString = await responseContent.ReadAsStringAsync();
                    
                    if (!string.IsNullOrEmpty(jsonResultString))
                    {
                        try
                        {
                            R? entity = JsonSerializer.Deserialize<R>(jsonResultString);
                            apiResponseDto.responseData = entity;
                            apiResponseDto.success = true;
                        }
                        catch(Exception e1)
                        {
                            if (requestData.ignoreResponseData)
                            {
                                apiResponseDto.success = true;
                            }
                            else
                            {
                                apiResponseDto.errorMsg = e1.Message;
                                apiResponseDto.success = false;
                            }
                        }
                    }
                }
                else if (httpResponseMessage != null && !httpResponseMessage.IsSuccessStatusCode)
                {
                    switch (httpResponseMessage.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            apiResponseDto.errorMsg = Messages.MESSAGE_INVALID_REQUESTDATA + ": Wrong Username or password or Auth-Type.";
                            break;
                        case HttpStatusCode.Forbidden:
                            apiResponseDto.errorMsg = Messages.MESSAGE_INVALID_REQUESTDATA + ": Wrong Username or password or Auth-Type.";
                            break;
                        default:
                            apiResponseDto.errorMsg = Messages.MESSAGE_INVALID_REQUESTDATA + ": " + httpResponseMessage.StatusCode;
                            break;
                    }
                }
                else
                {
                    apiResponseDto.errorMsg = "No Response";
                }
            }
            catch(Exception e1)
            {
                apiResponseDto.errorMsg = e1.Message;
            }
            return apiResponseDto;
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
            foreach(TvHChannelDto item in getChannellist())
            {
                if(item.name.Equals(channelname, StringComparison.OrdinalIgnoreCase)
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

        private HttpClient createHttpClient(bool digest)
        {
            HttpClient client = null;
            if (digest)
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    Credentials = new NetworkCredential(_connectionData.credentials.username, _connectionData.credentials.password)
                };
                client = new HttpClient(handler);
            }
            else
            {
                client = new HttpClient();
                byte[] authToken = Encoding.ASCII.GetBytes($"{_connectionData.credentials.username}:{_connectionData.credentials.password}");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));
            }
            return client;
        }


        /// <summary>
        /// 32 zeichen
        /// </summary>
        /// <returns></returns>
        private string getGeneratedUuid()
        {
            string newGuid = Guid.NewGuid().ToString("N");
            return newGuid;
        }




    }

   
}
