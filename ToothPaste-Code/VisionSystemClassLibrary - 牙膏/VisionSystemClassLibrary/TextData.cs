/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：SystemString.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：文本

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

namespace VisionSystemClassLibrary.String
{
    public static class TextData
    {
        /// <summary>
        /// 算法参数的参数
        /// </summary>
        private static string[] sEnumType_CHN = new string[] { "颜色", "扫描", "类型", "检查", "水平", "垂直", "位置", "分割", "投影", "类型（光电）", "自适应类型", "分割类型" };

        private static string[] Check_Type_ENG = new string[] { "Grad L-D", "Grad D-L", "FirstGrad" };
        private static string[] Detect_Type_ENG = new string[] { "Gradient", "Threshold", "PixelCount", "Gray" };
        private static string[] Contrast_Color_ENG = new string[] { "Intensity", "Red", "Green", "Blue", "Single" };
        private static string[] Scan_Direction_ENG = new string[] { "Left-Right", "Right-Left", "Top-Bottom", "Bottom-Top", "Horizen", "Vertical" };
        private static string[] HSearch_ENG = new string[] { "Zero Step", "One Step", "Two Step" };
        private static string[] VSearch_ENG = new string[] { "Zero Step", "One Step", "Two Step" };
        private static string[] PosReference_ENG = new string[] { "On", "Off" };
        private static string[] DivideType_ENG = new string[] { "Single", "R-B", "All" };
        private static string[] ProjectionType_ENG = new string[] { "Horizen", "Vertical" };
        private static string[] Detect_Type_S_ENG = new string[] { "Average", "Edge", "Similarity" };
        private static string[] ADAPTIVE_TYPE_ENG = new string[] { "Mean", "Gaussian" };
        private static string[] THRESH_TYPE_ENG = new string[] { "BINARY", "BINARY_INV", "TRUNC", "TOZERO", "TOZERO_INV", "MASK", "OTSU" };
        private static string[][] sEnumArithmeticString_ENG = new string[][] { Contrast_Color_ENG, Scan_Direction_ENG, Detect_Type_ENG, Check_Type_ENG, HSearch_ENG, VSearch_ENG, PosReference_ENG, DivideType_ENG, ProjectionType_ENG, Detect_Type_S_ENG, ADAPTIVE_TYPE_ENG, THRESH_TYPE_ENG };

        private static string[] sCheck_Type_CHN = new string[] { "亮-暗前缘", "暗-亮前缘", "绝对前缘" };
        private static string[] sDetect_Type_CHN = new string[] { "梯度", "阈值", "像素计数", "灰度" };
        private static string[] sContrast_Color_CHN = new string[] { "全部", "红", "绿", "蓝", "单色" };
        private static string[] sScan_Direction_CHN = new string[] { "从左到右", "从右到左", "从上到下", "从下到上", "水平方向", "垂直方向" };
        private static string[] sHSearch_CHN = new string[] { "步长为0", "步长为1", "步长为2" };
        private static string[] sVSearch_CHN = new string[] { "步长为0", "步长为1", "步长为2" };
        private static string[] sPosReference_CHN = new string[] { "开", "关" };
        private static string[] sDivideType_CHN = new string[] { "单色分量", "红蓝之差", "全部" };
        private static string[] sProjectionType_CHN = new string[] { "水平", "垂直" };
        private static string[] sDetect_Type_S_CHN = new string[] { "平均", "边缘", "相似性" };
        private static string[] sADAPTIVE_TYPE_CHN = new string[] { "平均", "高斯" };
        private static string[] sTHRESH_TYPE_CHN = new string[] { "二值化", "二值化（反向）", "截断", "零填充", "零填充（反向）", "模板", "大津法" };
        private static string[][] sEnumArithmeticString_CHN = new string[][] { sContrast_Color_CHN, sScan_Direction_CHN, sDetect_Type_CHN, sCheck_Type_CHN, sHSearch_CHN, sVSearch_CHN, sPosReference_CHN, sDivideType_CHN, sProjectionType_CHN, sDetect_Type_S_CHN, sADAPTIVE_TYPE_CHN, sTHRESH_TYPE_CHN };

        /// <summary>
        /// 算法的参数
        /// </summary>
        private static string[] sToolType_CHN = new string[] { "格子", "质量", "直尺", "烟支", "乱烟", "拉线", "规则度量", "烟支（光电）", "散包（光电）", "定位（圆）", "定位（模板匹配）", "分类" };                         

