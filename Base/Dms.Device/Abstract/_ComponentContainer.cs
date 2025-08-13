///////////////////////////////////////////////////////////////////////////
// Copyright    : DMS Co., Ltd
// Issue Date   : 2009.01.30
// Author       : jemoon
// Description  : Abstract class for _IComponentContainer
//-------------------------------------------------------------------------
// Revison History
// * 2008.02.18 - jemoon : code review
///////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Dms.Common;
using System.Collections;
using System.IO;
using System.Xml.Serialization;
using System.ComponentModel;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace Dms.Device
{
    #region XmlInclude
    [XmlInclude(typeof(EqpUnit))]
    [XmlInclude(typeof(ServoMotor))]
    [XmlInclude(typeof(ServoMotor_Mc))]
    [XmlInclude(typeof(ServoMotor_Ec))]
    [XmlInclude(typeof(ServoUnit))]
    [XmlInclude(typeof(ServoUnits))]
    [XmlInclude(typeof(AbsodexMotor))]
    [XmlInclude(typeof(RbtHandInterlock))]
    [XmlInclude(typeof(AutoValve))]
    [XmlInclude(typeof(AutoValve_Io))]
    [XmlInclude(typeof(AutoValve_Ec))]
    [XmlInclude(typeof(AutoValve2))]
    [XmlInclude(typeof(ProcessUnit))]
    [XmlInclude(typeof(Buzzer))]
    [XmlInclude(typeof(Fan))]
    [XmlInclude(typeof(Fan_Io))]
    [XmlInclude(typeof(Fan_Ec))]
    [XmlInclude(typeof(Pump_Io))]
    [XmlInclude(typeof(Pump_Ec))]
    [XmlInclude(typeof(PumpUnit))]
    [XmlInclude(typeof(DryPump))]
    [XmlInclude(typeof(DryPumpUnit))]
    [XmlInclude(typeof(RotaryPump))]
    [XmlInclude(typeof(RotaryPumpUnit))]
    [XmlInclude(typeof(PumpUnitA))]
    [XmlInclude(typeof(AlarmItem))]
    [XmlInclude(typeof(ST540))]
    [XmlInclude(typeof(ST590))]
    [XmlInclude(typeof(Heater))]
    [XmlInclude(typeof(HeaterUnit))]
    [XmlInclude(typeof(HotWireUnit))]
    [XmlInclude(typeof(CirPumpUnit))]
    [XmlInclude(typeof(CirPumpAUnit))]
    [XmlInclude(typeof(DevPumpUnit))]
    [XmlInclude(typeof(TankUnit))]
    [XmlInclude(typeof(DevTankUnit))]
    [XmlInclude(typeof(Gauge))]
    [XmlInclude(typeof(Gauge_Io))]
    [XmlInclude(typeof(Gauge_Ec))]
    [XmlInclude(typeof(Gauge_Ap))]
    [XmlInclude(typeof(EmoSensor))]
    [XmlInclude(typeof(EmoSensor_Io))]
    [XmlInclude(typeof(EmoSensor_Ec))]
    [XmlInclude(typeof(LeakSensor))]
    [XmlInclude(typeof(LeakSensor_Io))]
    [XmlInclude(typeof(LeakSensor_Ec))]
    [XmlInclude(typeof(CoverSensor))]
    [XmlInclude(typeof(CoverSensor_Io))]
    [XmlInclude(typeof(CoverSensor_Ec))]
    [XmlInclude(typeof(DoorSensor))]
    [XmlInclude(typeof(DoorSensor_Io))]
    [XmlInclude(typeof(DoorSensor_Ec))]
    [XmlInclude(typeof(DoorSensor_Ap))]
    [XmlInclude(typeof(DoorLockSensor))]
    [XmlInclude(typeof(DoorLockSensor_Io))]
    [XmlInclude(typeof(DoorLockSensor_Ap))]
    [XmlInclude(typeof(AreaSensor))]
    [XmlInclude(typeof(AreaSensor_Io))]
    [XmlInclude(typeof(AreaSensor_Ec))]
    [XmlInclude(typeof(Sensor))]
    [XmlInclude(typeof(Sensor_Io))]
    [XmlInclude(typeof(Sensor_Ec))]
    [XmlInclude(typeof(GlsSensor))]
    [XmlInclude(typeof(GlsSensor_Io))]
    [XmlInclude(typeof(GlsSensor_Ec))]
    [XmlInclude(typeof(DualGlsSensor))]
    [XmlInclude(typeof(DualGlsSensor_Io))]
    [XmlInclude(typeof(DualGlsSensor_Ec))]
    [XmlInclude(typeof(LevelSensor))]
    [XmlInclude(typeof(LevelSensor_Io))]
    [XmlInclude(typeof(LevelSensor_Ec))]
    [XmlInclude(typeof(TankLevel))]
    [XmlInclude(typeof(TankLevelLoadCell))]
    [XmlInclude(typeof(TankLoadCell))]
    [XmlInclude(typeof(DevTankLevel))]
    [XmlInclude(typeof(BrokenDetectScanType))]
    [XmlInclude(typeof(BrokenDetectScanType_Io))]
    [XmlInclude(typeof(BrokenDetectScanType_Ec))]
    [XmlInclude(typeof(BrokenDetectFixedType))]
    [XmlInclude(typeof(BrokenDetectFixedType_Io))]
    [XmlInclude(typeof(BrokenDetectFixedType_Ec))]
    [XmlInclude(typeof(Cylinder))]
    [XmlInclude(typeof(Cylinder_Io))]
    [XmlInclude(typeof(Cylinder_Ec))]
    [XmlInclude(typeof(CylinderUnit))]
    [XmlInclude(typeof(ActuatorUnit))]
    [XmlInclude(typeof(BLDCMotor))]
    [XmlInclude(typeof(FBLMotor))]
    [XmlInclude(typeof(XqdMotor))]
    [XmlInclude(typeof(BLDCMotor_Ec))]
    [XmlInclude(typeof(ServoCvMotor))]
    [XmlInclude(typeof(ServoTilt))]
    [XmlInclude(typeof(CvUnit))]
    [XmlInclude(typeof(CvUnitA))]
    [XmlInclude(typeof(CvUnits))]
    [XmlInclude(typeof(RbMotor))]
    [XmlInclude(typeof(RbMotor_Io))]
    [XmlInclude(typeof(RbMotor_Xqd))]
    [XmlInclude(typeof(RbMotor_Ec))]
    [XmlInclude(typeof(RbUnit))]
    [XmlInclude(typeof(DBMotor))]
    [XmlInclude(typeof(DBUnit))]
    [XmlInclude(typeof(MitutoyoRbGapGauge))]
    [XmlInclude(typeof(HepaFilter))]
    [XmlInclude(typeof(HepaFilter_Io))]
    [XmlInclude(typeof(HepaFilter_Ec))]
    [XmlInclude(typeof(FanFilter))]
    [XmlInclude(typeof(FanFilterControl))] // 11.03.07 minhan
    [XmlInclude(typeof(Ionizer))]
    [XmlInclude(typeof(Ionizer_Io))]
    [XmlInclude(typeof(Ionizer_Ec))]
    [XmlInclude(typeof(AlarmResetSwitch))]
    [XmlInclude(typeof(AlarmResetSwitch_Io))]
    [XmlInclude(typeof(AlarmResetSwitch_Ec))]
    [XmlInclude(typeof(Buzzer))]
    [XmlInclude(typeof(Buzzer_Io))]
    [XmlInclude(typeof(Buzzer_Ec))]
    [XmlInclude(typeof(SignalTower))]
    [XmlInclude(typeof(SignalTower_Io))]
    [XmlInclude(typeof(SignalTower_Ec))]
    [XmlInclude(typeof(ESD))]
    [XmlInclude(typeof(ESD_Ec))]
    [XmlInclude(typeof(ColorBoard))]
    [XmlInclude(typeof(Mfc))]
    [XmlInclude(typeof(PSMAp))]
    [XmlInclude(typeof(EuvLamp))]
    [XmlInclude(typeof(EuvLamp_Io))]
    [XmlInclude(typeof(EuvLamp_Ec))]
    [XmlInclude(typeof(EuvUnitComm))]
    [XmlInclude(typeof(UshioEuvUnit))]
    [XmlInclude(typeof(UshioEuvUnit_Io))]
    [XmlInclude(typeof(UshioEuvUnit_Ec))]
    [XmlInclude(typeof(EyeEuvUnit))]
    [XmlInclude(typeof(ApdItem))]
    [XmlInclude(typeof(PartsItem))]
    [XmlInclude(typeof(HpmjItem))]
    [XmlInclude(typeof(ConiferIDReader))]
    [XmlInclude(typeof(ShinkoDryCleaner))]
    [XmlInclude(typeof(InterfaceStep))]
    [XmlInclude(typeof(TransferRobot))]
    [XmlInclude(typeof(GantryUnit))]
    [XmlInclude(typeof(FishHand))]
    [XmlInclude(typeof(ServoCheckPoint))]
    [XmlInclude(typeof(PowerJoint))]
    [XmlInclude(typeof(PowerJointAll))]
    [XmlInclude(typeof(Inverter))]
    [XmlInclude(typeof(Inverter_Io))]
    [XmlInclude(typeof(V1000))]
    [XmlInclude(typeof(Inverter_Ec))]
    [XmlInclude(typeof(Hpmj))]
    [XmlInclude(typeof(Hpmj_Io))]
    [XmlInclude(typeof(Hpmj_Ec))]
    [XmlInclude(typeof(SeAp))]
    [XmlInclude(typeof(TransferUnit))]
    [XmlInclude(typeof(TransferUnits))]
    [XmlInclude(typeof(MixTankItem))]
    [XmlInclude(typeof(MixTankUnit))]
    [XmlInclude(typeof(MixSupplyPumpUnit))]
    [XmlInclude(typeof(DetSupplyPumpUnit))]
    [XmlInclude(typeof(MeasureTankUnit))]
    [XmlInclude(typeof(DetTankUnit))]
    [XmlInclude(typeof(DetergentUnit))]
    [XmlInclude(typeof(Rfg))]
    [XmlInclude(typeof(Seren_Rfg))]
    [XmlInclude(typeof(RfTuner))]
    [XmlInclude(typeof(RfUnit))]
    [XmlInclude(typeof(Apc))]
    [XmlInclude(typeof(ShutterUnit))]
    [XmlInclude(typeof(PMChamber_RIE))]
    [XmlInclude(typeof(PMChamber_HWCVD))]
    [XmlInclude(typeof(BufferUnit))]
    [XmlInclude(typeof(LockChamber))]
    [XmlInclude(typeof(ServoMotorMp2300))]
    [XmlInclude(typeof(ServoUnitMp2300))]
    [XmlInclude(typeof(ServoUnitMp2300s))]
    [XmlInclude(typeof(LGDLoaderInterlock))]
    [XmlInclude(typeof(IntegratingFlowmeter))]
    [XmlInclude(typeof(SRU))]
    [XmlInclude(typeof(HardInterlockItem))]
    [XmlInclude(typeof(HeatExchanger))]
    [XmlInclude(typeof(LampSwitch))]
    [XmlInclude(typeof(PortUnit))]
    [XmlInclude(typeof(PortUnitA))]
    [XmlInclude(typeof(PortUnit_BoeHF_Theragen))]
    [XmlInclude(typeof(PortUnit_LGE_Solar))]
    [XmlInclude(typeof(YaskawaNX100))]
    [XmlInclude(typeof(Yaskawa_BoeHF_Theragen))]
    [XmlInclude(typeof(CstMappingUnitTakex))]
    [XmlInclude(typeof(CstMappingUnit_BoeHF_Theragen))]
    [XmlInclude(typeof(BCRn400k))]
    [XmlInclude(typeof(BCR_BoeHF))]
    [XmlInclude(typeof(LoaderUnit))]
    [XmlInclude(typeof(LoaderUnit_232C))]
    [XmlInclude(typeof(LoaderUnit_BoeHF_Theragen))]
    [XmlInclude(typeof(CimUnit))]
    [XmlInclude(typeof(StockerA))]
    [XmlInclude(typeof(HmiAutoValve))]
    [XmlInclude(typeof(GasControl))]
    [XmlInclude(typeof(MotorLoadFactor))]
    [XmlInclude(typeof(RPS))]
    [XmlInclude(typeof(PMChamber_PECVD))]
    [XmlInclude(typeof(TMChamber_PECVD))]
    [XmlInclude(typeof(BOELoaderInterface))]
    [XmlInclude(typeof(HpmjInterface))]
    [XmlInclude(typeof(ForcedExhaust))]
    [XmlInclude(typeof(FlowAccuItem))]
    [XmlInclude(typeof(FlowAccumulate))]
    [XmlInclude(typeof(EGiSInterface))]
    [XmlInclude(typeof(ETCOutput))]
    [XmlInclude(typeof(ETCOutput_Io))]
    [XmlInclude(typeof(ETCOutput_Ec))]
    [XmlInclude(typeof(FlowmeterDIW))]
    [XmlInclude(typeof(FlowmeterCDA))]
    [XmlInclude(typeof(SmartDamper))]
    [XmlInclude(typeof(D40A))]
    [XmlInclude(typeof(D4SL))]
    [XmlInclude(typeof(LFC))]
    [XmlInclude(typeof(Manometer))]
    [XmlInclude(typeof(LCT))]
    [XmlInclude(typeof(_GenericCollection<EqpUnit>))]
    [XmlInclude(typeof(_GenericCollection<ServoMotor>))]
    [XmlInclude(typeof(_GenericCollection<ServoMotor_Mc>))]
    [XmlInclude(typeof(_GenericCollection<ServoMotor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<ServoUnit>))]
    [XmlInclude(typeof(_GenericCollection<AbsodexMotor>))]
    [XmlInclude(typeof(_GenericCollection<AutoValve>))]
    [XmlInclude(typeof(_GenericCollection<AutoValve_Io>))]
    [XmlInclude(typeof(_GenericCollection<AutoValve_Ec>))]
    [XmlInclude(typeof(_GenericCollection<AutoValve2>))]
    [XmlInclude(typeof(_GenericCollection<Fan>))]
    [XmlInclude(typeof(_GenericCollection<Fan_Io>))]
    [XmlInclude(typeof(_GenericCollection<Fan_Ec>))]
    [XmlInclude(typeof(_GenericCollection<ProcessUnit>))]
    [XmlInclude(typeof(_GenericCollection<Pump>))]
    [XmlInclude(typeof(_GenericCollection<Pump_Io>))]
    [XmlInclude(typeof(_GenericCollection<Pump_Ec>))]
    [XmlInclude(typeof(_GenericCollection<PumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<DryPump>))]
    [XmlInclude(typeof(_GenericCollection<DryPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<RotaryPump>))]
    [XmlInclude(typeof(_GenericCollection<RotaryPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<PumpUnitA>))]
    [XmlInclude(typeof(_GenericCollection<AlarmItem>))]
    [XmlInclude(typeof(_GenericCollection<RbtHandInterlock>))]
    [XmlInclude(typeof(_GenericCollection<ST540>))]
    [XmlInclude(typeof(_GenericCollection<ST590>))]
    [XmlInclude(typeof(_GenericCollection<Heater>))]
    [XmlInclude(typeof(_GenericCollection<HeaterUnit>))]
    [XmlInclude(typeof(_GenericCollection<HotWireUnit>))]
    [XmlInclude(typeof(_GenericCollection<CirPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<CirPumpAUnit>))]
    [XmlInclude(typeof(_GenericCollection<DevPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<TankUnit>))]
    [XmlInclude(typeof(_GenericCollection<DevTankUnit>))]
    [XmlInclude(typeof(_GenericCollection<Gauge>))]
    [XmlInclude(typeof(_GenericCollection<Gauge_Io>))]
    [XmlInclude(typeof(_GenericCollection<Gauge_Ec>))]
    [XmlInclude(typeof(_GenericCollection<Gauge_Ap>))]
    [XmlInclude(typeof(_GenericCollection<EmoSensor>))]
    [XmlInclude(typeof(_GenericCollection<EmoSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<EmoSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<LeakSensor>))]
    [XmlInclude(typeof(_GenericCollection<LeakSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<LeakSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<CoverSensor>))]
    [XmlInclude(typeof(_GenericCollection<CoverSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<CoverSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<DoorSensor>))]
    [XmlInclude(typeof(_GenericCollection<DoorSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<DoorSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<DoorSensor_Ap>))]
    [XmlInclude(typeof(_GenericCollection<DoorLockSensor>))]
    [XmlInclude(typeof(_GenericCollection<DoorLockSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<DoorLockSensor_Ap>))]
    [XmlInclude(typeof(_GenericCollection<AreaSensor>))]
    [XmlInclude(typeof(_GenericCollection<AreaSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<AreaSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<Sensor>))]
    [XmlInclude(typeof(_GenericCollection<Sensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<Sensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<GlsSensor>))]
    [XmlInclude(typeof(_GenericCollection<GlsSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<GlsSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<DualGlsSensor>))]
    [XmlInclude(typeof(_GenericCollection<DualGlsSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<DualGlsSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<LevelSensor>))]
    [XmlInclude(typeof(_GenericCollection<LevelSensor_Io>))]
    [XmlInclude(typeof(_GenericCollection<LevelSensor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<TankLevel>))]
    [XmlInclude(typeof(_GenericCollection<TankLevelLoadCell>))]
    [XmlInclude(typeof(_GenericCollection<TankLoadCell>))]
    [XmlInclude(typeof(_GenericCollection<DevTankLevel>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectScanType>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectScanType_Io>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectScanType_Ec>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectFixedType>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectFixedType_Io>))]
    [XmlInclude(typeof(_GenericCollection<BrokenDetectFixedType_Ec>))]
    [XmlInclude(typeof(_GenericCollection<Cylinder>))]
    [XmlInclude(typeof(_GenericCollection<Cylinder_Io>))]
    [XmlInclude(typeof(_GenericCollection<Cylinder_Ec>))]
    [XmlInclude(typeof(_GenericCollection<CylinderUnit>))]
    [XmlInclude(typeof(_GenericCollection<ActuatorUnit>))]
    [XmlInclude(typeof(_GenericCollection<BLDCMotor>))]
    [XmlInclude(typeof(_GenericCollection<XqdMotor>))]
    [XmlInclude(typeof(_GenericCollection<FBLMotor>))]
    [XmlInclude(typeof(_GenericCollection<BLDCMotor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<ServoCvMotor>))]
    [XmlInclude(typeof(_GenericCollection<ServoTilt>))]
    [XmlInclude(typeof(_GenericCollection<CvUnit>))]
    [XmlInclude(typeof(_GenericCollection<CvUnitA>))]
    [XmlInclude(typeof(_GenericCollection<RbMotor>))]
    [XmlInclude(typeof(_GenericCollection<RbMotor_Io>))]
    [XmlInclude(typeof(_GenericCollection<RbMotor_Xqd>))]
    [XmlInclude(typeof(_GenericCollection<RbMotor_Ec>))]
    [XmlInclude(typeof(_GenericCollection<RbUnit>))]
    [XmlInclude(typeof(_GenericCollection<DBMotor>))]
    [XmlInclude(typeof(_GenericCollection<DBUnit>))]
    [XmlInclude(typeof(_GenericCollection<MitutoyoRbGapGauge>))]
    [XmlInclude(typeof(_GenericCollection<HepaFilter>))]
    [XmlInclude(typeof(_GenericCollection<HepaFilter_Io>))]
    [XmlInclude(typeof(_GenericCollection<HepaFilter_Ec>))]
    [XmlInclude(typeof(_GenericCollection<FanFilter>))]
    [XmlInclude(typeof(_GenericCollection<FanFilterControl>))] // 11.03.07 minhan
    [XmlInclude(typeof(_GenericCollection<Ionizer>))]
    [XmlInclude(typeof(_GenericCollection<Ionizer_Io>))]
    [XmlInclude(typeof(_GenericCollection<Ionizer_Ec>))]
    [XmlInclude(typeof(_GenericCollection<ESD>))]
    [XmlInclude(typeof(_GenericCollection<ESD_Ec>))]
    [XmlInclude(typeof(_GenericCollection<AlarmResetSwitch>))]
    [XmlInclude(typeof(_GenericCollection<AlarmResetSwitch_Io>))]
    [XmlInclude(typeof(_GenericCollection<AlarmResetSwitch_Ec>))]
    [XmlInclude(typeof(_GenericCollection<Buzzer>))]
    [XmlInclude(typeof(_GenericCollection<Buzzer_Io>))]
    [XmlInclude(typeof(_GenericCollection<Buzzer_Ec>))]
    [XmlInclude(typeof(_GenericCollection<SignalTower>))]
    [XmlInclude(typeof(_GenericCollection<SignalTower_Io>))]
    [XmlInclude(typeof(_GenericCollection<SignalTower_Ec>))]
    [XmlInclude(typeof(_GenericCollection<ColorBoard>))]
    [XmlInclude(typeof(_GenericCollection<Mfc>))]
    [XmlInclude(typeof(_GenericCollection<PSMAp>))]
    [XmlInclude(typeof(_GenericCollection<SeAp>))]
    [XmlInclude(typeof(_GenericCollection<EuvLamp>))]
    [XmlInclude(typeof(_GenericCollection<EuvLamp_Io>))]
    [XmlInclude(typeof(_GenericCollection<EuvLamp_Ec>))]
    [XmlInclude(typeof(_GenericCollection<EuvUnitComm>))]
    [XmlInclude(typeof(_GenericCollection<UshioEuvUnit>))]
    [XmlInclude(typeof(_GenericCollection<UshioEuvUnit_Io>))]
    [XmlInclude(typeof(_GenericCollection<UshioEuvUnit_Ec>))]
    [XmlInclude(typeof(_GenericCollection<EyeEuvUnit>))]
    [XmlInclude(typeof(_GenericCollection<ApdItem>))]
    [XmlInclude(typeof(_GenericCollection<PartsItem>))]
    [XmlInclude(typeof(_GenericCollection<HpmjItem>))]
    [XmlInclude(typeof(_GenericCollection<ConiferIDReader>))]
    [XmlInclude(typeof(_GenericCollection<ShinkoDryCleaner>))]
    [XmlInclude(typeof(_GenericCollection<InterfaceStep>))]
    [XmlInclude(typeof(_GenericCollection<TransferRobot>))]
    [XmlInclude(typeof(_GenericCollection<PowerJoint>))]
    [XmlInclude(typeof(_GenericCollection<PowerJointAll>))]
    [XmlInclude(typeof(_GenericCollection<GantryUnit>))]
    [XmlInclude(typeof(_GenericCollection<FishHand>))]
    [XmlInclude(typeof(_GenericCollection<ServoCheckPoint>))]
    [XmlInclude(typeof(_GenericCollection<Hpmj>))]
    [XmlInclude(typeof(_GenericCollection<Hpmj_Io>))]
    [XmlInclude(typeof(_GenericCollection<Hpmj_Ec>))]
    [XmlInclude(typeof(_GenericCollection<Inverter>))]
    [XmlInclude(typeof(_GenericCollection<Inverter_Io>))]
    [XmlInclude(typeof(_GenericCollection<V1000>))]
    [XmlInclude(typeof(_GenericCollection<Inverter_Ec>))]
    [XmlInclude(typeof(_GenericCollection<TransferUnit>))]
    [XmlInclude(typeof(_GenericCollection<MixTankItem>))]
    [XmlInclude(typeof(_GenericCollection<MixTankUnit>))]
    [XmlInclude(typeof(_GenericCollection<MixSupplyPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<DetSupplyPumpUnit>))]
    [XmlInclude(typeof(_GenericCollection<MeasureTankUnit>))]
    [XmlInclude(typeof(_GenericCollection<DetTankUnit>))]
    [XmlInclude(typeof(_GenericCollection<DetergentUnit>))]
    [XmlInclude(typeof(_GenericCollection<RfUnit>))]
    [XmlInclude(typeof(_GenericCollection<Rfg>))]
    [XmlInclude(typeof(_GenericCollection<Seren_Rfg>))]
    [XmlInclude(typeof(_GenericCollection<RfTuner>))]
    [XmlInclude(typeof(_GenericCollection<Apc>))]
    [XmlInclude(typeof(_GenericCollection<ShutterUnit>))]
    [XmlInclude(typeof(_GenericCollection<PMChamber_RIE>))]
    [XmlInclude(typeof(_GenericCollection<PMChamber_HWCVD>))]
    [XmlInclude(typeof(_GenericCollection<LockChamber>))]
    [XmlInclude(typeof(_GenericCollection<BufferUnit>))]
    [XmlInclude(typeof(_GenericCollection<ServoMotorMp2300>))]
    [XmlInclude(typeof(_GenericCollection<ServoUnitMp2300>))]
    [XmlInclude(typeof(_GenericCollection<LGDLoaderInterlock>))]
    [XmlInclude(typeof(_GenericCollection<IntegratingFlowmeter>))]
    [XmlInclude(typeof(_GenericCollection<SRU>))]
    [XmlInclude(typeof(_GenericCollection<HardInterlockItem>))]
    [XmlInclude(typeof(_GenericCollection<HeatExchanger>))]
    [XmlInclude(typeof(_GenericCollection<LampSwitch>))]
    [XmlInclude(typeof(_GenericCollection<PortUnit>))]
    [XmlInclude(typeof(_GenericCollection<PortUnitA>))]
    [XmlInclude(typeof(_GenericCollection<PortUnit_BoeHF_Theragen>))]
    [XmlInclude(typeof(_GenericCollection<PortUnit_LGE_Solar>))]
    [XmlInclude(typeof(_GenericCollection<YaskawaNX100>))]
    [XmlInclude(typeof(_GenericCollection<Yaskawa_BoeHF_Theragen>))]
    [XmlInclude(typeof(_GenericCollection<CstMappingUnitTakex>))]
    [XmlInclude(typeof(_GenericCollection<CstMappingUnit_BoeHF_Theragen>))]
    [XmlInclude(typeof(_GenericCollection<BCRn400k>))]
    [XmlInclude(typeof(_GenericCollection<BCR_BoeHF>))]
    [XmlInclude(typeof(_GenericCollection<LoaderUnit>))]
    [XmlInclude(typeof(_GenericCollection<LoaderUnit_232C>))]
    [XmlInclude(typeof(_GenericCollection<LoaderUnit_BoeHF_Theragen>))]
    [XmlInclude(typeof(_GenericCollection<StockerA>))]
    [XmlInclude(typeof(_GenericCollection<HmiAutoValve>))]
    [XmlInclude(typeof(_GenericCollection<GasControl>))]
    [XmlInclude(typeof(_GenericCollection<MotorLoadFactor>))]
    [XmlInclude(typeof(_GenericCollection<RPS>))]
    [XmlInclude(typeof(_GenericCollection<PMChamber_PECVD>))]
    [XmlInclude(typeof(_GenericCollection<TMChamber_PECVD>))]
    [XmlInclude(typeof(_GenericCollection<BOELoaderInterface>))]
    [XmlInclude(typeof(_GenericCollection<HpmjInterface>))]
    [XmlInclude(typeof(_GenericCollection<ForcedExhaust>))]
    [XmlInclude(typeof(_GenericCollection<FlowAccuItem>))]
    [XmlInclude(typeof(_GenericCollection<FlowAccumulate>))]
    [XmlInclude(typeof(_GenericCollection<EGiSInterface>))]
    [XmlInclude(typeof(_GenericCollection<ETCOutput>))]
    [XmlInclude(typeof(_GenericCollection<ETCOutput_Io>))]
    [XmlInclude(typeof(_GenericCollection<ETCOutput_Ec>))]
    [XmlInclude(typeof(_GenericCollection<FlowmeterDIW>))]
    [XmlInclude(typeof(_GenericCollection<FlowmeterCDA>))]
    [XmlInclude(typeof(_GenericCollection<SmartDamper>))]
    [XmlInclude(typeof(_GenericCollection<D40A>))]
    [XmlInclude(typeof(_GenericCollection<D4SL>))]
    [XmlInclude(typeof(_GenericCollection<LFC>))]
    [XmlInclude(typeof(_GenericCollection<Manometer>))]
    [XmlInclude(typeof(_GenericCollection<LCT>))]

    #endregion
    abstract public class _ComponentContainer : IComponentContainer
    {
        #region Fields
        protected static string m_FileName = "";
        protected ArrayList m_Items = new ArrayList();
        private static int m_CheckPath = -1;
        private static AppConfig m_AppConfig = AppConfig.Instance;
        #endregion

        #region Properties
        public string FileName
        {
            get { return m_FileName; }
        }

        public ArrayList Items
        {
            get { return m_Items; }
            set { m_Items = value; }
        }

        public object this[int index]
        {
            get
            {
                return m_Items[index];
            }
        }
        public object this[string name]
        {
            get
            {
                foreach (object item in this.Items)
                {
                    IGenericCollection collection = (item as IGenericCollection);
                    foreach (_Device device in collection)
                    {
                        if (name == device.Name)
                        {
                            return device;
                        }
                    }
                }

                return null;
            }
        }
        public ArrayList this[Type type]
        {
            get
            {
                return GetCollectionArray(type, Compatibility.Match);
            }
        }
        #endregion

        #region Define Collection
        public _ComponentContainer()
        {
            MakeContainer();
        }
        #endregion

        #region Methods
        protected virtual void MakeContainer()
        {
            //Add _GenericCollection for device container as belows

            //Add(new EqpUnits());
            //Add(new CvUnits());
            //Add(new _GenericCollection<AlarmResetSwitch>());
            //Add(new _GenericCollection<Buzzer>());
            //Add(new _GenericCollection<SignalTower>());
            //Add(new _GenericCollection<EmoSensor>());
            //Add(new _GenericCollection<DoorSensor>());
            //Add(new _GenericCollection<Sensor>());
            //Add(new _GenericCollection<BrokenDetectFixedType>());
            //Add(new _GenericCollection<BrokenDetectScanType>());
            //Add(new _GenericCollection<HepaFilter>());
            //Add(new _GenericCollection<FanFilter>());
            //Add(new _GenericCollection<Ionizer>());
            //Add(new _GenericCollection<Cylinder>());
            //Add(new _GenericCollection<GlsSensor>());
            //Add(new _GenericCollection<FBLMotor>());
            //Add(new _GenericCollection<ActuatorUnit>());
            //Add(new _GenericCollection<ApdItem>());
            //Add(new _GenericCollection<PartsItem>());
            //Add(new _GenericCollection<LevelSensor>());        
        }

        public void Add<T>(T collection)
        {
            m_Items.Add(collection);
        }

        public bool WriteXml(string fileName)
        {
            StreamWriter sw = null;
            XmlSerializer xmlSer = null;

            try
            {
                xmlSer = new XmlSerializer(this.GetType());

                // jemoon : 오류가 있는지 먼저 try
                sw = new StreamWriter(fileName + ".try");
                xmlSer.Serialize(sw, this);
                sw.Close();
                FileInfo file = new FileInfo(fileName + ".try");
                file.Delete();
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();

                return false;
            }

            try
            {   // jemoon : 오류가 없으면 실제로 쓰자
                // jemoon : backup 본을 하나 만들고
                FileInfo file = new FileInfo(fileName);
                if (file.Exists)
                {
                    file.CopyTo(fileName + ".old", true);
                }

                sw = new StreamWriter(fileName);
                xmlSer.Serialize(sw, this);
                sw.Close();

                m_FileName = fileName;
                string[] temp = fileName.Split('\\');
                string path = fileName.Replace(temp[temp.Length - 1], "");
                Environment.CurrentDirectory = path;

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                string msg = err.ToString();
                System.Windows.Forms.MessageBox.Show(err.ToString());

                if (sw != null) sw.Close();

                return false;
            }
        }

        public bool WriteXml()
        {
            if (m_CheckPath == -1) CheckPath();

            if (m_CheckPath == 1)
            {
                return WriteXml(m_FileName);
            }
            else
            {
                MessageBox.Show(this.GetType().Name + " : " + "The specified path is invalid. Check the path to the file and try again.");
                return false;
            }
        }

        public bool ReadXml(string fileName)
        {
            IGenericCollection ig;

            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Exists)
                {
                    m_FileName = fileName;
                }
                else
                {
                    //MessageBox.Show("File not found");
                    //OpenFileDialog dlg = new OpenFileDialog();
                    //dlg.Title = "Select XML file : " + this.GetType().Name;
                    //dlg.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    //if (DialogResult.OK == dlg.ShowDialog())
                    //{
                    //    m_FileName = dlg.FileName;
                    //}
                    string name = string.Format("{0}.xml", this.GetType().Name);
                    MessageBox.Show(name + " file does not exist in the specified location. Check the file and  try again.");
                    return false;
                }

                StreamReader sr = new StreamReader(m_FileName);
                XmlSerializer xmlSer = new XmlSerializer(this.GetType());
                _ComponentContainer container;
                container = xmlSer.Deserialize(sr) as _ComponentContainer;
                sr.Close();

                foreach (IGenericCollection cfg in container.Items)
                {
                    ig = cfg;

                    foreach (IGenericCollection org in this.Items)
                    {
                        if (cfg.GetType() == org.GetType())
                        {
                            org.Clear();
                            int count = cfg.Count;
                            for (int i = 0; i < count; i++)
                            {
                                org.Add(cfg.GetItem(i));
                            }
                            break;
                        }
                    }
                }

                SyncInstance(this);

                return true;
            }
            catch (Exception err)   //Don't Use XFunc.ExceptionHandler.Add(err);
            {
                MessageBox.Show(err.ToString());
                return false;
            }
        }

        public bool ReadXml()
        {
            if (m_CheckPath == -1) CheckPath();
            if (m_CheckPath == 1)
            {
                return ReadXml(m_FileName);
            }
            else return false;
        }

        public void SyncInstance(_ComponentContainer components)
        {
            foreach (IGenericCollection collection in components.Items)
            {
                collection.SyncInstance(components);
            }
        }

        public _GenericCollection<T> GetCollection<T>()
        {
            _GenericCollection<T> collection = GetCollection<T>(typeof(T), Compatibility.Match) as _GenericCollection<T>;

            return (collection != null) ? collection : new _GenericCollection<T>();
        }

        public _GenericCollection<T> GetCollection<T>(Compatibility compatibility)
        {
            _GenericCollection<T> collection = GetCollection<T>(typeof(T), compatibility) as _GenericCollection<T>;

            return (collection != null) ? collection : new _GenericCollection<T>();
        }

        private _GenericCollection<T> GetCollection<T>(Type type, Compatibility compatibility)
        {
            bool find = false;

            _GenericCollection<T> collection = new _GenericCollection<T>();

            foreach (object item in m_Items)
            {
                //IGenericCollection iCollection = item as IGenericCollection;
                //if (type == iCollection.ContainedItemType)

                collection = item as _GenericCollection<T>;
                if (collection != null)
                {   // null이 아니면 type match
                    find = true;
                    break;
                }
            }

            // 한개도 없으면 다른 조건으로 한번더 검색
            if (find && collection != null)
            {
                find &= (collection.Count > 0);
            }

            if (!find)
            {
                ArrayList items = GetCollectionArray(type, compatibility);
                int count = items.Count;

                if (collection == null) collection = new _GenericCollection<T>();

                for (int i = 0; i < count; i++)
                {
                    collection.Add(items[i]);
                }
            }

            return collection;
        }

        public ArrayList GetCollectionArray(Type type, Compatibility compatibility)
        {
            ArrayList matchedItems = new ArrayList();

            foreach (object item in m_Items)
            {
                IGenericCollection collection = (item as IGenericCollection);
                foreach (_Device device in collection)
                {
                    bool match = false;
                    //match |= (type == device.DeviceType);

                    //if (compatibility == Compatibility.Compatible)
                    //{
                    //    match |= (type == device.FamilyType);
                    //}

                    match = XFunc.CheckTypeCompatibility(type, device.DeviceType, compatibility);

                    if (match)
                    {
                        matchedItems.Add(device);
                    }
                }
            }

            return matchedItems;
        }

        private void CheckPath()
        {
            string filePath = m_AppConfig.ComponentContainerPathName;

            if (m_AppConfig.UseDefaultFilePath)
            {
                Directory.CreateDirectory(filePath);
            }

            if (Directory.Exists(filePath) == false)
            {
                MessageBox.Show("ComponentContainer Folder not found");
                FolderBrowserDialog dlg = new FolderBrowserDialog();
                dlg.Description = "ComponentContainer File Folder";
                dlg.SelectedPath = Application.StartupPath;
                dlg.ShowNewFolderButton = true;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    filePath = dlg.SelectedPath;
                    m_AppConfig.ComponentContainerPath.SelectedFolder = filePath;
                    m_AppConfig.WriteXml();

                    m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                    m_CheckPath = 1;
                }
                else
                {
                    m_CheckPath = 0;
                }
            }
            else
            {
                m_FileName = string.Format("{0}\\{1}.xml", filePath, this.GetType().Name);
                m_CheckPath = 1;
            }
        }
        #endregion
    }

    abstract public class _DmsComponents
    {
        #region Fields
        protected _ComponentContainer m_Container;
        #endregion

        #region Properties
        public _ComponentContainer ComponentContainer
        {
            get { return m_Container; }
            set { m_Container = value; }
        }
        public ArrayList ContainedItems
        {
            get { return m_Container.Items; }
            set { m_Container.Items = value; }
        }
        public ArrayList this[Type type]
        {
            get
            {
                return ComponentContainer[type];
            }
        }
        public object this[string name]
        {
            get
            {
                return ComponentContainer[name];
            }
        }
        #endregion

        #region Constructor
        //public _DmsComponents()
        //{
        //}
        #endregion

        #region Methods
        //public object GetCollection(Type type)
        //{
        //    return m_Container.GetCollection(type);
        //}
        public ArrayList GetCollectionArray(Type type, Compatibility compatibilty)
        {
            return m_Container.GetCollectionArray(type, compatibilty);
        }
        public bool WriteXml(string fileName)
        {
            return m_Container.WriteXml(fileName);
        }
        public bool WriteXml()
        {
            return m_Container.WriteXml();
        }
        public bool ReadXml(string fileName)
        {
            return m_Container.ReadXml(fileName);
        }

        public bool ReadXml()
        {
            return m_Container.ReadXml();
        }
        #endregion

        #region Gernerate C# Code
        public void WirteCsharpCode()
        {
            GenerateCsharpCode(this.ContainedItems);
        }

        protected CodeCompileUnit BuildCode(ArrayList componentContainer)
        {
            CodeCompileUnit compileUnit = new CodeCompileUnit();

            // Namespace
            CodeNamespace nameSpace = new CodeNamespace("Dms.Server");
            compileUnit.Namespaces.Add(nameSpace);

            // using
            nameSpace.Imports.Add(new CodeNamespaceImport("Dms.Device"));
            nameSpace.Imports.Add(new CodeNamespaceImport("Dms.ServerCommon"));

            // Build Class and Member
            foreach (IGenericCollection collection in componentContainer)
            {
                // Class
                string className = "eqp" + collection.Name;
                CodeTypeDeclaration class1 = new CodeTypeDeclaration(className);
                nameSpace.Types.Add(class1);

                // Properties
                foreach (_Device device in collection)
                {
                    string deviceName = XFunc.FilterigName(device.Name);
                    CodeTypeReference deviceType = new CodeTypeReference(device.GetType().Name);

                    // Declares a filed
                    CodeMemberField field = new CodeMemberField();
                    field.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                    field.Type = deviceType;
                    field.Name = "m_" + deviceName;
                    CodeVariableReferenceExpression initExpression = new CodeVariableReferenceExpression();
                    initExpression.VariableName = "DmsComponents.Instance[\"" + device.Name + "\"]" + " as " + device.GetType().Name;
                    field.InitExpression = initExpression;
                    class1.Members.Add(field);

                    // Declares a property of type String named StringProperty.
                    CodeMemberProperty property1 = new CodeMemberProperty();
                    property1.Name = "_" + deviceName + "_Name";
                    property1.Type = new CodeTypeReference("System.String");
                    property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                    property1.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(device.Name)));
                    class1.Members.Add(property1);

                    // Declares a property of type Device.
                    //CodeMemberProperty property2 = new CodeMemberProperty();
                    //property2.Name = "_" + deviceName;
                    //property2.Type = deviceType;
                    //property2.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                    //CodeVariableReferenceExpression variableRef1 = new CodeVariableReferenceExpression();
                    //variableRef1.VariableName = "DmsComponents.Instance[\"" + device.Name + "\"]" + " as " + device.GetType().Name;
                    //property2.GetStatements.Add(new CodeMethodReturnStatement(variableRef1));
                    //class1.Members.Add(property2);

                    CodeMemberProperty property3 = new CodeMemberProperty();
                    property3.Name = "_" + deviceName;
                    property3.Type = deviceType;
                    property3.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                    CodeVariableReferenceExpression variableRef2 = new CodeVariableReferenceExpression();
                    variableRef2.VariableName = field.Name;
                    property3.GetStatements.Add(new CodeMethodReturnStatement(variableRef2));
                    class1.Members.Add(property3);

                }

                //if (collection.Count > 0)
                {
                    // Declares a filed
                    CodeMemberField field = new CodeMemberField();
                    field.Attributes = MemberAttributes.Private | MemberAttributes.Static;
                    field.Type = new CodeTypeReference(typeof(IGenericCollection).Name);
                    field.Name = "m_Items";
                    CodeVariableReferenceExpression initExpression = new CodeVariableReferenceExpression();
                    initExpression.VariableName = "DmsComponents.Instance.ComponentContainer.GetCollection<" + collection.ContainedItemType.Name + ">()";
                    field.InitExpression = initExpression;
                    class1.Members.Add(field);


                    // Declares a property of Genenric Collection.
                    string typeName = typeof(IGenericCollection).Name;
                    CodeMemberProperty property1 = new CodeMemberProperty();
                    property1.Name = "_Items";
                    property1.Type = new CodeTypeReference(typeName);
                    property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                    CodeVariableReferenceExpression variableRef1 = new CodeVariableReferenceExpression();
                    variableRef1.VariableName = field.Name;
                    property1.GetStatements.Add(new CodeMethodReturnStatement(variableRef1));
                    class1.Members.Add(property1);
                }


                // ServoUnit TeachPointId
                if (collection.ContainedItemType == typeof(ServoUnit))
                {
                    foreach (_Device device in collection)
                    {
                        ServoUnit servoUnit = (ServoUnit)device;
                        string deviceName = XFunc.FilterigName(device.Name);

                        class1 = new CodeTypeDeclaration("eqpPos_" + deviceName);
                        nameSpace.Types.Add(class1);

                        if (servoUnit.TeachPointName == null) continue;

                        int posCount = servoUnit.TeachPointName.Length;
                        for (int i = 0; i < posCount; i++)
                        {
                            CodeTypeReference deviceType = new CodeTypeReference(typeof(Int16));

                            // Declares a property of type String named StringProperty.
                            CodeMemberProperty property1 = new CodeMemberProperty();
                            property1.Name = XFunc.FilterigName(servoUnit.TeachPointName[i]);
                            property1.Type = new CodeTypeReference(typeof(Int16));
                            property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                            property1.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(i)));
                            class1.Members.Add(property1);
                        }
                    }
                }

                // AbsodexMotor TeachPointId
                if (collection.ContainedItemType == typeof(AbsodexMotor))
                {
                    foreach (_Device device in collection)
                    {
                        AbsodexMotor motor = (AbsodexMotor)device;
                        string deviceName = XFunc.FilterigName(device.Name);

                        class1 = new CodeTypeDeclaration("eqpPos_" + deviceName);
                        nameSpace.Types.Add(class1);

                        if (motor.PointList == null) continue;

                        int posCount = motor.PointList.Length;
                        for (int i = 0; i < posCount; i++)
                        {
                            CodeTypeReference deviceType = new CodeTypeReference(typeof(Int16));

                            // Declares a property of type String named StringProperty.
                            CodeMemberProperty property1 = new CodeMemberProperty();
                            property1.Name = XFunc.FilterigName(motor.PointList[i]);
                            property1.Type = new CodeTypeReference(typeof(Int16));
                            property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                            property1.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(i)));
                            class1.Members.Add(property1);
                        }
                    }
                }

                // ServoUnitMp2300 TeachPointId
                if (collection.ContainedItemType == typeof(ServoUnitMp2300))
                {
                    foreach (_Device device in collection)
                    {
                        ServoUnitMp2300 servoUnit = (ServoUnitMp2300)device;
                        string deviceName = XFunc.FilterigName(device.Name);

                        class1 = new CodeTypeDeclaration("eqpPos_" + deviceName);
                        nameSpace.Types.Add(class1);

                        if (servoUnit.TeachPointName == null) continue;

                        int posCount = servoUnit.TeachPointName.Length;
                        for (int i = 0; i < posCount; i++)
                        {
                            CodeTypeReference deviceType = new CodeTypeReference(typeof(Int16));

                            // Declares a property of type String named StringProperty.
                            CodeMemberProperty property1 = new CodeMemberProperty();
                            property1.Name = XFunc.FilterigName(servoUnit.TeachPointName[i]);
                            property1.Type = new CodeTypeReference(typeof(Int16));
                            property1.Attributes = MemberAttributes.Public | MemberAttributes.Static;
                            property1.GetStatements.Add(new CodeMethodReturnStatement(new CodePrimitiveExpression(i)));
                            class1.Members.Add(property1);
                        }
                    }
                }
            }

            return compileUnit;
        }

        protected void GenerateCode(CodeDomProvider provider, CodeCompileUnit compileUnit)
        {
            string codeFile = "ComponentContainer." + provider.FileExtension;

            FileInfo file = new FileInfo(codeFile);
            if (file.Exists)
            {
                file.CopyTo(codeFile + ".old", true);
            }

            IndentedTextWriter writer = new IndentedTextWriter(new StreamWriter(codeFile, false, System.Text.Encoding.Default), "    ");
            CodeGeneratorOptions option = new CodeGeneratorOptions();
            option.BracingStyle = "C";
            provider.GenerateCodeFromCompileUnit(compileUnit, writer, option);
            writer.Close();
        }

        public void GenerateCsharpCode(ArrayList componentContainer)
        {
            GenerateCode(new CSharpCodeProvider(), BuildCode(componentContainer));
        }
        #endregion
    }
}
