using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MasterChief.DotNet4.Utilities.Operator;

namespace MasterChief.DotNet.Infrastructure.Zip
{
    /// <summary>
    ///     Zip 压缩与解压辅助类
    /// </summary>
    public sealed class ZipHelper
    {
        #region Methods

        /// <summary>
        ///     压缩文件夹
        /// </summary>
        /// <param name="compressFolder">需要压缩的文件夹</param>
        /// <param name="zipFile">压缩文件存放路径</param>
        /// <param name="compressionLevel">压缩级别</param>
        /// <param name="password">压缩密码</param>
        public static void Compress(string compressFolder, string zipFile, int compressionLevel = 9,
            string password = null)
        {
            ValidateOperator.Begin()
                .CheckDirectoryExist(compressFolder)
                .NotNullOrEmpty(zipFile, "压缩文件存放路径")
                .IsFilePath(zipFile)
                .CheckedFileExt(Path.GetExtension(zipFile), ".zip");
            CreateZipFolder(zipFile);
            var compressFiles = GetCompressFiles(compressFolder);

            using (var zipOutput = new ZipOutputStream(File.Create(zipFile)))
            {
                zipOutput.SetLevel(compressionLevel);
                zipOutput.Password = password;
                var buffer = new byte[4096];

                foreach (var item in compressFiles)
                {
                    var isFilePath = File.Exists(item);
                    var zipName = item.Replace(compressFolder, "");
                    zipName = isFilePath ? zipName : string.Format($"{zipName}/");
                    var fileEntry = new ZipEntry(zipName)
                    {
                        DateTime = DateTime.Now
                    };
                    zipOutput.PutNextEntry(fileEntry);
                    if (!isFilePath) continue;
                    using (var fileStream = File.OpenRead(item))
                    {
                        int sourceBytes;

                        do
                        {
                            sourceBytes = fileStream.Read(buffer, 0, buffer.Length);
                            zipOutput.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }
            }
        }

        /// <summary>
        ///     解压文件
        /// </summary>
        /// <param name="zipFile">zip文件</param>
        /// <param name="extractFolder">解压文件夹</param>
        /// <param name="password">压缩密码</param>
        public static void Extract(string zipFile, string extractFolder, string password = null)
        {
            ValidateOperator.Begin()
                .CheckFileExists(zipFile)
                .CheckedFileExt(Path.GetExtension(zipFile), ".zip")
                .NotNullOrEmpty(extractFolder, "解压文件夹");
            CreateExtractFolder(extractFolder);
            ZipFile file = null;
            try
            {
                var fileStream = File.OpenRead(zipFile);
                file = new ZipFile(fileStream);

                if (!string.IsNullOrEmpty(password)) file.Password = password;

                foreach (ZipEntry zipEntry in file)
                {
                    if (!zipEntry.IsFile) continue;

                    var extractFileName = zipEntry.Name;

                    var buffer = new byte[4096];
                    var zipStream = file.GetInputStream(zipEntry);

                    var fullZipPath = CreateExtractPath(extractFolder, extractFileName);
                    var folderName = Path.GetDirectoryName(fullZipPath);

                    if (!Directory.Exists(folderName))
                        Directory.CreateDirectory(folderName);

                    using (var streamWriter = File.Create(fullZipPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (file != null)
                {
                    file.IsStreamOwner = true;
                    file.Close();
                }
            }
        }

        private static string CreateExtractPath(string extractFolder, string extractFile)
        {
            var index = extractFile.IndexOf(@"\", StringComparison.Ordinal);
            if (index >= 0)
                extractFile = extractFile.Substring(index + 1);
            return Path.Combine(extractFolder, extractFile);
        }

        private static void CreateExtractFolder(string extractFolder)
        {
            if (!Directory.Exists(extractFolder))
                Directory.CreateDirectory(extractFolder);
        }

        private static void CreateZipFolder(string zipFile)
        {
            var folderName = new FileInfo(zipFile).DirectoryName;
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);
        }

        private static List<string> GetCompressFiles(string compressFolder)
        {
            var compressFiles = new List<string>();
            compressFiles.AddRange(Directory.EnumerateDirectories(
                compressFolder, "*.*", SearchOption.AllDirectories));
            compressFiles.AddRange(Directory.EnumerateFiles(
                compressFolder, "*.*", SearchOption.AllDirectories)
            );

            return compressFiles;
        }

        #endregion Methods
    }
}