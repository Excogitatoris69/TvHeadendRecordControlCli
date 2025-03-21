
using CliTvhRecControlCore.Interfaces;
using CliTvhRecControlCore.Services;
using CliTvhRecControlDomain.Dto;
using CommandLine;
using CommandLine.Text;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using TvhAdapter;
using TvhAdapterSimulate;

namespace Credentials
{
    public class MainConsoleApp
    {
        private static string buildString = null;
        private bool errorOccurs = false;
        private static string appTitle = "TvHeadend RecordControl CLI by O.Ma ";

        private List<TvHDvrProfileDto> _dvrProfileList = null;
        private List<TvHChannelDto> _channelList = null;
        private ITvhAdapter tvhAdapterImpl = null;
        private TvhService tvhService = null;

        //test and debug
        public bool offlineTest = false;
        public static void Main(string[] args)
        {
            MainConsoleApp me = new MainConsoleApp();
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            buildString = "Build: 21.03.2025 Release: "+fvi.FileVersion;
            me.showVersion();
            if (me.offlineTest)
                Console.WriteLine("***** Achtung: Test ohne realen TVH-Server. *********");
            try
            {
                me.executeMain(args);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Error: {0}", ex.Message);
                me.errorOccurs = true;
            }
            if(me.errorOccurs)
                Environment.Exit(1);
            else
                Environment.Exit(0);
        }

        public void executeMain(string[] args)
        {
            CliParams cliParams = readCommandlineArgs(args);
            execute(cliParams);
        }

        

