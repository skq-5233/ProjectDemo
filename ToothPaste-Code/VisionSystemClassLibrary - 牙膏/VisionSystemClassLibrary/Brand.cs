/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：Brand.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：品牌

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.IO;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class Brand
    {
        private Int32 iCURRENTBrandIndex = 1;//类型为CURRENT的品牌的索引值（0 ~ iBrandNumber - 1。取值为-1，表示该类型的品牌不存在）
        private string sCURRENTBrandName = "X6S-1";//类型为CURRENT的品牌的名称

        private const UInt16 uiBrandNumberMax = 1000;//最大品牌数量
        private UInt16 iBrandNumber = 2;//属性，有效品牌数量

        private Struct.BrandData[] bSystemBrandData;//品牌数据

        //

        private string ApplicationPath = "";//应用程序路径（如，D:\\VisionSystemUserInterface\\）
        private string sConfigDataPath = "";//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）
        private string sBrandPath = "";//品牌数据路径（如，D:\\VisionSystemUserInterface\\Brand\\）
        private string sBackupBrandPath = "";//备份品牌数据路径（如，D:\\VisionSystemUserInterface\\Backup\\Brand\\）

        //

        private static string BrandFileName = "Brand.dat";//品牌数据文件名称

        private static string BrandPathName = "Brand\\";//品牌数据路径
        private static string BackupBrandPathName = "Backup\\Brand\\";//备份品牌数据路径
        private static string sUSBDeviceBackupBrandPathName = "Backup\\Brand\\";//USB设备备份品牌数据路径

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数（默认），初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Brand()
        {
            _SetDefaultBrandData();//设置默认品牌数据
        }

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化，读取文件数据
        // 输入参数：1.sFilePath：文件数据路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Brand(string sPath)
        {
            _SetDefaultBrandData();//设置默认品牌数据

            //

            ApplicationPath = sPath;//应用程序路径（如，D:\\VisionSystemUserInterface\\）
            sConfigDataPath = sPath + System.ConfigDataPathName;//配置文件路径（如，D:\\VisionSystemUserInterface\\ConfigData\\）
            sBrandPath = sPath + BrandPathName;//品牌数据路径（如，D:\\VisionSystemUserInterface\\Brand\\）
            sBackupBrandPath = sPath + BackupBrandPathName;//备份品牌数据路径（如，D:\\VisionSystemUserInterface\\Backup\\Brand\\）

            FileStream filestream = null;
            BinaryReader binaryreader =null;

            try
            {
                filestream = new FileStream(sPath + System.ConfigDataPathName + BrandFileName, FileMode.Open); //打开Brand文件
                binaryreader = new BinaryReader(filestream);

                iBrandNumber = binaryreader.ReadUInt16();//读取烟包品牌总数
                iCURRENTBrandIndex = binaryreader.ReadInt32();//读取当前烟包品牌索引

                for (UInt16 i = 0; i < iBrandNumber; i++)//读取所有烟包品牌信息
                {
                    filestream.Seek((0x010 + i * 0x050), SeekOrigin.Begin);
                    bSystemBrandData[i].Type = (Enum.BrandType)binaryreader.ReadByte();//读取烟包品牌名称

                    filestream.Seek((0x020 + i * 0x050), SeekOrigin.Begin);
                    bSystemBrandData[i].Name = binaryreader.ReadString();//读取烟包品牌名称
                }

                sCURRENTBrandName = bSystemBrandData[iCURRENTBrandIndex].Name;//初始化当前烟包品牌名

                binaryreader.Close();//关闭Brand文件
                filestream.Close();
            }
            catch (Exception ex)
            {
                //不执行操作
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

        //属性

        //-----------------------------------------------------------------------
        // 功能说明：BackupBrandPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string BackupBrandPath
        {
            get
            {
                return sBackupBrandPath;
            }
            //set
            //{
            //    sBackupBrandPath = value;
            //}
        }

        //-----------------------------------------------------------------------
        // 功能说明：USBDeviceBackupBrandPathName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string USBDeviceBackupBrandPathName
        {
            get
            {
                return sUSBDeviceBackupBrandPathName;
            }
            //set
            //{
            //    sUSBDeviceBackupBrandPathName = value;
            //}
        }

        //-----------------------------------------------------------------------
        // 功能说明：BrandNumberMax属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static UInt16 BrandNumberMax
        {
            get
            {
                return uiBrandNumberMax;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CURRENTBrandIndex属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 CURRENTBrandIndex
        {
            get
            {
                return iCURRENTBrandIndex;
            }
            set
            {
                iCURRENTBrandIndex = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：CURRENTBrandName属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string CURRENTBrandName
        {
            get
            {
                return sCURRENTBrandName;
            }
            set
            {
                sCURRENTBrandName = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：BrandPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string BrandPath
        {
            get
            {
                return sBrandPath;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：SystemBrandData属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Struct.BrandData[] SystemBrandData
        {
            get
            {
                return bSystemBrandData;
            }
            set//设置
            {
                bSystemBrandData = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：ConfigDataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string ConfigDataPath
        {
            get
            {
                return sConfigDataPath;
            }
            set//设置
            {
                sConfigDataPath = value;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：BrandNumber属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public UInt16 BrandNumber
        {
            get//读取
            {
                return iBrandNumber;
            }
            set//设置
            {
                if (value != iBrandNumber)
                {
                    iBrandNumber = value;
                }
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：保存烟包品牌
        // 输入参数：1.brandIndex,需要写入烟包品牌索引
        //         2.iStyle：写入类型。取值范围：1，COPY；2.RENAME；3.DELETE；4.LOAD / RELOAD；5.RESTORE
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _Write(Int32 brandIndex, Int32 iStyle)
        {
            FileStream filestream = null;
            BinaryWriter binaryWriter = null;

            try
            {
                filestream = new FileStream(sConfigDataPath + BrandFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough); //打开Brand文件
                binaryWriter = new BinaryWriter(filestream);

                //

                if (1 == iStyle)//COPY
                {
                    filestream.Seek(0x000, SeekOrigin.Begin);
                    binaryWriter.Write(iBrandNumber);//写入品牌总数

                    //

                    filestream.Seek((0x010 + brandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write((Byte)bSystemBrandData[brandIndex].Type);//写入品牌名称

                    filestream.Seek((0x020 + brandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write(bSystemBrandData[brandIndex].Name);//写入品牌名称
                }
                else if (2 == iStyle)//RENAME
                {
                    filestream.Seek((0x010 + brandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write((Byte)bSystemBrandData[brandIndex].Type);//写入品牌名称

                    filestream.Seek((0x020 + brandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write(bSystemBrandData[brandIndex].Name);//写入品牌名称
                }
                else if (3 == iStyle)//DELETE
                {
                    filestream.Seek(0x000, SeekOrigin.Begin);
                    binaryWriter.Write(iBrandNumber);//写入品牌总数

                    filestream.Seek(0x002, SeekOrigin.Begin);
                    binaryWriter.Write(brandIndex);//写入当前烟包品牌索引

                    //

                    for (UInt16 i = 0; i < iBrandNumber + 1; i++)//遍历所有烟包品牌信息
                    {
                        filestream.Seek((0x010 + i * 0x050), SeekOrigin.Begin);
                        binaryWriter.Write((Byte)bSystemBrandData[i].Type);//写入烟包品牌名称

                        filestream.Seek((0x020 + i * 0x050), SeekOrigin.Begin);
                        binaryWriter.Write(bSystemBrandData[i].Name);//写入烟包品牌名称
                    }
                }
                else if (4 == iStyle)//LOAD / RELOAD
                {
                    filestream.Seek(0x002, SeekOrigin.Begin);
                    binaryWriter.Write(brandIndex);//写入当前烟包品牌索引

                    filestream.Seek((0x010 + brandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write((Byte)bSystemBrandData[brandIndex].Type);//写入烟包品牌类型

                    filestream.Seek((0x010 + iCURRENTBrandIndex * 0x050), SeekOrigin.Begin);
                    binaryWriter.Write((Byte)bSystemBrandData[iCURRENTBrandIndex].Type);//写入烟包品牌类型
                }
                else//5 == iStyle，RESTORE
                {
                    filestream.Seek(0x000, SeekOrigin.Begin);
                    binaryWriter.Write(iBrandNumber);//写入品牌总数

                    filestream.Seek(0x002, SeekOrigin.Begin);
                    binaryWriter.Write(brandIndex);//写入当前烟包品牌索引

                    //

                    for (UInt16 i = 0; i < iBrandNumber + 1; i++)//遍历所有烟包品牌信息
                    {
                        filestream.Seek((0x010 + i * 0x050), SeekOrigin.Begin);
                        binaryWriter.Write((Byte)bSystemBrandData[i].Type);//写入烟包品牌名称

                        filestream.Seek((0x020 + i * 0x050), SeekOrigin.Begin);
                        binaryWriter.Write(bSystemBrandData[i].Name);//写入烟包品牌名称
                    }
                }

                //

                binaryWriter.Close();//关闭Brand文件
                filestream.Close();
            }
            catch (Exception ex)
            {
                //不执行操作
            }

            if (null != binaryWriter)
            {
                binaryWriter.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：保存CURRENT品牌
        // 输入参数：无
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _SaveCurrent()
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                DirectoryInfo directoryinfoSource = new DirectoryInfo(sConfigDataPath.Substring(0, sConfigDataPath.Length - 1));//获取子目录

                foreach (var directories in directoryinfoSource.EnumerateDirectories("*", SearchOption.TopDirectoryOnly))
                {
                    _CopyDirectory(directories.FullName + "\\" + Camera.SampleImagePathName.Substring(0, Class.Camera.SampleImagePathName.Length - 1), sBrandPath + sCURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.SampleImagePathName.Substring(0, Class.Camera.SampleImagePathName.Length - 1));

                    //

                    File.Copy(directories.FullName + "\\" + Camera.ParameterFileName, sBrandPath + sCURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.ParameterFileName, true);
                    File.Copy(directories.FullName + "\\" + Camera.TolerancesFileName, sBrandPath + sCURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.TolerancesFileName, true);
                    File.Copy(directories.FullName + "\\" + Camera.ToolFileName, sBrandPath + sCURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.ToolFileName, true);

                    //Class.System.FileCopyFun(directories.FullName + "\\" + Class.Camera.ParameterFileName, BrandPath + CURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.ParameterFileName);
                    //Class.System.FileCopyFun(directories.FullName + "\\" + Class.Camera.TolerancesFileName, BrandPath + CURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.TolerancesFileName);
                    //Class.System.FileCopyFun(directories.FullName + "\\" + Class.Camera.ToolFileName, BrandPath + CURRENTBrandName + "\\" + directories.Name + "\\" + Class.Camera.ToolFileName);
                }
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：（重新）加载品牌
        // 输入参数：1.sBrand：品牌
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _LoadBrand(string sBrand)
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                _CopyDirectories(sBrandPath + sBrand, sConfigDataPath.Substring(0, sConfigDataPath.Length - 1));//拷贝文件夹中的所有子文件夹
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：拷贝品牌
        // 输入参数：1.sSourceBrand：源品牌
        //         2.sDestinationBrand：目标品牌
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _CopyBrand(string sSourceBrand, string sDestinationBrand)
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                _CopyDirectory(sBrandPath + sSourceBrand, sBrandPath + sDestinationBrand);//拷贝文件夹
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：重命名品牌
        // 输入参数：1.sSourceBrand：源品牌
        //         2.sDestinationBrand：目标品牌
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _RenameBrand(string sSourceBrand, string sDestinationBrand)
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                Directory.Move(sBrandPath + sSourceBrand, sBrandPath + sDestinationBrand);
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：删除品牌
        // 输入参数：1.sBrand：品牌
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _DeleteBrand(string sBrand)
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                Directory.Delete(sBrandPath + sBrand, true);
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：备份品牌
        // 输入参数：无
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _BackupBrands()
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                _CopyDirectories(sBrandPath.Substring(0, sBrandPath.Length - 1), sBackupBrandPath.Substring(0, sBackupBrandPath.Length - 1));//拷贝文件夹中的所有子文件夹
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //----------------------------------------------------------------------
        // 功能说明：恢复品牌
        // 输入参数：无
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public Boolean _RestoreBrands()
        {
            Boolean bReturn = true;//函数返回值

            try
            {
                _CopyDirectories(sBackupBrandPath.Substring(0, sBackupBrandPath.Length - 1), sBrandPath.Substring(0, sBrandPath.Length - 1));//拷贝文件夹中的所有子文件夹
            }
            catch (Exception ex)
            {
                bReturn = false;
            }

            return bReturn;
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：获取日期时间
        // 输入参数：1.datetime：日期时间
        // 输出参数：无
        // 返回值：操作是否成功。取值范围：true，是；false，否
        //----------------------------------------------------------------------
        public static string _GetDateTime(DateTime datetime)
        {
            return datetime.Year.ToString("D4") + "_" + datetime.Month.ToString("D2") + "_" + datetime.Day.ToString("D2") + "_" + datetime.Hour.ToString("D2") + "_" + datetime.Minute.ToString("D2") + "_" + datetime.Second.ToString("D2");
        }

        //----------------------------------------------------------------------
        // 功能说明：拷贝文件夹
        // 输入参数：1.sSourceDirectory：源路径
        //         2.sDestinationDirectory：目标路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _CopyDirectory(string sSourceDirectory, string sDestinationDirectory)//
        {
            DirectoryInfo directoryinfoSource = new DirectoryInfo(sSourceDirectory);//源路径信息

            Directory.CreateDirectory(sDestinationDirectory);//创建目标路径

            foreach (FileSystemInfo filesysteminfo in directoryinfoSource.GetFileSystemInfos())
            {
                string destinationName = Path.Combine(sDestinationDirectory, filesysteminfo.Name);

                if (filesysteminfo is FileInfo)//文件
                {
                    File.Copy(filesysteminfo.FullName, destinationName, true);//拷贝文件

                    //Class.System.FileCopyFun(filesysteminfo.FullName, destinationName);//拷贝文件
                }
                else if (filesysteminfo is DirectoryInfo)//文件夹
                {
                    Directory.CreateDirectory(destinationName);//创建文件夹

                    _CopyDirectory(filesysteminfo.FullName, destinationName);
                }
                else
                {
                    //不执行操作
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：拷贝文件夹中的所有子文件夹
        // 输入参数：1.sSourceDirectory：源路径
        //         2.sDestinationDirectory：目标路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _CopyDirectories(string sSourceDirectory, string sDestinationDirectory)//
        {
            DirectoryInfo directoryinfoSource = new DirectoryInfo(sSourceDirectory);//获取子目录

            foreach (var filesysteminfos in directoryinfoSource.EnumerateFileSystemInfos("*", SearchOption.TopDirectoryOnly))
            {
                if (filesysteminfos is FileInfo)//文件
                {
                    Directory.CreateDirectory(sDestinationDirectory);//创建目标路径

                    string destinationName = Path.Combine(sDestinationDirectory, filesysteminfos.Name);

                    File.Copy(filesysteminfos.FullName, destinationName, true);//拷贝文件

                    //Class.System.FileCopyFun(filesysteminfos.FullName, destinationName);//拷贝文件
                }
                else if (filesysteminfos is DirectoryInfo)//文件夹
                {
                    _CopyDirectory(filesysteminfos.FullName, sDestinationDirectory + "\\" + filesysteminfos.Name);//拷贝文件夹
                }
                else
                {
                    //不执行操作
                }
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：清空文件夹
        // 输入参数：1.sDirectory：路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _ClearDirectory(string sDirectory)//
        {
            Int32 i = 0;//循环控制变量

            DirectoryInfo directoryinfo = new DirectoryInfo(sDirectory);//路径
            DirectoryInfo[] di = directoryinfo.GetDirectories();
            if (null != di)
            {
                for (i = 0; i < di.Length; i++)
                {
                    di[i].Delete(true);
                }
            }

            //

            string[] sFileName = Directory.GetFiles(sDirectory);//获取文件

            if (null != sFileName)//有效
            {
                for (i = 0; i < sFileName.Length; i++)
                {
                    File.Delete(sFileName[i]);
                }
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：设置默认品牌数据
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefaultBrandData()
        {
            bSystemBrandData = new Struct.BrandData[uiBrandNumberMax];//申请内存空间

            for (Int32 i = 0; i < uiBrandNumberMax; i++)
            {
                bSystemBrandData[i] = new Struct.BrandData();
                bSystemBrandData[i].Name = "";//品牌名称
                bSystemBrandData[i].Type = Enum.BrandType.None;//品牌类型
            }

            //

            bSystemBrandData[0] = new Struct.BrandData();
            bSystemBrandData[0].Name = "X6S";//品牌名称
            bSystemBrandData[0].Type = Enum.BrandType.Master;//品牌类型

            bSystemBrandData[1] = new Struct.BrandData();
            bSystemBrandData[1].Name = "X6S-1";//品牌名称
            bSystemBrandData[1].Type = Enum.BrandType.Current;//品牌类型
        }
    }
}