using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;
using TvhAdapter.Mapper;

namespace TvHeadendAdapter.Mapper
{
    public class DvrProfileMapper : IBasicMapper<DvrProfileEntity, TvHDvrProfileDto>
    {
        public TvHDvrProfileDto mapToDto(DvrProfileEntity entity)
        {
            TvHDvrProfileDto dto = new TvHDvrProfileDto
            {
                uuid = entity.uuid,
                name = entity.name,
                postExtraTime = entity.postExtraTime,
                preExtraTime = entity.preExtraTime,
            };
            return dto;
        }

        public DvrProfileEntity mapToEntity(TvHDvrProfileDto dto)
        {
            //not used
            return null;
        }
    }
}
