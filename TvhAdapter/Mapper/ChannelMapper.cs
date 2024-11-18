using CliTvhRecControlDomain.Dto;
using TvhAdapter.JsonEntities;

namespace TvhAdapter.Mapper
{
    public class ChannelMapper : IBasicMapper<ChannelEntity, TvHChannelDto>
    {
        public TvHChannelDto mapToDto(ChannelEntity entity)
        {
            TvHChannelDto dto = new TvHChannelDto
            {
                uuid = entity.uuid!=null? entity.uuid: entity.key,
                name = entity.name!=null? entity.name: entity.val,
                number = entity.number,
                enabled = entity.enabled,
                icon = entity.icon,
                iconPublicUrl = entity.iconPublicUrl,
                tagUuidListList = new List<string>()
            };

            if(entity.tagUuidListList != null )
            {
                foreach (string item in entity.tagUuidListList)
                {
                    dto.tagUuidListList.Add(item);
                }
            } 
            return dto;
        }

        public ChannelEntity mapToEntity(TvHChannelDto dto)
        {
            //not used
            return null;
        }
    }
}