        private static string[] aArithmeticName_Grid_CHN = new string[] { "单元尺寸", "水平搜索", "垂直搜索", "位置参考", "颜色对比" };
        private static string[] aArithmeticName_Quality_CHN = new string[] { "单元尺寸", "水平搜索", "垂直搜索", "位置参考", "颜色对比" };
        private static string[] aArithmeticName_Ruler_CHN = new string[] { "颜色对比", "扫描方向", "角度调整", "检测方式", "检查类型", "边缘宽度", "偏移距离", "德尔塔值", "像素下限", "像素上限" };
        private static string[] aArithmeticName_Tobacco_CHN = new string[] { "模盒阈值", "阈值分割", "颜色对比", "烟梗阈值", "分割阈值" };
        private static string[] aArithmeticName_Disorder_CHN = new string[] { "颜色对比", "边缘阈值", "连接阈值", "反支阈值" };
        private static string[] aArithmeticName_Line_CHN = new string[] { "扫描方向", "基准一", "基准二", "接头基准" };
        private static string[] aArithmeticName_CurveDispersion_CHN = new string[] { "颜色对比", "统计方向", "区间宽度" };
        private static string[] aArithmeticName_Tobacco_D_CHN = new string[] { "颜色对比" };
        private static string[] aArithmeticName_BaleLoosing_CHN = new string[] { "颜色对比", "扫描方向", "检测方式（光电）", "阈值", "曲线索引" };
        private static string[] aArithmeticName_LocationCicle_CHN = new string[] { "颜色对比", "最大灰度", "自适应类型", "分割类型", "模板大小", "滤波参数", "Canny阈值", "累加阈值", "最小中心距", "最小半径", "最大半径" };
        private static string[] aArithmeticName_Location_TemplateMatch_CHN = new string[] { "模板左边缘", "模板上边缘", "模板宽度", "模板高度" };
        private static string[] aArithmeticName_Classify_CHN = new string[] { "模型宽度", "模型高度" };
        private static string[][] sArithmeticNameString_CHN
            = new string[][] { aArithmeticName_Grid_CHN, aArithmeticName_Quality_CHN, aArithmeticName_Ruler_CHN, aArithmeticName_Tobacco_CHN, aArithmeticName_Disorder_CHN, aArithmeticName_Line_CHN, aArithmeticName_CurveDispersion_CHN, aArithmeticName_Tobacco_D_CHN, aArithmeticName_BaleLoosing_CHN, aArithmeticName_LocationCicle_CHN, aArithmeticName_Location_TemplateMatch_CHN , aArithmeticName_Classify_CHN };

        private static string[] ArithmeticName_Grid_ENG = new string[] { "Cell Size", "H.Search", "V.Search", "Pos.Reference", "Contrast Color" };
        private static string[] ArithmeticName_Quality_ENG = new string[] { "Cell Size", "H.Search", "V.Search", "Pos.Reference", "Contrast Color" };
        private static string[] ArithmeticName_Ruler_ENG = new string[] { "Contrast Color", "Scan Direction", "Angle Adjust", "Detect Type", "Check Type", "Edge Width", "Distance", "Delta Level", "IntensityMin", "IntensityMax" };
        private static string[] ArithmeticName_Tobacco_ENG = new string[] { "Hls", "Devide", "Contrast Color", "R-B Difference", "ThresholdValue" };
        private static string[] ArithmeticName_Disorder_ENG = new string[] { "Contrast Color", "EdgeThreshold", "LinkThreshold", "Reverse" };
        private static string[] ArithmeticName_Line_ENG = new string[] { "Scan Direction", "Threshold1", "Threshold2", "PaperThreshold" };
        private static string[] ArithmeticName_CurveDispersion_ENG = new string[] { "Contrast Color", "ProjectionType", "IntervalWidth" };
        private static string[] ArithmeticName_Tobacco_D_ENG = new string[] { "Contrast Color" };
        private static string[] ArithmeticName_BaleLoosing_ENG = new string[] { "Contrast Color", "Scan Direction", "Detect Type(S)", "Threshold", "Curve Index" };
        private static string[] aArithmeticName_LocationCicle_ENG = new string[] { "Contrast Color", "Max Gray", "Adaptive Type", "Threshold Type", "Templete Size", "Filter Parameter", "Canny Threashold", "Accumulator Parameter", "Min Dis", "Min Radius", "Max Radius" };
        private static string[] aArithmeticName_Location_TemplateMatch_ENG = new string[] { "Template Left", "Template Top ", "Template Width", "Template Height" };
        private static string[] aArithmeticName_Classify_ENG = new string[] { "Model Width", "Model Height" };
        private static string[][] sArithmeticNameString_ENG
            = new string[][] { ArithmeticName_Grid_ENG, ArithmeticName_Quality_ENG, ArithmeticName_Ruler_ENG, ArithmeticName_Tobacco_ENG, ArithmeticName_Disorder_ENG, ArithmeticName_Line_ENG, ArithmeticName_CurveDispersion_ENG, ArithmeticName_Tobacco_D_ENG, ArithmeticName_BaleLoosing_ENG, aArithmeticName_LocationCicle_ENG, aArithmeticName_Location_TemplateMatch_ENG, aArithmeticName_Classify_ENG };

