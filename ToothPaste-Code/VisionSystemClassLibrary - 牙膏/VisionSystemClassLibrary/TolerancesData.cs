/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：TolerancesData.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：公差

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace VisionSystemClassLibrary.Class
{
    [Serializable]
    public class TolerancesData
    {
        private string sDataPath = "";//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）

        //

        private Int32 iTotalPage = 3;//包含的页码总数

        private List<Struct.TolerancesGraphData> tolerancesGraphData;//属性，曲线图数据

        //

        private Boolean bEjectLevelDisplay = false;//空头、缺支调节灵敏度是否显示。取值范围：true，是；false，否

        private Int16 iEjectLevel = 30;//空头、缺支调节灵敏度
        private Int16 iEjectLevel_Min = 0;//空头、缺支调节灵敏度下限
        private Int16 iEjectLevel_Max = 50;//空头、缺支调节灵敏度上限

        private Int32 iGraphNumber_Total;//公差数目(配置工具使用)

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数（默认），初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TolerancesData()
        {
            _SetDefaultData();//设置默认数据
        }

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化，读取文件数据
        // 输入参数：1.sPath：文件数据路径
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public TolerancesData(string sPath)//
        {
            _SetDefaultData();//设置默认数据

            //

            sDataPath = sPath;//相机数据路径（如，D:\\VisionSystemUserInterface\\ConfigData\\Top\\）

            //

            Int32 i = 0;//循环控制变量

            FileStream filestream = new FileStream(sPath + Camera.TolerancesFileName, FileMode.Open);
            BinaryReader binaryreader = new BinaryReader(filestream);

            filestream.Seek(0x00, SeekOrigin.Begin);//
            bEjectLevelDisplay = binaryreader.ReadBoolean();//

            filestream.Seek(0x10, SeekOrigin.Begin);
            iEjectLevel = binaryreader.ReadInt16();

            filestream.Seek(0x20, SeekOrigin.Begin);
            iGraphNumber_Total = binaryreader.ReadInt32();

            filestream.Seek(0x30, SeekOrigin.Begin);
            TotalPage = binaryreader.ReadInt32();

            tolerancesGraphData = new List<Struct.TolerancesGraphData>();
            for (i = 0; i < iGraphNumber_Total; i++)
            {
                Struct.TolerancesGraphData tolerancesGraphDataTemp = new Struct.TolerancesGraphData();

                filestream.Seek(0x40 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.ToolsIndex = binaryreader.ReadInt32();
                
                filestream.Seek(0x50 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.TolerancesID = binaryreader.ReadInt32();

                filestream.Seek(0x60 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.Page = binaryreader.ReadInt32();

                filestream.Seek(0x70 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.ButtonONOFFShow = binaryreader.ReadBoolean();
                filestream.Seek(0x80 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.ButtonLearningShow = binaryreader.ReadBoolean();
                filestream.Seek(0x90 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.EffectiveMin_ReadOnly = binaryreader.ReadBoolean();
                filestream.Seek(0xA0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.EffectiveMax_ReadOnly = binaryreader.ReadBoolean();

                filestream.Seek(0xB0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.AdditionalValueDisplay = binaryreader.ReadBoolean();
                filestream.Seek(0xC0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.AdditionalValueRatio = binaryreader.ReadSingle();
                filestream.Seek(0xD0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.AdditionalValueUnit = binaryreader.ReadString();
                filestream.Seek(0xE0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.EffectiveMin_Value = binaryreader.ReadInt32();
                filestream.Seek(0xF0 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.EffectiveMax_Value = binaryreader.ReadInt32();
                filestream.Seek(0x100 + i * 0xD0, SeekOrigin.Begin);
                tolerancesGraphDataTemp.ValueNumber = binaryreader.ReadInt32();

                tolerancesGraphData.Add(tolerancesGraphDataTemp);
            }

            binaryreader.Close();
            filestream.Close();

            //

            for (i = 0; i < tolerancesGraphData.Count; i++)
            {
                tolerancesGraphData[i].TolerancesGraphDataValue.Value = new Int32[tolerancesGraphData[i].ValueNumber];
                for (Int32 j = 0; j < tolerancesGraphData[i].ValueNumber; j++)
                {
                    tolerancesGraphData[i].TolerancesGraphDataValue.Value[j] = 0;
                }
                tolerancesGraphData[i].TolerancesGraphDataValue.MeanValue = 0;
                tolerancesGraphData[i].TolerancesGraphDataValue.CurrentValueIndex = -1;
                tolerancesGraphData[i].TolerancesGraphDataValue.AdditionalValue = _SetAdditionalValue(tolerancesGraphData[i].EffectiveMin_Value, tolerancesGraphData[i].EffectiveMax_Value, tolerancesGraphData[i].AdditionalValueRatio);//曲线图坐标轴上显示的附加数值
            }

            if ((iEjectLevel < iEjectLevel_Min) || (iEjectLevel > iEjectLevel_Max))//空头、缺支调节灵敏度超限
            {
                iEjectLevel = iEjectLevel_Max;
            } 
        }

        //属性

        // 功能说明：DataPath属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public string DataPath
        {
            set//读取
            {
                sDataPath = value;
            }
        }

        // 功能说明：EjectLevelDisplay属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Boolean EjectLevelDisplay
        {
            get//读取
            {
                return bEjectLevelDisplay;
            }
            set
            {
                bEjectLevelDisplay = value;
            }
        }

        // 功能说明：EjectLevel_Min属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectLevel_Min
        {
            get//读取
            {
                return iEjectLevel_Min;
            }
            set//设置
            {
                iEjectLevel_Min = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：EjectLevel_Max属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectLevel_Max
        {
            get//读取
            {
                return iEjectLevel_Max;
            }
            set//设置
            {
                iEjectLevel_Max = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：GraphNumber_Total属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 GraphNumber_Total
        {
            get//读取
            {
                return iGraphNumber_Total;
            }
            set//设置
            {
                iGraphNumber_Total = value;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：EjectLevel属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int16 EjectLevel
        {
            get//读取
            {
                return iEjectLevel;
            }
            set//设置
            {
                iEjectLevel = value;
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TotalPage属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public Int32 TotalPage
        {
            get//读取
            {
                return iTotalPage;
            }
            set//设置
            {
                iTotalPage = value;
            }
        }
        
        //-----------------------------------------------------------------------
        // 功能说明：GraphData属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public List<Struct.TolerancesGraphData> GraphData
        {
            get//读取
            {
                return tolerancesGraphData;
            }
            set//设置
            {
                tolerancesGraphData = value;
            }
        }

        //函数

        //----------------------------------------------------------------------
        // 功能说明：保存函数
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public Boolean _SaveParameter()//
        {
            FileStream filestream1 = null;
            BinaryReader binaryreader = null;

            FileStream filestream = null;
            BinaryWriter binarywriter = null;

            try
            {
                Int32 i = 0;//循环控制变量

                filestream1 = new FileStream(sDataPath + Camera.TolerancesFileName, FileMode.Open);
                binaryreader = new BinaryReader(filestream1);//读取文件数据

                Byte[] bData = binaryreader.ReadBytes((Int32)filestream1.Length);

                binaryreader.Close();
                filestream1.Close();

                filestream = new FileStream(sDataPath + Camera.TolerancesFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
                binarywriter = new BinaryWriter(filestream);

                BitConverter.GetBytes(iEjectLevel).CopyTo(bData, 0x10);

                for (i = 0; i < tolerancesGraphData.Count; i++)
                {
                    BitConverter.GetBytes(tolerancesGraphData[i].AdditionalValueRatio).CopyTo(bData, 0xC0 + i * 0xD0);
                    BitConverter.GetBytes(tolerancesGraphData[i].EffectiveMin_Value).CopyTo(bData, 0xE0 + i * 0xD0);
                    BitConverter.GetBytes(tolerancesGraphData[i].EffectiveMax_Value).CopyTo(bData, 0xF0 + i * 0xD0);
                }

                binarywriter.Write(bData);

                binarywriter.Close();
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

            if (null != filestream1)
            {
                filestream1.Close();
            }

            if (null != binarywriter)
            {
                binarywriter.Close();
            }

            if (null != filestream)
            {
                filestream.Close();
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：保存函数，提供配置工具使用
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public void _SaveParameterReload()//2017.02.09
        {
            Int32 i = 0;//循环控制变量

            FileStream filestream = new FileStream(sDataPath + Camera.TolerancesFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 8192, FileOptions.WriteThrough);
            BinaryWriter binarywriter = new BinaryWriter(filestream);

            filestream.Seek(0x00, SeekOrigin.Begin);//
            binarywriter.Write(bEjectLevelDisplay);//

            filestream.Seek(0x10, SeekOrigin.Begin);
            binarywriter.Write(iEjectLevel);

            filestream.Seek(0x20, SeekOrigin.Begin);
            if (tolerancesGraphData != null)//公差数据个数
            {
                binarywriter.Write(tolerancesGraphData.Count);
            }
            else
            {
                binarywriter.Write(0);
            }

            filestream.Seek(0x30, SeekOrigin.Begin);
            binarywriter.Write(TotalPage);

            for (i = 0; i < tolerancesGraphData.Count; i++)
            {
                filestream.Seek(0x40 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].ToolsIndex);

                filestream.Seek(0x50 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].TolerancesID);

                filestream.Seek(0x60 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].Page);

                filestream.Seek(0x70 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].ButtonONOFFShow);
                filestream.Seek(0x80 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].ButtonLearningShow);
                filestream.Seek(0x90 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].EffectiveMin_ReadOnly);
                filestream.Seek(0xA0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].EffectiveMax_ReadOnly);

                filestream.Seek(0xB0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].AdditionalValueDisplay);
                filestream.Seek(0xC0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].AdditionalValueRatio);
                filestream.Seek(0xD0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].AdditionalValueUnit);
                filestream.Seek(0xE0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].EffectiveMin_Value);
                filestream.Seek(0xF0 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].EffectiveMax_Value);
                filestream.Seek(0x100 + i * 0xD0, SeekOrigin.Begin);
                binarywriter.Write(tolerancesGraphData[i].ValueNumber);
            }

            binarywriter.Close();
            filestream.Close();
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesData类拷贝函数
        // 输入参数：1.TolerancesData：tolerancesData，TolerancesData参数
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public void _CopyTo(ref TolerancesData tolerancesData)
        {
            tolerancesData.TotalPage = TotalPage;
            tolerancesData.sDataPath = sDataPath;
            tolerancesData.iEjectLevel = iEjectLevel;

            tolerancesData.tolerancesGraphData.Clear();
            for (Int32 i = 0; i < tolerancesGraphData.Count; i++)
            {
                Struct.TolerancesGraphData tolerancesGraphDataTemp = new Struct.TolerancesGraphData();
                tolerancesGraphDataTemp._Init();
                tolerancesGraphData[i]._CopyTo(ref tolerancesGraphDataTemp);
                tolerancesData.tolerancesGraphData.Add(tolerancesGraphDataTemp);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesData类基准值计算函数
        // 输入参数：1.Int16：ejectLevel，整体灵敏度
        //           2.Int32: averagePixel，平均像素数
        //           3.float: precision，像素变化率
        // 输出参数：无
        // 返 回 值：Int32，基准值
        //----------------------------------------------------------------------
        public Int32 _GetEjectPixel(Int16 ejectLevel, Int32 ejectPixelMin, float precision)
        {
            Int32 ejectPixel = (Int32)(ejectPixelMin + (50 - ejectLevel) * precision);

            if (ejectPixel >= ejectPixelMin)//下限值必须小于上限值
            {
                ejectPixel -= 1;
            }
            return (ejectPixel > 0 ? ejectPixel : 0);
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesData类变化率计算函数
        // 输入参数：1.Int16：ejectLevel，整体灵敏度
        //           2.Int32: ejectPixel，基准值
        //           3.Int32: ejectPixelMin，基准值下限
        //           4.float: precision，像素变化率
        // 输出参数：无
        // 返 回 值：无
        public void _GetPrecision(Int16 ejectLevel, Int32 ejectPixel, Int32 ejectPixelMin, ref float precision)
        {
            if (ejectLevel != 50)//灵敏度不为零
            {
                precision = ((float)(ejectPixel - ejectPixelMin)) / (50 - ejectLevel);
            }
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesData类基准值计算函数
        // 输入参数：1.Int16：ejectLevel，整体灵敏度
        //           2.Int32: averagePixel，平均像素数
        //           3.float: precision，像素变化率
        // 输出参数：无
        // 返 回 值：Int32，基准值
        //----------------------------------------------------------------------
        public Int32 _GetEjectPixel_Tobacco_D(Int16 ejectLevel, Int32 ejectPixelMin, float precision)
        {
            Int32 ejectPixel = (Int32)(ejectPixelMin + ejectLevel * precision);

            return ejectPixel;
        }

        //-----------------------------------------------------------------------
        // 功能说明：TolerancesData类变化率计算函数
        // 输入参数：1.Int16：ejectLevel，整体灵敏度
        //           2.Int32: ejectPixel，基准值
        //           3.Int32: ejectPixelMax，基准值上限
        //           4.float: precision，像素变化率
        // 输出参数：无
        // 返 回 值：无
        public void _GetPrecision_Tobacco_D(Int16 ejectLevel, Int32 ejectPixel, Int32 ejectPixelMin, ref float precision)
        {
            if (ejectLevel != 0)//灵敏度不为零
            {
                precision = ((float)(ejectPixel - ejectPixelMin)) / ejectLevel;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：设置附加数值
        // 输入参数：1.imin：最小值
        //         2.imax：最大值
        //         3.iratio：系数
        // 输出参数：无
        // 返回值：附加数值
        //----------------------------------------------------------------------
        public static Double _SetAdditionalValue(Int32 imin, Int32 imax, float iratio)
        {
            if (0 == iratio)//无效
            {
                return 0;
            }
            else//有效
            {
                return (imax - imin) / iratio;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：学习
        // 输入参数：1.iCurrentValueIndex：绘制区域中当前显示的曲线值点
        //         2.iValue：曲线图数值
        //         3.iToolsMinValue：工具最小值
        //         4.iToolsMaxValue：工具最大值
        //         5.iLearnedValue：学习数值
        //         6.iLearnedValidValueNumber：学习中的有效数值数量
        //         7.iLearnedNonvalidValueNumber：学习中的无效数值数量
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public static void _Learn(Int32 iCurrentValueIndex, Int32[] iValue, Int32 iToolsMinValue, Int32 iToolsMaxValue, ref Int32 iLearnedValue, ref Int32 iLearnedValidValueNumber, ref Int32 iLearnedNonvalidValueNumber)
        {
            int i = 0;//循环控制变量

            for (i = 0; i <= iCurrentValueIndex; i++)//计算
            {
                if (iToolsMinValue <= iValue[i] && iToolsMaxValue >= iValue[i])//有效值
                {
                    iLearnedValidValueNumber++;
                }
                else//无效值
                {
                    iLearnedNonvalidValueNumber++;
                }

                iLearnedValue += iValue[i];
            }

            if (0 <= iCurrentValueIndex)//有效
            {
                iLearnedValue = iLearnedValue / (iCurrentValueIndex + 1);//学习数值
            }
        }

        //

        //----------------------------------------------------------------------
        // 功能说明：构造函数
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        private void _SetDefaultData()//
        {
            tolerancesGraphData = new List<Struct.TolerancesGraphData>();//曲线图数据
        }
    }
}