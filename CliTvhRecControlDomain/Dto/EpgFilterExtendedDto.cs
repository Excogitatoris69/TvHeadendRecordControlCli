using System.Collections.ObjectModel;

namespace CliTvhRecControlDomain.Dto
{
    /// <summary>
    /// Verbesserter EPG-Filter mit mehr Optionen.
    /// </summary>
    public class EpgFilterExtendedDto
    {
        public EpgFilterExtendedDto()
        {
            _channelList = new List<string>();
            forceTimeSort = true;
        }
        public long startTime { get; set; }
        public long stopTime { get; set; }

        public void setStartTime(DateTime time)
        {
            DateTimeOffset t = time;
            startTime = t.ToUnixTimeSeconds();
        }

        public void setStopTime(DateTime time)
        {
            DateTimeOffset t = time;
            stopTime = t.ToUnixTimeSeconds();
        }

        public int resultLimit { get; set; }//max. Anzahl der ergebnisliste

        public int durationMin { get; private set; }//Mindestlänge 0-x
        public int durationMax { get; private set; }//Max-Länge oder 0 für ignorieren

        public void setDuration(int min, int max)
        {
            durationMin = min;
            durationMax = max;
        }

        //wenn leer dann alle
        private List<string> _channelList;

        public ReadOnlyCollection<string> channelList
        {
            get
            {
                return _channelList.AsReadOnly();
            }
        }

        public void addChannel(string channelName)
        {
            if (!_channelList.Contains(channelName))
            {
                _channelList.Add(channelName);
            }
        }

        //public bool inversChannelList { get; set; } //true -> alle ausser die geanannten


        public bool forceTimeSort { get; set; }//nur bei channelfilter sortierung nach Zeit

    }
}
