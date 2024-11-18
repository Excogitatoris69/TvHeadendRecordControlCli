using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;
using TvhAdapter.Mapper;

namespace TvHeadendAdapter.Mapper
{
    public class DvrUpcomingMapper : IBasicMapper<DvrUpcomingEntity, TvHDvrUpcomingDto>
    {
        public TvHDvrUpcomingDto mapToDto(DvrUpcomingEntity entity)
        {
            TvHDvrUpcomingDto dto = new TvHDvrUpcomingDto
            {
                uuid = entity.uuid,
                channelName = entity.channelName,
                channelUuid = entity.channelUuid,
                title = entity.title.GetFirstTitle(),
                startTime = entity.startTime,
                stoptime = entity.stopTime,
                startTimeReal = entity.startTimeReal,
                stopTimeReal = entity.stopTimeReal,
                owner = entity.owner,
                dvrProfileUuid = entity.dvrProfileUuid,
                scheduledStatus = entity.scheduledStatus,
                status = entity.status,
            };
            return dto;
        }

        public DvrUpcomingEntity mapToEntity(TvHDvrUpcomingDto dto)
        {
            //not used
            return null;
        }
    }
}
