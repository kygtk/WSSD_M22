///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2010.07.21
// Author       : EUN
// Description  : PECVD Transfer Chamber(reference project-10EV01)
///////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;
using System.Windows.Forms;
using Dms.Common;
using Dms.Device;

namespace Dms.Device
{
    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class TMChamber_PECVD : TransferUnit
    {
        #region Fields
        private _ServoUnit m_HandTransferUnit;
        private BLDCMotor m_Motor;
        private CylinderUnit m_HandCylinderUnit;

        private TagTrayTransferIfFlag m_IfFlag;
        #endregion

        #region Properties
        [Category("Modules"),
        Description("Select the Servo Unit")]
        public _ServoUnit HandTransferUnit
        {
            get { return m_HandTransferUnit; }
            set { m_HandTransferUnit = value; }
        }
        [Category("Modules")]
        public BLDCMotor Motor
        {
            get { return m_Motor; }
            set { m_Motor = value; }
        }
        [Category("Modules"),
        Description("Select the Cylinder Type")]
        public CylinderUnit HandCylinderUnit
        {
            get { return m_HandCylinderUnit; }
            set { m_HandCylinderUnit = value; }
        }
        [Browsable(false), XmlIgnore()]
        public TagTrayTransferIfFlag IfFlag
        {
            get { return m_IfFlag; }
            set { m_IfFlag = value; }
        }
        #endregion

        #region Constructor
        public TMChamber_PECVD()
        {
            this.Name = "__ PECVD TM Chamber";
        }
        #endregion

        #region Override
        public override DmsErrors Initialize(IServerManager server, _GenInfoHandler geninfos)
        {
            ////////////////////////////////////////////////////////////////////////////////////////
            // 초기화 순서는 아래의 Flow를 따라야 한다.


            ////////////////////////////////////////////////////////////////////////////////////////
            // 1. 이미 초기화완료 되었는지 Check
            if (Initialized == true) return DmsErrors.Success;


            ////////////////////////////////////////////////////////////////////////////////////////
            // 2. DeviceI/O 등록
            bool ok = true;
            ok &= GenerateAssociatedDevices();


            ////////////////////////////////////////////////////////////////////////////////////////
            // 3. 필수 I/O 들이 등록되어 있는지 Check
            #region Example
            //ok &= (m_DiAlarm != null);
            #endregion


            ////////////////////////////////////////////////////////////////////////////////////////
            if (!ok)
            {
                SetLog(this.Name, "Initialize", 0, 0, "Initialize Failed");
                return DmsErrors.NotInitialized;
            }
            else
            {
                ////////////////////////////////////////////////////////////////////////////////////////
                // 4. Tag 생성
                CreateTag(m_Server.TagContainer);


                ////////////////////////////////////////////////////////////////////////////////////////
                // 5. Alarm Item 생성
                #region Example
                //ALM_ReadyFail = new Alarm(this.Name + " Ready Fail", AlarmLevel.S, AlarmCode.EquipmentSafety);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 6. SetupItem 생성
                #region Example
                //m_SetupCvTimeoutMargin = new TagSetupInfo("C/V Slip Timeout Margin", OptionType.None, OptionFormat.Digit, UnitType.mm, "15");
                //m_Server.SetupGenInfo.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion


                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건
                m_IfFlag = new TagTrayTransferIfFlag(this);
                m_IfFlag.Reset();



                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();


                // 9. I/O 초기값 설정, Simulation code
                #region Example
                //if (m_Simul.Device)
                //{
                //    m_DiReady.SetState(true);
                //}
                #endregion

                ////////////////////////////////////////////////////////////////////////////////////////
                // 10. Set Flag
                m_Initialized = ok;


                ////////////////////////////////////////////////////////////////////////////////////////
                // 11. 초기화완료 확인이후 수행 조건
                if (m_Initialized)
                {
                }


                ////////////////////////////////////////////////////////////////////////////////////////
                // 12. 초기화 결과 Return : 초기화 완료 일때만 DmsErrors.Success return
                return m_Initialized ? DmsErrors.Success : DmsErrors.InternalError;
            }
        }

        public override void CreateTag(DeviceTags tagContainer)
        {

        }

        public override void UpdateTag()
        {

        }
        #endregion
    }
}
