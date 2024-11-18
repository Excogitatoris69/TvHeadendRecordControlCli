using CliTvhRecControlCore.Services;
using CliTvhRecControlDomain.Dto;
using TvhAdapter;
using TvhAdapterSimulate;

namespace CliTvhRecControlTest
{
    [TestClass]
    public class UnitTest2_TvhService
    {

        TvhService tvhService = null;

        [TestInitialize]
        public void initTest()
        {

            //TvHAdapterImpl testTvhAdapterImpl = new TvHAdapterImpl();
            TvHAdapterSimulateImpl testTvhAdapterImpl = new TvHAdapterSimulateImpl();
            testTvhAdapterImpl.connectionData = new CliTvhRecControlDomain.Dto.TvHConnectionDataDto
            {
                serverUrl = "http://ptvheadend:9981",
                credentials = new CliTvhRecControlDomain.Dto.TvHCredentialsDto
                {
                    username = "tvuser",
                    password = "tvuser"
                }
            };
            tvhService = new TvhService(testTvhAdapterImpl);
        }

        [TestMethod]
        public void test_01_getServerInfo()
        {
            TvHServerInfoDto dto = tvhService.getServerInfo();
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.versionTvhServerSoftware);
        }

        [TestMethod]
        public void test_02_getChannellist()
        {
            List<TvHChannelDto> list = tvhService.getChannellist();
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Count > 0);
        }

        [TestMethod]
        public void test_03_getDvrProfilelist()
        {
            List<TvHDvrProfileDto> list = tvhService.getDvrProfileList();
            Assert.IsNotNull(list);
            Assert.IsNotNull(list.Count > 0);
        }


        [TestMethod]
        public void test_04_addEpgToDvr()
        {
            TvHDvrAddEntryDto tvHDvrAddEntryDto = new TvHDvrAddEntryDto
            {
                channelName = "ZDF HD", // 025748e75588d837b303dd69d761d587
                //channelName = "025748e75588d837b303dd69d761d587", 
                title = "Test1",
                comment = "ein comment",
                subtitle = "ein Subtitle",
                description = "ein Description",
                //dvrProfileUuid = "97993e21300bdbe066435bd0408d4328", // hier anpassen an gültige Profil-ID
                                                                        // 97993e21300bdbe066435bd0408d4328   withoutPadding
                dvrProfileName = "withoutPadding",
                languageShort = "Und",
            };
            //DateTime start = DateTime.Now.AddMinutes(3);
            DateTime start = DateTime.Now.AddDays(2);
            DateTime stop = start.AddHours(1);
            tvHDvrAddEntryDto.setStartTime(start);
            tvHDvrAddEntryDto.setStopTime(stop);

            TvHResponseDto dto = tvhService.addEpgToDvr(tvHDvrAddEntryDto);
            //TvHServerInfoDto dto = tvhService.getServerInfo();
            Assert.IsNotNull(dto);
            Assert.IsNotNull(dto.successful);

            List<TvHDvrUpcomingDto> list = tvhService.getDvrUpcominglist();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count == 1);

            TvHDvrUpcomingDto removeTvHDvrUpcomingDto = list[0];
            TvHResponseDto dto2 = tvhService.removeUpcomingDvrEntry(removeTvHDvrUpcomingDto.uuid);
            Assert.IsNotNull(dto2);
            Assert.IsNotNull(dto2.successful);
        }

        [TestMethod]
        public void test_05_removeUpcomingDvrEntry()
        {
            // f1201dd1f1cb11e2fbf2ab01dc7965ca
            //TvHResponseDto dto = tvhService.removeUpcomingDvrEntry("1e0eaa01d561659d55e05ceb305332fe");  
            //Assert.IsNotNull(dto);
            //Assert.IsNotNull(dto.successful);
        }

        [TestMethod]
        public void test_06_getDvrUpcominglist()
        {
            //List<TvHDvrUpcomingDto> list = tvhService.getDvrUpcominglist();
            //Assert.IsNotNull(list);
            //Assert.IsTrue(list.Count > 0);
            
        }

    }
}