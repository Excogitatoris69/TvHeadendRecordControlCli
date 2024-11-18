using System.Data;
using System.Runtime;
using System.Security.Cryptography;
using System.Text;

namespace Credentials
{
    public class CredentialController
    {
        private readonly string credentialPath = "TvHeadendRecordControlCli";
        private readonly string credentialFile = "credentials.csv";
        private byte[] key;
        private byte[] iv;

        public CredentialController()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            _fullFilePath = Path.Combine(appData, credentialPath, credentialFile);
            initCrypto();
        }

        private string _fullFilePath;
        public string filePath {
            get
            {
                return _fullFilePath;
            }
        }

        public Result addOrUpdateCredential(CredentialEntity credentialEntity)
        {
            Result result = new Result();
            result.success = true;

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string dirPath = Path.Combine(appData, credentialPath);

            //Names are uppercase
            //credentialEntity.Credentialname = credentialEntity.Credentialname.ToUpper();

            try
            {
                //create path
                if (!Directory.Exists(dirPath)) 
                {
                    DirectoryInfo dirInfo =  Directory.CreateDirectory(dirPath);
                }
                if (Directory.Exists(dirPath))
                {
                    List<CredentialEntity> list = readCredentialfile(_fullFilePath);
                    if(list.Count == 0)
                    {//add
                        list.Add(credentialEntity);
                    }
                    else
                    {//update or add
                        CredentialEntity foundItem = null;
                        foreach (CredentialEntity item in list)
                        {
                            if(item.Credentialname.Equals(credentialEntity.Credentialname, StringComparison.OrdinalIgnoreCase))
                            {
                                foundItem = item; break;
                            }
                        }
                        if(foundItem != null)
                        {//update
                            foundItem.ServerUrl = credentialEntity.ServerUrl;
                            foundItem.UserName = credentialEntity.UserName;
                            foundItem.Password = credentialEntity.Password;
                        }
                        else
                        {//add
                            list.Add(credentialEntity);
                        }
                    }
                    writeCredentialfile(list, _fullFilePath);
                }
                else
                {
                    result.success = false;
                    result.errorMsg = "Could not create path " + dirPath;
                }

            }
            catch (Exception ex) 
            { 
                result.success = false;
                result.errorMsg = ex.Message;
            }


            return result;
        }

