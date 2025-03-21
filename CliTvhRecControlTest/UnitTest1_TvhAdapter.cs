using CliTvhRecControlCore.Services;
using CliTvhRecControlDomain.Dto;
using TvhAdapter;

namespace CliTvhRecControlTest
{

    [TestClass]
    public class UnitTest1_TvhAdapter
    {
        TvHConnectionDataDto connectionData = null;

        [TestInitialize]
        public void initTest()
        {
            connectionData = new TvHConnectionDataDto();
            connectionData.serverUrl = "http://ptvheadend:9981";
            connectionData.credentials = new TvHCredentialsDto
            {
                username = "tvuser",
                password = "tvuser"
            };
        }

        [TestMethod]
        public void test_01_getServerInfo()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;
            TvHServerInfoDto dto = testTvhAdapterImpl.getServerInfo();
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.versionTvhServerSoftware);

            
        }

        [TestMethod]
        public void test_02_DvrProfile()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;
            List<TvHDvrProfileDto> list = testTvhAdapterImpl.getDvrProfileList();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }

        [TestMethod]
        public void test_03_AddRemoveDvr()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;

            //add
            TvHDvrProfileDto aTvHDvrProfileDto = testTvhAdapterImpl.getDvrConfigByNameOrUuid("withoutPadding");
            TvHDvrAddEntryDto tvHDvrAddEntryDto = new TvHDvrAddEntryDto
            {
                channelName = "ZDF HD",
                title = "Test1",
                comment = "ein comment",
                subtitle = "ein Subtitle",
                description = "eine Description",
                dispDescription = "eine dispDescription",
                dispExtratext = "eine dispExtratext",
                dispSubtitle = "eine dispSubtitle",
                dvrProfileUuid = aTvHDvrProfileDto.uuid, // hier anpassen an gültige Profil-ID
                languageShort = "ger",//Und 
            };
            //DateTime start = new DateTime(2024, 1, 2, 20, 15, 00);
            //DateTime stop = new DateTime(2024, 1, 2, 21, 45, 00);
            DateTime start = DateTime.Now.AddDays(2);
            DateTime stop = start.AddHours(2);
            tvHDvrAddEntryDto.setStartTime(start);
            tvHDvrAddEntryDto.setStopTime(stop);

            TvHResponseDto tvHResponseDto = testTvhAdapterImpl.addEpgToDvr(tvHDvrAddEntryDto);
            Assert.IsTrue(tvHResponseDto.successful);
            string newEntyUuid = tvHResponseDto.uuid;

            //getUpcomminglist
            List<TvHDvrUpcomingDto> list = testTvhAdapterImpl.getDvrUpcominglist();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
            bool found = false;
            TvHDvrUpcomingDto foundTvHDvrUpcomingDto = null;
            foreach (TvHDvrUpcomingDto item in list)
            {
                if (item.uuid == newEntyUuid)
                {
                    found = true;
                    foundTvHDvrUpcomingDto = item;
                    break;
                }
            }
            Assert.IsTrue(found);
            //remove
            tvHResponseDto = testTvhAdapterImpl.removeFromDvr(foundTvHDvrUpcomingDto);
            Assert.IsTrue(tvHResponseDto.successful);
        }

        [TestMethod]
        public void test_04_GetChannellist()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;
            List<TvHChannelDto> list = testTvhAdapterImpl.getChannellist();
            Assert.IsNotNull(list);
        }

        [TestMethod]
        public void test_05_getDvrUpcominglist()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;
            List<TvHDvrUpcomingDto> list = testTvhAdapterImpl.getDvrUpcominglist();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count > 0);
        }


        [TestMethod]
        public void test_06_getDvrEntryClass()
        {
            TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            testTvhAdapterImpl.connectionData = connectionData;
            testTvhAdapterImpl.getDvrEntryClass();
            
            Assert.IsTrue(true);
        }
    }
}