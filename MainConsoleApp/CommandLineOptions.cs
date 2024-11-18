using CommandLine;

namespace Credentials
{
    public class CommandLineOptions
    {
        [Option(longName: "command", HelpText = "Command \n(dvrcreate, dvrremove, dvrlist, dvrprofilelist, channellist, channellistraw, serverinfo, livestream, credentiallist, savecredential, removecredential)", Required = true)]
        public string Command { get; set; }

        [Option(longName: "serverurl", HelpText = "Server-URL (e.g.: http://ptvheadend:9981)", Required = false)]
        public string Serverurl { get; set; }

        [Option(longName: "username", HelpText = "Username", Required = false)]
        public string Username { get; set; }

        [Option(longName: "password", HelpText = "Password", Required = false)]
        public string Password { get; set; }

        [Option(longName: "credentialname", HelpText = "Name of credential", Required = false)]
        public string Credentialname { get; set; }

        [Option(longName: "starttime", HelpText = "Starttime (Unixtime)", Required = false)]
        public long StarttimeUnix { get; set; }

        //[Option(longName: "startnow", HelpText = "Start is rigth now", Required = false)]
        //public bool StartRightnow { get; set; }


        [Option(longName: "endtime", HelpText = "Endtime (Unixtime)", Required = false)]
        public long EndtimeUnix { get; set; }


        [Option(longName: "starttimeHuman", HelpText = "Starttime (DD.MM.YYYY hh:mm)", Required = false)]
        public string StarttimeHuman { get; set; }

        [Option(longName: "endtimeHuman", HelpText = "Endtime (DD.MM.YYYY hh:mm)", Required = false)]
        public string EndtimeHuman { get; set; }



        [Option(longName: "channel", HelpText = "Channelname", Required = false)]
        public string Channelname { get; set; }

        [Option( longName: "title", HelpText = "Title", Required = false)]
        public string Title { get; set; }

        [Option(longName: "description", HelpText = "Description", Required = false)]
        public string Description { get; set; }

        [Option(longName: "uuid", HelpText = "UUID", Required = false)]
        public string Uuid { get; set; }

        //[Option(longName: "language", HelpText = "Language", Required = false)]
        //public string Language { get; set; }

        [Option(longName: "config", HelpText = "DVR-Profilename", Required = false)]
        public string DvrProfileName { get; set; }

        [Option(longName: "comment", HelpText = "DVR-Comment", Required = false)]
        public string DvrComment { get; set; }

        [Option(longName: "streamplayer", HelpText = "Streamplayer path", Required = false)]
        public string StreamplayerPath { get; set; }


    }
}
