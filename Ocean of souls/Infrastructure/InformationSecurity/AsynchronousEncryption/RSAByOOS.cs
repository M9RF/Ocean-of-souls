using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Ocean_of_souls.Infrastructure.InformationSecurity.AsynchronousEncryption
{
    class RSAByOOS
    {
        public string EncryptString { get; set; }

        public string DecryptString { get; set; }

        public string PublicKeyString { get; set; }

        public RSAByOOS(string message)
        {
            var cryptoServiceProvider = new RSACryptoServiceProvider(2048);
            cryptoServiceProvider.FromXmlString(GetPrivateKey());
            var publicKey = cryptoServiceProvider.ExportParameters(false);

            EncryptString = Encrypt(message, GetKeyString(publicKey));
            PublicKeyString = GetKeyString(publicKey);
        }

        public RSAByOOS(string ecnryptString, string publicKeyString)
        {
            var cryptoServiceProvider = new RSACryptoServiceProvider(2048);
            cryptoServiceProvider.FromXmlString(GetPrivateKey());
            var publicKey = cryptoServiceProvider.ExportParameters(false);

            DecryptString = Decrypt(ecnryptString, GetPrivateKey());
        }

        public string GetPrivateKey()
        {
            var text = "";
            using (StreamReader reader = new StreamReader("PRIVATEKEY.txt"))
            {
                text = reader.ReadToEnd();
            }

            return text;
        }

        public string GetKeyString(RSAParameters publicKey)
        {

            var stringWriter = new System.IO.StringWriter();
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
            xmlSerializer.Serialize(stringWriter, publicKey);
            return stringWriter.ToString();
        }

        public string Encrypt(string textToEncrypt, string publicKeyString)
        {
            var bytesToEncrypt = Encoding.UTF8.GetBytes(textToEncrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(publicKeyString.ToString());
                    var encryptedData = rsa.Encrypt(bytesToEncrypt, true);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    return base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }

        public string Decrypt(string textToDecrypt, string privateKeyString)
        {
            var bytesToDescrypt = Encoding.UTF8.GetBytes(textToDecrypt);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    rsa.FromXmlString(privateKeyString);

                    var resultBytes = Convert.FromBase64String(textToDecrypt);
                    var decryptedBytes = rsa.Decrypt(resultBytes, true);
                    var decryptedData = Encoding.UTF8.GetString(decryptedBytes);
                    return decryptedData.ToString();
                }

                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
