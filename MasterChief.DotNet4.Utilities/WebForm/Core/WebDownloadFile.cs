namespace MasterChief.DotNet4.Utilities.WebForm.Core
{
    using MasterChief.DotNet4.Utilities.Common;
    using MasterChief.DotNet4.Utilities.Operator;
    using MasterChief.DotNet4.Utilities.Result;
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// 文件下载
    /// </summary>
    public class WebDownloadFile
    {
        #region Methods

        //http协议从1.1开始支持获取文件的部分内容，这为并行下载以及断点续传提供了技术支持。
        //它通过在Header里两个参数实现的，客户端发请求时对应的是 Range ，服务器端响应时对应的是 Content-Range ；
        //Range 参数还支持多个区间，用逗号分隔，下面对另一个内容为”hello world”的文件”a.html”多区间请求，
        //这时response的 Content-Type 不再是原文件mime类型，而用一种 multipart/byteranges 类型表示
        /// <summary>
        ///  输出硬盘文件，提供下载 支持大文件、续传、速度限制、资源占用小
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">带文件名下载路径</param>
        /// <param name="limitSpeed">每秒允许下载的字节数</param>
        public static FileDownloadResult FileDownload(string fileName, string filePhysicsPath, ulong limitSpeed)
        {
            CheckResult checkedDownFile = CheckedFileDownload(fileName, filePhysicsPath);

            if (!checkedDownFile.State)
            {
                return FileDownloadResult.Fail(fileName, filePhysicsPath, checkedDownFile.Message);
            }

            try
            {
                using (FileStream fileStream = new FileStream(filePhysicsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader fileReader = new BinaryReader(fileStream))
                    {
                        HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                        HttpContext.Current.Response.Buffer = false;
                        long fileLength = fileStream.Length,
                             startIndex = 0;
                        int pack = 10240; //10K bytes
                        // int sleep = 200;   //每秒5次   即5*10K bytes每秒
                        int sleep = (int)Math.Floor((double)((ulong)(1000 * pack) / limitSpeed)) + 1;

                        if (HttpContext.Current.Request.Headers["Range"] != null)
                        {
                            HttpContext.Current.Response.StatusCode = 206;
                            string[] buffer = HttpContext.Current.Request.Headers["Range"].Split(new char[] { '=', '-' });
                            startIndex = Convert.ToInt64(buffer[1]);
                        }

                        HttpContext.Current.Response.AddHeader("Content-Length", (fileLength - startIndex).ToString());

                        if (startIndex != 0)
                        {
                            HttpContext.Current.Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startIndex, fileLength - 1, fileLength));
                        }

                        HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                        HttpContext.Current.Response.ContentType = MimeTypes.ApplicationOctetStream;
                        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                        fileReader.BaseStream.Seek(startIndex, SeekOrigin.Begin);
                        int maxCount = (int)Math.Floor((double)((fileLength - startIndex) / pack)) + 1;

                        for (int i = 0; i < maxCount; i++)
                        {
                            if (HttpContext.Current.Response.IsClientConnected)
                            {
                                HttpContext.Current.Response.BinaryWrite(fileReader.ReadBytes(pack));
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
        /// 分块下载
        /// </summary>
        /// <param name="fileName">下载文件名</param>
        /// <param name="filePhysicsPath">文件物理路径</param>
        /// <returns>下载是否成功</returns>
        public static void FileDownload(string fileName, string filePhysicsPath)
        {
            ValidateOperator.Begin().NotNullOrEmpty(fileName, "下载文件名").CheckFileExists(filePhysicsPath);
            string filePath = filePhysicsPath;
            long chunkSize = 204800;             //块大小
            byte[] buffer = new byte[chunkSize]; //200K的缓冲区
            long dataToRead = 0;                 //已读的字节数
            fileName = string.IsNullOrEmpty(fileName) == true ? Path.GetFileName(filePath) : fileName;

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                dataToRead = fileStream.Length;
                HttpContext.Current.Response.ContentType = MimeTypes.ApplicationOctetStream;
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachement;filename=" + HttpUtility.UrlEncode(fileName, Encoding.UTF8));
                HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                {
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        int length = fileStream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
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
            if (string.IsNullOrEmpty(fileName))
            {
                return CheckResult.Fail("下载文件名称不能为空。");
            }

            if (!CheckHelper.IsFilePath(filePhysicsPath) || !File.Exists(filePhysicsPath))
            {
                return CheckResult.Fail("下载文件路径不合法或者文件不实际存在。");
            }

            return CheckResult.Success();
        }

        #endregion Methods
    }
}