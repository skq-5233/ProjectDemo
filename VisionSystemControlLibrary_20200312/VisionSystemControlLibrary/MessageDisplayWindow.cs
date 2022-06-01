/****************************************************************

中国电子科技集团公司第41研究所对本代码拥有全部版权，未经许可不得
引用本代码任何内容，也不得用于非中国电子科技集团公司第41研究所之
外的任何商业或非商业项目。

课题名称：FOCKE复合型烟支检测器
课题令号：41S1301
开发部门：研发一部

文件名称：MessageDisplayWindow.cs
开发环境：Microsoft Visual Studio 2010
运行环境：Windows

功能描述：提示信息窗口

原作者：视觉检测团队
完成日期：2014/08/18
特别说明：经视觉检测团队授权并遵守代码使用条款，方可使用本代码并获得技术支持，否则所产生的一切后果由使用者承担

修改者：无
完成日期：无
修改记录：无

****************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisionSystemControlLibrary
{
    public partial class MessageDisplayWindow : Form
    {
        //提示信息窗口

        private Int32 iWindowParameter = 0;//属性，窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件。取值范围：
        //1.BACKUP BRANDS，【CREATE FOLDER】成功
        //2.BACKUP BRANDS，【CREATE FOLDER】失败
        //3.BACKUP BRANDS，【DELETE】确认
        //4.BACKUP BRANDS，【DELETE】等待
        //6.BACKUP BRANDS，【BACK UP】确认
        //7.BACKUP BRANDS，【BACK UP】等待
        //9.BRAND，【SAVE CURRENT】确认
        //10.BRAND，【SAVE CURRENT】成功
        //11.BRAND，【SAVE CURRENT】失败
        //12.BRAND，【LOAD BRAND】（【RELOAD BRAND】）确认
        //13.BRAND，【LOAD BRAND】（【RELOAD BRAND】）等待
        //18.BRAND，【COPY BRAND】成功
        //19.BRAND，【COPY BRAND】失败（拷贝文件）
        //20.BRAND，【COPY BRAND】失败（重名）
        //21.BRAND，【COPY BRAND】失败（数量达到最大值）
        //22.BRAND，【RENAME BRAND】成功
        //23.BRAND，【RENAME BRAND】失败（拷贝文件）
        //24.BRAND，【RENAME BRAND】失败（重名）
        //25.BRAND，【DELETE BRAND】确认
        //26.BRAND，【DELETE BRAND】成功
        //27.BRAND，【DELETE BRAND】失败
        //28.DEVICES SETUP，【RESET DEVICE】确认
        //29.DEVICES SETUP，【RESET DEVICE】成功
        //30.DEVICES SETUP，【RESET DEVICE】失败
        //31.DEVICES SETUP，【CONFIG DEVICE】等待
        //33.DEVICES SETUP，【ALIGN DATE/TIME】确认
        //34.DEVICES SETUP，【ALIGN DATE/TIME】成功
        //35.DEVICES SETUP，【ALIGN DATE/TIME】失败
        //36.IMAGE CONFIGURATION，【SAVE PRODUCT】确认
        //37.IMAGE CONFIGURATION，【SAVE PRODUCT】成功
        //38.IMAGE CONFIGURATION，【SAVE PRODUCT】失败
        //39.QUALITY CHECK，【SAVE PRODUCT】确认
        //40.QUALITY CHECK，【SAVE PRODUCT】成功
        //41.QUALITY CHECK，【SAVE PRODUCT】失败
        //77.QUALITY CHECK，【LEARN SAMPLE】确认
        //78.QUALITY CHECK，【LEARN SAMPLE】成功
        //79.QUALITY CHECK，【LEARN SAMPLE】失败
        //42.REJECTS，【BACKUP SINGLE IMAGE】确认
        //43.REJECTS，【BACKUP ALL IMAGES】确认
        //81.REJECTS，【BACKUP ALL IMAGES】等待
        //44.REJECTS，【CLEAR ALL】确认
        //82.REJECTS，【CLEAR ALL】等待
        //45.RESTORE BRANDS，【DELETE】确认
        //46.RESTORE BRANDS，【DELETE】等待
        //48.RESTORE BRANDS，【RESTORE】确认
        //80.RESTORE BRANDS，【RESTORE】等待
        //51.RESTORE BRANDS，RESTORE（LOCAL DISK），覆盖已存在文件确认
        //52.RESTORE BRANDS，RESTORE（USB DEVICE），覆盖已存在文件确认
        //53.SYSTEM，【OK】等待
        //73.SYSTEM，【OK】（保存参数）确认
        //54.TOLERANCES，【LEARN】（曲线图控件1）确认
        //55.TOLERANCES，【LEARN】（曲线图控件1）错误
        //56.TOLERANCES，【LEARN】（曲线图控件2）确认
        //57.TOLERANCES，【LEARN】（曲线图控件2）错误
        //58.TOLERANCES，【LEARN】（曲线图控件3）确认
        //59.TOLERANCES，【LEARN】（曲线图控件3）错误
        //60.TOLERANCES，【LEARN】（曲线图控件4）确认
        //61.TOLERANCES，【LEARN】（曲线图控件4）错误
        //62.TOLERANCES，【LEARN】成功
        //63.TOLERANCES，【LEARN】失败
        //64.TOLERANCES，【SAVE PRODUCT】确认
        //65.TOLERANCES，【SAVE PRODUCT】成功
        //66.TOLERANCES，【SAVE PRODUCT】失败
        //67.TOLERANCES，【RESET GRAPHS】确认
        //68.TOLERANCES，【RESET GRAPHS】成功
        //69.TOLERANCES，【RESET GRAPHS】失败
        //70.WORK，【系统更新】确认
        //92.WORK，【系统更新】等待
        //71.WORK，点击左侧TRADEMARK图标
        //72.WORK，点击右侧TRADEMARK图标
        //76.WORK，退出程序确认
        //75.DEVICE CONFIGURATION，【OK】确认

        //83.STATISTICS RECORD，【DELETE】（【DELETE ALL】）确认
        //84.STATISTICS RECORD，【DELETE】（【DELETE ALL】）等待
        //85.STATISTICS RECORD，【OK】确认
        //88.STATISTICS RECORD，更新统计数据

        //86.STATISTICS，获取统计数据

        //89.FAULT MESSAGE，获取故障信息数据
        //90.FAULT MESSAGE，清除所有故障信息数据确认
        //91.FAULT MESSAGE，清除所有故障信息数据

        //93.EDIT TOOLS，【NEW TOOL】成功
        //94.EDIT TOOLS，【NEW TOOL】失败（重名）
        //95.EDIT TOOLS，【NEW TOOL】失败（数量达到最大值）
        //96.EDIT TOOLS，【COPY TOOL】成功
        //97.EDIT TOOLS，【COPY TOOL】失败（重名）
        //98.EDIT TOOLS，【COPY TOOL】失败（数量达到最大值）
        //99.EDIT TOOLS，【RENAME TOOL】成功
        //100.EDIT TOOLS，【RENAME TOOL】失败（重名）
        //101.EDIT TOOLS，【DELETE TOOL】确认
        //102.EDIT TOOLS，【DELETE TOOL】成功
        //103.EDIT TOOLS，【DELETE TOOL】失败

        //104.PARAMETER SETTINGS，【保存参数】确认
        //105.PARAMETER SETTINGS，【保存参数】确认

        //106.Net Check，网络查询

        //107.IMAGE CONFIGURATION，校准结束提示信息

        [Browsable(true), Description("BACKUP BRANDS，【CREATE FOLDER】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_CreateFolder_Success;

        [Browsable(true), Description("BACKUP BRANDS，【CREATE FOLDER】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_CreateFolder_Failure;

        [Browsable(true), Description("BACKUP BRANDS，【DELETE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_Delete_Confirm;

        [Browsable(true), Description("BACKUP BRANDS，【DELETE】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_Delete_Wait;

        [Browsable(true), Description("BACKUP BRANDS，【BACK UP】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_Backup_Confirm;

        [Browsable(true), Description("BACKUP BRANDS，【BACK UP】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_BackupBrands_Backup_Wait;

        //

        [Browsable(true), Description("BRAND，【SAVE CURRENT】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_SaveCurrent_Confirm;

        [Browsable(true), Description("BRAND，【SAVE CURRENT】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_SaveCurrent_Success;

        [Browsable(true), Description("BRAND，【SAVE CURRENT】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_SaveCurrent_Failure;

        [Browsable(true), Description("BRAND，【LOAD BRAND】（【RELOAD BRAND】）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_LoadReloadBrand_Confirm;

        [Browsable(true), Description("BRAND，【LOAD BRAND】（【RELOAD BRAND】）等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_LoadReloadBrand_Wait;

        [Browsable(true), Description("BRAND，【COPY BRAND】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_CopyBrand_Success;

        [Browsable(true), Description("BRAND，【COPY BRAND】失败（拷贝文件），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_CopyBrand_Failure_1;

        [Browsable(true), Description("BRAND，【COPY BRAND】失败（重名），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_CopyBrand_Failure_2;

        [Browsable(true), Description("BRAND，【COPY BRAND】失败（数量达到最大值），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_CopyBrand_Failure_3;

        [Browsable(true), Description("BRAND，【RENAME BRAND】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_RenameBrand_Success;

        [Browsable(true), Description("BRAND，【RENAME BRAND】失败（拷贝文件），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_RenameBrand_Failure_1;

        [Browsable(true), Description("BRAND，【RENAME BRAND】失败（重名），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_RenameBrand_Failure_2;

        [Browsable(true), Description("BRAND，【DELETE BRAND】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_DeleteBrand_Confirm;

        [Browsable(true), Description("BRAND，【DELETE BRAND】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_DeleteBrand_Success;

        [Browsable(true), Description("BRAND，【DELETE BRAND】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Brand_DeleteBrand_Failure;

        //

        [Browsable(true), Description("DEVICES SETUP，【RESET DEVICE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_ResetDevice_Confirm;

        [Browsable(true), Description("DEVICES SETUP，【RESET DEVICE】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_ResetDevice_Success;

        [Browsable(true), Description("DEVICES SETUP，【RESET DEVICE】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_ResetDevice_Failure;

        [Browsable(true), Description("DEVICES SETUP，【CONFIG DEVICE】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_ConfigDevice_Wait;

        [Browsable(true), Description("DEVICES SETUP，【ALIGN DATE/TIME】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_AlignDateTime_Confirm;

        [Browsable(true), Description("DEVICES SETUP，【ALIGN DATE/TIME】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_AlignDateTime_Success;

        [Browsable(true), Description("DEVICES SETUP，【ALIGN DATE/TIME】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DevicesSetup_AlignDateTime_Failure;

        //

        [Browsable(true), Description("IMAGE CONFIGURATION，【SAVE PRODUCT】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_ImageConfiguration_SaveProduct_Confirm;

        [Browsable(true), Description("IMAGE CONFIGURATION，【SAVE PRODUCT】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_ImageConfiguration_SaveProduct_Success;

        [Browsable(true), Description("IMAGE CONFIGURATION，【SAVE PRODUCT】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_ImageConfiguration_SaveProduct_Failure;

        //

        [Browsable(true), Description("QUALITY CHECK，【SAVE PRODUCT】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_SaveProduct_Confirm;

        [Browsable(true), Description("QUALITY CHECK，【SAVE PRODUCT】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_SaveProduct_Success;

        [Browsable(true), Description("QUALITY CHECK，【SAVE PRODUCT】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_SaveProduct_Failure;

        [Browsable(true), Description("QUALITY CHECK，【LEARN SAMPLE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_LearnSample_Confirm;

        [Browsable(true), Description("QUALITY CHECK，【LEARN SAMPLE】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_LearnSample_Success;

        [Browsable(true), Description("QUALITY CHECK，【LEARN SAMPLE】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_QualityCheck_LearnSample_Failure;

        //

        [Browsable(true), Description("REJECTS，【BACKUP SINGLE IMAGE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Rejects_BackupSingleImage_Confirm;

        [Browsable(true), Description("REJECTS，【BACKUP ALL IMAGES】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Rejects_BackupAllImages_Confirm;

        [Browsable(true), Description("REJECTS，【BACKUP ALL IMAGES】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Rejects_BackupAllImages_Wait;

        [Browsable(true), Description("REJECTS，【CLEAR ALL】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Rejects_ClearAll_Confirm;

        [Browsable(true), Description("REJECTS，【CLEAR ALL】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Rejects_ClearAll_Wait;

        //

        [Browsable(true), Description("RESTORE BRANDS，【DELETE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Delete_Confirm;

        [Browsable(true), Description("RESTORE BRANDS，【DELETE】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Delete_Wait;

        [Browsable(true), Description("RESTORE BRANDS，【RESTORE】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Restore_Confirm;

        [Browsable(true), Description("RESTORE BRANDS，【RESTORE】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Restore_Wait;

        [Browsable(true), Description("RESTORE BRANDS，RESTORE（LOCAL DISK），覆盖已存在文件确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk;

        [Browsable(true), Description("RESTORE BRANDS，RESTORE（USB DEVICE），覆盖已存在文件确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice;

        //

        [Browsable(true), Description("SYSTEM，【OK】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_System_Ok_Wait;

        [Browsable(true), Description("SYSTEM，【OK】（保存参数）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_System_Ok_Confirm;

        //

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件1）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph1_Confirm;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件1）错误，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph1_Failure;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件2）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph2_Confirm;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件2）错误，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph2_Failure;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件3）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph3_Confirm;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件3）错误，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph3_Failure;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件4）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph4_Confirm;

        [Browsable(true), Description("TOLERANCES，【LEARN】（曲线图控件4）错误，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Graph4_Failure;

        [Browsable(true), Description("TOLERANCES，【LEARN】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Success;

        [Browsable(true), Description("TOLERANCES，【LEARN】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_Learn_Failure;

        [Browsable(true), Description("TOLERANCES，【SAVE PRODUCT】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_SaveProduct_Confirm;

        [Browsable(true), Description("TOLERANCES，【SAVE PRODUCT】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_SaveProduct_Success;

        [Browsable(true), Description("TOLERANCES，【SAVE PRODUCT】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_SaveProduct_Failure;

        [Browsable(true), Description("TOLERANCES，【RESET GRAPHS】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_ResetGraphs_Confirm;

        [Browsable(true), Description("TOLERANCES，【RESET GRAPHS】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_ResetGraphs_Success;

        [Browsable(true), Description("TOLERANCES，【RESET GRAPHS】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Tolerances_ResetGraphs_Failure;

        //

        [Browsable(true), Description("WORK，【系统更新】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Work_Update_Confirm;

        [Browsable(true), Description("WORK，【系统更新】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Work_Update_Wait;

        [Browsable(true), Description("WORK，点击左侧TRADEMARK图标，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Work_LeftTrademark;

        [Browsable(true), Description("WORK，点击右侧TRADEMARK图标，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Work_RightTrademark;

        [Browsable(true), Description("WORK，退出程序确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Work_ExitApplication_Confirm;

        //

        [Browsable(true), Description("DEVICE CONFIGURATION，【OK】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_DeviceConfiguration_Ok_Confirm;

        //

        [Browsable(true), Description("STATISTICS RECORD，【DELETE】（【DELETE ALL】）确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_StatisticsRecord_Delete_Confirm;

        [Browsable(true), Description("STATISTICS RECORD，【DELETE】（【DELETE ALL】）等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_StatisticsRecord_Delete_Wait;
        
        [Browsable(true), Description("STATISTICS RECORD，【OK】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_StatisticsRecord_Ok_Confirm;

        [Browsable(true), Description("STATISTICS RECORD，更新统计数据，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_StatisticsRecord_GetRecord_Wait;

        //

        [Browsable(true), Description("STATISTICS，获取统计数据，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_Statistics_GetRecordData_Wait;

        //

        [Browsable(true), Description("FAULT MESSAGE，获取故障信息数据，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_FaultMessage_GetData_Wait;

        [Browsable(true), Description("FAULT MESSAGE，清除所有故障信息数据确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_FaultMessage_ClearAll_Confirm;

        [Browsable(true), Description("FAULT MESSAGE，清除所有故障信息数据，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_FaultMessage_ClearAll_Wait;

        //

        [Browsable(true), Description("EDIT TOOLS，【NEW TOOL】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_NewTool_Success;

        [Browsable(true), Description("EDIT TOOLS，【NEW TOOL】失败（重名），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_NewTool_Failure_1;

        [Browsable(true), Description("EDIT TOOLS，【NEW TOOL】失败（数量达到最大值），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_NewTool_Failure_2;

        [Browsable(true), Description("EDIT TOOLS，【COPY TOOL】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_CopyTool_Success;

        [Browsable(true), Description("EDIT TOOLS，【COPY TOOL】失败（重名），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_CopyTool_Failure_1;

        [Browsable(true), Description("EDIT TOOLS，【COPY TOOL】失败（数量达到最大值），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_CopyTool_Failure_2;

        [Browsable(true), Description("EDIT TOOLS，【RENAME TOOL】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_RenameTool_Success;

        [Browsable(true), Description("EDIT TOOLS，【RENAME TOOL】失败（重名），窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_RenameTool_Failure_1;

        [Browsable(true), Description("EDIT TOOLS，【DELETE TOOL】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_DeleteTool_Confirm;

        [Browsable(true), Description("EDIT TOOLS，【DELETE TOOL】成功，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_DeleteTool_Success;

        [Browsable(true), Description("EDIT TOOLS，【DELETE TOOL】失败，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_EditTools_DeleteTool_Failure;

        //

        [Browsable(true), Description("PARAMETER SETTINGS，【保存参数】确认，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_ParameterSettings_Save_Confirm;

        [Browsable(true), Description("PARAMETER SETTINGS，【保存参数】等待，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_ParameterSettings_Save_Wait;

        [Browsable(true), Description("Net Check，网络查询，窗口关闭时产生的事件"), Category("MessageDisplayWindow 事件")]
        public event EventHandler WindowClose_NetCheck;

        //

        //构造函数

        //----------------------------------------------------------------------
        // 功能说明：构造函数，初始化
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        public MessageDisplayWindow()
        {
            InitializeComponent();
        }

        //属性

        //----------------------------------------------------------------------
        // 功能说明：MessageDisplayControl属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("控件"), Category("MessageDisplayWindow 通用")]
        public MessageDisplay MessageDisplayControl//属性
        {
            get//读取
            {
                return messageDisplay;
            }
        }

        //----------------------------------------------------------------------
        // 功能说明：WindowParameter属性的实现
        // 输入参数：无
        // 输出参数：无
        // 返回值：无
        //----------------------------------------------------------------------
        [Browsable(false), Description("窗口特征数值，表示调用窗口的父级窗口类型，以便产生相应的事件"), Category("MessageDisplayWindow 通用")]
        public Int32 WindowParameter//属性
        {
            get//读取
            {
                return iWindowParameter;
            }
            set//设置
            {
                iWindowParameter = value;
            }
        }

        //事件

        //-----------------------------------------------------------------------
        // 功能说明：窗口关闭时产生的事件，执行相关操作
        // 输入参数：1.sender：按钮自身的引用
        //         2.e：事件传递的参数
        // 输出参数： 无
        // 返 回 值： 无
        //----------------------------------------------------------------------
        private void messageDisplay_Close_Click(object sender, EventArgs e)
        {
            if (1 == iWindowParameter)//BACKUP BRANDS，【CREATE FOLDER】成功
            {
                if (null != WindowClose_BackupBrands_CreateFolder_Success)//有效
                {
                    WindowClose_BackupBrands_CreateFolder_Success(this, new CustomEventArgs());
                }
            }
            else if (2 == iWindowParameter)//BACKUP BRANDS，【CREATE FOLDER】失败
            {
                if (null != WindowClose_BackupBrands_CreateFolder_Failure)//有效
                {
                    WindowClose_BackupBrands_CreateFolder_Failure(this, new CustomEventArgs());
                }
            }
            else if (3 == iWindowParameter)//BACKUP BRANDS，【DELETE】确认
            {
                if (null != WindowClose_BackupBrands_Delete_Confirm)//有效
                {
                    WindowClose_BackupBrands_Delete_Confirm(this, new CustomEventArgs());
                }
            }
            else if (4 == iWindowParameter)//BACKUP BRANDS，【DELETE】等待
            {
                if (null != WindowClose_BackupBrands_Delete_Wait)//有效
                {
                    WindowClose_BackupBrands_Delete_Wait(this, new CustomEventArgs());
                }
            }
            else if (6 == iWindowParameter)//BACKUP BRANDS，【BACK UP】确认
            {
                if (null != WindowClose_BackupBrands_Backup_Confirm)//有效
                {
                    WindowClose_BackupBrands_Backup_Confirm(this, new CustomEventArgs());
                }
            }
            else if (7 == iWindowParameter)//BACKUP BRANDS，【BACK UP】等待
            {
                if (null != WindowClose_BackupBrands_Backup_Wait)//有效
                {
                    WindowClose_BackupBrands_Backup_Wait(this, new CustomEventArgs());
                }
            }
            else if (9 == iWindowParameter)//BRAND，【SAVE CURRENT】确认
            {
                if (null != WindowClose_Brand_SaveCurrent_Confirm)//有效
                {
                    WindowClose_Brand_SaveCurrent_Confirm(this, new CustomEventArgs());
                }
            }
            else if (10 == iWindowParameter)//BRAND，【SAVE CURRENT】成功
            {
                if (null != WindowClose_Brand_SaveCurrent_Success)//有效
                {
                    WindowClose_Brand_SaveCurrent_Success(this, new CustomEventArgs());
                }
            }
            else if (11 == iWindowParameter)//BRAND，【SAVE CURRENT】失败
            {
                if (null != WindowClose_Brand_SaveCurrent_Failure)//有效
                {
                    WindowClose_Brand_SaveCurrent_Failure(this, new CustomEventArgs());
                }
            }
            else if (12 == iWindowParameter)//BRAND，【LOAD BRAND】（【RELOAD BRAND】）确认
            {
                if (null != WindowClose_Brand_LoadReloadBrand_Confirm)//有效
                {
                    WindowClose_Brand_LoadReloadBrand_Confirm(this, new CustomEventArgs());
                }
            }
            else if (13 == iWindowParameter)//BRAND，【LOAD BRAND】（【RELOAD BRAND】）等待
            {
                if (null != WindowClose_Brand_LoadReloadBrand_Wait)//有效
                {
                    WindowClose_Brand_LoadReloadBrand_Wait(this, new CustomEventArgs());
                }
            }
            else if (18 == iWindowParameter)//BRAND，【COPY BRAND】成功
            {
                if (null != WindowClose_Brand_CopyBrand_Success)//有效
                {
                    WindowClose_Brand_CopyBrand_Success(this, new CustomEventArgs());
                }
            }
            else if (19 == iWindowParameter)//BRAND，【COPY BRAND】失败（拷贝文件）
            {
                if (null != WindowClose_Brand_CopyBrand_Failure_1)//有效
                {
                    WindowClose_Brand_CopyBrand_Failure_1(this, new CustomEventArgs());
                }
            }
            else if (20 == iWindowParameter)//BRAND，【COPY BRAND】失败（重名）
            {
                if (null != WindowClose_Brand_CopyBrand_Failure_2)//有效
                {
                    WindowClose_Brand_CopyBrand_Failure_2(this, new CustomEventArgs());
                }
            }
            else if (21 == iWindowParameter)//BRAND，【COPY BRAND】失败（数量达到最大值）
            {
                if (null != WindowClose_Brand_CopyBrand_Failure_3)//有效
                {
                    WindowClose_Brand_CopyBrand_Failure_3(this, new CustomEventArgs());
                }
            }
            else if (22 == iWindowParameter)//BRAND，【RENAME BRAND】成功
            {
                if (null != WindowClose_Brand_RenameBrand_Success)//有效
                {
                    WindowClose_Brand_RenameBrand_Success(this, new CustomEventArgs());
                }
            }
            else if (23 == iWindowParameter)//BRAND，【RENAME BRAND】失败（拷贝文件）
            {
                if (null != WindowClose_Brand_RenameBrand_Failure_1)//有效
                {
                    WindowClose_Brand_RenameBrand_Failure_1(this, new CustomEventArgs());
                }
            }
            else if (24 == iWindowParameter)//BRAND，【RENAME BRAND】失败（重名）
            {
                if (null != WindowClose_Brand_RenameBrand_Failure_2)//有效
                {
                    WindowClose_Brand_RenameBrand_Failure_2(this, new CustomEventArgs());
                }
            }
            else if (25 == iWindowParameter)//BRAND，【DELETE BRAND】确认
            {
                if (null != WindowClose_Brand_DeleteBrand_Confirm)//有效
                {
                    WindowClose_Brand_DeleteBrand_Confirm(this, new CustomEventArgs());
                }
            }
            else if (26 == iWindowParameter)//BRAND，【DELETE BRAND】成功
            {
                if (null != WindowClose_Brand_DeleteBrand_Success)//有效
                {
                    WindowClose_Brand_DeleteBrand_Success(this, new CustomEventArgs());
                }
            }
            else if (27 == iWindowParameter)//BRAND，【DELETE BRAND】失败
            {
                if (null != WindowClose_Brand_DeleteBrand_Failure)//有效
                {
                    WindowClose_Brand_DeleteBrand_Failure(this, new CustomEventArgs());
                }
            }
            else if (28 == iWindowParameter)//DEVICES SETUP，【RESET DEVICE】确认
            {
                if (null != WindowClose_DevicesSetup_ResetDevice_Confirm)//有效
                {
                    WindowClose_DevicesSetup_ResetDevice_Confirm(this, new CustomEventArgs());
                }
            }
            else if (29 == iWindowParameter)//DEVICES SETUP，【RESET DEVICE】成功
            {
                if (null != WindowClose_DevicesSetup_ResetDevice_Success)//有效
                {
                    WindowClose_DevicesSetup_ResetDevice_Success(this, new CustomEventArgs());
                }
            }
            else if (30 == iWindowParameter)//DEVICES SETUP，【RESET DEVICE】失败
            {
                if (null != WindowClose_DevicesSetup_ResetDevice_Failure)//有效
                {
                    WindowClose_DevicesSetup_ResetDevice_Failure(this, new CustomEventArgs());
                }
            }
            else if (31 == iWindowParameter)//DEVICES SETUP，【CONFIG DEVICE】等待
            {
                if (null != WindowClose_DevicesSetup_ConfigDevice_Wait)//有效
                {
                    WindowClose_DevicesSetup_ConfigDevice_Wait(this, new CustomEventArgs());
                }
            }
            else if (33 == iWindowParameter)//DEVICES SETUP，【ALIGN DATE/TIME】确认
            {
                if (null != WindowClose_DevicesSetup_AlignDateTime_Confirm)//有效
                {
                    WindowClose_DevicesSetup_AlignDateTime_Confirm(this, new CustomEventArgs());
                }
            }
            else if (34 == iWindowParameter)//DEVICES SETUP，【ALIGN DATE/TIME】成功
            {
                if (null != WindowClose_DevicesSetup_AlignDateTime_Success)//有效
                {
                    WindowClose_DevicesSetup_AlignDateTime_Success(this, new CustomEventArgs());
                }
            }
            else if (35 == iWindowParameter)//DEVICES SETUP，【ALIGN DATE/TIME】失败
            {
                if (null != WindowClose_DevicesSetup_AlignDateTime_Failure)//有效
                {
                    WindowClose_DevicesSetup_AlignDateTime_Failure(this, new CustomEventArgs());
                }
            }
            else if (36 == iWindowParameter)//IMAGE CONFIGURATION，【SAVE PRODUCT】确认
            {
                if (null != WindowClose_ImageConfiguration_SaveProduct_Confirm)//有效
                {
                    WindowClose_ImageConfiguration_SaveProduct_Confirm(this, new CustomEventArgs());
                }
            }
            else if (37 == iWindowParameter)//IMAGE CONFIGURATION，【SAVE PRODUCT】成功
            {
                if (null != WindowClose_ImageConfiguration_SaveProduct_Success)//有效
                {
                    WindowClose_ImageConfiguration_SaveProduct_Success(this, new CustomEventArgs());
                }
            }
            else if (38 == iWindowParameter)//IMAGE CONFIGURATION，【SAVE PRODUCT】失败
            {
                if (null != WindowClose_ImageConfiguration_SaveProduct_Failure)//有效
                {
                    WindowClose_ImageConfiguration_SaveProduct_Failure(this, new CustomEventArgs());
                }
            }
            else if (39 == iWindowParameter)//QUALITY CHECK，【SAVE PRODUCT】确认
            {
                if (null != WindowClose_QualityCheck_SaveProduct_Confirm)//有效
                {
                    WindowClose_QualityCheck_SaveProduct_Confirm(this, new CustomEventArgs());
                }
            }
            else if (40 == iWindowParameter)//QUALITY CHECK，【SAVE PRODUCT】成功
            {
                if (null != WindowClose_QualityCheck_SaveProduct_Success)//有效
                {
                    WindowClose_QualityCheck_SaveProduct_Success(this, new CustomEventArgs());
                }
            }
            else if (41 == iWindowParameter)//QUALITY CHECK，【SAVE PRODUCT】失败
            {
                if (null != WindowClose_QualityCheck_SaveProduct_Failure)//有效
                {
                    WindowClose_QualityCheck_SaveProduct_Failure(this, new CustomEventArgs());
                }
            }
            else if (77 == iWindowParameter)//QUALITY CHECK，【LEARN SAMPLE】确认
            {
                if (null != WindowClose_QualityCheck_LearnSample_Confirm)//有效
                {
                    WindowClose_QualityCheck_LearnSample_Confirm(this, new CustomEventArgs());
                }
            }
            else if (78 == iWindowParameter)//QUALITY CHECK，【LEARN SAMPLE】成功
            {
                if (null != WindowClose_QualityCheck_LearnSample_Success)//有效
                {
                    WindowClose_QualityCheck_LearnSample_Success(this, new CustomEventArgs());
                }
            }
            else if (79 == iWindowParameter)//QUALITY CHECK，【LEARN SAMPLE】失败
            {
                if (null != WindowClose_QualityCheck_LearnSample_Failure)//有效
                {
                    WindowClose_QualityCheck_LearnSample_Failure(this, new CustomEventArgs());
                }
            }
            else if (42 == iWindowParameter)//REJECTS，【BACKUP SINGLE IMAGE】确认
            {
                if (null != WindowClose_Rejects_BackupSingleImage_Confirm)//有效
                {
                    WindowClose_Rejects_BackupSingleImage_Confirm(this, new CustomEventArgs());
                }
            }
            else if (43 == iWindowParameter)//REJECTS，【BACKUP ALL IMAGES】确认
            {
                if (null != WindowClose_Rejects_BackupAllImages_Confirm)//有效
                {
                    WindowClose_Rejects_BackupAllImages_Confirm(this, new CustomEventArgs());
                }
            }
            else if (81 == iWindowParameter)//REJECTS，【BACKUP ALL IMAGES】等待
            {
                if (null != WindowClose_Rejects_BackupAllImages_Wait)//有效
                {
                    WindowClose_Rejects_BackupAllImages_Wait(this, new CustomEventArgs());
                }
            }
            else if (44 == iWindowParameter)//REJECTS，【CLEAR ALL】确认
            {
                if (null != WindowClose_Rejects_ClearAll_Confirm)//有效
                {
                    WindowClose_Rejects_ClearAll_Confirm(this, new CustomEventArgs());
                }
            }
            else if (82 == iWindowParameter)//REJECTS，【CLEAR ALL】等待
            {
                if (null != WindowClose_Rejects_ClearAll_Wait)//有效
                {
                    WindowClose_Rejects_ClearAll_Wait(this, new CustomEventArgs());
                }
            }
            else if (45 == iWindowParameter)//RESTORE BRANDS，【DELETE】确认
            {
                if (null != WindowClose_RestoreBrands_Delete_Confirm)//有效
                {
                    WindowClose_RestoreBrands_Delete_Confirm(this, new CustomEventArgs());
                }
            }
            else if (46 == iWindowParameter)//RESTORE BRANDS，【DELETE】等待
            {
                if (null != WindowClose_RestoreBrands_Delete_Wait)//有效
                {
                    WindowClose_RestoreBrands_Delete_Wait(this, new CustomEventArgs());
                }
            }
            else if (48 == iWindowParameter)//RESTORE BRANDS，【RESTORE】确认
            {
                if (null != WindowClose_RestoreBrands_Restore_Confirm)//有效
                {
                    WindowClose_RestoreBrands_Restore_Confirm(this, new CustomEventArgs());
                }
            }
            else if (80 == iWindowParameter)//RESTORE BRANDS，【RESTORE】等待
            {
                if (null != WindowClose_RestoreBrands_Restore_Wait)//有效
                {
                    WindowClose_RestoreBrands_Restore_Wait(this, new CustomEventArgs());
                }
            }
            else if (51 == iWindowParameter)//RESTORE BRANDS，RESTORE（LOCAL DISK），覆盖已存在文件确认
            {
                if (null != WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk)//有效
                {
                    WindowClose_RestoreBrands_Restore_Overwrite_Confirm_LocalDisk(this, new CustomEventArgs());
                }
            }
            else if (52 == iWindowParameter)//RESTORE BRANDS，RESTORE（USB DEVICE），覆盖已存在文件确认
            {
                if (null != WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice)//有效
                {
                    WindowClose_RestoreBrands_Restore_Overwrite_Confirm_USBDevice(this, new CustomEventArgs());
                }
            }
            else if (53 == iWindowParameter)//SYSTEM，【OK】等待
            {
                if (null != WindowClose_System_Ok_Wait)//有效
                {
                    WindowClose_System_Ok_Wait(this, new CustomEventArgs());
                }
            }
            else if (73 == iWindowParameter)//SYSTEM，【OK】（保存参数）确认
            {
                if (null != WindowClose_System_Ok_Confirm)//有效
                {
                    WindowClose_System_Ok_Confirm(this, new CustomEventArgs());
                }
            }
            else if (54 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件1）确认
            {
                if (null != WindowClose_Tolerances_Learn_Graph1_Confirm)//有效
                {
                    WindowClose_Tolerances_Learn_Graph1_Confirm(this, new CustomEventArgs());
                }
            }
            else if (55 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件1）错误
            {
                if (null != WindowClose_Tolerances_Learn_Graph1_Failure)//有效
                {
                    WindowClose_Tolerances_Learn_Graph1_Failure(this, new CustomEventArgs());
                }
            }
            else if (56 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件2）确认
            {
                if (null != WindowClose_Tolerances_Learn_Graph2_Confirm)//有效
                {
                    WindowClose_Tolerances_Learn_Graph2_Confirm(this, new CustomEventArgs());
                }
            }
            else if (57 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件2）错误
            {
                if (null != WindowClose_Tolerances_Learn_Graph2_Failure)//有效
                {
                    WindowClose_Tolerances_Learn_Graph2_Failure(this, new CustomEventArgs());
                }
            }
            else if (58 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件3）确认
            {
                if (null != WindowClose_Tolerances_Learn_Graph3_Confirm)//有效
                {
                    WindowClose_Tolerances_Learn_Graph3_Confirm(this, new CustomEventArgs());
                }
            }
            else if (59 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件3）错误
            {
                if (null != WindowClose_Tolerances_Learn_Graph3_Failure)//有效
                {
                    WindowClose_Tolerances_Learn_Graph3_Failure(this, new CustomEventArgs());
                }
            }
            else if (60 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件4）确认
            {
                if (null != WindowClose_Tolerances_Learn_Graph4_Confirm)//有效
                {
                    WindowClose_Tolerances_Learn_Graph4_Confirm(this, new CustomEventArgs());
                }
            }
            else if (61 == iWindowParameter)//TOLERANCES，【LEARN】（曲线图控件4）错误
            {
                if (null != WindowClose_Tolerances_Learn_Graph4_Failure)//有效
                {
                    WindowClose_Tolerances_Learn_Graph4_Failure(this, new CustomEventArgs());
                }
            }
            else if (62 == iWindowParameter)//TOLERANCES，【LEARN】成功
            {
                if (null != WindowClose_Tolerances_Learn_Success)//有效
                {
                    WindowClose_Tolerances_Learn_Success(this, new CustomEventArgs());
                }
            }
            else if (63 == iWindowParameter)//TOLERANCES，【LEARN】失败
            {
                if (null != WindowClose_Tolerances_Learn_Failure)//有效
                {
                    WindowClose_Tolerances_Learn_Failure(this, new CustomEventArgs());
                }
            }
            else if (64 == iWindowParameter)//TOLERANCES，【SAVE PRODUCT】确认
            {
                if (null != WindowClose_Tolerances_SaveProduct_Confirm)//有效
                {
                    WindowClose_Tolerances_SaveProduct_Confirm(this, new CustomEventArgs());
                }
            }
            else if (65 == iWindowParameter)//TOLERANCES，【SAVE PRODUCT】成功
            {
                if (null != WindowClose_Tolerances_SaveProduct_Success)//有效
                {
                    WindowClose_Tolerances_SaveProduct_Success(this, new CustomEventArgs());
                }
            }
            else if (66 == iWindowParameter)//TOLERANCES，【SAVE PRODUCT】失败
            {
                if (null != WindowClose_Tolerances_SaveProduct_Failure)//有效
                {
                    WindowClose_Tolerances_SaveProduct_Failure(this, new CustomEventArgs());
                }
            }
            else if (67 == iWindowParameter)//TOLERANCES，【RESET GRAPHS】确认
            {
                if (null != WindowClose_Tolerances_ResetGraphs_Confirm)//有效
                {
                    WindowClose_Tolerances_ResetGraphs_Confirm(this, new CustomEventArgs());
                }
            }
            else if (68 == iWindowParameter)//TOLERANCES，【RESET GRAPHS】成功
            {
                if (null != WindowClose_Tolerances_ResetGraphs_Success)//有效
                {
                    WindowClose_Tolerances_ResetGraphs_Success(this, new CustomEventArgs());
                }
            }
            else if (69 == iWindowParameter)//TOLERANCES，【RESET GRAPHS】失败
            {
                if (null != WindowClose_Tolerances_ResetGraphs_Failure)//有效
                {
                    WindowClose_Tolerances_ResetGraphs_Failure(this, new CustomEventArgs());
                }
            }
            else if (70 == iWindowParameter)//WORK，【系统更新】确认
            {
                if (null != WindowClose_Work_Update_Confirm)//有效
                {
                    WindowClose_Work_Update_Confirm(this, new CustomEventArgs());
                }
            }
            else if (92 == iWindowParameter)//WORK，【系统更新】等待
            {
                if (null != WindowClose_Work_Update_Wait)//有效
                {
                    WindowClose_Work_Update_Wait(this, new CustomEventArgs());
                }
            }
            else if (71 == iWindowParameter)//WORK，点击左侧TRADEMARK图标
            {
                if (null != WindowClose_Work_LeftTrademark)//有效
                {
                    WindowClose_Work_LeftTrademark(this, new CustomEventArgs());
                }
            }
            else if (72 == iWindowParameter)//WORK，点击右侧TRADEMARK图标
            {
                if (null != WindowClose_Work_RightTrademark)//有效
                {
                    WindowClose_Work_RightTrademark(this, new CustomEventArgs());
                }
            }
            else if (76 == iWindowParameter)//WORK，退出程序确认
            {
                if (null != WindowClose_Work_ExitApplication_Confirm)//有效
                {
                    WindowClose_Work_ExitApplication_Confirm(this, new CustomEventArgs());
                }
            }
            else if (75 == iWindowParameter)//DEVICE CONFIGURATION，【OK】确认
            {
                if (null != WindowClose_DeviceConfiguration_Ok_Confirm)//有效
                {
                    WindowClose_DeviceConfiguration_Ok_Confirm(this, new CustomEventArgs());
                }
            }
            else if (83 == iWindowParameter)//STATISTICS RECORD，【DELETE】（【DELETE ALL】）确认
            {
                if (null != WindowClose_StatisticsRecord_Delete_Confirm)//有效
                {
                    WindowClose_StatisticsRecord_Delete_Confirm(this, new CustomEventArgs());
                }
            }
            else if (84 == iWindowParameter)//STATISTICS RECORD，【DELETE】（【DELETE ALL】）等待
            {
                if (null != WindowClose_StatisticsRecord_Delete_Wait)//有效
                {
                    WindowClose_StatisticsRecord_Delete_Wait(this, new CustomEventArgs());
                }
            }
            else if (85 == iWindowParameter)//STATISTICS RECORD，【OK】确认
            {
                if (null != WindowClose_StatisticsRecord_Ok_Confirm)//有效
                {
                    WindowClose_StatisticsRecord_Ok_Confirm(this, new CustomEventArgs());
                }
            }
            else if (88 == iWindowParameter)//STATISTICS RECORD，更新统计数据
            {
                if (null != WindowClose_StatisticsRecord_GetRecord_Wait)//有效
                {
                    WindowClose_StatisticsRecord_GetRecord_Wait(this, new CustomEventArgs());
                }
            }

            else if (86 == iWindowParameter)//STATISTICS，获取统计数据
            {
                if (null != WindowClose_Statistics_GetRecordData_Wait)//有效
                {
                    WindowClose_Statistics_GetRecordData_Wait(this, new CustomEventArgs());
                }
            }
            else if (89 == iWindowParameter)//FAULT MESSAGE，获取故障信息数据
            {
                if (null != WindowClose_FaultMessage_GetData_Wait)//有效
                {
                    WindowClose_FaultMessage_GetData_Wait(this, new CustomEventArgs());
                }
            }
            else if (90 == iWindowParameter)//FAULT MESSAGE，清除所有故障信息数据确认
            {
                if (null != WindowClose_FaultMessage_ClearAll_Confirm)//有效
                {
                    WindowClose_FaultMessage_ClearAll_Confirm(this, new CustomEventArgs());
                }
            }
            else if (91 == iWindowParameter)//FAULT MESSAGE，清除所有故障信息数据
            {
                if (null != WindowClose_FaultMessage_ClearAll_Wait)//有效
                {
                    WindowClose_FaultMessage_ClearAll_Wait(this, new CustomEventArgs());
                }
            }
            else if (93 == iWindowParameter)//EDIT TOOLS，【NEW TOOL】成功
            {
                if (null != WindowClose_EditTools_NewTool_Success)//有效
                {
                    WindowClose_EditTools_NewTool_Success(this, new CustomEventArgs());
                }
            }
            else if (94 == iWindowParameter)//EDIT TOOLS，【NEW TOOL】失败（重名）
            {
                if (null != WindowClose_EditTools_NewTool_Failure_1)//有效
                {
                    WindowClose_EditTools_NewTool_Failure_1(this, new CustomEventArgs());
                }
            }
            else if (95 == iWindowParameter)//EDIT TOOLS，【NEW TOOL】失败（数量达到最大值）
            {
                if (null != WindowClose_EditTools_NewTool_Failure_2)//有效
                {
                    WindowClose_EditTools_NewTool_Failure_2(this, new CustomEventArgs());
                }
            }
            else if (96 == iWindowParameter)//EDIT TOOLS，【COPY TOOL】成功
            {
                if (null != WindowClose_EditTools_CopyTool_Success)//有效
                {
                    WindowClose_EditTools_CopyTool_Success(this, new CustomEventArgs());
                }
            }
            else if (97 == iWindowParameter)//EDIT TOOLS，【COPY TOOL】失败（重名）
            {
                if (null != WindowClose_EditTools_CopyTool_Failure_1)//有效
                {
                    WindowClose_EditTools_CopyTool_Failure_1(this, new CustomEventArgs());
                }
            }
            else if (98 == iWindowParameter)//EDIT TOOLS，【COPY TOOL】失败（数量达到最大值）
            {
                if (null != WindowClose_EditTools_CopyTool_Failure_2)//有效
                {
                    WindowClose_EditTools_CopyTool_Failure_2(this, new CustomEventArgs());
                }
            }
            else if (99 == iWindowParameter)//EDIT TOOLS，【RENAME TOOL】成功
            {
                if (null != WindowClose_EditTools_RenameTool_Success)//有效
                {
                    WindowClose_EditTools_RenameTool_Success(this, new CustomEventArgs());
                }
            }
            else if (100 == iWindowParameter)//EDIT TOOLS，【RENAME TOOL】失败（重名）
            {
                if (null != WindowClose_EditTools_RenameTool_Failure_1)//有效
                {
                    WindowClose_EditTools_RenameTool_Failure_1(this, new CustomEventArgs());
                }
            }
            else if (101 == iWindowParameter)//EDIT TOOLS，【DELETE TOOL】确认
            {
                if (null != WindowClose_EditTools_DeleteTool_Confirm)//有效
                {
                    WindowClose_EditTools_DeleteTool_Confirm(this, new CustomEventArgs());
                }
            }
            else if (102 == iWindowParameter)//EDIT TOOLS，【DELETE TOOL】成功
            {
                if (null != WindowClose_EditTools_DeleteTool_Success)//有效
                {
                    WindowClose_EditTools_DeleteTool_Success(this, new CustomEventArgs());
                }
            }
            else if (103 == iWindowParameter)//EDIT TOOLS，【DELETE TOOL】失败
            {
                if (null != WindowClose_EditTools_DeleteTool_Failure)//有效
                {
                    WindowClose_EditTools_DeleteTool_Failure(this, new CustomEventArgs());
                }
            }
            else if (104 == iWindowParameter)//PARAMETER SETTINGS，【保存参数】确认
            {
                if (null != WindowClose_ParameterSettings_Save_Confirm)//有效
                {
                    WindowClose_ParameterSettings_Save_Confirm(this, new CustomEventArgs());
                }
            }
            else if (105 == iWindowParameter)//PARAMETER SETTINGS，【保存参数】等待
            {
                if (null != WindowClose_ParameterSettings_Save_Wait)//有效
                {
                    WindowClose_ParameterSettings_Save_Wait(this, new CustomEventArgs());
                }
            }
            else if (106 == iWindowParameter)//Net Check，【保存参数】等待
            {
                if (null != WindowClose_NetCheck)//有效
                {
                    WindowClose_NetCheck(this, new CustomEventArgs());
                }
            }
            else//其它
            {
                //不执行操作
            }
        }
    }
}