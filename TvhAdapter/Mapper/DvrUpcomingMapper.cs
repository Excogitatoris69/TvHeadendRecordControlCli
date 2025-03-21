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
                dispDescription = entity.dispDescription,
                dispExtratext = entity.dispExtratext,
                dispSubtitle = entity.dispSubtitle,
                startTime = entity.startTime,
                stoptime = entity.stopTime,
                startTimeReal = entity.startTimeReal,
                stopTimeReal = entity.stopTimeReal,
                owner = entity.owner,
                dvrProfileUuid = entity.dvrProfileUuid,
                scheduledStatus = entity.scheduledStatus,
                status = entity.status,
            };
            dto.title = entity.title != null ? entity.title.GetFirstTitle() : "";
            dto.description = entity.description != null ? entity.description.GetFirstTitle() : "";
            dto.subtitle = entity.subtitle != null ? entity.subtitle.GetFirstTitle() : "";
            return dto;
        }

        public DvrUpcomingEntity mapToEntity(TvHDvrUpcomingDto dto)
        {
            //not used
            return null;
        }
    }
}
