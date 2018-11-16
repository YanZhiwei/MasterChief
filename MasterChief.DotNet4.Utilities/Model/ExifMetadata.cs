namespace MasterChief.DotNet4.Utilities.Model
{
    /// <summary>
    /// JPG照片的Meta数据结构
    /// </summary>
    public struct ExifMetadata
    {
        #region Fields

        /// <summary>
        /// 拍照时镜头的光圈. 单位是 APEX
        /// </summary>
        public ExifMetadataDetail Aperture;

        /// <summary>
        /// 被拍摄对象的明度, 单位是 APEX.
        /// </summary>
        public ExifMetadataDetail Brightness;

        /// <summary>
        /// 表示数字相机的模块代码. 在 Exif 标准中, 这个标签是可选的, 但在DCF中它也是必需的
        /// </summary>
        public ExifMetadataDetail CameraModel;

        /// <summary>
        /// 色彩滤镜矩阵（CFA）几何样式
        /// </summary>
        public ExifMetadataDetail CfaPattern;

        /// <summary>
        /// 色彩空间
        /// </summary>
        public ExifMetadataDetail ColorSpace;

        /// <summary>
        /// JPEG (粗略的估计)的平均压缩率.
        /// </summary>
        public ExifMetadataDetail CompBPP;

        /// <summary>
        /// 像素数据的顺序
        /// </summary>
        public ExifMetadataDetail CompConfig;

        /// <summary>
        /// 照片在被拍下来的日期/时间
        /// </summary>
        public ExifMetadataDetail DatePictureTaken;

        /// <summary>
        /// 数字相机的制造商
        /// </summary>
        public ExifMetadataDetail EquipmentMake;

        /// <summary>
        /// 照片拍摄时的曝光补偿. 单位是APEX(EV).
        /// </summary>
        public ExifMetadataDetail ExposureBias;

        /// <summary>
        /// 曝光指数
        /// </summary>
        public ExifMetadataDetail ExposureIndex;

        /// <summary>
        /// 拍照时相机使用的曝光程序. '1' 表示手动曝光, '2' 表示正常程序曝光, '3' 表示光圈优先曝光, '4' 表示快门优先曝光, '5' 表示创意程序(慢速程序), '6' 表示动作程序(高速程序), '7'表示 肖像模式, '8' 表示风景模式.
        /// </summary>
        public ExifMetadataDetail ExposureProg;

        /// <summary>
        /// 曝光时间 (快门速度的倒数). 单位是秒.
        /// </summary>
        public ExifMetadataDetail ExposureTime;

        /// <summary>
        /// '0' 表示闪光灯没有闪光, '1' 表示闪光灯闪光, '5' 表示闪光但没有检测反射光, '7' 表示闪光且检测了反射光.
        /// </summary>
        public ExifMetadataDetail Flash;

        //曝光时间
        /// <summary>
        /// 拍照时的光圈F-number(F-stop).
        /// </summary>
        public ExifMetadataDetail FNumber;

        /// <summary>
        /// 棸距长度. 单位是毫米
        /// </summary>
        public ExifMetadataDetail FocalLength;

        /// <summary>
        /// CCD的像素密度
        /// </summary>
        public ExifMetadataDetail FocalResUnit;

        /// <summary>
        /// CCD的像素密度 X轴
        /// </summary>
        public ExifMetadataDetail FocalXRes;

        /// <summary>
        /// CCD的像素密度 Y轴
        /// </summary>
        public ExifMetadataDetail FocalYRes;

        /// <summary>
        /// FlashPix的版本信息
        /// </summary>
        public ExifMetadataDetail FPXVer;

        /// <summary>
        /// 拍照时的光圈F-number(F-stop).
        /// </summary>
        public ExifMetadataDetail Fstop;

        /// <summary>
        /// 图片高度
        /// </summary>
        public ExifMetadataDetail ImageHeight;

        /// <summary>
        /// 图片宽度
        /// </summary>
        public ExifMetadataDetail ImageWidth;

        /// <summary>
        /// CCD 的感光度, 等效于 Ag-Hr 胶片的速率.
        /// </summary>
        public ExifMetadataDetail ISOSpeed;

        /// <summary>
        /// 光源, 实际上是表示白平衡设置
        /// </summary>
        public ExifMetadataDetail LightSource;

        /// <summary>
        /// 镜头的最大光圈值
        /// </summary>
        public ExifMetadataDetail MaxAperture;

        /// <summary>
        /// 曝光的测光方法. '0' 表示未知, '1' 为平均测光, '2' 为中央重点测光, '3' 是点测光, '4' 是多点测光, '5' 是多区域测光, '6' 部分测光, '255' 则是其他.
        /// </summary>
        public ExifMetadataDetail MeteringMode;

        /// <summary>
        /// OECF
        /// </summary>
        public ExifMetadataDetail OECF;

        /// <summary>
        /// 场景类型
        /// </summary>
        public ExifMetadataDetail SceneType;

        /// <summary>
        /// 图像传感器单元的类型
        /// </summary>
        public ExifMetadataDetail SensingMethod;

        /// <summary>
        /// 用APEX表示出的快门速度
        /// </summary>
        public ExifMetadataDetail ShutterSpeed;

        /// <summary>
        /// 光谱灵敏度
        /// </summary>
        public ExifMetadataDetail SpectralSense;

        /// <summary>
        /// 焦点的距离, 单位是米
        /// </summary>
        public ExifMetadataDetail SubjectDist;

        /// <summary>
        /// Exif 的版本号
        /// </summary>
        public ExifMetadataDetail Ver;

        /// <summary>
        /// 水平分辨率
        /// </summary>
        public ExifMetadataDetail XResolution;

        /// <summary>
        /// 垂直分辨率
        /// </summary>
        public ExifMetadataDetail YResolution;

        #endregion Fields
    }
}