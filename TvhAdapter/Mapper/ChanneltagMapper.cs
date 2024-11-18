using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;

namespace TvhAdapter.Mapper
{
    public class ChanneltagMapper : IBasicMapper<ChanneltagEntity, TvHChanneltagDto>
    {
        public TvHChanneltagDto mapToDto(ChanneltagEntity entity)
        {
            TvHChanneltagDto dto = new TvHChanneltagDto
            {
                uuid = entity.uuid,
                name = entity.name,
                comment = entity.comment,
            };
            return dto;
        }

        public ChanneltagEntity mapToEntity(TvHChanneltagDto dto)
        {
            //not used
            return null;
        }
    }
}
