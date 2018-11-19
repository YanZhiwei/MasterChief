using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace MasterChief.DotNet4.Utilities.Common
{
    /// <summary>
    /// 文件以及文件夹操作帮助类
    /// </summary>
    public static class FileHelper
    {
        #region Fields

        /// <summary>
        /// OF_READWRITE
        /// </summary>
        public const int OF_READWRITE = 2;

        /// <summary>
        /// OF_SHARE_DENY_NONE
        /// </summary>
        public const int OF_SHARE_DENY_NONE = 0x40;

        /// <summary>
        /// 路径分割符
        /// </summary>
        public const string PATH_SPLIT_CHAR = "\\";

        /// <summary>
        /// HFILE_ERROR
        /// </summary>
        public static readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        #endregion Fields

        #region Methods

        /// <summary>
        /// 修改文件路径后缀名
        /// <para>eg:FileHelper.CreateTempPath("jpg");</para>
        /// </summary>
        /// <param name="extension">文件后缀;eg:xml</param>
        /// <returns>临时文件路径</returns>
        public static string ChangeFileType(string extension)
        {
            string path = Path.GetTempFileName();
            return Path.ChangeExtension(path, extension);
        }

        /// <summary>
        /// 验证格式
        /// </summary>
        /// <param name="allType">所有格式;eg .jpeg|.jpg|.bmp|.gif|.png</param>
        /// <param name="chkType">被检查的格式</param>
        /// <returns>是否是符合的文件格式</returns>
        public static bool CheckValidExt(string allType, string chkType)
        {
            bool flag = false;
            string[] data = allType.Split('|');

            foreach (string temp in data)
            {
                if (string.Equals(temp, chkType, StringComparison.OrdinalIgnoreCase))
                {
                    flag = true;
                    break;
                }
            }

            return flag;
        }

        [DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr hObject);

        /// <summary>
        /// 复制指定目录的所有文件,不包含子目录及子目录中的文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,表示覆盖同名文件,否则不覆盖</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite)
        {
            CopyFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        /// 复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            //复制当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }

            //复制子目录
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }

                    CopyFiles(sourceSubDir, targetSubDir, overWrite, true);
                }
            }
        }

        /// <summary>
        /// 复制指定目录的所有文件，并把复制的文件备份到备份目录中
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        /// <param name="backDir">备份目录</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir, string backDir)
        {
            if (!Directory.Exists(backDir))
            {
                Directory.CreateDirectory(backDir);
            }

            //复制当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        string backFileName = Path.Combine(backDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));
                        File.Copy(targetFileName, backFileName, true);
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
                else
                {
                    File.Copy(sourceFileName, targetFileName, overWrite);
                }
            }

            //复制子目录
            if (copySubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }

                    string backSubDir = Path.Combine(backDir, targetSubDir.Substring(targetSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                    if (!Directory.Exists(backSubDir))
                    {
                        Directory.CreateDirectory(backSubDir);
                    }

                    CopyFiles(sourceSubDir, targetSubDir, overWrite, true, backSubDir);
                }
            }
        }

        /// <summary>
        /// 复制本机大文件
        /// <para>eg:FileHelper.CopyLocalBigFile(@"C:\Users\YanZh_000\Downloads\TheInterview.mp4", @"D:\The Interview(1080p).mp4", 1024 * 1024 * 5))</para>
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
            bool _copyResult = true;
            //将源文件 读取成文件流
            FileStream _fromFile = new FileStream(fromPath, FileMode.Open, FileAccess.Read);
            //已追加的方式 写入文件流
            FileStream _toFile = new FileStream(toPath, FileMode.Append, FileAccess.Write);

            try
            {
                //实际读取的文件长度
                int _toCopyLength = 0;

                //如果每次读取的长度小于 源文件的长度 分段读取
                if (eachReadLength < _fromFile.Length)
                {
                    byte[] _buffer = new byte[eachReadLength];
                    long _copied = 0;

                    while (_copied <= _fromFile.Length - eachReadLength)
                    {
                        _toCopyLength = _fromFile.Read(_buffer, 0, eachReadLength);
                        _fromFile.Flush();
                        _toFile.Write(_buffer, 0, eachReadLength);
                        _toFile.Flush();
                        //流的当前位置
                        _toFile.Position = _fromFile.Position;
                        _copied += _toCopyLength;
                    }

                    int _left = (int)(_fromFile.Length - _copied);
                    _toCopyLength = _fromFile.Read(_buffer, 0, _left);
                    _fromFile.Flush();
                    _toFile.Write(_buffer, 0, _left);
                    _toFile.Flush();
                }
                else
                {
                    //如果每次拷贝的文件长度大于源文件的长度 则将实际文件长度直接拷贝
                    byte[] _buffer = new byte[_fromFile.Length];
                    _fromFile.Read(_buffer, 0, _buffer.Length);
                    _fromFile.Flush();
                    _toFile.Write(_buffer, 0, _buffer.Length);
                    _toFile.Flush();
                }
            }
            catch (Exception)
            {
                _copyResult = false;
            }
            finally
            {
                _fromFile.Close();
                _toFile.Close();
            }

            return _copyResult;
        }

        /// <summary>
        /// 文件复制备份【同目录下】
        /// <para>eg:FileHelper.CopyToBak(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        public static bool CopyToBak(string filePath)
        {
            bool _result = true;

            try
            {
                int _fileCount = 0;
                string _bakName = "";

                do
                {
                    _fileCount++;
                    _bakName = string.Format("{0}.{1}.bak", filePath, _fileCount);
                }
                while (File.Exists(_bakName));

                File.Copy(filePath, _bakName);
                File.Delete(filePath);
                _result = true;
            }
            catch (Exception)
            {
                _result = false;
            }

            return _result;
        }

        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);

            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        /// <summary>
        /// 建立子目录
        /// </summary>
        /// <param name="parentDir">目录路径</param>
        /// <param name="subDirName">子目录名称</param>
        public static void CreateDirectory(string parentDir, string subDirName)
        {
            CreateDirectory(parentDir + PATH_SPLIT_CHAR + subDirName);
        }

        /// <summary>
        /// 创建文件路径
        /// <para>eg:FileHelper.CreatePath(@"C:\aa\cc\dd\ee.xml");</para>
        /// </summary>
        /// <param name="path">需要创建的路径</param>
        /// <returns>是否创建成功</returns>
        public static bool CreatePath(string path)
        {
            bool _result = true;

            if (!string.IsNullOrEmpty(path) && !File.Exists(path))
            {
                try
                {
                    string _directory = GetExceptName(path);
                    CreateDirectory(_directory);
                    FileStream _fileStream = File.Create(path);
                    _fileStream.Close();
                }
                catch (Exception)
                {
                    _result = false;
                }
            }

            return _result;
        }

        /// <summary>
        /// 根据现有路径创建临时文件路径
        /// </summary>
        /// <param name="filePath">文件全路径</param>
        /// <returns></returns>
        /// 时间：2015-12-17 14:13
        /// 备注：
        /// <exception cref="System.ArgumentException"></exception>
        public static string CreateTempFilePath(this string filePath)
        {
            FileInfo _sourceFile = new FileInfo(filePath);
            string _sourceFileTemp = Path.Combine(_sourceFile.DirectoryName, Guid.NewGuid().ToString() + _sourceFile.Extension);
            _sourceFile.CopyTo(_sourceFileTemp);
            return _sourceFileTemp;
        }

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteDirectory(string targetDir)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(targetDir);

            if (dirInfo.Exists)
            {
                DeleteFiles(targetDir, true);
                dirInfo.Delete(true);
            }
        }

        /// <summary>
        /// 删除指定目录的所有文件，不包含子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        public static void DeleteFiles(string targetDir)
        {
            DeleteFiles(targetDir, false);
        }

        /// <summary>
        /// 删除指定目录的所有文件和子目录
        /// </summary>
        /// <param name="targetDir">操作目录</param>
        /// <param name="delSubDir">如果为true,包含对子目录的操作</param>
        public static void DeleteFiles(string targetDir, bool delSubDir)
        {
            foreach (string fileName in Directory.GetFiles(targetDir))
            {
                File.SetAttributes(fileName, FileAttributes.Normal);
                File.Delete(fileName);
            }

            if (delSubDir)
            {
                DirectoryInfo dir = new DirectoryInfo(targetDir);

                foreach (DirectoryInfo subDi in dir.GetDirectories())
                {
                    DeleteFiles(subDi.FullName, true);
                    subDi.Delete();
                }
            }
        }

        /// <summary>
        /// 删除指定目录的所有子目录,不包括对当前目录文件的删除
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteSubDirectory(string targetDir)
        {
            foreach (string subDir in Directory.GetDirectories(targetDir))
            {
                DeleteDirectory(subDir);
            }
        }

        /// <summary>
        /// 将byte[]导出到文件
        /// <para>eg: FileHelper.SaveFile(_bytes, _outputFilePath); </para>
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="filePath"></param>
        public static void SaveFile(byte[] bytes, string filePath)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// 文件是否被占用
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>是否被占用</returns>
        public static bool FileIsTake(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;  //文件不存在
            }

            IntPtr _vHandle = _lopen(fileName, OF_READWRITE | OF_SHARE_DENY_NONE);

            if (_vHandle == HFILE_ERROR)
            {
                return true;//文件被占用！
            }

            CloseHandle(_vHandle);
            return false;
        }

        /// <summary>
        /// 获取除后缀外的路径
        /// <para>eg:FileHelper.GetExceptEx(@"C:\yanzhiwei.docx");==>"C:\yanzhiwei"</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>除后缀外的路径</returns>
        public static string GetExceptEx(string path)
        {
            string _fileName = string.Empty;

            if (RegexHelper.IsMatch(path, RegexPattern.FileCheck, out Match _result))
            {
                _fileName = _result.Result("${fpath}") + _result.Result("${fname}") + _result.Result("${namext}");
            }

            return _fileName;
        }

        /// <summary>
        /// 获取除文件外的路径
        /// <para>eg:FileHelper.GetExceptName(@"C:\yanzhiwei.docx");==>"C:\"</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>除文件外的路径</returns>
        public static string GetExceptName(string path)
        {
            string _fileName = string.Empty;

            if (RegexHelper.IsMatch(path, RegexPattern.FileCheck, out Match _result))
            {
                _fileName = _result.Result("${fpath}");
            }

            return _fileName;
        }

        /// <summary>
        /// 从路径中获取文件后缀
        /// <para>eg:FileHelper.GetFileEx(@"C:\yanzhiwei.docx");==>".docx"</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件后缀</returns>
        public static string GetFileEx(string path)
        {
            string _fileName = string.Empty;

            if (RegexHelper.IsMatch(path, RegexPattern.FileCheck, out Match _result))
            {
                _fileName = _result.Result("${suffix}");
            }

            return _fileName;
        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <param name="regexString">需要匹配的正则表达式</param>
        /// <returns>文件信息</returns>
        public static FileProperties GetFileInfo(string filepath, string regexString)
        {
            ValidateOperator.Begin().NotNullOrEmpty(filepath, "文件路径").IsFilePath(filepath).NotNullOrEmpty(regexString, "正则表达式");
            Match _uploadfolder = Regex.Match(filepath, regexString, RegexOptions.IgnoreCase);

            if (_uploadfolder.Success)
            {
                FileProperties _fileInfo = new FileProperties
                {
                    Root = _uploadfolder.Groups[1].Value,
                    Folder = _uploadfolder.Groups[2].Value,
                    SubFolder = _uploadfolder.Groups[3].Value,
                    FileName = _uploadfolder.Groups[4].Value,
                    FileNameExt = _uploadfolder.Groups[5].Value
                };
                return _fileInfo;
            }

            return null;
        }

        /// <summary>
        /// 从路径中获取文件名称（包括后缀）
        /// <para>eg:FileHelper.GetFileName(@"C:\yanzhiwei.docx");==>yanzhiwei.docx</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件名称（包括后缀）</returns>
        public static string GetFileName(string path)
        {
            string _fileName = string.Empty;

            if (RegexHelper.IsMatch(path, RegexPattern.FileCheck, out Match _result))
            {
                _fileName = _result.Result("${fname}") + _result.Result("${namext}") + _result.Result("${suffix}");
            }

            return _fileName;
        }

        /// <summary>
        /// 从路径中获取文件名称（不包括后缀）
        /// <para>eg:FileHelper.GetFileNameOnly(@"C:\yanzhiwei.docx");==>yanzhiwei</para>
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>文件名称（不包括后缀）</returns>
        public static string GetFileNameOnly(string path)
        {
            string _fileName = string.Empty;

            if (RegexHelper.IsMatch(path, RegexPattern.FileCheck, out Match _result))
            {
                _fileName = _result.Result("${fname}") + _result.Result("${namext}");
            }

            return _fileName;
        }

        /// <summary>
        /// 获取文件大小—kb
        /// <para>eg:FileHelper.GetKBSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小_kb</returns>
        public static double GetKBSize(string filePath)
        {
            double _kb = 0;
            long _size = GetSize(filePath);

            if (_size != 0)
            {
                _kb = _size / 1024d;
            }

            return _kb;
        }

        /// <summary>
        /// 获取文件大小—mb
        /// <para>eg:FileHelper.GetMBSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小_mb</returns>
        public static double GetMBSize(string filePath)
        {
            double _mb = 0;
            long _size = GetSize(filePath);

            if (_size != 0)
            {
                _mb = _size / 1048576d;//1024*1024==1048576;
            }

            return _mb;
        }

        /// <summary>
        /// 获取文件大小—字节
        /// <para>eg:FileHelper.GetSize(TestFilePath);</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>文件大小</returns>
        public static long GetSize(string filePath)
        {
            byte[] _buffer = ReadFile(filePath);
            return _buffer == null ? 0 : _buffer.Length;
        }

        /// <summary>
        /// 获取可用的文件名称
        /// </summary>
        /// <param name="fileName">文件名称</param>
        /// <returns>返回可用的用户名称</returns>
        public static string GetValidFileName(string fileName)
        {
            return Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "").Replace(" ", string.Empty);
        }

        /// <summary>
        /// 剪切指定目录的所有文件,不包含子目录
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite)
        {
            MoveFiles(sourceDir, targetDir, overWrite, false);
        }

        /// <summary>
        /// 剪切指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="moveSubDir">如果为true,包含目录,否则不包含</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite, bool moveSubDir)
        {
            //移动当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Delete(targetFileName);
                        File.Move(sourceFileName, targetFileName);
                    }
                }
                else
                {
                    File.Move(sourceFileName, targetFileName);
                }
            }

            if (moveSubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf(PATH_SPLIT_CHAR) + 1));

                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }

                    MoveFiles(sourceSubDir, targetSubDir, overWrite, true);
                    Directory.Delete(sourceSubDir);
                }
            }
        }

        /// <summary>
        /// 打开文件或者文件夹
        /// </summary>
        /// <param name="path">文件夹或者文件的路径</param>
        public static void OpenFolderAndFile(string path)
        {
            Process.Start(path);
        }

        /// <summary>
        /// 将文件转换成二进制流
        /// <para>eg:FileHelper.ReadFile(@"C:\demo.txt");</para>
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>二进制数组</returns>
        public static byte[] ReadFile(string filePath)
        {
            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                byte[] _buffur = new byte[stream.Length];
                stream.Read(_buffur, 0, (int)stream.Length);
                return _buffur;
            }
        }

        /// <summary>
        /// 递归获取文件夹目录下文件
        /// </summary>
        /// <param name="pathName">需要递归遍历的文件夹</param>
        /// <param name="fileHanlder">遍历规则『委托』</param>
        public static void RecursiveFolder(string pathName, Action<FileInfo> fileHanlder)
        {
            Queue<string> _pathQueue = new Queue<string>();
            _pathQueue.Enqueue(pathName);

            while (_pathQueue.Count > 0)
            {
                string _path = _pathQueue.Dequeue();
                DirectorySecurity _pathSecurity = new DirectorySecurity(_path, AccessControlSections.Access);

                if (!_pathSecurity.AreAccessRulesProtected)                                    //文件夹权限是否可访问
                {
                    DirectoryInfo _directoryInfo = new DirectoryInfo(_path);

                    foreach (DirectoryInfo diChild in _directoryInfo.GetDirectories())
                    {
                        _pathQueue.Enqueue(diChild.FullName);
                    }

                    foreach (FileInfo file in _directoryInfo.GetFiles())
                    {
                        fileHanlder(file);
                    }
                }
            }
        }

        /// <summary>
        /// 设置程序开机启动_注册表形式
        /// </summary>
        /// <param name="path">需要开机启动的exe路径</param>
        /// <param name="keyName">注册表中键值名称</param>
        /// <param name="set">true设置开机启动，false取消开机启动</param>
        public static void StartupSet(string path, string keyName, bool set)
        {
            /*
             * 知识：
             * 1.管理员权限问题
             *   在打开的工程中，看下Properties 下面是否有app.manifest 这个文件，如果没有，右击工程在菜单中选择“属性”
             *   选中"Security"，在界面中勾选"Enable ClickOnce Security Settings"后，在Properties下就有自动生成app.manifest文件。
             *   打开app.manifest文件，将<requestedExecutionLevel level="asInvoker" uiAccess="false" />改为
             *   <requestedExecutionLevel level="requireAdministrator" uiAccess="false" />
             *   然后在"Security"中再勾去"Enable ClickOnce Security Settings"后，重新编译即可。
             * 参考:
             * 1. http://syxc.iteye.com/blog/673972
             * 2. http://zouqinghua11111.blog.163.com/blog/static/67997654201242334620628/
             * 3. http://stackoverflow.com/questions/5089601/run-the-application-at-windows-startup
             */
            using (RegistryKey _reg = Registry.LocalMachine)
            {
                RegistryKey _run = _reg.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

                if (set)
                {
                    _run.SetValue(keyName, path);
                }
                else
                {
                    object _value = _run.GetValue(keyName);

                    if (_value != null)
                    {
                        _run.DeleteValue(keyName);
                    }
                }
            }
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        #endregion Methods
    }
}