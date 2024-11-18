using CliTvhRecControlDomain.Dto;
using Credentials;
using System.Text;

namespace CliTvhRecControlTest
{

    [TestClass]
    public class UnitTest3_MainConsoleApp
    {
        TvHConnectionDataDto connectionData = null;
        Credentials.MainConsoleApp app = null; 
        private StringBuilder ConsoleOutput { get; set; }

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
            app = new Credentials.MainConsoleApp();
            app.offlineTest = true;
            ConsoleOutput = new StringBuilder();
            Console.SetOut(new StringWriter(this.ConsoleOutput));    // Associate StringBuilder with StdOut
            this.ConsoleOutput.Clear();    // Clear text from any previous text runs
        }


        [TestMethod]
        public void test_01_serverinfo()
        {
            string[] args = {
                "--serverurl=http://ptvheadend:9981"
                ,"--username=tvuser"
                ,"--password=tvuser"
                ,"--command=serverinfo"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex) 
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void test_02_channellist()
        {
            string[] args = {
                "--serverurl=http://ptvheadend:9981"
                ,"--username=tvuser"
                ,"--password=tvuser"
                ,"--command=channellist"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void test_03_dvrprofilelist()
        {
            string[] args = {
                "--serverurl=http://ptvheadend:9981"
                ,"--username=tvuser"
                ,"--password=tvuser"
                ,"--command=dvrprofilelist"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void test_04_addCredential()
        {
            string[] args = {
                "--serverurl=http://ptvheadend:9981"
                ,"--username=tvuser"
                ,"--password=tvuser"
                ,"--command=savecredential"
                ,"--credentialname=tvuser"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void test_05_listCredential()
        {
            string[] args = {
                "--command=credentiallist"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }

        [TestMethod]
        public void test_06_changeCredential()
        {
            string[] args = {
                "--serverurl=http://ptvheadend:9981"
                ,"--username=tvuser"
                ,"--password=tvuser"
                ,"--command=savecredential"
                ,"--credentialname=tvuser"
            };
            try
            {
                //app.executeMain(args);
                //Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
                args[1] = "--username=tvuser2";
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }


        [TestMethod]
        public void test_07_removeCredential()
        {
            string[] args = {
                "--command=removecredential"
                ,"--credentialname=tvuser"
            };
            try
            {
                app.executeMain(args);
                Assert.IsTrue(this.ConsoleOutput.ToString().Contains("Successful"));
            }
            catch (Exception ex)
            {
                Assert.IsTrue(false);
            }
        }


    }
}