        /// <summary>
        /// 相机配置参数
        /// </summary>
        private static string[] sImageParameter_CHN = new string[] { "光照时间", "光照强度", "增益", "曝光时间", "白平衡", "白平衡（红）", "白平衡（绿）", "白平衡（蓝）" };//相机参数
        private static string[] sImageParameter_ENG = new string[] { "Strobo Time", "Strobo Current", "Gain", "Exposure Time", "White Balance", "      Red", "      Green", "      Blue" };//相机参数

        private static string[] sImageParameter_WhiteBalance_CHN = new string[] { "自动", "手动" };//相机参数，白平衡
        private static string[] sImageParameter_WhiteBalance_ENG = new string[] { "Auto", "Manual" };//相机参数，白平衡

        /// <summary>
        /// 光电配置参数
        /// </summary>
        private static string[] sSerialPortParameter_CHN = new string[] { "烟支一", "烟支二", "烟支三", "烟支四", "烟支五", "烟支六", "烟支七", "烟支八", "烟支九", "烟支十", "烟支十一", "烟支十二", "烟支十三", "烟支十四", "烟支十五", "烟支十六", "烟支十七", "烟支十八", "烟支十九", "烟支二十" };//传感器参数
        private static string[] sSerialPortParameter_ENG = new string[] { "Cigarette1", "Cigarette2", "Cigarette3", "Cigarette4", "Cigarette5", "Cigarette6", "Cigarette7", "Cigarette8", "Cigarette9", "Cigarette10", "Cigarette11", "Cigarette12", "Cigarette13", "Cigarette14", "Cigarette15", "Cigarette16", "Cigarette17", "Cigarette18", "Cigarette19", "Cigarette20" };//传感器参数
    
        //属性

        // 功能说明：Contrast_Color_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] Contrast_Color_CHN
        {
            get
            {
                return sContrast_Color_CHN;
            }
        }

        // 功能说明：Scan_Direction_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] Scan_Direction_CHN
        {
            get
            {
                return sScan_Direction_CHN;
            }
        }

        // 功能说明：Detect_Type_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] Detect_Type_CHN
        {
            get
            {
                return sDetect_Type_CHN;
            }
        }

        // 功能说明：Check_Type_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] Check_Type_CHN
        {
            get
            {
                return sCheck_Type_CHN;
            }
        }

        // 功能说明：PosReference_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] PosReference_CHN
        {
            get
            {
                return sPosReference_CHN;
            }
        }

        // 功能说明：DivideType_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] DivideType_CHN
        {
            get
            {
                return sDivideType_CHN;
            }
        }

        // 功能说明：ProjectionType_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ProjectionType_CHN
        {
            get
            {
                return sProjectionType_CHN;
            }
        }

        // 功能说明：Detect_Type_S_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] Detect_Type_S_CHN
        {
            get
            {
                return sDetect_Type_S_CHN;
            }
        }

        // 功能说明：ADAPTIVE_TYPE_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ADAPTIVE_TYPE_CHN
        {
            get
            {
                return sADAPTIVE_TYPE_CHN;
            }
        }

        // 功能说明：THRESH_TYPE_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] THRESH_TYPE_CHN
        {
            get
            {
                return sTHRESH_TYPE_CHN;
            }
        }
        
        // 功能说明：HSearch_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] HSearch_CHN
        {
            get
            {
                return sHSearch_CHN;
            }
        }

        // 功能说明：VSearch_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] VSearch_CHN
        {
            get
            {
                return sVSearch_CHN;
            }
        }

        // 功能说明：SerialPortParameter_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] SerialPortParameter_CHN
        {
            get
            {
                return sSerialPortParameter_CHN;
            }
        }

        // 功能说明：SerialPortParameter_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] SerialPortParameter_ENG
        {
            get
            {
                return sSerialPortParameter_ENG;
            }
        }

