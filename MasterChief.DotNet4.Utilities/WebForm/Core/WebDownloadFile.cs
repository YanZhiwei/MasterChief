using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using MasterChief.DotNet4.Utilities.Operator;
using MasterChief.DotNet4.Utilities.Result;

namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    /// <summary>
    ///     文件下载
    /// </summary>
    public class WebDownloadFile
    {
        #region Methods

        /*
         *http协议从1.1开始支持获取文件的部分内容，这为并行下载以及断点续传提供了技术支持。
         *它通过在Header里两个参数实现的，客户端发请求时对应的是 Range ，服务器端响应时对应的是 Content-Range ；
         *Range 参数还支持多个区间，用逗号分隔，下面对另一个内容为”hello world”的文件”a.html”多区间请求，
         * 这时response的 Content-Type 不再是原文件mime类型，而用一种 multipart/byteranges 类型表示
         */


        /// <summary>
        ///     输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">带文件名下载路径</param>
        /// <param name="limitSpeed">每秒允许下载的字节数</param>
        public static FileDownloadResult FileDownload(HttpContext context, string fileName, string filePhysicsPath, ulong limitSpeed)
        {
            var checkedDownFile = CheckedFileDownload(fileName, filePhysicsPath);

            if (!checkedDownFile.State)
                return FileDownloadResult.Fail(fileName, filePhysicsPath, checkedDownFile.Message);

            try
            {
                using (var fileStream =
                    new FileStream(filePhysicsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var fileReader = new BinaryReader(fileStream))
                    {
                        context.Response.AddHeader("Accept-Ranges", "bytes");
                        context.Response.Buffer = false;
                        long fileLength = fileStream.Length,
                            startIndex = 0;
                        var pack = 10240; //10K bytes
                        // int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        // ReSharper disable once PossibleLossOfFraction
                        var sleep = (int)Math.Floor((double)((ulong)(1000 * pack) / limitSpeed)) + 1;

                        if (context.Request.Headers["Range"] != null)
                        {
                            context.Response.StatusCode = 206;
                            var buffer = context.Request.Headers["Range"].Split('=', '-');
                            startIndex = Convert.ToInt64(buffer[1]);
                        }

                        context.Response.AddHeader("Content-Length", (fileLength - startIndex).ToString());

                        if (startIndex != 0)
                            context.Response.AddHeader("Content-Range",
                                $" bytes {startIndex}-{fileLength - 1}/{fileLength}");

                        context.Response.AddHeader("Connection", "Keep-Alive");
                        context.Response.ContentType = MimeTypes.ApplicationOctetStream;
                        context.Response.AddHeader("Content-Disposition",
                            "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                        fileReader.BaseStream.Seek(startIndex, SeekOrigin.Begin);
                        // ReSharper disable once PossibleLossOfFraction
                        var maxCount = (int)Math.Floor((double)((fileLength - startIndex) / pack)) + 1;

                        for (var i = 0; i < maxCount; i++)
                        {
                            if (context.Response.IsClientConnected)
                            {
                                context.Response.BinaryWrite(fileReader.ReadBytes(pack));
                                Thread.Sleep(sleep);
                            }
                            else
                            {
                                i = maxCount;
                            }
                        }
                    }
                }

                return FileDownloadResult.Success(fileName, filePhysicsPath);
            }
            catch (Exception ex)
            {
                return FileDownloadResult.Fail(fileName, filePhysicsPath, ex.Message);
            }
        }

        /// <summary>
        ///     分块下载
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">文件物理路径</param>
        /// <returns>下载是否成功</returns>
        public static void FileDownload(HttpContext context, string fileName, string filePhysicsPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(fileName, "下载文件名").CheckFileExists(filePhysicsPath);
            var filePath = filePhysicsPath;
            long chunkSize = 204800; //块大小
            var buffer = new byte[chunkSize]; //200K的缓冲区
            fileName = string.IsNullOrEmpty(fileName) ? Path.GetFileName(filePath) : fileName;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var dataToRead = fileStream.Length; //已读的字节数
                context.Response.ContentType = MimeTypes.ApplicationOctetStream;
                context.Response.AddHeader("Content-Disposition",
                    "attachement;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                context.Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                {
                    if (context.Response.IsClientConnected)
                    {
                        var length = fileStream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        context.Response.OutputStream.Write(buffer, 0, length);
                        context.Response.Flush();
                        context.Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        dataToRead = -1;
                    }
                }
            }
        }

        private static CheckResult CheckedFileDownload(string fileName, string filePhysicsPath)
        {
            if (string.IsNullOrEmpty(fileName)) return CheckResult.Fail("下载文件名称不能为空。");

            if (!File.Exists(filePhysicsPath)) return CheckResult.Fail("下载文件路径不合法或者文件不实际存在。");

            return CheckResult.Success();
        }

        #endregion Methods
    }
}