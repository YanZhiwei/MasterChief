namespace MasterChief.DotNet4.Utilities
{
    /// <summary>
    /// 媒体类型通常是通过 HTTP 协议，由 Web 服务器告知浏览器的，更准确地说，是通过 Content-Type 来表示的
    /// </summary>
    public static class MimeTypes
    {
        #region application/*

        /// <summary>
        /// force-download
        /// </summary>
        public const string ApplicationForceDownload = "application/force-download";

        /// <summary>
        ///json
        /// </summary>
        public const string ApplicationJson = "application/json";

        /// <summary>
        /// ApplicationOctetStream
        /// </summary>
        public const string ApplicationOctetStream = "application/octet-stream";

        /// <summary>
        /// pdf
        /// </summary>
        public const string ApplicationPdf = "application/pdf";

        /// <summary>
        /// RSS XML
        /// </summary>
        public const string ApplicationRssXml = "application/rss+xml";

        /// <summary>
        /// The application x WWW form urlencoded
        /// 它会将表单的数据处理为一条消息，以标签为单元，用分隔符分开。既可以上传键值对，也可以上传文件。当上传的字段是文件时，会有Content-Type来表名文件类型；content-disposition，用来说明字段的一些信息
        /// </summary>
        public const string ApplicationXWwwFormUrlencoded = "application/x-www-form-urlencoded";

        /// <summary>
        ///x-zip-co
        /// </summary>
        public const string ApplicationXZipCo = "application/x-zip-co";

        #endregion application/*

        #region image/*

        /// <summary>
        ///  BMP
        /// </summary>
        public const string ImageBmp = "image/bmp";

        /// <summary>
        ///  GIF
        /// </summary>
        public const string ImageGif = "image/gif";

        /// <summary>
        /// JPEG
        /// </summary>
        public const string ImageJpeg = "image/jpeg";

        /// <summary>
        ///  JPEG
        /// </summary>
        public const string ImagePJpeg = "image/pjpeg";

        /// <summary>
        ///  PNG
        /// </summary>
        public const string ImagePng = "image/png";

        /// <summary>
        ///  tiff
        /// </summary>
        public const string ImageTiff = "image/tiff";

        #endregion image/*

        #region text/*

        /// <summary>
        /// CSS
        /// </summary>
        public const string TextCss = "text/css";

        /// <summary>
        ///  CSV
        /// </summary>
        public const string TextCsv = "text/csv";

        /// <summary>
        ///  javascript
        /// </summary>
        public const string TextJavascript = "text/javascript";

        /// <summary>
        /// The text plain
        /// </summary>
        public const string TextPlain = "text/plain";

        /// <summary>
        /// The text XLSX
        /// </summary>
        public const string TextXlsx = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        #endregion text/*
    }
}