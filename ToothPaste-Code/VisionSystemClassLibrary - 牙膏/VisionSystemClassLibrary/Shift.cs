/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：System.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：班次

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Timers;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;
using System.Runtime.Serialization.Formatters.Binary;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Shift
    {
        private static Byte CameraNumberMax = 4;                                //最大挂在相机个数4
        private static Byte CameraChooseState;                                  //检测项，01表示端口1，02表示端口2，04表示端口3，08表示端口4

        private const Int32 iMinShiftNumber = 1;//班次数目最小值
        private const Int32 iMaxShiftNumber = 24;//班次数目最大值
        
        private Boolean bShiftState = false;//班次使能状态
        private Int32 iShiftNumber;//班次数目

        private Struct.ShiftData sDataOfShift = new Struct.ShiftData();//班次数据
         
        [NonSerialized]
        private Timer timer = new Timer();//创建定时器对象

        //

        private Int32 SingleShiftChangeState = 0;
        private Boolean bShiftTimeChangeState = false;

        private static string sShiftDataPath = "";//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）
        private static string sShiftFileName = "Shift.dat";//Shift数据文件名称

        private static string StatisticsDataPath = "";//统计数据路径(如：D:\\VisionSystemUserInterface\\Statistics\\)
        private static string StatisticsFileName = "Statistics.dat";//Statistics数据文件名称

        //

        public event EventHandler ShiftChange; //换班时产生的事件

        private static Boolean bDeletingStatics = false; //正在执行删除操作

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数(默认)
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Shift()
        {
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数（HMI）
        // 输入参数：1.sAppPath：，应用程序路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Shift(global::System.String sAppPath)
        {
            sShiftDataPath = sAppPath + Class.System.ConfigDataPathName;//配置文件路径（如，D:\\X6S\\ConfigData\\）
            StatisticsDataPath = sAppPath + Class.System.StatisticsPathName;//统计数据路径(如：D:\\X6S\\Statistics\\)

            if (_ReadShiftState())//当前班次使能
            {
                _ReadShiftTime();//读取当前班次时间信息
                _InitShiftTime(DateTime.Now, ref sDataOfShift.TimeData);//更新系统班次日期
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数（Controller）
        // 输入参数：1.sAppPath：应用程序路径
        //         2.byteIndex：相机端口索引值（相机端口数值 - 1）
        //         3.toolNumber：对应相机工具个数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Shift(global::System.String sAppPath, Byte[] byteIndex, Int32[] toolNumber, Byte bCameraNumberMax, Byte bCameraChooseState)
        {
            sShiftDataPath = sAppPath + Class.System.ConfigDataPathName;//配置文件路径（如，D:\\X6S\\ConfigData\\）
            StatisticsDataPath = sAppPath + Class.System.StatisticsPathName;//统计数据路径(如：D:\\X6S\\Statistics\\)

            if (_ReadShiftState())//当前班次使能
            {
                CameraNumberMax = bCameraNumberMax;
                CameraChooseState = bCameraChooseState;

                _ReadShiftTime();//读取当前班次时间信息
                _InitShiftTime(DateTime.Now, ref sDataOfShift.TimeData);//更新系统班次日期

                //

                sDataOfShift.CurrentIndexOld = sDataOfShift.CurrentIndex = _GetCurrentShift(DateTime.Now);//获取当前班次
                
                _InitInformationOfStatistics(byteIndex, toolNumber);

                for (Byte i = 0; i < CameraNumberMax; i++)//遍历当前所有相机
                {
                    if ((CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        _ReadCurrentShiftInformation(Convert.ToByte(i + 1), sDataOfShift.CurrentIndex);//读取历史班次信息
                    }
                }

                timer_Init();//定时器初始化
            }
        }

        //属性

        // 功能说明：DeletingStatics属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean DeletingStatics
        {
            get
            {
                return bDeletingStatics;
            }
        }

        // 功能说明：ShiftTimeChangeState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ShiftTimeChangeState
        {
            get
            {
                return bShiftTimeChangeState;
            }
            set
            {
                bShiftTimeChangeState = value;
            }
        }

        // 功能说明：MinShiftNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static Int32 MinShiftNumber
        {
            get
            {
                return iMinShiftNumber;
            }
        }

        // 功能说明：MaxShiftNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static Int32 MaxShiftNumber
        {
            get
            {
                return iMaxShiftNumber;
            }
        }

        // 功能说明：DataOfShift属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.ShiftData DataOfShift
        {
            get
            {
                return sDataOfShift;
            }
            set
            {
                sDataOfShift = value;
            }
        }

        // 功能说明：ShiftState属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ShiftState
        {
            get
            {
                return bShiftState;
            }
            set
            {
                bShiftState = value;
            }
        }

        // 功能说明：ShiftNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 ShiftNumber
        {
            get
            {
                return iShiftNumber;
            }
            set
            {
                iShiftNumber = value;
            }
        }

        // 功能说明：ShiftDataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string ShiftDataPath
        {
            get
            {
                return sShiftDataPath;
            }
            set
            {
                sShiftDataPath = value;
            }
        }

        // 功能说明：ShiftFileName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string ShiftFileName
        {
            get
            {
                return sShiftFileName;
            }
        }

        // 功能说明：CurrentShiftIndex属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 CurrentShiftIndex
        {
            get
            {
                return sDataOfShift.CurrentIndex;
            }
        }

        // 功能说明：CurrentShiftIndexOld属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 CurrentShiftIndexOld
        {
            get
            {
                return sDataOfShift.CurrentIndexOld;
            }
        }

        //函数

        //-----------------------------------------------------------------------
        // 功能说明：读取接受上位机设置时间函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：ShiftTime[]，返回配置文件班次时间数组
        //----------------------------------------------------------------------
        public Struct.ShiftTime[] _ReadShiftTimeNew(string shiftDataPath)
        {
            FileStream filestream = new FileStream(shiftDataPath + sShiftFileName, FileMode.Open); //打开Shift文件
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek(0x10, SeekOrigin.Begin);
            Int32 shiftNumber = binaryreader.ReadInt32();//读取班次数量

            Struct.ShiftTime[] shiftTime = null;

            if ((shiftNumber >= iMinShiftNumber) && (shiftNumber <= iMaxShiftNumber))//当前班次有效
            {
                shiftTime = new Struct.ShiftTime[shiftNumber];

                for (Int32 i = 1; i <= shiftNumber; i++)//遍历当前所有班次
                {
                    filestream.Seek((0x20 + (i - 1) * 0x08), SeekOrigin.Begin);

                    shiftTime[i - 1].Start.Hour = binaryreader.ReadUInt16();//读取班次开始时间
                    shiftTime[i - 1].Start.Minute = binaryreader.ReadUInt16();

                    shiftTime[i - 1].End.Hour = binaryreader.ReadUInt16();//读取班次结束时间
                    shiftTime[i - 1].End.Minute = binaryreader.ReadUInt16();
                }
            }
            binaryreader.Close();//关闭Shift文件
            filestream.Close();

            _InitShiftTime(DateTime.Now, ref shiftTime);

            return shiftTime;
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存班次时间函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteShiftTime()
        {
            FileStream filestream = new FileStream(sShiftDataPath + sShiftFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开Shift文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

            filestream.Seek(0x10, SeekOrigin.Begin);
            binarywriter.Write(sDataOfShift.TimeData.Length);//写入班次数量

            for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
            {
                filestream.Seek(0x20 + (i - 1) * 0x08, SeekOrigin.Begin);//

                binarywriter.Write(sDataOfShift.TimeData[i - 1].Start.Hour);//写入班次开始时间
                binarywriter.Write(sDataOfShift.TimeData[i - 1].Start.Minute);

                binarywriter.Write(sDataOfShift.TimeData[i - 1].End.Hour);//写入班次结束时间
                binarywriter.Write(sDataOfShift.TimeData[i - 1].End.Minute);
            }
            binarywriter.Close();//关闭Shift文件
            filestream.Close();
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：写入当前班次信息函数
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteCurrentShiftOldInformation(Byte byteIndex, Int32 shift)
        {
            try
            {
                if (shift > 0)//当前班次有效 ,未执行删除操作
                {
                    string cameraPath = _GetAllCameraPath(byteIndex);
                    DirectoryInfo dir = new DirectoryInfo(cameraPath);

                    if (!dir.Exists) //当前路径存在
                    {
                        dir.Create();
                    }

                    string cameraShiftPath = _GetCameraShiftPath(byteIndex, shift);
                    dir = new DirectoryInfo(cameraShiftPath);

                    if (!dir.Exists) //当前路径存在
                    {
                        dir.Create();
                    }

                    string currentStatisticsDataPath = _GetCurrentDayShiftDataPath(byteIndex, shift);
                    dir = new DirectoryInfo(currentStatisticsDataPath);

                    if (!dir.Exists) //当前路径存在
                    {
                        dir.Create();
                    }

                    FileStream filestream = new FileStream(currentStatisticsDataPath + StatisticsFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开统计文件
                    BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

                    binarywriter.Write(Convert.ToByte(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera));//写入相机类型
                    binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber);//写入检测烟包总数

                    filestream.Seek(0x20, SeekOrigin.Begin);
                    binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].BrandName);//写入烟包品牌名称

                    filestream.Seek(0x30, SeekOrigin.Begin);
                    binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber);//写入缺陷烟包总数
                    binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy);//写入缺陷烟包总数（关联）

                    filestream.Seek(0x40, SeekOrigin.Begin);
                    binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length);//写入检测工具总数

                    filestream.Seek(0x50, SeekOrigin.Begin);
                    for (Int32 i = 0; i < sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length; i++)
                    {
                        binarywriter.Write(sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool[i]);//写入检测工具缺陷总数
                    }

                    binarywriter.Close();//关闭统计文件
                    filestream.Close();
                }
            }
            catch(Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新并保存当前班次统计信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、string：brandName，烟包品牌
        //           3、ImageInformation[]：imageInformations，图像信息
        //           4、Image<Bgr, Byte>：image，缺陷图像
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _UpdateCurrentImageInformation(Byte byteIndex, string brandName, Struct.ImageInformation[] imageInformations = null, Image<Bgr, Byte> image = null)
        {
            if (sDataOfShift.CurrentIndex > 0)//当前班次有效
            {
                if (false == bDeletingStatics) //未执行删除操作
                {
                    if (true == _CheckDiskMemory(0.8))//计算当前根目录存储空间是否满足0.8要求，不满足则停止保存，等待停机删除
                    {
                    }
                    else//执行缺陷图像统计信息计数
                    {
                        if ((null != image) && (null != imageInformations)) //当前图像有效
                        {
                            Int32 i = 0; //循环变量

                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera = (Enum.CameraType)byteIndex;
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber++;
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy++;

                            Boolean bError = false;
                            for (i = 0; i < imageInformations.Length; i++) //遍历图像信息
                            {
                                if (imageInformations[i].Type == Enum.ImageType.Error) //检测结果有缺陷
                                {
                                    sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool[imageInformations[i].ToolsIndex]++;
                                    bError = true;
                                }
                            }
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].BrandName = brandName;

                            string cameraPath = _GetAllCameraPath(byteIndex);
                            DirectoryInfo dir = new DirectoryInfo(cameraPath);

                            if (!dir.Exists) //当前路径存在
                            {
                                dir.Create();
                            }

                            string cameraShiftPath = _GetCameraShiftPath(byteIndex, sDataOfShift.CurrentIndex);
                            dir = new DirectoryInfo(cameraShiftPath);

                            if (!dir.Exists) //当前路径存在
                            {
                                dir.Create();
                            }

                            string currentStatisticsDataPath = _GetCurrentDayShiftDataPath(byteIndex, sDataOfShift.CurrentIndex);
                            dir = new DirectoryInfo(currentStatisticsDataPath);

                            if (!dir.Exists)//当前路径存在
                            {
                                dir.Create();
                            }

                            string processResult = Enum.ImageType.Ok.ToString();
                            if (bError) //检测结果有缺陷
                            {
                                sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber++;
                                processResult = Enum.ImageType.Error.ToString();
                            }

                            string fileName = currentStatisticsDataPath + sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy.ToString("00000000") + "_" + processResult;

                            if (bError) //检测结果有缺陷
                            {
                                fileName += "_" + sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber.ToString("00000000");
                            }

                            for (i = 0; i < imageInformations.Length; i++) //遍历图像信息
                            {
                                if (imageInformations[i].Type == Enum.ImageType.Error) //检测结果有缺陷
                                {
                                    fileName += "_" + imageInformations[i].ToolsIndex.ToString("00");
                                }
                            }

                            fileName += "_";

                            //GeneralFunction._SaveImage(image, fileName + Camera.JPGFile, ".jpg");
                            image.Save(fileName + Camera.BMPFile);//保存图像"00000001_Error_00000001_00_01_"或"00000001_OK"
                            _WriteImageInformation(fileName + Camera.DatFile, imageInformations);//保存缺陷图像信息"00000001_Error_00000001_00_01_"或"00000001_OK"
                        }
                        else//检测结果完好，统计信息计数
                        {
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera = (Enum.CameraType)byteIndex;
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber++;
                            sDataOfShift.InformationOfStatistics[sDataOfShift.CurrentIndex - 1].DataOfStatistics[0].BrandName = brandName;
                        }
                    }
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：更新所有班次历史统计信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        // 输出参数：无
        // 返 回 值：StatisticsRecordList，返回统计数信息
        //---------------------------------------------------------------------
        public Struct.StatisticsRecordList _UpdateStatisticsRecordList(Byte byteIndex)
        {
            Struct.StatisticsRecordList StatisticsRecordDataList = new Struct.StatisticsRecordList();
            StatisticsRecordDataList._InitData(sDataOfShift.TimeData.Length, 0);//初始化

            StatisticsRecordDataList.CurrentShiftIndex = sDataOfShift.CurrentIndex;//上位机班次索引从零开始

            for (Int32 j = 1; j <= sDataOfShift.TimeData.Length; j++)//遍历当前所有班次，查询各班次统计信息
            {
                _CheckHistoryShiftStaticNumber(byteIndex, j, ref StatisticsRecordDataList);
            }
            return StatisticsRecordDataList;
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新班次统计信息给上位机
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        //           3、SYSTEMTIME：startTime，班次开始时间
        //           4、SYSTEMTIME：endTime，班次结束时间
        //           5、Enum.CameraType：typeOfCamera，相机类型
        //           6、string：brandName,烟包品牌名称
        //           7、UInt32：inspectedNumber,检测烟包总数
        //           8、UInt32：rejectedNumber,缺陷烟包总数
        //           9、UInt32[]：rejectedStatistics_Tool,工具剔除烟包数量
        //           10、Enum.RelevancyType：relevancyType，关联类型
        // 输出参数：无
        // 返 回 值：无
        //---------------------------------------------------------------------
        public void _UpdateHistoryShift(Byte byteIndex, Int32 shift, Struct.SYSTEMTIME startTime, Struct.SYSTEMTIME endTime, ref Enum.CameraType typeOfCamera, ref string brandName, ref UInt32 inspectedNumber, ref UInt32 rejectedNumber, ref UInt32[] rejectedStatistics_Tool, Enum.RelevancyType relevancyType = Enum.RelevancyType.None)
        {
            if (shift > 0)//当前班次有效
            {
                if ((sDataOfShift.CurrentIndex > 0) && (shift == sDataOfShift.CurrentIndex) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].Start, startTime) == 0) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].End, endTime) == 0))//查询当前班次
                {
                    typeOfCamera = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera;
                    brandName = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].BrandName;
                    inspectedNumber = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber;

                    switch (relevancyType) //关联类型
                    {
                        case Enum.RelevancyType.None:
                            rejectedNumber = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber;
                            break;
                        case Enum.RelevancyType.Inner:
                            rejectedNumber = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy;
                            break;
                        case Enum.RelevancyType.Extra:
                            rejectedNumber = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy;
                            break;
                        default:
                            break;
                    }
                    rejectedStatistics_Tool = new UInt32[sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length];
                    sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.CopyTo(rejectedStatistics_Tool, 0);
                }
                else
                {
                    try
                    {
                        DirectoryInfo dir = new DirectoryInfo(_GetHistoryDayShiftDataPath(byteIndex, shift, startTime, endTime));

                        if (dir.Exists)                                                    //当前路径存在
                        {
                            if (File.Exists(dir.FullName + StatisticsFileName))  //统计文件存在
                            {
                                FileStream filestream = new FileStream(dir.FullName + StatisticsFileName, FileMode.Open); //打开统计文件
                                BinaryReader binaryreader = new BinaryReader(filestream);

                                typeOfCamera = (Enum.CameraType)binaryreader.ReadByte();
                                inspectedNumber = binaryreader.ReadUInt32();

                                filestream.Seek((0x20), SeekOrigin.Begin);
                                brandName = binaryreader.ReadString();

                                switch (relevancyType) //关联类型
                                {
                                    case Enum.RelevancyType.None:
                                        filestream.Seek((0x30), SeekOrigin.Begin);
                                        rejectedNumber = binaryreader.ReadUInt32();
                                        break;
                                    case Enum.RelevancyType.Inner:
                                        filestream.Seek((0x34), SeekOrigin.Begin);
                                        rejectedNumber = binaryreader.ReadUInt32();
                                        break;
                                    case Enum.RelevancyType.Extra:
                                        filestream.Seek((0x34), SeekOrigin.Begin);
                                        rejectedNumber = binaryreader.ReadUInt32();
                                        break;
                                    default:
                                        break;
                                }

                                filestream.Seek((0x40), SeekOrigin.Begin);
                                Int32 toolNumber = binaryreader.ReadInt32();

                                filestream.Seek((0x50), SeekOrigin.Begin);
                                rejectedStatistics_Tool = new UInt32[toolNumber];
                                for (Int32 i = 0; i < rejectedStatistics_Tool.Length; i++)//遍历当前所有工具
                                {
                                    rejectedStatistics_Tool[i] = binaryreader.ReadUInt32();
                                }
                                binaryreader.Close();//关闭统计文件
                                filestream.Close();
                            }
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新各工具历史缺陷图像给上位机
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        //           3、SYSTEMTIME：startTime，班次开始时间
        //           4、SYSTEMTIME：endTime，班次结束时间
        //           5、Int32：toolIndex，工具索引
        //           6、Int32：imageIndex，各工具历史班次缺陷图像索引
        //           7、Image<Bgr, Byte>：image，缺陷图像
        //           8、ImageInformation：imageInformation，缺陷图像信息
        //           9、Boolean：bToolState，深度学习工具使能标记
        //           10、Boolean：bDeepLearningState，深度学习标记结果
        //           11、string：sTypeName，深度学习结果
        //           12、Enum.RelevancyType：relevancyType，关联类型
        // 输出参数：无
        // 返 回 值：无
        //---------------------------------------------------------------------
        public void _UpdateHistoryShiftImage(Byte byteIndex, Int32 shift, Struct.SYSTEMTIME startTime, Struct.SYSTEMTIME endTime, Int32 toolIndex, Int32 imageIndex, ref Image<Bgr, Byte> image, ref Struct.ImageInformation imageInformation, ref Boolean bToolState, ref Boolean bDeepLearningState, ref string sTypeName, Enum.RelevancyType relevancyType = Enum.RelevancyType.None)
        {
            try
            {
                if (shift > 0)//当前班次有效
                {
                    DirectoryInfo dir = new DirectoryInfo(_GetHistoryDayShiftDataPath(byteIndex, shift, startTime, endTime));

                    if ((dir.Exists) && (imageIndex >= 0))                    //当前路径存在，且缺陷图像索引有效
                    {
                        FileInfo[] rejectedStatistics = new FileInfo[0];

                        if (toolIndex < 0)//当前工具全选
                        {
                            switch (relevancyType) //关联类型
                            {
                                case Enum.RelevancyType.None:
                                    rejectedStatistics = dir.GetFiles("*_Error_" + (imageIndex + 1).ToString("00000000") + "*" + Class.Camera.BMPFile);

                                    if (rejectedStatistics.Length > 0) //缺陷图像数量有效
                                    {
                                        image = new Image<Bgr, byte>(rejectedStatistics[0].FullName);
                                        //image = GeneralFunction._ReadImage(rejectedStatistics[0].FullName);

                                        Struct.ImageInformation[] imageInformations = new Struct.ImageInformation[0];
                                        _ReadImageInformation(rejectedStatistics[0].FullName.Substring(0, rejectedStatistics[0].FullName.Length - rejectedStatistics[0].Extension.Length) + Class.Camera.DatFile, ref imageInformations);//读取缺陷图像信息

                                        Int32 iToolIndex = 0;
                                        if (false == rejectedStatistics[0].Name.Contains("Ok")) //烟包缺陷
                                        {
                                            for (iToolIndex = 0; iToolIndex < imageInformations.Length; iToolIndex++) //遍历图像信息
                                            {
                                                if (imageInformations[iToolIndex].Type == Enum.ImageType.Error) //检测结果有缺陷
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        _GetDeepLearningInfo(imageInformations, ref bToolState, ref bDeepLearningState, ref sTypeName);

                                        imageInformations[iToolIndex]._CopyTo(imageInformation);//更新对应工具图像信息
                                    }
                                    break;
                                case Enum.RelevancyType.Inner:
                                    rejectedStatistics = dir.GetFiles((imageIndex + 1).ToString("00000000") + "*" + Class.Camera.BMPFile);
                                    image = new Image<Bgr, byte>(rejectedStatistics[0].FullName);
                                    //image = GeneralFunction._ReadImage(rejectedStatistics[0].FullName);

                                    if (rejectedStatistics.Length > 0) //缺陷图像数量有效
                                    {
                                        image = new Image<Bgr, byte>(rejectedStatistics[0].FullName);
                                        //image = GeneralFunction._ReadImage(rejectedStatistics[0].FullName);

                                        Struct.ImageInformation[] imageInformations = new Struct.ImageInformation[0];
                                        _ReadImageInformation(rejectedStatistics[0].FullName.Substring(0, rejectedStatistics[0].FullName.Length - rejectedStatistics[0].Extension.Length) + Class.Camera.DatFile, ref imageInformations);//读取缺陷图像信息

                                        Int32 iToolIndex = 0;
                                        if (false == rejectedStatistics[0].Name.Contains("Ok")) //烟包缺陷
                                        {
                                            for (iToolIndex = 0; iToolIndex < imageInformations.Length; iToolIndex++) //遍历图像信息
                                            {
                                                if (imageInformations[iToolIndex].Type == Enum.ImageType.Error) //检测结果有缺陷
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        _GetDeepLearningInfo(imageInformations, ref bToolState, ref bDeepLearningState, ref sTypeName);

                                        imageInformations[iToolIndex]._CopyTo(imageInformation);//更新对应工具图像信息
                                    }
                                    break;
                                case Enum.RelevancyType.Extra:
                                    rejectedStatistics = dir.GetFiles((imageIndex + 1).ToString("00000000") + "*" + Class.Camera.BMPFile);
                                    image = new Image<Bgr, byte>(rejectedStatistics[0].FullName);
                                    //image = GeneralFunction._ReadImage(rejectedStatistics[0].FullName);

                                    if (rejectedStatistics.Length > 0) //缺陷图像数量有效
                                    {
                                        image = new Image<Bgr, byte>(rejectedStatistics[0].FullName);
                                        //image = GeneralFunction._ReadImage(rejectedStatistics[0].FullName);

                                        Struct.ImageInformation[] imageInformations = new Struct.ImageInformation[0];
                                        _ReadImageInformation(rejectedStatistics[0].FullName.Substring(0, rejectedStatistics[0].FullName.Length - rejectedStatistics[0].Extension.Length) + Class.Camera.DatFile, ref imageInformations);//读取缺陷图像信息

                                        Int32 iToolIndex = 0;
                                        if (false == rejectedStatistics[0].Name.Contains("Ok")) //烟包缺陷
                                        {
                                            for (iToolIndex = 0; iToolIndex < imageInformations.Length; iToolIndex++) //遍历图像信息
                                            {
                                                if (imageInformations[iToolIndex].Type == Enum.ImageType.Error) //检测结果有缺陷
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        _GetDeepLearningInfo(imageInformations, ref bToolState, ref bDeepLearningState, ref sTypeName);

                                        imageInformations[iToolIndex]._CopyTo(imageInformation);//更新对应工具图像信息
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        else//当前选择某个工具，查询对应工具缺陷图像
                        {
                            switch (relevancyType) //关联类型
                            {
                                case Enum.RelevancyType.None:
                                    rejectedStatistics = dir.GetFiles("*" + "_" + toolIndex.ToString("00") + "_" + "*" + Class.Camera.BMPFile);
                                    if (rejectedStatistics.Length > imageIndex)//图像存在
                                    {
                                        image = new Image<Bgr, byte>(rejectedStatistics[imageIndex].FullName);
                                        //image = GeneralFunction._ReadImage(rejectedStatistics[imageIndex].FullName);

                                        Struct.ImageInformation[] imageInformations = new Struct.ImageInformation[0];
                                        _ReadImageInformation(rejectedStatistics[imageIndex].FullName.Substring(0, rejectedStatistics[imageIndex].FullName.Length - rejectedStatistics[imageIndex].Extension.Length) + Class.Camera.DatFile, ref imageInformations);//读取缺陷图像信息

                                        _GetDeepLearningInfo(imageInformations, ref bToolState, ref bDeepLearningState, ref sTypeName);

                                        imageInformations[toolIndex]._CopyTo(imageInformation);//更新对应工具图像信息
                                    }
                                    break;
                                case Enum.RelevancyType.Inner:
                                    break;
                                case Enum.RelevancyType.Extra:
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：查找深度学习信息
        // 输入参数：1、ImageInformation：imageInformation，缺陷图像信息
        //           2、Boolean：bToolState，深度学习工具使能标记
        //           3、Boolean：bDeepLearningState，深度学习标记
        //           4、string：sTypeName，深度学习结果
        // 输出参数：无
        // 返 回 值：无
        //---------------------------------------------------------------------
        private void _GetDeepLearningInfo(Struct.ImageInformation[] imageInformations, ref Boolean bToolState, ref Boolean bDeepLearningState, ref string sTypeName)
        {
            bToolState = false;
            bDeepLearningState = false;
            sTypeName = "OK";

            for (Int32 i = 0; i < imageInformations.Length; i++) //循环所有工具
            {
                if(imageInformations[i].ToolState && imageInformations[i].DeepLearningState)//含有深度学习工具
                {
                    bToolState = true;
                    bDeepLearningState = true;
                    if (imageInformations[i].Type == Enum.ImageType.Error) //检测结果有缺陷
                    {
                        sTypeName = "NG";
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：返回当前班次
        // 输入参数：1、DateTime:currentDateTime，系统当前时间
        // 输出参数：无
        // 返 回 值：Int32，返回当前班次，索引从1、2、3开始，无效班次-1、-2、-3，0无意义
        //----------------------------------------------------------------------
        public Int32 _GetCurrentShift()
        {
            DateTime currentDateTime = DateTime.Now;

            for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
            {
                DateTime startTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].Start.Hour, sDataOfShift.TimeData[i - 1].Start.Minute, currentDateTime.Second);
                DateTime endTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].End.Hour, sDataOfShift.TimeData[i - 1].End.Minute, currentDateTime.Second);

                _UpdateStartTimeAndEndTime(currentDateTime, ref startTime, ref endTime);

                if ((currentDateTime.CompareTo(startTime) >= 0) && (currentDateTime.CompareTo(endTime) < 0)) //返回当前有效班次
                {
                    return i;
                }

                startTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].End.Hour, sDataOfShift.TimeData[i - 1].End.Minute, currentDateTime.Second);
                endTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i % sDataOfShift.TimeData.Length].Start.Hour, sDataOfShift.TimeData[i % sDataOfShift.TimeData.Length].Start.Minute, currentDateTime.Second);

                _UpdateStartTimeAndEndTime(currentDateTime, ref startTime, ref endTime);

                if ((currentDateTime.CompareTo(startTime) >= 0) && (currentDateTime.CompareTo(endTime) < 0)) //返回当前无效班次
                {
                    return -i;
                }
            }
            return 0;
        }

        //-----------------------------------------------------------------------
        // 功能说明：返回当前班次数据
        // 输入参数：1、Int32：shift，班次索引（班次索引从1、2、3开始，无效班次-1、-2、-3，0无意义）
        // 输出参数：无
        // 返 回 值：ShiftTime，返回当前班次起止时间
        //----------------------------------------------------------------------
        public Struct.ShiftTime _GetCurrentShiftTimeData(Int32 shift)
        {
            if (shift > 0)//当前班次有效
            {
                return sDataOfShift.TimeData[shift - 1];
            }
            else if (shift < 0)//当前空班次
            {
                Struct.ShiftTime shiftTime = new Struct.ShiftTime();
                shiftTime.Start = sDataOfShift.TimeData[(-shift - 1) % sDataOfShift.TimeData.Length].End;
                shiftTime.End = sDataOfShift.TimeData[(-shift) % sDataOfShift.TimeData.Length].Start;

                if (shift == -sDataOfShift.TimeData.Length)//当前空班次为最后一班，班次时间加一天
                {
                    DateTime dateTime = new DateTime(shiftTime.End.Year, shiftTime.End.Month, shiftTime.End.Day, shiftTime.End.Hour, shiftTime.End.Minute, shiftTime.End.Second);
                    dateTime = dateTime.AddDays(1);
                    shiftTime.End._InitData(dateTime);
                }
                return shiftTime;
            }
            else
            {
                Struct.ShiftTime shiftTime = new Struct.ShiftTime();
                return shiftTime;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：返回所有班次数据
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：ShiftTime[]，返回所有班次起止时间信息
        //----------------------------------------------------------------------
        public Struct.ShiftTime[] _GetCurrentShiftTimeArray()
        {
            return sDataOfShift.TimeData;
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除所有历史班次信息
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _DeleteAllCameraInformation()
        {
            if (false == bDeletingStatics) //未执行删除操作
            {
                bDeletingStatics = true;

                for (Byte i = 0; i < CameraNumberMax; i++)//清除所有历史班次内存
                {
                    if ((CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                    {
                        for (Int32 j = 1; j <= sDataOfShift.TimeData.Length; j++)//遍历当前所有班次
                        {
                            if ((sDataOfShift.CurrentIndex > 0) && (j == sDataOfShift.CurrentIndex))//查询当前班次
                            {
                                _DeleteShiftMemory(Convert.ToByte(i + 1), j);
                            }
                        }
                    }
                }
                DirectoryInfo dir = new DirectoryInfo(StatisticsDataPath);

                if (dir.Exists)   //当前路径存在
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        bDeletingStatics = false;
                    }
                }
                bDeletingStatics = false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除当前相机所有班次信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _DeleteSingleCameraInformation(Byte byteIndex)
        {
            if (false == bDeletingStatics) //未执行删除操作
            {
                bDeletingStatics = true;

                for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//清除当前相机所有班次信息内存
                {
                    if ((sDataOfShift.CurrentIndex > 0) && (i == sDataOfShift.CurrentIndex))//查询当前班次
                    {
                        _DeleteShiftMemory(byteIndex, i);
                    }
                }
                DirectoryInfo dir = new DirectoryInfo(_GetAllCameraPath(byteIndex));

                if (dir.Exists) //当前路径存在
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        bDeletingStatics = false;
                    }
                }
                bDeletingStatics = false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除当前班次所有统计信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _DeleteShiftInformation(Byte byteIndex, Int32 shift)
        {
            if (false == bDeletingStatics) //未执行删除操作
            {
                bDeletingStatics = true;

                if ((sDataOfShift.CurrentIndex > 0) && (shift == sDataOfShift.CurrentIndex))//查询当前班次
                {
                    _DeleteShiftMemory(byteIndex, shift);
                }
                DirectoryInfo dir = new DirectoryInfo(_GetCameraShiftPath(byteIndex, shift));

                if (dir.Exists)                                                    //当前路径存在
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        bDeletingStatics = false;
                    }
                }
                bDeletingStatics = false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除历史班次信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        //           3、SYSTEMTIME：startTime,班次开始时间
        //           4、SYSTEMTIME：endTime,班次结束时间
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _DeleteHistoryShiftInformation(Byte byteIndex, Int32 shift, Struct.SYSTEMTIME startTime, Struct.SYSTEMTIME endTime)
        {
            if (false == DeletingStatics) //未执行删除操作
            {
                if ((sDataOfShift.CurrentIndex > 0) && (shift == sDataOfShift.CurrentIndex) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].Start, startTime) == 0) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].End, endTime) == 0))//查询当前班次
                {
                    _DeleteShiftMemory(byteIndex, shift);
                }
                DirectoryInfo dir = new DirectoryInfo(_GetHistoryDayShiftDataPath(byteIndex, shift, startTime, endTime));

                if (dir.Exists)                                                    //当前路径存在
                {
                    try
                    {
                        dir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        bDeletingStatics = false;
                    }
                }
                bDeletingStatics = false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建新班次
        // 输入参数：1、ShiftTime[]：shifttime，班次时间
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CreateNewShift(Struct.ShiftTime[] shifttime)
        {
            if ((shifttime.Length >= iMinShiftNumber) && (shifttime.Length <= iMaxShiftNumber)) //当前班次数量范围1-24
            {
                sDataOfShift.TimeData = new Struct.ShiftTime[shifttime.Length];

                for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
                {
                    sDataOfShift.TimeData[i - 1].Start = new Struct.SYSTEMTIME();
                    sDataOfShift.TimeData[i - 1].Start = shifttime[i - 1].Start;

                    sDataOfShift.TimeData[i - 1].End = new Struct.SYSTEMTIME();
                    sDataOfShift.TimeData[i - 1].End = shifttime[i - 1].End;
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：复制班次时间
        // 输入参数：1、ShiftTime[]：shifttime，班次时间
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref Struct.ShiftTime[] shifttime)
        {
            shifttime = new Struct.ShiftTime[sDataOfShift.TimeData.Length];

            for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
            {
                shifttime[i - 1].Start = new Struct.SYSTEMTIME();
                shifttime[i - 1].Start = sDataOfShift.TimeData[i - 1].Start;

                shifttime[i - 1].End = new Struct.SYSTEMTIME();
                shifttime[i - 1].End = sDataOfShift.TimeData[i - 1].End;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：string，返回字符串形式的班次时间
        //----------------------------------------------------------------------
        public static string _GetDateTime(Struct.SYSTEMTIME systemtime_start, Struct.SYSTEMTIME systemtime_end)
        {
            return _GetDateTime(systemtime_start) + " ~ " + _GetDateTime(systemtime_end);
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string _GetDateTime(Struct.SYSTEMTIME systemtime)
        {
            return systemtime.Year.ToString("D4") + "." + systemtime.Month.ToString("D2") + "." + systemtime.Day.ToString("D2") + "，" + systemtime.Hour.ToString("D2") + "：" + systemtime.Minute.ToString("D2");
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private static string _GetDateTime(DateTime dateTime)
        {
            return dateTime.Year.ToString("D4") + "." + dateTime.Month.ToString("D2") + "." + dateTime.Day.ToString("D2") + "，" + dateTime.Hour.ToString("D2") + "：" + dateTime.Minute.ToString("D2") + "：" + dateTime.Second.ToString("D2") + "：" + dateTime.Millisecond.ToString("D3");
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private static string _GetShiftTime(Struct.SYSTEMTIME systemtime)
        {
            return systemtime.Hour.ToString("D2") + "：" + systemtime.Minute.ToString("D2");
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取字符串形式的班次开始结束时间
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string _GetShiftTime(Struct.SYSTEMTIME systemtime_start, Struct.SYSTEMTIME systemtime_end)
        {
            return _GetShiftTime(systemtime_start) + " ~ " + _GetShiftTime(systemtime_end);
        }

        //-----------------------------------------------------------------------
        // 功能说明：时间比较
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：0，相同；小于0，1小于2；大于0，1大于2
        //----------------------------------------------------------------------
        public static Int32 _Compare(Struct.SYSTEMTIME systemtime_1, Struct.SYSTEMTIME systemtime_2)
        {
            try
            {
                DateTime datetime_1 = new DateTime(systemtime_1.Year, systemtime_1.Month, systemtime_1.Day, systemtime_1.Hour, systemtime_1.Minute, systemtime_1.Second);//比较时间1
                DateTime datetime_2 = new DateTime(systemtime_2.Year, systemtime_2.Month, systemtime_2.Day, systemtime_2.Hour, systemtime_2.Minute, systemtime_2.Second);//比较时间2

                return datetime_1.CompareTo(datetime_2);//返回时间比较结果
            }
            catch (Exception ex)
            {
                if ((systemtime_1.Hour != systemtime_2.Hour) || (systemtime_1.Minute != systemtime_2.Minute))
                {
                    return -1;//返回时间比较结果
                }
                else
                {
                    return 0;//返回时间比较结果
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：比较
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：true，相同；false，不同
        //----------------------------------------------------------------------
        public static Boolean _Compare(Struct.ShiftTime shifttime_1, Struct.ShiftTime shifttime_2)
        {
            if ((_Compare(shifttime_1.Start, shifttime_2.Start) != 0) || (_Compare(shifttime_1.End, shifttime_2.End) != 0))//班次起止时间发生变化
            {
                return false;
            }
            return true;
        }

        //-----------------------------------------------------------------------
        // 功能说明：班次时间比较
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：true，相同；false，不同
        //----------------------------------------------------------------------
        public static Boolean _Compare(Struct.ShiftTime[] shifttime_1, Struct.ShiftTime[] shifttime_2)
        {
            if (shifttime_1.Length == shifttime_2.Length) //当前班次数量发生变化
            {
                for (Int32 i = 0; i < shifttime_1.Length; i++)//遍历当前所有班次
                {
                    if (!_Compare(shifttime_1[i], shifttime_2[i]))//班次起止时间发生变化
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：检查班次设置是否合法
        // 输入参数：1、ShiftTime[]：shifttime，班次时间
        //           2、Int32：minShiftNumber，班次数量下限
        //           2、Int32：maxShiftNumber，班次数量上限
        // 输出参数：无
        // 返 回 值：true，合法；false，非法
        //----------------------------------------------------------------------
        public static Boolean _Check(Struct.ShiftTime[] shifttime)
        {
            if ((shifttime.Length >= iMinShiftNumber) && (shifttime.Length <= iMaxShiftNumber)) //当前班次数量范围1-24
            {
                DateTime currentDateTime = DateTime.Now;

                if (shifttime.Length > 1)//当前大于一个班次
                {
                    for (Int32 i = 1; i < shifttime.Length; i++)//遍历所有新班次
                    {
                        DateTime startTimeNew = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shifttime[i].Start.Hour, shifttime[i].Start.Minute, currentDateTime.Second);
                        DateTime endTimeNew = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shifttime[i].End.Hour, shifttime[i].End.Minute, currentDateTime.Second);

                        for (Int32 j = 0; j < i; j++)//遍历所有旧班次
                        {
                            DateTime startTimeOld = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shifttime[j].Start.Hour, shifttime[j].Start.Minute, currentDateTime.Second);
                            DateTime endTimeOld = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shifttime[j].End.Hour, shifttime[j].End.Minute, currentDateTime.Second);

                            if (startTimeOld.CompareTo(endTimeOld) < 0)//旧班次运行起止时间未发生穿越
                            {
                                if (startTimeNew.CompareTo(endTimeNew) < 0)//新班次运行起止时间未发生穿越
                                {
                                    if (startTimeNew.CompareTo(endTimeOld) < 0)//新班次开始时间必须大于等于老班次结束时间
                                    {
                                        return false;
                                    }
                                }
                                else//新班次运行起止时间发生穿越
                                {
                                    if (i < (shifttime.Length - 1))//只允许末班次穿越
                                    {
                                        return false;
                                    }
                                    else//末班次发生穿越
                                    {
                                        if (startTimeNew.CompareTo(endTimeOld) >= 0)//新班次开始时间必须大于等于老班次结束时间
                                        {
                                            if (endTimeNew.CompareTo(startTimeOld) > 0)//新班次结束时间必须小于等于老班次开始时间
                                            {
                                                return false;
                                            }
                                        }
                                        else//新班次开始时间小于老班次结束时间
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                            else //旧班次运行起止时间发生穿越
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }
                else//当前只有一个班次
                {
                    return true;
                }
            }
            else//当前班次数量超限
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：检查日期时间是否合法
        // 输入参数：1、SYSTEMTIME：systemtime，时间
        // 输出参数：无
        // 返 回 值：true，合法；false，非法
        //----------------------------------------------------------------------
        public static Boolean _Check(Struct.SYSTEMTIME systemtime)
        {
            Boolean bReturn = false;//返回值

            DateTime datetime = new DateTime();//临时变量

            try
            {
                datetime = new DateTime(systemtime.Year, systemtime.Month, systemtime.Day, systemtime.Hour, systemtime.Minute, systemtime.Second);

                bReturn = true;
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            //

            return bReturn;
        }

        //-----------------------------------------------------------------------
        // 功能说明：检查班次设置是否合法
        // 输入参数：1、SYSTEMTIME：systemtime_1，时间1
        //           2、SYSTEMTIME：systemtime_2，时间2
        // 输出参数：无
        // 返 回 值：true，合法；false，非法
        //----------------------------------------------------------------------
        public static Boolean _Check(Struct.SYSTEMTIME systemtime_1, Struct.SYSTEMTIME systemtime_2)
        {
            if (_Compare(systemtime_1, systemtime_2) < 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：创建新班次
        // 输入参数：1、Int32：ishiftnumber，班次数量
        //           2、Int32：minShiftNumber，班次数量下限
        //           2、Int32：maxShiftNumber，班次数量上限
        // 输出参数：无
        // 返 回 值：true，合法；false，非法
        //----------------------------------------------------------------------
        public static Struct.ShiftTime[] _CreateNewShift(Int32 ishiftnumber)
        {
            if ((ishiftnumber >= iMinShiftNumber) && (ishiftnumber <= iMaxShiftNumber)) //当前班次数量范围1-24
            {
                Int32 minuteBuff = 24 * 60 / ishiftnumber;
                Struct.ShiftTime[] shiftTimeData = new Struct.ShiftTime[ishiftnumber];

                DateTime currentDateTime = DateTime.Now;
                DateTime dateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);

                for (Int32 i = 0; i < ishiftnumber; i++)
                {
                    shiftTimeData[i].Start = new Struct.SYSTEMTIME();
                    shiftTimeData[i].Start._InitData(dateTime);
                    shiftTimeData[i].Start.Hour = Convert.ToUInt16(dateTime.Hour);
                    shiftTimeData[i].Start.Minute = Convert.ToUInt16(dateTime.Minute);

                    if (i == (ishiftnumber - 1))//当前末班次结束时间为0点
                    {
                        shiftTimeData[i].End = new Struct.SYSTEMTIME();
                        dateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);
                        dateTime.AddDays(1);
                        shiftTimeData[i].End._InitData(dateTime);
                    }
                    else
                    {
                        dateTime = dateTime.AddMinutes(minuteBuff);

                        shiftTimeData[i].End = new Struct.SYSTEMTIME();
                        shiftTimeData[i].End._InitData(dateTime);
                        shiftTimeData[i].End.Hour = Convert.ToUInt16(dateTime.Hour);
                        shiftTimeData[i].End.Minute = Convert.ToUInt16(dateTime.Minute);
                    }
                }
                return shiftTimeData;
            }
            else
            {
                return null;
            }
        }

        //

        //-----------------------------------------------------------------------
        // 功能说明：读取班次使能状态
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：Boolean，返回Shift文件是否存在标记
        //----------------------------------------------------------------------
        public Boolean _ReadShiftState()
        {
            FileStream filestream = null;
            BinaryReader binaryreader = null;

            try
            {
                filestream = new FileStream(sShiftDataPath + sShiftFileName, FileMode.Open); //打开Shift文件
                binaryreader = new BinaryReader(filestream);

                bShiftState = binaryreader.ReadBoolean();//读取班次使能状态

                binaryreader.Close();//关闭Shift文件
                filestream.Close();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：保存班次使能状态
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _WriteShiftState()
        {
            FileStream filestream = new FileStream(sShiftDataPath + sShiftFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开Shift文件
            BinaryWriter binarywriter = new BinaryWriter(filestream);//写入系统文件数据

            binarywriter.Write(bShiftState);//写入班次使能状态

            binarywriter.Close();//关闭Shift文件
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取班次时间函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _ReadShiftTime()
        {
            FileStream filestream = new FileStream(sShiftDataPath + sShiftFileName, FileMode.Open); //打开Shift文件
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek(0x10, SeekOrigin.Begin);
            iShiftNumber = binaryreader.ReadInt32();//读取班次数量

            sDataOfShift.TimeData = new Struct.ShiftTime[iShiftNumber];
            for (Int32 i = 0; i < iShiftNumber; i++)//初始化班次时间数组
            {
                sDataOfShift.TimeData[i] = new Struct.ShiftTime();
                sDataOfShift.TimeData[i]._InitData();
            }

            if ((iShiftNumber >= iMinShiftNumber) && (iShiftNumber <= iMaxShiftNumber))//当前班次有效
            {
                for (Int32 i = 1; i <= iShiftNumber; i++)//遍历当前所有班次
                {
                    filestream.Seek(0x20 + (i - 1) * 0x08, SeekOrigin.Begin);

                    sDataOfShift.TimeData[i - 1].Start.Hour = binaryreader.ReadUInt16();//读取班次开始时间
                    sDataOfShift.TimeData[i - 1].Start.Minute = binaryreader.ReadUInt16();

                    sDataOfShift.TimeData[i - 1].End.Hour = binaryreader.ReadUInt16();//读取班次结束时间
                    sDataOfShift.TimeData[i - 1].End.Minute = binaryreader.ReadUInt16();
                }
            }
            _CopyTo(ref sDataOfShift.TimeDataOld);

            binaryreader.Close();//关闭Shift文件
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：初始化InformationOfStatistics
        // 输入参数：1、Byte[]：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32[]：toolNumber，对应相机工具个数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _InitInformationOfStatistics(Byte[] byteIndex, Int32[] toolNumber)
        {
            sDataOfShift.InformationOfStatistics = new Struct.StatisticsInformation[sDataOfShift.TimeData.Length];
            for (Int32 i = 0; i < sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
            {
                sDataOfShift.InformationOfStatistics[i] = new Struct.StatisticsInformation();

                sDataOfShift.InformationOfStatistics[i].DataOfStatistics = new Struct.StatisticsData[1];
                sDataOfShift.InformationOfStatistics[i].DataOfStatistics[0] = new Struct.StatisticsData();
                sDataOfShift.InformationOfStatistics[i].DataOfStatistics[0].BrandName = "";

                sDataOfShift.InformationOfStatistics[i].DataOfStatistics[0].CameraStatisticsData = new Struct.StatisticsData_Camera[CameraNumberMax];
                for (Int32 j = 0; j < CameraNumberMax; j++)//遍历当前所有相机
                {
                    if ((CameraChooseState & (0x01 << j)) != 0)//当前相机开启
                    {
                        sDataOfShift.InformationOfStatistics[i].DataOfStatistics[0].CameraStatisticsData[j] = new Struct.StatisticsData_Camera();
                        sDataOfShift.InformationOfStatistics[i].DataOfStatistics[0].CameraStatisticsData[j].RejectedStatistics_Tool = new UInt32[toolNumber[j]];
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取当前班次信息函数
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _ReadCurrentShiftInformation(Byte byteIndex, Int32 shift)
        {
            try
            {
                if (shift > 0)//当前班次有效
                {
                    DirectoryInfo dir = new DirectoryInfo(_GetCurrentDayShiftDataPath(byteIndex, shift));

                    if (dir.Exists) //当前班次统计路径存在
                    {
                        if (File.Exists(dir.FullName + StatisticsFileName))  //统计文件存在
                        {
                            FileStream filestream = new FileStream(dir.FullName + StatisticsFileName, FileMode.Open); //打开统计文件
                            BinaryReader binaryreader = new BinaryReader(filestream);

                            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera = (Enum.CameraType)binaryreader.ReadByte();//读取相机类型
                            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber = binaryreader.ReadUInt32();//读取检测烟包总数

                            filestream.Seek((0x20), SeekOrigin.Begin);
                            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].BrandName = binaryreader.ReadString();//读取烟包品牌名称

                            filestream.Seek((0x30), SeekOrigin.Begin);
                            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber = binaryreader.ReadUInt32();
                            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy = binaryreader.ReadUInt32();

                            filestream.Seek((0x40), SeekOrigin.Begin);
                            Int32 toolNumber = binaryreader.ReadInt32();

                            if (toolNumber == sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length)
                            {
                                filestream.Seek((0x50), SeekOrigin.Begin);
                                for (Int32 i = 0; i < sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length; i++)//遍历当前所有工具
                                {
                                    sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool[i] = binaryreader.ReadUInt32();
                                }
                                binaryreader.Close();//关闭统计文件
                                filestream.Close();
                            }
                            else
                            {
                                binaryreader.Close();//关闭统计文件
                                filestream.Close();

                                dir.Delete(true);

                                sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber = 0;//读取检测烟包总数

                                sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber = 0;
                                sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy = 0;

                                for (Int32 i = 0; i < sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length; i++)//遍历当前所有工具
                                {
                                    sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool[i] = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取相机路径
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        // 输出参数：无
        // 返 回 值：string，返回当前相机路径
        //----------------------------------------------------------------------
        private static string _GetAllCameraPath(Byte byteIndex)
        {
            return StatisticsDataPath + ((Enum.PortType)byteIndex).ToString() + "\\";//统计数据相机路径(如：D:\\X6S\\Statistics\\Camera_1\\)
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取班次路径
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：string，返回当前班次路径
        //----------------------------------------------------------------------
        private string _GetCameraShiftPath(Byte byteIndex, Int32 shift)
        {
            return _GetAllCameraPath(byteIndex) + shift.ToString() + "\\";//统计数据班次路径(如：D:\\X6S\\Statistics\\Top\\1);//统计数据相机路径(如：D:\\X6S\\Statistics\\Top\\)
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取统计信息路径
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：string，返回当前班次统计文件路径
        //----------------------------------------------------------------------
        private string _GetCurrentDayShiftDataPath(Byte byteIndex, Int32 shift)
        {
            return _GetCameraShiftPath(byteIndex, shift) + _GetDateTime(sDataOfShift.TimeData[shift - 1].Start, sDataOfShift.TimeData[shift - 1].End) + "\\";//统计数据班次路径(如：D:\\X6S\\Statistics\\Top\\1\\2016.01.01,08:00~2016.01.01,16:00\\)
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取统计信息路径
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        //           3、SYSTEMTIME：startTime,班次开始时间
        //           4、SYSTEMTIME：endTime,班次结束时间
        // 输出参数：无
        // 返 回 值：string，返回历史班次统计文件路径
        //----------------------------------------------------------------------
        private string _GetHistoryDayShiftDataPath(Byte byteIndex, Int32 shift, Struct.SYSTEMTIME startTime, Struct.SYSTEMTIME endTime)
        {
            return _GetCameraShiftPath(byteIndex, shift) + _GetDateTime(startTime, endTime) + "\\";//统计数据班次路径(如：D:\\X6S\\Statistics\\Top\\1\\2016.01.01,08:00~2016.01.01,16:00\\)
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器初始化函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void timer_Init()
        {
            timer.Elapsed += new ElapsedEventHandler(timer_Tick);
            timer.Interval = 1000;
            timer.Enabled = true;
        }

        //-----------------------------------------------------------------------
        // 功能说明：定时器中断响应函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void timer_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;

            sDataOfShift.CurrentIndex = _GetCurrentShift(dateTime);//获取当前班次

            _InitShiftTime(dateTime, ref sDataOfShift.TimeDataOld);
            
            if ((sDataOfShift.TimeData.Length == 1) && (sDataOfShift.TimeData[0].Start.Hour == sDataOfShift.TimeData[0].End.Hour) && (sDataOfShift.TimeData[0].Start.Minute == sDataOfShift.TimeData[0].End.Minute))//当前只有一个班次，且班次起止时间相同
            {
                if ((dateTime.Hour == sDataOfShift.TimeDataOld[0].End.Hour) && (dateTime.Minute == sDataOfShift.TimeDataOld[0].End.Minute))//班次发生切换
                {
                    SingleShiftChangeState++;

                    if (SingleShiftChangeState == 1)
                    {
                        if (sDataOfShift.CurrentIndexOld > 0)//更换前班次为有效班次
                        {
                            _Change();//调用班次改变函数

                            for (Byte i = 0; i < CameraNumberMax; i++)//班次切换，清除老班次信息
                            {
                                if ((CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                                {
                                    _DeleteShiftMemory(Convert.ToByte(i + 1), sDataOfShift.CurrentIndexOld);
                                }
                            }
                        }
                        sDataOfShift.CurrentIndexOld = sDataOfShift.CurrentIndex;
                        _InitShiftTime(DateTime.Now, ref sDataOfShift.TimeData);//更新系统班次日期

                        if (bShiftTimeChangeState == false)//班次状态发生改变
                        {
                            bShiftTimeChangeState = true;
                        }
                    }
                }
                else
                {
                    SingleShiftChangeState = 0;
                }
            }
            else//当前未多班次或者但班次没有占满24小时
            {
                if (sDataOfShift.CurrentIndexOld != sDataOfShift.CurrentIndex)//班次发生改变
                {
                    if (sDataOfShift.CurrentIndexOld >= 0)//更换前班次为有效班次
                    {
                        _Change();//调用班次改变函数

                        for (Byte i = 0; i < CameraNumberMax; i++)//班次切换，清除老班次信息
                        {
                            if ((CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                            {
                                _DeleteShiftMemory(Convert.ToByte(i + 1), sDataOfShift.CurrentIndexOld);
                            }
                        }
                    }
                    sDataOfShift.CurrentIndexOld = sDataOfShift.CurrentIndex;
                    _InitShiftTime(DateTime.Now, ref sDataOfShift.TimeData);//更新系统班次日期

                    if (bShiftTimeChangeState == false)//班次状态发生改变
                    {
                        bShiftTimeChangeState = true;
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：班次状态改变函数
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _Change()
        {
            try
            {
                //事件

                if (null != ShiftChange)//有效
                {
                    ShiftChange(this, new EventArgs());
                }

                //待定
            }
            catch (global::System.Exception ex)
            {
                ex.ToString();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：检查各班次统计信息
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        //           3、StatisticsRecordList：statisticsRecordDataList，上位机统计信息
        // 输出参数：无
        // 返 回 值：Int32，返回各历史班次统计信息
        //----------------------------------------------------------------------
        private void _CheckHistoryShiftStaticNumber(Byte byteIndex, Int32 shift, ref Struct.StatisticsRecordList statisticsRecordDataList)
        {
            Struct.SYSTEMTIME start = new Struct.SYSTEMTIME();
            Struct.SYSTEMTIME end = new Struct.SYSTEMTIME();

            DirectoryInfo dir = new DirectoryInfo(_GetCameraShiftPath(byteIndex, shift));

            if (dir.Exists) //当前路径存在
            {
                DirectoryInfo[] subDirectoryInfo = dir.GetDirectories();

                if ((sDataOfShift.CurrentIndex > 0) && (shift == sDataOfShift.CurrentIndex))//查询当前班次统计信息
                {
                    if (Directory.Exists(_GetCurrentDayShiftDataPath(byteIndex, shift)))//当前班次统计文件夹不存在
                    {
                        statisticsRecordDataList.RecordNumber += subDirectoryInfo.Length;//班次统计信息计数
                        statisticsRecordDataList.RecordListData[shift - 1]._InitData(subDirectoryInfo.Length);//初始化
                    }
                    else
                    {
                        statisticsRecordDataList.RecordNumber += subDirectoryInfo.Length + 1;//班次统计信息计数
                        statisticsRecordDataList.RecordListData[shift - 1]._InitData(subDirectoryInfo.Length + 1);//初始化
                    }

                    for (Int32 i = 0; i < subDirectoryInfo.Length; i++)//遍历班次所有统计信息
                    {
                        _GetDateTime(subDirectoryInfo[i].Name, ref start, ref end);//班次起止时间有效

                        Boolean dateTimeSub = (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].Start, start) == 0) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].End, end) == 0);

                        if (dateTimeSub)//当前班次索引
                        {
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].Start = sDataOfShift.TimeData[shift - 1].Start;
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].End = sDataOfShift.TimeData[shift - 1].End;//初始化班次起止时间

                            statisticsRecordDataList.RecordListData[shift - 1].BrandName[i] = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].BrandName;//初始化烟包品牌
                        }
                        else
                        {
                            statisticsRecordDataList.RecordListData[shift - 1].CurrentStatisticsRecordIndex++;
                           
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].Start = start;
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].End = end;//初始化班次起止时间

                            string filePath = subDirectoryInfo[i].FullName + "\\" + StatisticsFileName;

                            try
                            {
                                if (File.Exists(filePath))//班次统计文件存在
                                {
                                    FileStream filestream = new FileStream(filePath, FileMode.Open); //打开统计文件
                                    BinaryReader binaryreader = new BinaryReader(filestream);

                                    filestream.Seek((0x20), SeekOrigin.Begin);
                                    statisticsRecordDataList.RecordListData[shift - 1].BrandName[i] = binaryreader.ReadString();//读取烟包品牌名称

                                    binaryreader.Close();//关闭统计文件
                                    filestream.Close();
                                }
                            }
                            catch(Exception ex)
                            {

                            }
                        }
                    }

                    if (statisticsRecordDataList.RecordListData[shift - 1].TimeData.Length > 1)//当前班次统计条目大于1，根据当前班次位置排序
                    {
                        for (Int32 i = statisticsRecordDataList.RecordListData[shift - 1].TimeData.Length - 1; i > statisticsRecordDataList.RecordListData[shift - 1].CurrentStatisticsRecordIndex; i--)//遍历班次所有统计信息
                        {
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].Start = statisticsRecordDataList.RecordListData[shift - 1].TimeData[i - 1].Start;
                            statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].End = statisticsRecordDataList.RecordListData[shift - 1].TimeData[i - 1].End;//初始化班次起止时间

                            statisticsRecordDataList.RecordListData[shift - 1].BrandName[i] = statisticsRecordDataList.RecordListData[shift - 1].BrandName[i - 1];//初始化烟包品牌
                        }
                    }
                    statisticsRecordDataList.RecordListData[shift - 1].TimeData[statisticsRecordDataList.RecordListData[shift - 1].CurrentStatisticsRecordIndex].Start = sDataOfShift.TimeData[shift - 1].Start;
                    statisticsRecordDataList.RecordListData[shift - 1].TimeData[statisticsRecordDataList.RecordListData[shift - 1].CurrentStatisticsRecordIndex].End = sDataOfShift.TimeData[shift - 1].End;//初始化班次起止时间

                    statisticsRecordDataList.RecordListData[shift - 1].BrandName[statisticsRecordDataList.RecordListData[shift - 1].CurrentStatisticsRecordIndex] = sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].BrandName;//初始化烟包品牌
                }
                else
                {
                    statisticsRecordDataList.RecordNumber += subDirectoryInfo.Length;//班次统计信息计数
                    statisticsRecordDataList.RecordListData[shift - 1]._InitData(subDirectoryInfo.Length);//初始化

                    for (Int32 i = 0; i < subDirectoryInfo.Length; i++)//遍历班次所有统计信息
                    {
                        _GetDateTime(subDirectoryInfo[i].Name, ref start, ref end);//班次起止时间有效

                        statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].Start = start;
                        statisticsRecordDataList.RecordListData[shift - 1].TimeData[i].End = end;//初始化班次起止时间

                        string filePath = subDirectoryInfo[i].FullName + "\\" + StatisticsFileName;

                        try
                        {
                            if (File.Exists(filePath))//班次统计文件存在
                            {
                                FileStream filestream = new FileStream(filePath, FileMode.Open); //打开统计文件
                                BinaryReader binaryreader = new BinaryReader(filestream);

                                filestream.Seek((0x20), SeekOrigin.Begin);
                                statisticsRecordDataList.RecordListData[shift - 1].BrandName[i] = binaryreader.ReadString();//读取烟包品牌名称

                                binaryreader.Close();//关闭统计文件
                                filestream.Close();
                            }
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：根据文件信息获取班次起止时间
        // 输入参数：1、string：subDirectoryInfo，文件夹名称
        //           2、SYSTEMTIME：start，班次开始时间
        //           3、SYSTEMTIME：end，班次结束时间
        // 输出参数：无
        // 返 回 值：Boolean，解析班次起止时间是否成功
        //----------------------------------------------------------------------
        private static Boolean _GetDateTime(string subDirectoryInfo, ref Struct.SYSTEMTIME start, ref Struct.SYSTEMTIME end)
        {
            try
            {
                Int32 index = 0;

                start.Year = Convert.ToUInt16(subDirectoryInfo.Substring(index, 4));

                index += 5;
                start.Month = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 3;
                start.Day = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 3;
                start.Hour = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 3;
                start.Minute = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 5;
                end.Year = Convert.ToUInt16(subDirectoryInfo.Substring(index, 4));

                index += 5;
                end.Month = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 3;
                end.Day = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                index += 3;
                end.Hour = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));
                
                index += 3;
                end.Minute = Convert.ToUInt16(subDirectoryInfo.Substring(index, 2));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：检查硬盘占用率是否超限
        // 输入参数：1、Double：dDriverFreeRate，硬盘剩余空间百分比
        // 输出参数：无
        // 返 回 值：Boolean，返回存盘驱动器是否有多余存储空间
        //----------------------------------------------------------------------
        private static Boolean _CheckDiskMemory(Double dDriverFreeRate)
        {
            Boolean checkResult = false;

            DirectoryInfo directoryInfo = new DirectoryInfo(sShiftDataPath);
            DriveInfo[] driveInfo = DriveInfo.GetDrives();

            foreach (DriveInfo d in driveInfo)
            {
                if (d.RootDirectory.Name == directoryInfo.Root.Name)//当前根目录有效
                {
                    if ((Double)(d.TotalSize - d.TotalFreeSpace) > (Double)d.TotalSize * dDriverFreeRate)//磁盘最小内存保留
                    {
                        checkResult = true;
                        break;
                    }
                }
            }
            return checkResult;
        }

        ////-----------------------------------------------------------------------
        //// 功能说明：读取图像信息
        //// 输入参数：1、string：fileStream，文件路径
        ////           2、Struct.ImageInformation：information，图像信息
        //// 输出参数：无
        //// 返 回 值：无
        ////----------------------------------------------------------------------
        //private void _ReadImageInformation(string filePath, ref Struct.ImageInformation information)
        //{
        //    FileStream fileStream = new FileStream(filePath, FileMode.Open); //打开REJECT文件
        //    BinaryReader binaryReader = new BinaryReader(fileStream);

        //    Int32 i = 0;//循环控制变量


        //    fileStream.Seek(0x000, SeekOrigin.Begin);
        //    information.Valid = binaryReader.ReadBoolean();//图像是否有效。true：是；false：否

        //    fileStream.Seek(0x010, SeekOrigin.Begin);
        //    information.ToolsIndex = binaryReader.ReadInt32();//图像所属的工具索引值（从0开始）

        //    fileStream.Seek(0x020, SeekOrigin.Begin);
        //    information.Type = (Enum.ImageType)binaryReader.ReadByte();//图像类型

        //    fileStream.Seek(0x030, SeekOrigin.Begin);
        //    information.Scale = binaryReader.ReadDouble();//缩放比例

        //    fileStream.Seek(0x040, SeekOrigin.Begin);
        //    information.Name = binaryReader.ReadString();//信息名称

        //    if (null == information.Value)//无效
        //    {
        //        information.Value = new Boolean[Struct.ImageInformation.TotalNumber];

        //        for (i = 0; i < Struct.ImageInformation.TotalNumber; i++)//
        //        {
        //            fileStream.Seek((0x070 + i * 0x010), SeekOrigin.Begin);
        //            information.Value[i] = binaryReader.ReadBoolean();//区块的数值。取值范围：true，表示区块有效；false，表示区块无效
        //        }
        //    }

        //    fileStream.Seek(0x070 + i * 0x010, SeekOrigin.Begin);
        //    information.ValueDisplay = binaryReader.ReadBoolean();//在显示图像的标题栏中是否显示最小值、最大值和当前值。true：是；false：否

        //    fileStream.Seek(0x070 + (i + 1) * 0x010, SeekOrigin.Begin);
        //    information.MinValue = binaryReader.ReadInt16();//最小值

        //    fileStream.Seek(0x070 + (i + 2) * 0x010, SeekOrigin.Begin);
        //    information.MaxValue = binaryReader.ReadInt16();//最大值

        //    fileStream.Seek(0x070 + (i + 3) * 0x010, SeekOrigin.Begin);
        //    information.CurrentValue = binaryReader.ReadInt16();//当前值

        //    fileStream.Seek(0x070 + (i + 4) * 0x010, SeekOrigin.Begin);
        //    information.DateTimeImage = DateTime.FromBinary(binaryReader.ReadInt64());//图像产生的时间

        //    fileStream.Seek(0x070 + (i + 5) * 0x010, SeekOrigin.Begin);
        //    information.ErrorValue = binaryReader.ReadInt16();//图像显示的Error数值（取值为-1，表示该数值无意义，即在图像上不显示）

        //    fileStream.Seek(0x070 + (i + 6) * 0x010, SeekOrigin.Begin);
        //    information.StepValue = binaryReader.ReadInt16();//图像显示的Step数值（取值为-1，表示该数值无意义，即在图像上不显示）

        //    binaryReader.Close();//关闭图像信息文件
        //    fileStream.Close();
        //}

        //-----------------------------------------------------------------------
        // 功能说明：写入图像信息
        // 输入参数：1、string：fileStream，文件路径
        //           2、Struct.ImageInformation[]：informations，图像信息
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _WriteImageInformation(string filePath, Struct.ImageInformation[] informations)
        {
            MemoryStream memoryStream = new MemoryStream();//流对象
            BinaryFormatter Formatter = new BinaryFormatter();//格式化对象
            Formatter.Serialize(memoryStream, informations);

            memoryStream.Position = 0;
            Byte[] memoryStreamBytes = new Byte[memoryStream.Length];
            memoryStream.Read(memoryStreamBytes, 0, (Int32)memoryStream.Length);

            FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate); //打开REJECT文件
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(memoryStreamBytes);

            binaryWriter.Close();//关闭图像信息文件
            fileStream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：读取图像信息
        // 输入参数：1、string：fileStream，文件路径
        //           2、Struct.ImageInformation[]：informations，图像信息
        // 输出参数：无
        // 返 回 值：Boolean，读取图像信息状态
        //----------------------------------------------------------------------
        private Boolean _ReadImageInformation(string filePath, ref Struct.ImageInformation[] informations)
        {
            FileStream fileStream = null;
            BinaryReader binaryreader = null;

            try
            {
                fileStream = new FileStream(filePath, FileMode.Open); //打开REJECT文件
                binaryreader = new BinaryReader(fileStream);

                Byte[] bData = binaryreader.ReadBytes((Int32)(fileStream.Length));

                binaryreader.Close();//关闭图像信息文件
                fileStream.Close();

                MemoryStream MemoryStream = new MemoryStream();//流对象
                MemoryStream.Write(bData, 0, bData.Length);
                MemoryStream.Position = 0;

                BinaryFormatter Formatter = new BinaryFormatter();//格式化对象
                informations = (Struct.ImageInformation[])Formatter.Deserialize(MemoryStream);
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            if (null != binaryreader)
            {
                binaryreader.Close();
            }

            if (null != fileStream)
            {
                fileStream.Close();
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新系统班次日期
        // 输入参数：1、DateTime：currentDateTime，当前系统时间
        //           2、ShiftTime[]：shiftTime，班次时间数组
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _InitShiftTime(DateTime currentDateTime, ref Struct.ShiftTime[] shiftTime)
        {
            Boolean currentDateTimeState = false;
            DateTime dateTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shiftTime[0].Start.Hour, shiftTime[0].Start.Minute, currentDateTime.Second);

            if (currentDateTime.CompareTo(dateTime) <= 0)//当前时间小于第一班次开始时间
            {
                currentDateTimeState = true;
            }
                
            for (Int32 i = 1; i <= shiftTime.Length; i++)//遍历当前所有班次
            {
                DateTime startTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shiftTime[i - 1].Start.Hour, shiftTime[i - 1].Start.Minute, currentDateTime.Second);
                DateTime endTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, shiftTime[i - 1].End.Hour, shiftTime[i - 1].End.Minute, currentDateTime.Second);

                if (startTime.CompareTo(endTime) >= 0)//当前有效班次发生穿越
                {
                    endTime = endTime.AddDays(1);
                }

                if (currentDateTimeState)//当前时间小于第一班次开始时间
                {
                    startTime = startTime.AddDays(-1);
                    endTime = endTime.AddDays(-1);
                }
                shiftTime[i - 1].Start._InitData(startTime);
                shiftTime[i - 1].End._InitData(endTime);
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：返回当前班次
        // 输入参数：1、DateTime:currentDateTime，系统当前时间
        // 输出参数：无
        // 返 回 值：Int32，返回当前班次
        //----------------------------------------------------------------------
        private Int32 _GetCurrentShift(DateTime currentDateTime)
        {
            for (Int32 i = 1; i <= sDataOfShift.TimeData.Length; i++)//遍历当前所有班次
            {
                DateTime startTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].Start.Hour, sDataOfShift.TimeData[i - 1].Start.Minute, currentDateTime.Second);
                DateTime endTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].End.Hour, sDataOfShift.TimeData[i - 1].End.Minute, currentDateTime.Second);

                _UpdateStartTimeAndEndTime(currentDateTime, ref startTime, ref endTime);

                if ((currentDateTime.CompareTo(startTime) >= 0) && (currentDateTime.CompareTo(endTime) < 0)) //返回当前有效班次
                {
                    return sDataOfShift.CurrentIndex = i;
                }

                startTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i - 1].End.Hour, sDataOfShift.TimeData[i - 1].End.Minute, currentDateTime.Second);
                endTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day, sDataOfShift.TimeData[i % sDataOfShift.TimeData.Length].Start.Hour, sDataOfShift.TimeData[i % sDataOfShift.TimeData.Length].Start.Minute, currentDateTime.Second);

                _UpdateStartTimeAndEndTime(currentDateTime, ref startTime, ref endTime);

                if ((currentDateTime.CompareTo(startTime) >= 0) && (currentDateTime.CompareTo(endTime) < 0)) //返回当前无效班次
                {
                    return sDataOfShift.CurrentIndex = -i;
                }
            }
            return 0;
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：更新班次起止时间信息
        // 输入参数：1、DateTime:currentDateTime，系统当前时间
        //           2、DateTime:startTime，班次开始时间
        //           3、DateTime:endTime，班次结束时间
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _UpdateStartTimeAndEndTime(DateTime currentDateTime, ref DateTime startTime, ref  DateTime endTime)
        {
            if (sDataOfShift.TimeData.Length > 1)//多班次时，班次首位时间不允许相同
            {
                if (startTime.CompareTo(endTime) > 0)//当前有效班次发生穿越
                {
                    if (currentDateTime.CompareTo(startTime) >= 0)//当前时间大于班次开始时间，未穿越到第二天
                    {
                        endTime = endTime.AddDays(1);
                    }
                    else//当前时间穿越到第二天
                    {
                        startTime = startTime.AddDays(-1);
                    }
                }
            }
            else//单班次时，班次首位时间相同，存在穿越现象
            {
                if (startTime.CompareTo(endTime) >= 0)//当前有效班次发生穿越
                {
                    if (currentDateTime.CompareTo(startTime) >= 0)//当前时间大于班次开始时间，未穿越到第二天
                    {
                        endTime = endTime.AddDays(1);
                    }
                    else//当前时间穿越到第二天
                    {
                        startTime = startTime.AddDays(-1);
                    }
                }
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除当前班次所有统计信息内存
        // 输入参数：1、Byte：byteIndex，相机类型索引值（相机类型数值 - 1）；byteType取值为2，该值表示相机端口索引值（相机端口数值 - 1））
        //           2、Int32：shift,历史班次
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private void _DeleteShiftMemory(Byte byteIndex, Int32 shift)
        {
            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].TypeOfCamera = (Enum.CameraType)0;
            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].InspectedNumber = 0;

            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber = 0;
            sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedNumber_Relevancy = 0;

            for (Int32 j = 0; j < sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool.Length; j++)//遍历当前所有工具
            {
                sDataOfShift.InformationOfStatistics[shift - 1].DataOfStatistics[0].CameraStatisticsData[byteIndex - 1].RejectedStatistics_Tool[j] = 0;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除统计信息
        // 输入参数：1、Byte：bCameraNumberMax，最大挂载相机数量
        //           2、Byte：bCameraChooseState，被启用相机标记
        //           3、Double：dSourceDriverFreeRate，硬盘空闲百分率（基础判断条件）
        //           3、Double：dDstDriverFreeRate，硬盘空闲百分率（目标）
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _DeleteStatics(Byte bCameraNumberMax, Byte bCameraChooseState, Double dSourceDriverFreeRate, Double dDstDriverFreeRate)
        {
            if (false == bDeletingStatics) //未执行删除操作
            {
                if ((true == _CheckDiskMemory(dSourceDriverFreeRate)) && (_GetDirectoryLength(StatisticsDataPath) > 1024 * 1024))//计算启动时当前根目录存储空间是否满足要求，且当前统计文件夹不小于1MB
                {
                    bDeletingStatics = true;
                }

                Int32 iRecicleCount = 0;
                while (true == _CheckDiskMemory(dDstDriverFreeRate))//计算当前根目录存储空间是否满足要求
                {
                    bDeletingStatics = true;

                    for (Byte i = 0; i < CameraNumberMax; i++)//清除所有历史班次内存
                    {
                        if ((CameraChooseState & (0x01 << i)) != 0)//当前相机开启
                        {
                            for (Int32 j = 1; j <= sDataOfShift.TimeData.Length; j++)//遍历当前所有班次
                            {
                                DirectoryInfo dir = new DirectoryInfo(_GetCameraShiftPath(Convert.ToByte(i + 1), j));

                                if (dir.Exists) //当前路径存在
                                {
                                    DirectoryInfo[] subDirectoryInfo = dir.GetDirectories();

                                    for (Int32 k = 0; k < subDirectoryInfo.Length; k++) //遍历每天统计文件
                                    {
                                        try
                                        {
                                            if (subDirectoryInfo[k].Exists)  //当前路径存在
                                            {
                                                Struct.SYSTEMTIME startTime = new Struct.SYSTEMTIME();
                                                Struct.SYSTEMTIME endTime = new Struct.SYSTEMTIME();

                                                _GetDateTime(subDirectoryInfo[k].Name, ref startTime, ref endTime);

                                                if ((sDataOfShift.CurrentIndex > 0) && (j == sDataOfShift.CurrentIndex) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].Start, startTime) == 0) && (_Compare(sDataOfShift.TimeData[sDataOfShift.CurrentIndex - 1].End, endTime) == 0))//查询当前班次
                                                {
                                                    _DeleteShiftMemory(Convert.ToByte(i + 1), j);
                                                }

                                                DirectoryInfo dirHistory = new DirectoryInfo(_GetHistoryDayShiftDataPath(Convert.ToByte(i + 1), j, startTime, endTime));

                                                if (dirHistory.Exists)                                                    //当前路径存在
                                                {
                                                    dirHistory.Delete(true);
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                }
                            }
                        }
                    }
                    iRecicleCount++;

                    if ((_GetDirectoryLength(StatisticsDataPath) <= 1024 * 1024) || (iRecicleCount >= 10)) //当前统计文件夹小于1MB
                    {
                        break;
                    }
                }
                bDeletingStatics = false; //删除操作执行完毕
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除统计信息
        // 输入参数：1.sAppPath：，应用程序路径
        //           2、Byte：bCameraNumberMax，最大挂载相机数量
        //           3、Byte：bCameraChooseState，被启用相机标记
        //           4、Double：dSourceDriverFreeRate，硬盘空闲百分率（基础判断条件）
        //           5、Double：dDstDriverFreeRate，硬盘空闲百分率（目标）
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static void _LoadDeleteStatics(global::System.String sAppPath, Byte bCameraNumberMax, Byte bCameraChooseState, Double dSourceDriverFreeRate, Double dDstDriverFreeRate)
        {
            sShiftDataPath = sAppPath + Class.System.ConfigDataPathName;//配置文件路径（如，D:\\X6S\\ConfigData\\）
            StatisticsDataPath = sAppPath + Class.System.StatisticsPathName;//统计数据路径(如：D:\\X6S\\Statistics\\)

            if (false == bDeletingStatics) //未执行删除操作
            {
                if ((true == _CheckDiskMemory(dSourceDriverFreeRate)) && (_GetDirectoryLength(StatisticsDataPath) > 1024 * 1024))//计算启动时当前根目录存储空间是否满足要求，且当前统计文件夹不小于1MB
                {
                    bDeletingStatics = true;

                    for (Byte i = 0; i < bCameraNumberMax; i++) //遍历所有统计文件夹
                    {
                        if ((bCameraChooseState & (0x01 << i)) == 0)//当前相机未开启
                        {
                            DirectoryInfo dir = new DirectoryInfo(_GetAllCameraPath(Convert.ToByte(i + 1)));

                            if (dir.Exists) //当前路径存在
                            {
                                dir.Delete(true);
                            }
                        }
                    }
                }

                Int32 iRecicleCount = 0;
                while (true == _CheckDiskMemory(dDstDriverFreeRate))//计算当前根目录存储空间是否满足要求
                {
                    bDeletingStatics = true;

                    for (Byte i = 0; i < bCameraNumberMax; i++)//清除所有历史班次内存
                    {
                        if ((bCameraChooseState & (0x01 << i)) != 0)//当前相机开启
                        {
                            DirectoryInfo dir = new DirectoryInfo(_GetAllCameraPath(Convert.ToByte(i + 1)));

                            if (dir.Exists) //当前路径存在
                            {
                                DirectoryInfo[] subDirectoryInfo = dir.GetDirectories();

                                for (Int32 j = 0; j < subDirectoryInfo.Length; j++) //循环所有文件，成功删除一个后退出
                                {
                                    if (subDirectoryInfo[j].Exists)  //当前路径存在
                                    {
                                        DirectoryInfo[] subSubDirectoryInfo = subDirectoryInfo[j].GetDirectories();

                                        for (Int32 k = 0; k < subSubDirectoryInfo.Length; k++) //遍历每天统计文件
                                        {
                                            try
                                            {
                                                if (subSubDirectoryInfo[k].Exists)  //当前路径存在
                                                {
                                                    subSubDirectoryInfo[k].Delete(true);
                                                    break;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    iRecicleCount++;

                    if ((_GetDirectoryLength(StatisticsDataPath) <= 1024 * 1024) || (iRecicleCount >= 10)) //当前统计文件夹小于1MB
                    {
                        break;
                    }
                }
                bDeletingStatics = false; //删除操作执行完毕
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：清除统计信息
        // 输入参数：1.string：dirPath，文件夹路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        private static long _GetDirectoryLength(string dirPath)
        {
            try
            {
                if (!Directory.Exists(dirPath)) //当前路径不存在
                {
                    return 0;
                }
                else
                {
                    long len = 0;

                    DirectoryInfo di = new DirectoryInfo(dirPath);

                    FileInfo[] fFileInfo = di.GetFiles();

                    foreach (FileInfo fi in fFileInfo)
                    {
                        len += fi.Length;
                    }

                    DirectoryInfo[] dDirectoryInfo = di.GetDirectories();
                    if (dDirectoryInfo.Length > 0)
                    {
                        for (Int32 i = 0; i < dDirectoryInfo.Length; i++)
                        {
                            len += _GetDirectoryLength(dDirectoryInfo[i].FullName);
                        }
                    }
                    return len;
                }
            }
            catch (Exception ex)
            {
                //_WriteException(ex.ToString(), "0000" + dirPath);
                return 0;
            }
        }
    }
}