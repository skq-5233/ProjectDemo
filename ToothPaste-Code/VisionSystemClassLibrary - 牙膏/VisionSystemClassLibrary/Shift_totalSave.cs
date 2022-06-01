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
完成日期：2020/05/11
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/
using System;
using System.Collections.Generic;
using System.IO;

using Emgu.CV;
using Emgu.CV.Structure;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Shift_totalSave
    {
        private Boolean bShiftState = false;//班次使能状态

        private Boolean bTransMode = false;//默认NAS全保存，为True时为以太网传输
        
        private DirectoryInfo dDataPath;//统计数据路径(如：J:\\SavingImageRootFolder\\)

        private string ShiftFileName = "Shift_totalSave.dat";//Shift_totalSave数据文件名称

        private string sSavingFileFoamat = ".bmp";

        private Byte bCreateFolderType = 1;//文件夹创建方式

        private Dictionary<string, string>[] dRequestInfo;//请求属性信息

        private string sServerIP = "10.11.15.1";//服务器IP地址

        //构造函数

        //-----------------------------------------------------------------------
        // 功能说明：构造函数(默认)
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Shift_totalSave()
        {
        }

        //-----------------------------------------------------------------------
        // 功能说明：构造函数（Controller）
        // 输入参数：1.sAppPath：应用程序路径
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Shift_totalSave(global::System.String sAppPath)
        {
            if (_ReadShiftState(sAppPath))//当前班次使能
            {
            }
        }
        
        //属性

        // 功能说明：RequestInfo属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Dictionary<string, string>[] RequestInfo
        {
            get
            {
                return dRequestInfo;
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
        }

        // 功能说明：TransMode属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean TransMode
        {
            get
            {
                return bTransMode;
            }
        }

        // 功能说明：SavingFileFoamat属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string SavingFileFoamat
        {
            get
            {
                return sSavingFileFoamat;
            }
        }

        // 功能说明：ServerIP属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ServerIP
        {
            get
            {
                return sServerIP;
            }
        }

        // 功能说明：ImageSavingRootExists属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean ImageSavingRootExists  //文件保存根目录是否存在
        {
            get
            {
                try
                {
                    return dDataPath.Root.Exists;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：读取班次使能状态
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：Boolean，返回Shift文件是否存在标记
        //----------------------------------------------------------------------
        private Boolean _ReadShiftState(global::System.String sAppPath)
        {
            FileStream filestream = null;
            BinaryReader binaryreader = null;

            try
            {
                filestream = new FileStream(sAppPath + System.ConfigDataPathName + ShiftFileName, FileMode.Open);
                binaryreader = new BinaryReader(filestream);

                bShiftState = binaryreader.ReadBoolean();//读取班次使能状态

                if (bShiftState) //班次使能
                {
                    bTransMode = binaryreader.ReadBoolean();//读取保存方式状态
                    sServerIP = binaryreader.ReadString();//读取服务器端IP

                    filestream.Seek(0x10, SeekOrigin.Begin);//读取存储路径
                    dDataPath = new DirectoryInfo(binaryreader.ReadString());

                    filestream.Seek(0x30, SeekOrigin.Begin);//读取文件夹创建方式
                    bCreateFolderType = binaryreader.ReadByte();

                    filestream.Seek(0x40, SeekOrigin.Begin);//读取文件保存格式
                    sSavingFileFoamat = binaryreader.ReadString();

                    dRequestInfo = new Dictionary<string, string>[4];//请求属性信息

                    for (Int32 j = 0; j < 4; j++) //最大四相机
                    {
                        filestream.Seek(0x50 + j * 0x300, SeekOrigin.Begin);//读取参数数量，预留8对参数
                        Int32 iParameterCount = binaryreader.ReadInt32();

                        dRequestInfo[j] = new Dictionary<string, string>();
                        
                        for (Int32 i = 0; i < iParameterCount; i++) //循环读取参数信息
                        {
                            filestream.Seek(0x60 + j * 0x300 + 0x20 * i, SeekOrigin.Begin);
                            string sParameterKey = binaryreader.ReadString();

                            filestream.Seek(0x70 + j * 0x300 + 0x20 * i, SeekOrigin.Begin);
                            string sParameterValue = binaryreader.ReadString();

                            dRequestInfo[j].Add(sParameterKey, sParameterValue);
                        }
                        //dRequestInfo.Add("width", "744");
                        //dRequestInfo.Add("height", "480");
                        //dRequestInfo.Add("chns", "3");
                        //dRequestInfo.Add("filename", sPic_Name);
                        //dRequestInfo.Add("function", "detection");
                        //dRequestInfo.Add("devicetype", sProduct_Name);
                        //dRequestInfo.Add("comppartno", sProduct_ID);
                        //dRequestInfo.Add("internalpartno", sCamera_Name);
                        //dRequestInfo.Add("timgnum", "6");
                        //dRequestInfo.Add("imgindex", "1");
                        //dRequestInfo.Add("requestid", "158994457300");
                        //dRequestInfo.Add("machineid", "machine1");
                        //dRequestInfo.Add("boardid", "submachine123");
                        //dRequestInfo.Add("timestamp", "");
                        //dRequestInfo.Add("fileSuffix", "");
                    }
                }
                binaryreader.Close();//关闭Shift文件
                filestream.Close();

                return true;
            }
            catch (Exception ex)
            {
                bShiftState = false;

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
        // 功能说明：获取相机路径
        // 输入参数：1、VisionSystemClassLibrary.Enum.CameraType：cameraType，上位机相机类型
        // 回输出参数：无
        // 返  值：string，返回当前相机路径
        //----------------------------------------------------------------------
        private string _GetCameraPath(VisionSystemClassLibrary.Enum.CameraType cameraType)
        {
            return dDataPath.FullName + cameraType.ToString() + "\\";//统计数据相机路径(如：E:\\SavingImageRootFolder\\Camera_1\\)
        }

        //-----------------------------------------------------------------------
        // 功能说明：获取班次路径
        // 输入参数：1、VisionSystemClassLibrary.Enum.CameraType：cameraType，上位机相机类型
        //                    2、DateTime：dateTime，当前路径
        // 输出参数：无
        // 返 回 值：string，返回当前班次路径
        //----------------------------------------------------------------------
        private string _GetCameraDayPath(VisionSystemClassLibrary.Enum.CameraType cameraType, DateTime dateTime)
        {
            if (24 <= bCreateFolderType) //按天存储
            {
                return _GetCameraPath(cameraType) + dateTime.ToString("yyyyMMdd") + "\\";//统计数据路径(如：J:\\SavingImageRootFolder\\Camera_1\\20190103\\)
            }
            else if ((bCreateFolderType > 0)) //按小时存储
            {
                return _GetCameraPath(cameraType) + dateTime.ToString("yyyyMMdd") + (dateTime.Hour / bCreateFolderType).ToString("00") + "\\";//统计数据路径(如：J:\\SavingImageRootFolder\\Camera_1\\20190103\\)
            }
            else
            {
                return "";
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：更新并保存当前班次统计信息
        // 输入参数：1、VisionSystemClassLibrary.Enum.CameraType：cameraType，上位机相机类型
        //           3、ImageInformation[]：imageInformations，图像信息
        //           4、Image<Bgr, Byte>：image，缺陷图像
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _UpdateCurrentImageInformation(VisionSystemClassLibrary.Enum.CameraType cameraType, DateTime dateTime, Image<Bgr, Byte> image)
        {
            //if (true == _CheckDiskMemory(0.8))//计算当前根目录存储空间是否满足0.8要求，不满足则停止保存，等待停机删除
            //{
            //}
            //else//执行缺陷图像统计信息计数
            {
                if (null != image) //当前图像有效
                {
                    string cameraPath = _GetCameraPath(cameraType);
                    DirectoryInfo dir = new DirectoryInfo(cameraPath);

                    if (false == dir.Exists) //创建当前路径存在
                    {
                        dir.Create();
                    }

                    string cameraDayPath = _GetCameraDayPath(cameraType, dateTime);

                    if ("" != cameraDayPath)
                    {
                        dir = new DirectoryInfo(cameraDayPath);

                        if (false == dir.Exists) //创建当前路径存在
                        {
                            dir.Create();
                        }

                        string fileName = cameraDayPath + "_" + dateTime.ToString("yyyyMMddHHmmssfff") + sSavingFileFoamat;
                        //image.Save(fileName);//保存图像"zuo1_ID00105_2020-05-13 14:33:22:345_4_0
                        GeneralFunction._SaveImage(image, fileName, sSavingFileFoamat);
                    }
                }
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：检查硬盘占用率是否超限
        // 输入参数：1、Double：dDriverFreeRate，硬盘剩余空间百分比
        // 输出参数：无
        // 返 回 值：Boolean，返回存盘驱动器是否有多余存储空间
        //----------------------------------------------------------------------
        private Boolean _CheckDiskMemory(Double dDriverFreeRate)
        {
            Boolean checkResult = false;

            DriveInfo[] driveInfo = DriveInfo.GetDrives();

            foreach (DriveInfo d in driveInfo)
            {
                if (d.RootDirectory.Name == dDataPath.Root.Name)//当前根目录有效
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
    }
}
