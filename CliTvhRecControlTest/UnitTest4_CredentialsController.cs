using CliTvhRecControlDomain.Dto;
using Credentials;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Text;

namespace CliTvhRecControlTest
{

    [TestClass]
    public class UnitTest4_CredentialsController
    {
        CredentialController credentialController = null;



        [TestInitialize]
        public void initTest()
        {
            credentialController = new CredentialController();
        }


        [TestMethod]
        public void test_01_addOrUpdateCredential()
        {
            CredentialEntity credentialEntity = new CredentialEntity
            {
                Credentialname = "tvuser",
                ServerUrl = "http://ptvheadend:9981",
                UserName = "tvuser",
                Password = "tvuser"
            };
            Result result = credentialController.addOrUpdateCredential(credentialEntity);
            Assert.IsTrue(result.success);
            credentialEntity.UserName = "tvuserNew";
            result = credentialController.addOrUpdateCredential(credentialEntity);
            Assert.IsTrue(result.success);

            credentialEntity.Credentialname = "tvuser2";
            credentialEntity.UserName = "tvuser2";
            result = credentialController.addOrUpdateCredential(credentialEntity);
            Assert.IsTrue(result.success);
        }

        [TestMethod]
        public void test_02_listCredential()
        {
            string path = credentialController.filePath;
            List<CredentialEntity> result = credentialController.getCredentialsList();
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0 );
        }

        [TestMethod]
        public void test_03_getCredentialByName()
        {
            CredentialEntity result = credentialController.getCredentialByName("tvuser");
            Assert.IsNotNull(result);
            
        }


        [TestMethod]
        public void test_04_removeCredential()
        {
            CredentialEntity credentialEntity = new CredentialEntity
            {
                Credentialname = "tvuser",
                ServerUrl = "http://ptvheadend2:9981",
                UserName = "tvuser",
                Password = "tvuser"
            };
            Result result = credentialController.removeCredential(credentialEntity);
            Assert.IsTrue(result.success);
        }


            /* 
        [TestMethod]
        public void test_05_crypto()
        {
            byte[] key = new byte[] { 
                0x18, 0xA5, 0x74, 0x9D,
                0xF6, 0xC4, 0x89, 0xB2,
                0xD6, 0x6D, 0xB9, 0x7F,
                0x87, 0x59, 0xDB, 0xAC,
            };
            byte[] iv = new byte[] {
                0xF6, 0xC4, 0x89, 0xB2,
                0x18, 0xA5, 0x74, 0x9D,
                0x87, 0x59, 0xDB, 0xAC,
                0xD6, 0x6D, 0xB9, 0x7F,
            };

            //set  private --> public
            string plainText = "Hallo Welt";
            string encStr = credentialController.EncryptToBase64(plainText);
            Assert.IsNotNull(encStr);
            string decStr = credentialController.DecryptFromBase64(encStr);
            Assert.IsNotNull(decStr);
            Assert.IsTrue(decStr.Equals(plainText));
            
        }
            */

    }
}