using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Credentials
{
    public class CliParams
    {
        public string ServerUrl { get; set; }
        public string ServerUrlApi { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Credentialname { get; set; }
        public string Command { get; set; }
        public long StarttimeUnix { get; set; }
        //public bool IsStartnow { get; set; }
        public long EndtimeUnix { get; set; }
        public string StarttimeHuman { get; set; }
        public string EndtimeHuman { get; set; }
        public string ChannelName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Uuid { get; set; }
        public string Language { get; set; }
        public string DvrProfileName { get; set; }
        public string Comment { get; set; }
        //public Priority Priority { get; set; }
        public string StreamplayerPath { get; set; }
    }

    
}
