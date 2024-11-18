using CliTvhRecControlDomain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TvhAdapter.JsonEntities;

namespace TvhAdapter.Mapper
{
    public class ServerInfoMapper : IBasicMapper<ServerInfoEntity, TvHServerInfoDto>
    {
        public TvHServerInfoDto mapToDto(ServerInfoEntity entity)
        {
            TvHServerInfoDto dto = new TvHServerInfoDto
            {
                versionTvhServerSoftware = entity.versionTvhServerSoftware,
                versionApi = entity.versionApi
            };
            return dto;
        }

        public ServerInfoEntity mapToEntity(TvHServerInfoDto dto)
        {
            //not used
            return null;
        }
    }
}
