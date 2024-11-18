using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvhAdapter.Mapper
{
    public interface IBasicMapper<E, D>
    {
        public D mapToDto(E entity);
        public E mapToEntity(D dto);
    }
}