        // 功能说明：ImageParameter_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ImageParameter_CHN
        {
            get
            {
                return sImageParameter_CHN;
            }
        }

        // 功能说明：ImageParameter_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ImageParameter_ENG
        {
            get
            {
                return sImageParameter_ENG;
            }
        }

        // 功能说明：ImageParameter_WhiteBalance_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ImageParameter_WhiteBalance_CHN
        {
            get
            {
                return sImageParameter_WhiteBalance_CHN;
            }
        }

        // 功能说明：ImageParameter_WhiteBalance_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ImageParameter_WhiteBalance_ENG
        {
            get
            {
                return sImageParameter_WhiteBalance_ENG;
            }
        }

        // 功能说明：EnumArithmeticString_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[][] EnumArithmeticString_CHN
        {
            get
            {
                return sEnumArithmeticString_CHN;
            }
        }

        // 功能说明：EnumArithmeticString_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[][] EnumArithmeticString_ENG
        {
            get
            {
                return sEnumArithmeticString_ENG;
            }
        }
        

        // 功能说明：ArithmeticNameString_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[][] ArithmeticNameString_CHN
        {
            get
            {
                return sArithmeticNameString_CHN;
            }
        }

        // 功能说明：ArithmeticNameString_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[][] ArithmeticNameString_ENG
        {
            get
            {
                return sArithmeticNameString_ENG;
            }
        }
        
        // 功能说明：ToolType_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ToolType_CHN
        {
            get
            {
                return sToolType_CHN;
            }
        }

        // 功能说明：EnumType_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] EnumType_CHN
        {
            get
            {
                return sEnumType_CHN;
            }
        }

        // 功能说明：ArithmeticName_Grid_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Grid_CHN
        {
            get
            {
                return aArithmeticName_Grid_CHN;
            }
        }

        // 功能说明：ArithmeticName_Quality_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Quality_CHN
        {
            get
            {
                return aArithmeticName_Quality_CHN;
            }
        }

        // 功能说明：ArithmeticName_Ruler_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Ruler_CHN
        {
            get
            {
                return aArithmeticName_Ruler_CHN;
            }
        }

        // 功能说明：ArithmeticName_Tobacco_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Tobacco_CHN
        {
            get
            {
                return aArithmeticName_Tobacco_CHN;
            }
        }

        // 功能说明：ArithmeticName_Disorder_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Disorder_CHN
        {
            get
            {
                return aArithmeticName_Disorder_CHN;
            }
        }

        // 功能说明：ArithmeticName_Line_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Line_CHN
        {
            get
            {
                return aArithmeticName_Line_CHN;
            }
        }

        // 功能说明：ArithmeticName_CurveDispersion_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_CurveDispersion_CHN
        {
            get
            {
                return aArithmeticName_CurveDispersion_CHN;
            }
        }

        // 功能说明：ArithmeticName_Tobacco_D_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Tobacco_D_CHN
        {
            get
            {
                return aArithmeticName_Tobacco_D_CHN;
            }
        }

        // 功能说明：ArithmeticName_BaleLoosing_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_BaleLoosing_CHN
        {
            get
            {
                return aArithmeticName_BaleLoosing_CHN;
            }
        }

        // 功能说明：ArithmeticName_LocationCicle_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_LocationCicle_CHN
        {
            get
            {
                return aArithmeticName_LocationCicle_CHN;
            }
        }

        // 功能说明：ArithmeticName_LocationCicle_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_LocationCicle_ENG
        {
            get
            {
                return aArithmeticName_LocationCicle_ENG;
            }
        }

        // 功能说明：ArithmeticName_Location_TemplateMatch_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Location_TemplateMatch_CHN
        {
            get
            {
                return aArithmeticName_Location_TemplateMatch_CHN;
            }
        }

        // 功能说明：ArithmeticName_Location_TemplateMatch_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Location_TemplateMatch_ENG
        {
            get
            {
                return aArithmeticName_Location_TemplateMatch_ENG;
            }
        }

        // 功能说明：ArithmeticName_Classify_CHN属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Classify_CHN
        {
            get
            {
                return aArithmeticName_Classify_CHN;
            }
        }

        // 功能说明：ArithmeticName_Classify_ENG属性
        // 输入参数：无
        // 输出参数：无
        // 返 回 值：无
        //----------------------------------------------------------------------
        public static string[] ArithmeticName_Classify_ENG
        {
            get
            {
                return aArithmeticName_Classify_ENG;
            }
        }
    }
}
