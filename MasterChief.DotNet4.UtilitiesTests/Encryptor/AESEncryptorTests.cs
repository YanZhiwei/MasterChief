using System.Security.Cryptography;
using MasterChief.DotNet4.Utilities.Common;
using MasterChief.DotNet4.Utilities.Encryptor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MasterChief.DotNet4.UtilitiesTests.Encryptor
{
    [TestClass()]
    public class AESEncryptorTests
    {
        private AESEncryptor _fileEncryptor = null;

        [TestInitialize]
        public void Init()
        {
            Aes aes = AESEncryptor.CreateAES("DotnetDownloadConfig");
            byte[] iv = ByteHelper.ParseHexString("0102030405060708090a0a0c0d010208");
            _fileEncryptor = new AESEncryptor(aes.Key, iv);
        }

        [TestMethod()]
        public void DecryptTest()
        {
            string actual = _fileEncryptor.Decrypt("Eg6BUavJgIpZjY+qAZIcxA==");
            Assert.AreEqual("Thunder.zip", actual);
        }

        [TestMethod()]
        public void EncryptTest()
        {
            string actual = _fileEncryptor.Encrypt("Thunder.zip");
            Assert.AreEqual("Eg6BUavJgIpZjY+qAZIcxA==", actual);
        }
    }
}