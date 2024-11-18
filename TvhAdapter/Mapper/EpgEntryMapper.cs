using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;
using TvhAdapter.Mapper;

namespace TvHeadendAdapter.Mapper
{
    public class EpgEntryMapper : IBasicMapper<EpgEntity, TvHEpgEntryDto>
    {
        public TvHEpgEntryDto mapToDto(EpgEntity entity)
        {
            TvHEpgEntryDto dto = new TvHEpgEntryDto
            {
                channelName = entity.channelName,
                channelUuid = entity.channelUuid,
                title = entity.title,
                subtitle = entity.subtitle,
                description = entity.description,
                summary = entity.summary,
                startTime = entity.startTime,
                stopTime = entity.stopTime,
                dvrUuid = entity.dvrUuid,
                dvrState = entity.dvrState,
                eventId = entity.eventId,
                nextEventId = entity.nextEventId,
            };
            return dto;
        }

        public EpgEntity mapToEntity(TvHEpgEntryDto dto)
        {
            //not used
            return null;
        }
    }
}
