///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2008.09.19
// Author       : Hoon
// Description  : Collection of CtlServoUnit
//-------------------------------------------------------------------------
// Revison History
// * 2008.09.19 - Level Sensor 2ea, LoadCell 사용
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using Dms.Data;
using System.ComponentModel;
using System.Drawing.Design;
using System.Xml.Serialization;

namespace Dms.Device
{
    public enum LevelLoadCell
    {
        LL, L, M, H, HH
    }

    [Editor(typeof(UIEditorObjectSelect), typeof(UITypeEditor))]
    public class TankLevelLoadCell : TankLevel
    {
        #region Fields
        public Gauge m_LevelGauge;
        #endregion

        #region Properties
        [Category("DMS : Setting")]
        public Gauge Gauge
        {
            get { return m_LevelGauge; }
            set { m_LevelGauge = value; }
        }
        public override int Count
        {
            get { return 5; }
        }
        #endregion

        #region Constructor
        public TankLevelLoadCell()
        {
            this.Name = "__ Tank Level";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Server 내부로 전달
        /// </summary>
        #endregion

        #region Override
        public override DmsErrors Initialize()
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
            //ok &= (m_sensors.Count >= 4);


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
                //SetupGenInfoProvider.Instance.InitFromDB(m_SetupCvTimeoutMargin);
                #endregion
                SetupDevTankLevelProvider setupDevTankLevelProvider = SetupDevTankLevelProvider.Instance;
                m_SetupTankLevel = new TagSetupInfo(this.Name + " LL", OptionType.None, OptionFormat.Digit, UnitType.L, "30");
                setupDevTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevels.Add(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " L", OptionType.None, OptionFormat.Digit, UnitType.L, "50");
                setupDevTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevels.Add(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " M", OptionType.None, OptionFormat.Digit, UnitType.L, "100");
                setupDevTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevels.Add(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " H", OptionType.None, OptionFormat.Digit, UnitType.L, "130");
                setupDevTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevels.Add(m_SetupTankLevel);
                m_SetupTankLevel = new TagSetupInfo(this.Name + " HH", OptionType.None, OptionFormat.Digit, UnitType.L, "150");
                setupDevTankLevelProvider.InitFromDB(m_SetupTankLevel);
                m_SetupTankLevels.Add(m_SetupTankLevel);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 7. 기타 초기화 조건

                IsSupplyStop = new IsConfirmed(IsLoadCellSupplyStop);
                IsSupplyRequest = new IsConfirmed(IsLoadCellSupplyRequest);
                IsRunEnable = new IsConfirmed(IsLoadCellRunEnable);
                IsBottomLevel = new IsConfirmed(m_Sensors[0].IsConfirmed);
                IsTopLevel = new IsConfirmed(m_Sensors[1].IsConfirmed);

                IsSupplyStopDetect = new IsDetected(IsLoadCellSupplyStop);
                IsSupplyRequestDetect = new IsDetected(IsLoadCellSupplyRequest);
                IsRunEnableDetect = new IsDetected(IsLoadCellRunEnable);
                IsBottomLevelDetect = new IsDetected(m_Sensors[0].IsDetected);
                IsTopLevelDetect = new IsDetected(m_Sensors[1].IsDetected);

                ////////////////////////////////////////////////////////////////////////////////////////
                // 8. Tag Update Timer 등록
                SetSubscriber();

                ////////////////////////////////////////////////////////////////////////////////////////
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
        #endregion

        public bool IsLoadCellSupplyStop()
        {
            if (AppConfig.Instance.Simul.Device)
            {
                if (m_LevelGauge.SimulCurValue >= SetupTankLevels[(int)LevelLoadCell.H].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (m_LevelGauge.CurValue >= SetupTankLevels[(int)LevelLoadCell.H].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
        }
        public bool IsLoadCellSupplyRequest()
        {
            if (AppConfig.Instance.Simul.Device)
            {
                if (m_LevelGauge.SimulCurValue >= SetupTankLevels[(int)LevelLoadCell.M].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (m_LevelGauge.CurValue >= SetupTankLevels[(int)LevelLoadCell.M].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
        }
        public bool IsLoadCellRunEnable()
        {
            if (AppConfig.Instance.Simul.Device)
            {
                if (m_LevelGauge.SimulCurValue >= SetupTankLevels[(int)LevelLoadCell.L].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
            else
            {
                if (m_LevelGauge.CurValue >= SetupTankLevels[(int)LevelLoadCell.L].GetValue<int>())
                {
                    return true;
                }
                else return false;
            }
        }
    }
}