        private void execute(CliParams cliParams)
        {
            if (cliParams == null)
            {
                Console.WriteLine("Error");
                errorOccurs = true;
                return;
            }
            if (string.IsNullOrEmpty(cliParams.Command))
            {
                errorOccurs = true;
                return;
            }
            
            if (offlineTest)
            {
                tvhAdapterImpl = new TvHAdapterSimulateImpl();
            }
            else
            {
                tvhAdapterImpl = new TvHAdapterImpl();

            }

            if ( !cliParams.Command.Equals(CMD_LIST_CREDENTIAL) && !cliParams.Command.Equals(CMD_SAVE_CREDENTIAL) &&
                !cliParams.Command.Equals(CMD_REMOVE_CREDENTIAL) && !string.IsNullOrEmpty(cliParams.Credentialname))
            {
                CredentialController credentialController = new CredentialController();
                CredentialEntity item = credentialController.getCredentialByName(cliParams.Credentialname);
                if(item != null)
                {
                    Console.WriteLine("Use credential {0} in file {1}", cliParams.Credentialname, credentialController.filePath);
                    tvhAdapterImpl.connectionData = new CliTvhRecControlDomain.Dto.TvHConnectionDataDto
                    {
                        serverUrl = item.ServerUrl,
                        credentials = new CliTvhRecControlDomain.Dto.TvHCredentialsDto
                        {
                            username = item.UserName,
                            password = item.Password
                        }
                    };
                }
                else
                {
                    Console.WriteLine("Error: No credential found with name {0} in file {1}", cliParams.Credentialname, credentialController.filePath);
                    errorOccurs = true;
                    return;
                }
            }
            else
            {
                tvhAdapterImpl.connectionData = new CliTvhRecControlDomain.Dto.TvHConnectionDataDto
                {
                    serverUrl = cliParams.ServerUrl,
                    credentials = new CliTvhRecControlDomain.Dto.TvHCredentialsDto
                    {
                        username = cliParams.UserName,
                        password = cliParams.Password
                    }
                };
            }
            tvhService = new TvhService(tvhAdapterImpl);
            readChannelAndDvrProfileList();//immer lesen und cachen, weil heufig benoetigt

            //------------
            if (cliParams.Command.Equals(CMD_SERVERINFO))
            {
                TvHServerInfoDto serverinfo = tvhService.getServerInfo();
                Console.WriteLine("TvHeadend-Version: {0}\nAPI: {1}",serverinfo.versionTvhServerSoftware, serverinfo.versionApi);
                Console.WriteLine("URL: {0}", cliParams.ServerUrl);
                Console.WriteLine("Successful.");
            }

            //------------
            if (cliParams.Command.Equals(CMD_DVR_LIST))
            {
                
                List<TvHDvrUpcomingDto> list = tvhService.getDvrUpcominglist();
                Console.WriteLine("=============");
                Console.WriteLine("DVR-Upcomming");
                Console.WriteLine("=============");
                foreach (TvHDvrUpcomingDto item in list)
                {
                    Console.WriteLine("[{0}] [{1} - {2}] '{3}' [{4}] [{5}] {6}"
                        , item.channelName
                        , item.getStartTimeString()
                        , item.getStopTimeString()
                        , item.title
                        , item.scheduledStatus
                        , getDvrProfileByUuidOrName(item.dvrProfileUuid).name
                        , item.uuid)
                        ;
                }
                Console.WriteLine("-------------");
                Console.WriteLine("Successful.");

            }
            if (cliParams.Command.Equals(CMD_CHANNELLIST))
            {
                string[] buildedList = buildFormatedChannellist(_channelList,2, true);
                Console.WriteLine("=============");
                Console.WriteLine("Channels");
                Console.WriteLine("=============");
                for (int i = 0; i < buildedList.Length; i++)
                {
                    Console.WriteLine(buildedList[i]);
                }
                Console.WriteLine("-------------");
                Console.WriteLine("Successful.");
            }
            if (cliParams.Command.Equals(CMD_CHANNELLIST_RAW))
            {
                //string[] buildedList = buildFormatedChannellist(_channelList, 2, true);
                Console.WriteLine("=============");
                Console.WriteLine("Channels (raw)");
                Console.WriteLine("=============");
                for (int i = 0; i < _channelList.Count; i++)
                {
                    Console.WriteLine(_channelList[i].name);
                }
                Console.WriteLine("-------------");
                Console.WriteLine("Successful.");
            }
            if (cliParams.Command.Equals(CMD_DVR_PROFILELIST))
            {
                List<TvHDvrProfileDto> list = tvhService.getDvrProfileList();
                string[] buildedList = buildFormatedDvrProfilelist(list, 1, true);
                Console.WriteLine("=============");
                Console.WriteLine("DVR Profiles");
                Console.WriteLine("=============");
                for (int i = 0; i < buildedList.Length; i++)
                {
                    Console.WriteLine(buildedList[i]);
                }
                Console.WriteLine("-------------");
                Console.WriteLine("Successful.");
            }

            if (cliParams.Command.Equals(CMD_DVR_CREATE))
            {
                Console.WriteLine("=============");
                Console.WriteLine("DVR Create");
                Console.WriteLine("=============");
                //check datetimes
                long unixStarttime = 0;
                long unixEndtime = 0;

                if (!string.IsNullOrEmpty(cliParams.StarttimeHuman) || !string.IsNullOrEmpty(cliParams.EndtimeHuman))//human time
                {
                    bool timeOk1 = false;
                    bool timeOk2 = false;
                    
                    timeOk1 = parseHumanTimeToUnixTime(cliParams.StarttimeHuman, "dd.MM.yyyy HH:mm", out unixStarttime);
                    timeOk2 = parseHumanTimeToUnixTime(cliParams.EndtimeHuman, "dd.MM.yyyy HH:mm", out unixEndtime); 
                    if (!timeOk1 || !timeOk2)
                    {
                        Console.WriteLine("Error: Human time format not valid. {0}  {1}", cliParams.StarttimeHuman, cliParams.EndtimeHuman);
                        errorOccurs = true;
                        return;
                    }
                }
                else // unix time
                {
                    unixStarttime = cliParams.StarttimeUnix;
                    unixEndtime = cliParams.EndtimeUnix;
                }
                TvHDvrAddEntryDto tvHDvrAddEntryDto = new TvHDvrAddEntryDto();
                tvHDvrAddEntryDto.comment = cliParams.Comment;
                tvHDvrAddEntryDto.channelName = cliParams.ChannelName;
                tvHDvrAddEntryDto.title = cliParams.Title;
                tvHDvrAddEntryDto.description = cliParams.Description;
                tvHDvrAddEntryDto.startTime = unixStarttime;
                tvHDvrAddEntryDto.stopTime = unixEndtime;
                tvHDvrAddEntryDto.dvrProfileName = cliParams.DvrProfileName;
                tvHDvrAddEntryDto.languageShort = "und";
                TvHResponseDto response =  tvhService.addEpgToDvr(tvHDvrAddEntryDto);
                if ((response.successful))
                {
                    Console.WriteLine("Successful. [{0}] [{1} - {2}] '{3}' [{4}] {5}"
                        , tvHDvrAddEntryDto.channelName
                        , tvHDvrAddEntryDto.startTimeHuman
                        , tvHDvrAddEntryDto.stopTimeHuman
                        , tvHDvrAddEntryDto.title
                        , getDvrProfileByUuidOrName(tvHDvrAddEntryDto.dvrProfileName).name
                        , response.uuid);
                    
                }
                else
                {
                    Console.WriteLine("Error: {0}" , response.errorMessage);
                    errorOccurs = true;
                    return;
                }
            }
            if (cliParams.Command.Equals(CMD_DVR_REMOVE))
            {
                Console.WriteLine("=============");
                Console.WriteLine("DVR Remove");
                Console.WriteLine("=============");
                if (!string.IsNullOrEmpty(cliParams.Uuid))
                {
                    TvHResponseDto response = tvhService.removeUpcomingDvrEntry(cliParams.Uuid);
                    if(response.successful)
                    {
                        Console.WriteLine("Successful");
                        
                    }
                    else
                    {
                        Console.WriteLine("Error: {0}", response.errorMessage);
                        errorOccurs = true;
                        return;
                    }
                }
                else
                {
                    //search with starttime and channel
                    List<TvHDvrUpcomingDto> list = tvhService.getDvrUpcominglist();
                    string foundUuid = null;
                    foreach(TvHDvrUpcomingDto item in list)
                    {
                        if (item.channelName.Equals(cliParams.ChannelName) && item.startTime == cliParams.StarttimeUnix) 
                        {
                            foundUuid = item.uuid;
                            break;
                        }
                    }
                    if (!string.IsNullOrEmpty(foundUuid))
                    {
                        TvHResponseDto response = tvhService.removeUpcomingDvrEntry(foundUuid);
                        if (response.successful)
                        {
                            Console.WriteLine("Successful");
                            
                        }
                        else
                        {
                            Console.WriteLine("Error: {0}", response.errorMessage);
                            errorOccurs = true;
                            return;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Dvr-Entry not found.");
                        errorOccurs = true;
                        return;
                    }
                }

            }
            if (cliParams.Command.Equals(CMD_LIVESTREAM))
            {
                Console.WriteLine("==================");
                Console.WriteLine("Open Livestream...");
                Console.WriteLine("==================");

                if (string.IsNullOrEmpty(cliParams.StreamplayerPath))
                {
                    Console.WriteLine("Error: Path to streamplayer not found. {0}", cliParams.StreamplayerPath);
                    errorOccurs = true;
                    return;
                }
                else
                {
                    //split path and params
                    string[] streamPlayerPathTokens = cliParams.StreamplayerPath.Split(','); // path params
                    if (!string.IsNullOrEmpty(streamPlayerPathTokens[0]) && !File.Exists(streamPlayerPathTokens[0].Trim()))
                    {
                        Console.WriteLine("Error: Path to streamplayer not found. {0} ", cliParams.StreamplayerPath);
                        errorOccurs = true;
                        return;
                    }
                    else
                    {
                        string streamUrl = tvhService.getStreamUrlOfChannel(cliParams.ChannelName);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(streamUrl);
                        if (streamPlayerPathTokens.Length == 2)
                            sb.Append(streamPlayerPathTokens[1]);
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.Arguments = sb.ToString();
                        startInfo.FileName = streamPlayerPathTokens[0].Trim();
                        startInfo.WindowStyle = ProcessWindowStyle.Normal;
                        startInfo.UseShellExecute = false;
                        Process exeProcess = Process.Start(startInfo);
                        Console.WriteLine("Successful.");
                    }
                }

            }
            if (cliParams.Command.Equals(CMD_SAVE_CREDENTIAL))
            {
                CredentialController credentialController = new CredentialController();
                CredentialEntity credentialEntity = new CredentialEntity();
                credentialEntity.Credentialname = cliParams.Credentialname;
                credentialEntity.ServerUrl = cliParams.ServerUrl;
                credentialEntity.UserName = cliParams.UserName;
                credentialEntity.Password = cliParams.Password;
                Result result = credentialController.addOrUpdateCredential(credentialEntity);
                if (result.success == true)
                {
                    Console.WriteLine("Successful saved in file " + credentialController.filePath);
                }
                else
                {
                    Console.WriteLine("Error: {0}", result.errorMsg);
                }
            }

            if (cliParams.Command.Equals(CMD_LIST_CREDENTIAL))
            {
                Console.WriteLine("===============");
                Console.WriteLine("Credential list");
                Console.WriteLine("===============");
                CredentialController credentialController = new CredentialController();
                List<CredentialEntity> list = credentialController.getCredentialsList();
                if (list != null && list.Count > 0)
                {
                    Console.WriteLine("File: " + credentialController.filePath);
                    foreach (CredentialEntity item in list)
                    {
                        Console.WriteLine("[{0}] - '{1}' {2}/{3}", item.Credentialname, item.ServerUrl, item.UserName, item.Password);
                    }
                }
                else
                {
                    Console.WriteLine("No entries found. File: {0}", credentialController.filePath);
                }
                Console.WriteLine("Successful");
            }

            if (cliParams.Command.Equals(CMD_REMOVE_CREDENTIAL))
            {
                CredentialController credentialController = new CredentialController();
                CredentialEntity item = credentialController.getCredentialByName(cliParams.Credentialname);
                if ((item != null))
                {
                    Result result = credentialController.removeCredential(item);
                    if ((result.success))
                    {
                        Console.WriteLine("Successful removed from file " + credentialController.filePath);
                    }
                    else
                    {
                        Console.WriteLine("Error: {0}. File: [1}", result.errorMsg, credentialController.filePath);
                    }
                }
                else
                {
                    Console.WriteLine("Error: No credential found. File: {0}" + credentialController.filePath);
                }
            }

        }

        //================================================================================

        private void readChannelAndDvrProfileList()
        {
            _dvrProfileList = tvhService.getDvrProfileList();
            _channelList = tvhService.getChannellist();
        }

        private TvHDvrProfileDto getDvrProfileByUuidOrName(string uuidOrName)
        {
            TvHDvrProfileDto result = null;
            foreach (TvHDvrProfileDto item in _dvrProfileList)
            {
                if (item.uuid == uuidOrName || item.name == uuidOrName)
                {
                    result = item; break;
                }
            }
            return result;
        }


        private bool parseHumanTimeToUnixTime(string starttimeHuman, string format, out long unixTime)
        {
            bool result = true;
            try
            {
                CultureInfo culture = new CultureInfo("de-DE");
                DateTime dateTimestart = DateTime.ParseExact(starttimeHuman, format, culture, DateTimeStyles.None);
                DateTimeOffset t = dateTimestart;
                unixTime = t.ToUnixTimeSeconds();
            }
            catch (Exception)
            {
                unixTime = 0l;
                result = false;
            }
            return result;
        }

        /// <summary>
        /// Build Tabel with number, channelnames and uuid
        /// [1] Name  6c94df5db542bcd1dff3d782f6f0a695  [2] Name  6c94df5db542bcd1dff3d782f6f0a695
        /// </summary>
        /// <param name="list"></param>
        /// <param name="columnsCount"></param>
        /// <param name="withUuid"></param>
        /// <returns></returns>
        private string[] buildFormatedChannellist(List<TvHChannelDto> list, int columnsCount, bool withUuid)
        {
            int rowCount = (int)Math.Ceiling((double)list.Count / columnsCount); 
            string[] resultList = new string[rowCount];
            // find longest name
            int maxNameLength = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name.Length > maxNameLength)
                {
                    maxNameLength = list[i].name.Length;
                }
            }
            maxNameLength++;// add space
            //number of digits from channelNumber
            int maxNumberLength = 1;
            if (list.Count > 9)
                maxNumberLength = 2;
            else if (list.Count > 99)
                maxNumberLength = 3;
            else if (list.Count > 999)
                maxNumberLength = 4;
            else if (list.Count > 9999)
                maxNumberLength = 5;
            //format string
            StringBuilder formatSb = new StringBuilder();// "[{0,2}]  {1,-20}" , maxNumberLength, maxNamelength
            formatSb.Append("[{0,");
            formatSb.Append(maxNumberLength);
            formatSb.Append("}] {1,-");
            formatSb.Append(maxNameLength);
            formatSb.Append("}");
            if (withUuid)
            {
                formatSb.Append("{2}  ");
            }
            string formatString = formatSb.ToString();

