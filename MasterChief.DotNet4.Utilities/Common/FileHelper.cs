using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    ///     文件以及文件夹操作帮助类
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        ///     从路径中获取文件名称（包括后缀）
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns>文件名称（包括后缀）</returns>
        public static string GetFileName(string filePath)
        {
            return Path.GetFileName(filePath);
        }

        /// <summary>
        ///     创建文件路径
        ///     <para>eg:FileHelper.CreatePath(@"C:\aa\cc\dd\ee.xml");</para>
        /// </summary>
        /// <param name="path">需要创建的路径</param>
        /// <returns>是否创建成功</returns>
        public static bool CreatePath(string path)
        {
            var result = true;

            if (!string.IsNullOrEmpty(path) && !File.Exists(path))
                try
                {
                    var directory = Path.GetDirectoryName(path);
                    CreateDirectory(directory);
                    using (File.Create(path))
                    {
                    }
                }

                catch (Exception)
                {
                    result = false;
                }

            return result;
        }

        #region Fields

        /// <summary>
        ///     OF_READWRITE
        /// </summary>
        public const int OfReadwrite = 2;

        /// <summary>
        ///     OF_SHARE_DENY_NONE
        /// </summary>
        public const int OfShareDenyNone = 0x40;

        /// <summary>
        ///     路径分割符
        /// </summary>
        public const string PathSplitChar = "\\";

        /// <summary>
        ///     HFILE_ERROR
        /// </summary>
        public static readonly IntPtr HfileError = new IntPtr(-1);

        /// <summary>
        ///     从路径中获取文件名称（不包括后缀）
        ///     <para>eg:FileHelper.GetFileNameOnly(@"C:\yanzhiwei.docx");==>yanzhiwei</para>
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns>文件名称（不包括后缀）</returns>
        public static string GetFileNameOnly(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }

        #endregion Fields

        #region Methods

        /// <summary>
        ///     修改文件路径后缀名
        ///     <para>eg:FileHelper.CreateTempPath("jpg");</para>
        /// </summary>
        /// <param name="extension">文件后缀;eg:xml</param>
        /// <returns>临时文件路径</returns>
        public static string ChangeFileType(string extension)
        {
            var path = Path.GetTempFileName();
            return Path.ChangeExtension(path, extension);
        }

        /// <summary>
        ///     验证格式
        /// </summary>
        /// <param name="allType">所有格式;eg .jpeg|.jpg|.bmp|.gif|.png</param>
        /// <param name="chkType">被检查的格式</param>
        /// <returns>是否是符合的文件格式</returns>
        public static bool CheckValidExt(string allType, string chkType)
        {
            var flag = false;
            var data = allType.Split('|');

            foreach (var temp in data)
                if (string.Equals(temp, chkType, StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                    break;
                }

            return flag;
        }

        /// <summary>
        ///     复制指定目录的所有文件,不包含子目录及子目录中的文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,表示覆盖同名文件,否则不覆盖</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite)
        {
            CopyFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        ///     复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //复制当前目录文件
            foreach (var sourceFile in Directory.GetFiles(sourceDir))
            {
                var targetFile = Path.Combine(targetDir,
                    sourceFile.Substring(
                        sourceFile.LastIndexOf(PathSplitChar, StringComparison.OrdinalIgnoreCase) + 1));

                if (File.Exists(targetFile))
                {
                    if (overWrite)
                    {
                        File.SetAttributes(targetFile, FileAttributes.Normal);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        File.Copy(sourceFile, targetFile, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFile, targetFile, overWrite);
                }
            }

            //复制子目录
            if (copySubDir)
                foreach (var sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    var targetSubDir = Path.Combine(targetDir,
                        sourceSubDir.Substring(sourceSubDir.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));

                    if (!Directory.Exists(targetSubDir)) Directory.CreateDirectory(targetSubDir);

                    CopyFiles(sourceSubDir, targetSubDir, overWrite, true);
                }
        }

        /// <summary>
        ///     复制指定目录的所有文件，并把复制的文件备份到备份目录中
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        /// <param name="backDir">备份目录</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir,
            string backDir)
        {
            if (!Directory.Exists(backDir)) Directory.CreateDirectory(backDir);

            //复制当前目录文件
            foreach (var sourceFile in Directory.GetFiles(sourceDir))
            {
                var targetFile = Path.Combine(targetDir,
                    sourceFile.Substring(
                        sourceFile.LastIndexOf(PathSplitChar, StringComparison.OrdinalIgnoreCase) + 1));

                if (File.Exists(targetFile))
                {
                    if (overWrite)
                    {
                        File.SetAttributes(targetFile, FileAttributes.Normal);
                        var backFileName = Path.Combine(backDir,
                            sourceFile.Substring(sourceFile.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));
                        File.Copy(targetFile, backFileName, true);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        File.Copy(sourceFile, targetFile, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFile, targetFile, overWrite);
                }
            }

            //复制子目录
            if (copySubDir)
                foreach (var sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    var targetSubDir = Path.Combine(targetDir,
                        sourceSubDir.Substring(sourceSubDir.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));

                    if (!Directory.Exists(targetSubDir)) Directory.CreateDirectory(targetSubDir);

                    var backSubDir = Path.Combine(backDir,
                        targetSubDir.Substring(targetSubDir.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));

                    if (!Directory.Exists(backSubDir)) Directory.CreateDirectory(backSubDir);

                    CopyFiles(sourceSubDir, targetSubDir, overWrite, true, backSubDir);
                }
        }

        /// <summary>
        ///     复制本机大文件
        ///     <para>
        ///         eg:FileHelper.CopyLocalBigFile(@"C:\Users\YanZh_000\Downloads\TheInterview.mp4", @"D:\The
        ///         Interview(1080p).mp4", 1024 * 1024 * 5))
        ///     </para>
        /// </summary>
        /// <param name="fromPath">源文件的路径</param>
        /// <param name="toPath">文件保存的路径</param>
        /// <param name="eachReadLength">每次读取的长度</param>
        /// <returns>是否复制成功</returns>
        public static bool CopyLargeFile(string fromPath, string toPath, int eachReadLength)
        {
            /*
             * 知识：
             * FileStream缓冲读取和写入可以提高性能。FileStream读取文件的时候，是先讲流放入内存，经Flash()方法后将内存中（缓冲中）的数据写入件。如果文件非常大，势必消耗性能。
             *
             * 参考
             * 1：http://www.cnblogs.com/zfanlong1314/p/3922803.html
             */
            var copyResult = true;
            //将源文件 读取成文件流
            var fileStream = new FileStream(fromPath, FileMode.Open, FileAccess.Read);
            //已追加的方式 写入文件流
            var appendFile = new FileStream(toPath, FileMode.Append, FileAccess.Write);

            try
            {
                //实际读取的文件长度
                int readFileCount;

                //如果每次读取的长度小于 源文件的长度 分段读取
                if (eachReadLength < fileStream.Length)
                {
                    var buffer = new byte[eachReadLength];
                    long copyCnt = 0;

                    while (copyCnt <= fileStream.Length - eachReadLength)
                    {
                        readFileCount = fileStream.Read(buffer, 0, eachReadLength);
                        fileStream.Flush();
                        appendFile.Write(buffer, 0, eachReadLength);
                        appendFile.Flush();
                        //流的当前位置
                        appendFile.Position = fileStream.Position;
                        copyCnt += readFileCount;
                    }

                    var leftCount = (int) (fileStream.Length - copyCnt);
                    fileStream.Read(buffer, 0, leftCount);
                    fileStream.Flush();
                    appendFile.Write(buffer, 0, leftCount);
                    appendFile.Flush();
                }
                else
                {
                    //如果每次拷贝的文件长度大于源文件的长度 则将实际文件长度直接拷贝
                    var buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);
                    fileStream.Flush();
                    appendFile.Write(buffer, 0, buffer.Length);
                    appendFile.Flush();
                }
            }
            catch (Exception)
            {
                copyResult = false;
            }
            finally
            {
                fileStream.Close();
                appendFile.Close();
            }

            return copyResult;
        }

        /// <summary>
        ///     文件复制备份【同目录下】
        ///     <para>eg:FileHelper.CopyToBak(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static bool CopyToBak(string filePath)
        {
            bool result;

            try
            {
                var fileCount = 0;
                string bakName;

                do
                {
                    fileCount++;
                    bakName = $"{filePath}.{fileCount}.bak";
                } while (File.Exists(bakName));

                File.Copy(filePath, bakName);
                File.Delete(filePath);
                result = true;
            }
            catch (Exception)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        ///     创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public static void CreateDirectory(string targetDir)
        {
            var directoryInfo = new DirectoryInfo(targetDir);

            if (!directoryInfo.Exists) directoryInfo.Create();
        }

        /// <summary>
        ///     建立子目录
        /// </summary>
        /// <param name="parentDir">目录路径</param>
        /// <param name="subDirName">子目录名称</param>
        public static void CreateDirectory(string parentDir, string subDirName)
        {
            CreateDirectory(parentDir + PathSplitChar + subDirName);
        }


        /// <summary>
        ///     根据现有路径创建临时文件路径
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        /// 时间：2015-12-17 14:13
        /// 备注：
        public static string CreateTempFilePath(this string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            var sourceFileTemp = Path.Combine(fileInfo.DirectoryName ?? throw new InvalidOperationException(),
                Guid.NewGuid() + fileInfo.Extension);
            fileInfo.CopyTo(sourceFileTemp);
            return sourceFileTemp;
        }

        /// <summary>
        ///     删除指定目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteDirectory(string targetDir)
        {
            var dirInfo = new DirectoryInfo(targetDir);

            if (dirInfo.Exists)
            {
                DeleteFiles(targetDir, true);
                dirInfo.Delete(true);
            }
        }

        /// <summary>
        ///     删除指定目录的所有文件，不包含子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        public static void DeleteFiles(string targetDir)
        {
            DeleteFiles(targetDir, false);
        }

        /// <summary>
        ///     删除指定目录的所有文件和子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        /// <param name="delSubDir">如果为true,包含对子目录的操作</param>
        public static void DeleteFiles(string targetDir, bool delSubDir)
        {
            foreach (var fileName in Directory.GetFiles(targetDir))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }

            if (delSubDir)
            {
                var dir = new DirectoryInfo(targetDir);

                foreach (var subDi in dir.GetDirectories())
                {
                    DeleteFiles(subDi.FullName, true);
                    subDi.Delete();
                }
            }
        }

        /// <summary>
        ///     删除指定目录的所有子目录,不包括对当前目录文件的删除
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteSubDirectory(string targetDir)
        {
            foreach (var subDir in Directory.GetDirectories(targetDir)) DeleteDirectory(subDir);
        }

        /// <summary>
        ///     文件是否被占用
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>是否被占用</returns>
        public static bool FileIsTake(string fileName)
        {
            if (!File.Exists(fileName)) return false; //文件不存在

            var intPtr = _lopen(fileName, OfReadwrite | OfShareDenyNone);

            if (intPtr == HfileError) return true; //文件被占用！

            CloseHandle(intPtr);
            return false;
        }


        /// <summary>
        ///     获取文件大小—kb
        ///     <para>eg:FileHelper.GetKBSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小_kb</returns>
        public static double GetKbSize(string filePath)
        {
            double kb = 0;
            var size = GetSize(filePath);

            if (size != 0) kb = size / 1024d;

            return kb;
        }

        /// <summary>
        ///     获取文件大小—mb
        ///     <para>eg:FileHelper.GetMBSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小_mb</returns>
        public static double GetMbSize(string filePath)
        {
            double mb = 0;
            var size = GetSize(filePath);

            if (size != 0) mb = size / 1048576d; //1024*1024==1048576;

            return mb;
        }

        /// <summary>
        ///     获取文件大小—字节
        ///     <para>eg:FileHelper.GetSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小</returns>
        public static long GetSize(string filePath)
        {
            var buffer = ReadFile(filePath);
            return buffer == null ? 0 : buffer.Length;
        }

        /// <summary>
        ///     获取可用的文件名称
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回可用的用户名称</returns>
        public static string GetValidFileName(string fileName)
        {
            return Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "").Replace(" ", string.Empty);
        }

        /// <summary>
        ///     剪切指定目录的所有文件,不包含子目录
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite)
        {
            MoveFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        ///     剪切指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="moveSubDir">如果为true,包含目录,否则不包含</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite, bool moveSubDir)
        {
            //移动当前目录文件
            foreach (var sourceFile in Directory.GetFiles(sourceDir))
            {
                var targetFileName = Path.Combine(targetDir,
                    sourceFile.Substring(sourceFile.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Delete(targetFileName);
                        File.Move(sourceFile, targetFileName);
                    }
                }
                else
                {
                    File.Move(sourceFile, targetFileName);
                }
            }

            if (moveSubDir)
                foreach (var sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    var targetSubDir = Path.Combine(targetDir,
                        sourceSubDir.Substring(sourceSubDir.LastIndexOf(PathSplitChar, StringComparison.Ordinal) + 1));

                    if (!Directory.Exists(targetSubDir)) Directory.CreateDirectory(targetSubDir);

                    MoveFiles(sourceSubDir, targetSubDir, overWrite, true);
                    Directory.Delete(sourceSubDir);
                }
        }

        /// <summary>
        ///     打开文件或者文件夹
        /// </summary>
        /// <param name="path">文件夹或者文件的路径</param>
        public static void OpenFolderAndFile(string path)
        {
            Process.Start(path);
        }

        /// <summary>
        ///     将文件转换成二进制流
        ///     <para>eg:FileHelper.ReadFile(@"C:\demo.txt");</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>二进制数组</returns>
        public static byte[] ReadFile(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var buffur = new byte[stream.Length];
                stream.Read(buffur, 0, (int) stream.Length);
                return buffur;
            }
        }

        /// <summary>
        ///     递归获取文件夹目录下文件
        /// </summary>
        /// <param name="pathName">需要递归遍历的文件夹</param>
        /// <param name="fileHanlder">遍历规则『委托』</param>
        public static void RecursiveFolder(string pathName, Action<FileInfo> fileHanlder)
        {
            var fileQueue = new Queue<string>();
            fileQueue.Enqueue(pathName);

            while (fileQueue.Count > 0)
            {
                var path = fileQueue.Dequeue();
                var pathSecurity = new DirectorySecurity(path, AccessControlSections.Access);

                if (!pathSecurity.AreAccessRulesProtected) //文件夹权限是否可访问
                {
                    var directoryInfo = new DirectoryInfo(path);

                    foreach (var diChild in directoryInfo.GetDirectories()) fileQueue.Enqueue(diChild.FullName);

                    foreach (var file in directoryInfo.GetFiles()) fileHanlder(file);
                }
            }
        }

        /// <summary>
        ///     将byte[]导出到文件
        ///     <para>eg: FileHelper.SaveFile(_bytes, _outputFilePath); </para>
        /// </summary>
        /// <param name="data">文件流</param>
        /// <param name="filePath">存储路径</param>
        public static void SaveFile(byte[] data, string filePath)
        {
            File.WriteAllBytes(filePath, data);
        }


        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll")]
        private static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        #endregion Methods
    }
}