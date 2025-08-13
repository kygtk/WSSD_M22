using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Dms.Device;

namespace Dms.ServerCommon
{
    #region XmlInclude
    [XmlInclude(typeof(EquipmentType))]
    //[XmlInclude(typeof(CylinerActTime))] // 11.02.01 minhan
    //[XmlInclude(typeof(IfSignalFromCim))] // 10.12.25 minhan
    //[XmlInclude(typeof(IfSignalToCim))]
    //[XmlInclude(typeof(FlowAccumulate))] // 11.02.01 minhan
    [XmlInclude(typeof(DMSAdmin))] // 10.12.25 minhan
    [XmlInclude(typeof(_GenericCollection<EquipmentType>))]
    //[XmlInclude(typeof(_GenericCollection<CylinerActTime>))] // 11.02.01 minhan
    //[XmlInclude(typeof(_GenericCollection<IfSignalFromCim>))]
    //[XmlInclude(typeof(_GenericCollection<IfSignalToCim>))]
    //[XmlInclude(typeof(_GenericCollection<FlowAccumulate>))] // 11.02.01 minhan
    [XmlInclude(typeof(_GenericCollection<DMSAdmin>))] // 10.12.25 minhan

    #endregion
    public class ComponentContainer : _ComponentContainer
    {
        protected override void MakeContainer()
        {
            Add(new EqpUnits());
            Add(new _GenericCollection<EquipmentType>());
            Add(new TransferUnits());
            Add(new _GenericCollection<AlarmResetSwitch>());        //Add(new _GenericCollection<AlarmResetSwitch_Io>());       //Add(new _GenericCollection<AlarmResetSwitch_Ec>());
            Add(new _GenericCollection<Buzzer>());                  //Add(new _GenericCollection<Buzzer_Io>());                 //Add(new _GenericCollection<Buzzer_Ec>());
            Add(new _GenericCollection<SignalTower>());             //Add(new _GenericCollection<SignalTower_Io>());            //Add(new _GenericCollection<SignalTower_Ec>());
            Add(new _GenericCollection<EmoSensor>());               //Add(new _GenericCollection<EmoSensor_Io>());              //Add(new _GenericCollection<EmoSensor_Ec>());
            Add(new _GenericCollection<LeakSensor>());              //Add(new _GenericCollection<LeakSensor_Io>());             //Add(new _GenericCollection<LeakSensor_Ec>());
            Add(new _GenericCollection<CoverSensor>());             //Add(new _GenericCollection<CoverSensor_Io>());            //Add(new _GenericCollection<CoverSensor_Ec>());
            Add(new _GenericCollection<DoorSensor>());              //Add(new _GenericCollection<DoorSensor_Io>());             //Add(new _GenericCollection<DoorSensor_Ec>());             //Add(new _GenericCollection<DoorSensor_Ap>());
            Add(new _GenericCollection<DoorLockSensor>());          //Add(new _GenericCollection<DoorLockSensor_Io>());                                                                     //Add(new _GenericCollection<DoorLockSensor_Ap>());
            Add(new _GenericCollection<Sensor>());                  //Add(new _GenericCollection<Sensor_Io>());                 //Add(new _GenericCollection<Sensor_Ec>());
            Add(new _GenericCollection<BrokenDetectFixedType>());   //Add(new _GenericCollection<BrokenDetectFixedType_Io>());  //Add(new _GenericCollection<BrokenDetectFixedType_Ec>());
            Add(new _GenericCollection<BrokenDetectScanType>());    //Add(new _GenericCollection<BrokenDetectScanType_Io>());   //Add(new _GenericCollection<BrokenDetectScanType_Ec>()); 
            Add(new _GenericCollection<HepaFilter>());              //Add(new _GenericCollection<HepaFilter_Io>());             //Add(new _GenericCollection<HepaFilter_Ec>());
            Add(new _GenericCollection<Ionizer>());                 //Add(new _GenericCollection<Ionizer_Io>());                //Add(new _GenericCollection<Ionizer_Ec>());
            Add(new _GenericCollection<ESD>());                                                                                 //Add(new _GenericCollection<ESD_Ec>());
            Add(new _GenericCollection<Gauge>());                   //Add(new _GenericCollection<Gauge_Io>());                  //Add(new _GenericCollection<Gauge_Ec>());                  //Add(new _GenericCollection<Gauge_Ap>());
            Add(new _GenericCollection<AutoValve>());               //Add(new _GenericCollection<AutoValve_Io>());              //Add(new _GenericCollection<AutoValve_Ec>());
            Add(new _GenericCollection<Fan>());                     //Add(new _GenericCollection<Fan_Io>());                    //Add(new _GenericCollection<Fan_Ec>());
            Add(new _GenericCollection<Cylinder>());                //Add(new _GenericCollection<Cylinder_Io>());               //Add(new _GenericCollection<Cylinder_Ec>());
            Add(new _GenericCollection<GlsSensor>());               //Add(new _GenericCollection<GlsSensor_Io>());              //Add(new _GenericCollection<GlsSensor_Ec>());
            Add(new _GenericCollection<LevelSensor>());             //Add(new _GenericCollection<LevelSensor_Io>());            //Add(new _GenericCollection<LevelSensor_Ec>());
            Add(new _GenericCollection<BLDCMotor>());               //Add(new _GenericCollection<FBLMotor>());                  //Add(new _GenericCollection<BLDCMotor_Ec>());
            Add(new _GenericCollection<ActuatorUnit>());
            Add(new _GenericCollection<RbMotor>());                 //Add(new _GenericCollection<RbMotor_Io>());                //Add(new _GenericCollection<RbMotor_Ec>());
            Add(new _GenericCollection<RbUnit>());
            //Add(new _GenericCollection<Mfc>());
            //Add(new _GenericCollection<SeAp>());
            Add(new _GenericCollection<ProcessUnit>());
            Add(new _GenericCollection<PartsItem>());
            Add(new _GenericCollection<HpmjInterface>());//2010.11.4 kang
            Add(new _GenericCollection<BOELoaderInterface>());//2010.11.4 kang
            Add(new _GenericCollection<EGiSInterface>());//Mr.kang
            Add(new _GenericCollection<InterfaceStep>()); // 10.12.21 minhan
            Add(new _GenericCollection<Inverter>());
            Add(new _GenericCollection<Hpmj>());
            Add(new _GenericCollection<HpmjItem>());
            //Add(new _GenericCollection<ServoMotorMp2300>());
            //Add(new ServoUnitMp2300s());
            //Add(new _GenericCollection<LGDLoaderInterlock>()); // 10.12.25 minhan
            //Add(new _GenericCollection<SRU>());//2009.09.15 kimgun
            Add(new _GenericCollection<TankLevel>());
            Add(new _GenericCollection<TankUnit>());
            Add(new _GenericCollection<Pump>());                    //Add(new _GenericCollection<Pump_Io>());                   //Add(new _GenericCollection<Pump_Ec>());
            Add(new _GenericCollection<PumpUnit>());
            //Add(new _GenericCollection<IfSignalFromCim>());
            //Add(new _GenericCollection<IfSignalToCim>());
            //Add(new _GenericCollection<HardInterlockItem>());//2009.09.21 kimgun
            //Add(new _GenericCollection<InterfaceStep>());
            //Add(new _GenericCollection<ServoCheckPoint>());
            //Add(new _GenericCollection<PowerJoint>());
            //Add(new _GenericCollection<FanFilter>());
            Add(new _GenericCollection<FanFilterControl>()); // 11.03.07 minhan
            Add(new _GenericCollection<ServoMotor>());              //Add(new _GenericCollection<ServoMotor_Mc>());             //Add(new _GenericCollection<ServoMotor_Ec>());
            Add(new ServoUnits());
            Add(new _GenericCollection<ServoCvMotor>());
            //Add(new _GenericCollection<MitutoyoRbGapGauge>());
            //Add(new _GenericCollection<ColorBoard>());
            //Add(new _GenericCollection<PSMAp>());
            Add(new _GenericCollection<EuvLamp>());                 //Add(new _GenericCollection<EuvLamp_Io>());                //Add(new _GenericCollection<EuvLamp_Ec>());
            Add(new _GenericCollection<EuvUnitComm>());
            Add(new _GenericCollection<UshioEuvUnit>());            //Add(new _GenericCollection<UshioEuvUnit_Io>());           //Add(new _GenericCollection<UshioEuvUnit_Ec>());
            //Add(new _GenericCollection<MotorLoadFactor>()); // 10.12.25 minhan
            //Add(new _GenericCollection<ForcedExhaust>());
            //Add(new _GenericCollection<CylinerActTime>()); // 11.02.01 minhan
            //Add(new _GenericCollection<FlowAccumulate>()); // 11.02.01 minhan

            Add(new _GenericCollection<ETCOutput>());               //Add(new _GenericCollection<ETCOutput_Io>());              //Add(new _GenericCollection<ETCOutput_Ec>());

            Add(new _GenericCollection<FlowmeterDIW>());
            Add(new _GenericCollection<FlowmeterCDA>());
            Add(new _GenericCollection<SmartDamper>());
            Add(new _GenericCollection<D40A>());
            Add(new _GenericCollection<D4SL>());
            Add(new _GenericCollection<LFC>());
            Add(new _GenericCollection<Manometer>());
            Add(new _GenericCollection<LCT>());


            Add(new _GenericCollection<DMSAdmin>()); // 10.12.25 minhan
            Add(new _GenericCollection<ApdItem>());//2010.06.28 kimgun apditem이 맨 마지막에 나와야 devicetag로 연결된 item들이 모두 제대로 연결된다.
        }
    }

    public class DmsComponents : _DmsComponents
    {
        #region Singleton code...
        public static readonly DmsComponents Instance = new DmsComponents();
        #endregion

        #region Constructor
        private DmsComponents()
        {
            m_Container = new ComponentContainer();
        }
        #endregion
    }
}
