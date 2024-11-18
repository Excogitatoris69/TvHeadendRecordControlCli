using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TvhAdapter
{
   
    public class ApiResponseDto<R>
    {
        public bool success { get; set; }
        public string? errorMsg { get; set; }//interen Fehlermeldungen
        public R responseData { get; set; }//enthält die Antwortdaten
    }
}