            //table
            StringBuilder sb = new StringBuilder();
            int channelNumber = 0;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columnsCount; colIndex++)
                {
                    channelNumber = colIndex * rowCount + 1 + rowIndex;
                    if (colIndex == 0)
                    {
                        sb.Clear();
                    }
                    if (colIndex == 0 || (colIndex != 0 && channelNumber <= list.Count))
                    {
                        if (withUuid)
                            sb.AppendFormat(formatString, channelNumber, list[channelNumber - 1].name, list[channelNumber - 1].uuid);
                        else
                            sb.AppendFormat(formatString, channelNumber, list[channelNumber - 1].name);
                    }
                    resultList[rowIndex] = sb.ToString();
                }
            }
            return resultList;
        }

        /// <summary>
        /// Build Table with number, profilenames and uuid
        /// [1] Name  6c94df5db542bcd1dff3d782f6f0a695  [2] Name  6c94df5db542bcd1dff3d782f6f0a695
        /// </summary>
        /// <param name="list"></param>
        /// <param name="columnsCount"></param>
        /// <param name="withUuid"></param>
        /// <returns></returns>
        private string[] buildFormatedDvrProfilelist(List<TvHDvrProfileDto> list, int columnsCount, bool withUuid)
        {
            int rowCount = (int)Math.Ceiling((double)list.Count / columnsCount);
            string[] resultList = new string[rowCount];
            // find longest name
            int maxNameLength = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].name.Length > maxNameLength)
                {
                    maxNameLength = list[i].name.Length;
                }
            }
            maxNameLength++;// add space
            //number of digits from channelNumber
            int maxNumberLength = 1;
            if (list.Count > 9)
                maxNumberLength = 2;
            else if (list.Count > 99)
                maxNumberLength = 3;
            else if (list.Count > 999)
                maxNumberLength = 4;
            else if (list.Count > 9999)
                maxNumberLength = 5;
            //format string
            // number name preExtratime postExtratime uuid
            // "[{0,2}]  {1,-20}" , maxNumberLength, maxNamelength
            // "[{0,2}] {1,-20} {2} {3}:{4}"

            StringBuilder formatSb = new StringBuilder();
            formatSb.Append("[{0,");
            formatSb.Append(maxNumberLength);
            formatSb.Append("}]");

            formatSb.Append(" {1,-");
            formatSb.Append(maxNameLength);
            formatSb.Append("}");

            formatSb.Append(" ({3} <-> {4})   ");

            if (withUuid)
            {
                formatSb.Append("{2}  ");
            }
            string formatString = formatSb.ToString();

            //table
            StringBuilder sb = new StringBuilder();
            int channelNumber = 0;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columnsCount; colIndex++)
                {
                    channelNumber = colIndex * rowCount + 1 + rowIndex;
                    if (colIndex == 0)
                    {
                        sb.Clear();
                    }
                    if (colIndex == 0 || (colIndex != 0 && channelNumber <= list.Count))
                    {
                        if (withUuid)
                            sb.AppendFormat(formatString, channelNumber, list[channelNumber - 1].name, list[channelNumber - 1].uuid, list[channelNumber - 1].preExtraTime , list[channelNumber - 1].postExtraTime);
                        else
                            sb.AppendFormat(formatString, channelNumber, list[channelNumber - 1].name);
                    }
                    resultList[rowIndex] = sb.ToString();
                }
            }
            return resultList;
        }


        private void showVersion()
        {
            Console.OutputEncoding = Encoding.Latin1;
            Console.WriteLine("** {0} {1}", appTitle, buildString);
        }

        static void DisplayHelp<T>(ParserResult<T> result)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = ""; // appTitle+buildString; //change header
                h.Copyright = ""; //change copyright text
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }
               

        private CliParams readCommandlineArgs(string[] args)
        {
            CliParams cliParams = new CliParams();
            Parser parser = new Parser(with => with.HelpWriter = null);
            ParserResult<CommandLineOptions> parserResult = parser.ParseArguments<CommandLineOptions>(args);
            parserResult
            .WithNotParsed(errs => DisplayHelp(parserResult));
            //.WithParsed<CommandLineOptions>(options => Run(options))
            //ParserResult<CommandLineOptions> result = Parser.Default.ParseArguments<CommandLineOptions>(args);
            ParserResult<CommandLineOptions> parseResult2 = parserResult.WithParsed<CommandLineOptions>(cmdParam =>
            {
                cliParams.Command = cmdParam.Command;
                cliParams.ServerUrl = cmdParam.Serverurl;
                cliParams.UserName = cmdParam.Username;
                cliParams.Password = cmdParam.Password;
                cliParams.Credentialname = cmdParam.Credentialname;
                cliParams.StarttimeUnix = cmdParam.StarttimeUnix;
                cliParams.EndtimeUnix = cmdParam.EndtimeUnix;
                cliParams.StarttimeHuman = cmdParam.StarttimeHuman;
                cliParams.EndtimeHuman = cmdParam.EndtimeHuman;
                cliParams.ChannelName = cmdParam.Channelname;
                cliParams.Title = cmdParam.Title;
                cliParams.Description = cmdParam.Description;
                cliParams.Uuid = cmdParam.Uuid;
                //cliParams.Language = cmdParam.Language;
                cliParams.DvrProfileName = cmdParam.DvrProfileName;
                cliParams.Comment = cmdParam.DvrComment;
                //cliParams.Priority = Priority.Unknown;
                cliParams.StreamplayerPath = cmdParam.StreamplayerPath;
            });
            return cliParams;
        }

        private static readonly string CMD_DVR_CREATE = "dvrcreate";
        private static readonly string CMD_DVR_REMOVE = "dvrremove";
        private static readonly string CMD_DVR_LIST = "dvrlist";
        private static readonly string CMD_DVR_PROFILELIST = "dvrprofilelist";
        private static readonly string CMD_CHANNELLIST = "channellist";
        private static readonly string CMD_CHANNELLIST_RAW = "channellistraw";
        private static readonly string CMD_SERVERINFO = "serverinfo";
        private static readonly string CMD_LIVESTREAM = "livestream";

        private static readonly string CMD_LIST_CREDENTIAL = "credentiallist";
        private static readonly string CMD_SAVE_CREDENTIAL = "savecredential";
        private static readonly string CMD_REMOVE_CREDENTIAL = "removecredential";

        //Todo: next steps
        
        
        
    }
}