        /// <summary>
        /// Remove credential in list.
        /// </summary>
        /// <param name="credentialEntity"></param>
        /// <returns></returns>
        public Result removeCredential(CredentialEntity credentialEntity)
        {
            Result result = new Result();
            result.success = true;

            CredentialEntity foundCredentialEntity = null;
            try
            {
                List<CredentialEntity> list = readCredentialfile(_fullFilePath);
                foreach (CredentialEntity item in list)
                {
                    if (item.Credentialname.Equals(credentialEntity.Credentialname, StringComparison.OrdinalIgnoreCase))
                    {
                        foundCredentialEntity = item; break;
                    }
                }
                if (foundCredentialEntity != null)
                {
                    list.Remove(foundCredentialEntity);
                    try
                    {
                        writeCredentialfile(list, _fullFilePath);
                    }
                    catch (Exception ex)
                    {
                        result.success = false;
                        result.errorMsg = ex.Message;
                    }
                }
                else
                {
                    result.success = false;
                    result.errorMsg = "Credential does not exist.";
                }
            }
            catch (Exception ex)
            {
                result.success = false;
                result.errorMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// Get list of all credentials or empty list.
        /// </summary>
        /// <returns></returns>
        public List<CredentialEntity> getCredentialsList()
        {
            return readCredentialfile(_fullFilePath);
        }

        /// <summary>
        /// Get Credential entry by name or null.
        /// </summary>
        /// <param name="credentialName"></param>
        /// <returns></returns>
        public CredentialEntity getCredentialByName(string credentialName)
        {
            CredentialEntity credentialEntity = null;
            try
            {
                List<CredentialEntity> list = readCredentialfile(_fullFilePath);
                foreach (CredentialEntity item in list)
                {
                    if (item.Credentialname.Equals(credentialName, StringComparison.OrdinalIgnoreCase))
                    {
                        credentialEntity = item; break;
                    }
                }
            }
            catch (Exception ) { } //empty
            return credentialEntity;
        }

        //=================================================================
        //=================================================================

        private void initCrypto()
        {
            key = new byte[] {
                0x18, 0xA5, 0x74, 0x9D,
                0xF6, 0xC4, 0x89, 0xB2,
                0xD6, 0x6D, 0xB9, 0x7F,
                0x87, 0x59, 0xDB, 0xAC,
            };
            iv = new byte[] {
                0xF6, 0xC4, 0x89, 0xB2,
                0x18, 0xA5, 0x74, 0x9D,
                0x87, 0x59, 0xDB, 0xAC,
                0xD6, 0x6D, 0xB9, 0x7F,
            };
        }

        private List<CredentialEntity> readCredentialfile(string filePath)
        {
            List<CredentialEntity> result = new List<CredentialEntity>();
            if(!File.Exists(filePath)) return result;

            string line = null;
            CredentialEntity aCredentialEntity = null;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                while (!streamReader.EndOfStream)
                {
                    line = streamReader.ReadLine();
                    //"TVUSER";"http://ptvheadend:9981";"tvuser";"password123"
                    string[] csvTokens = line.Split(";");
                    aCredentialEntity = new CredentialEntity();
                    aCredentialEntity.Credentialname = cleanValue(csvTokens[0]);
                    aCredentialEntity.ServerUrl = cleanValue(csvTokens[1]);
                    aCredentialEntity.UserName = cleanValue(csvTokens[2]);
                    //if not encrypted read as plain text
                    try
                    {
                        aCredentialEntity.Password = decryptFromBase64(cleanValue(csvTokens[3]));
                    }
                    catch (Exception ex)
                    {
                        aCredentialEntity.Password = cleanValue(csvTokens[3]);
                    }

                    if (!result.Contains(aCredentialEntity))
                    {
                        result.Add(aCredentialEntity);
                    }
                }
            }
            return result;
        }

        private void writeCredentialfile(List<CredentialEntity> list, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (StreamWriter writer = new StreamWriter(filePath, false)) 
            {
                foreach (CredentialEntity aCredentialEntity in list) 
                {
                    sb.Clear();
                    sb.Append("\"");
                    sb.Append(aCredentialEntity.Credentialname);
                    sb.Append("\";\"");
                    sb.Append(aCredentialEntity.ServerUrl);
                    sb.Append("\";\"");
                    sb.Append(aCredentialEntity.UserName);
                    sb.Append("\";\"");
                    sb.Append(encryptToBase64(aCredentialEntity.Password));
                    sb.Append("\"");
                    writer.WriteLine(sb.ToString());
                }
            }    
        }

        /// <summary>
        /// Remove Quotes and spaces 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string cleanValue(string value)
        {
            return value.Replace('"', ' ').Trim();
        }

        private string encryptToBase64(string plainText)
        {
            byte[] cipheredtext;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        cipheredtext = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(cipheredtext);
        }

        private string decryptFromBase64(string encryptedTextBase64)
        {
            byte[] cipheredtext = Convert.FromBase64String(encryptedTextBase64);
            string plainText = String.Empty;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(key, iv);
                using (MemoryStream memoryStream = new MemoryStream(cipheredtext))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            plainText = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }


    }


    public class Result
    {
        public bool success { get; set; }
        public string errorMsg { get; set; }
    }

    public class CredentialEntity
    {
        public string Credentialname { get; set; }
        public string ServerUrl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is CredentialEntity entity &&
                   Credentialname == entity.Credentialname;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Credentialname);
        }
    }

}
