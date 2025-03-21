using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;
using TvhAdapter.Mapper;

namespace TvHeadendAdapter.Mapper
{
    public class DvrAddEntitylMapper : IBasicMapper<DvrAddEntity, TvHDvrAddEntryDto>
    {
        public TvHDvrAddEntryDto mapToDto(DvrAddEntity entity)
        {
            //not used
            return null;
        }

        public DvrAddEntity mapToEntity(TvHDvrAddEntryDto dto)
        {
            DvrAddEntity entity = new DvrAddEntity
            {
                enabled=dto.enable,
                priority=dto.priority,
                channelName = dto.channelName,
                dvrProfileUuid = dto.dvrProfileUuid,
                startTime = dto.startTime,
                stopTime = dto.stopTime,
                title = new MultilingualText(),
                subtitle = new MultilingualText(),
                description = new MultilingualText(),
                dispDescription = dto.dispDescription,
                dispSubtitle = dto.dispSubtitle,
                dispExtratext = dto.dispExtratext,
                comment=dto.comment,
            };
            entity.title.Language = new Dictionary<string, object>
            {
                { dto.languageShort, dto.title }
            };
            entity.subtitle.Language = new Dictionary<string, object>
            {
                { dto.languageShort, dto.subtitle }
            };
            entity.description.Language = new Dictionary<string, object>
            {
                { dto.languageShort, dto.description }
            };
            return entity;
        }
    }

}
