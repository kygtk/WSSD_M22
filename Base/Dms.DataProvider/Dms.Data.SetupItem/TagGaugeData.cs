using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public enum GaugeType
    {
        PG35,
        DW605,
        Sensys,
        JLK25,
        ULK25,
        CKDFlow,
        WaterResistance,
        HpmjCurrent,//2009.09.21 kimgun
        PT100,
        GsEuv,
        LevelGauge,
        ColorBoard,
        HPMJPress,
        ApPCWFlow,
        ApN2Flow,
        ApCDAFlow,
        ApVoltage,
        ApWatt, //Mr.kang
        SEBA_SVF_PC_120,
        SEBA_SCTUF_PC_030,
        SEBA_SCTUF_PC_050,
        SEBA_SCTUF_PC_080,
        densitometer,
        PCG550,
        RfPos,
        RfPower,
        MfcFlow,

        AP_DIW_Flow,
        AP_DIW_Press,
        AP_CDA_Flow,
        AP_CDA_Press,
        AP_SmartDamper_Pressure,
        AP_SmartDamper_ValveAngle,
        AP_LFC_Flow,
        AP_LFC_Press,
        AP_LFC_OpenRate,
        AP_Manometer_Exhaust,
        AP_LCT_Level1,
        AP_LCT_Level2,
        AP_LCT_Consistence,
    }
    public enum ScaleType
    {
        Common,
        Log,
        Log10
    }

    [Serializable()]
    public class TagCalibrationInfo
    {
        private string name;
        private GaugeType type;
        private ScaleType scale;
        private TagUnit unit;
        private double filterGain;
        private short adcMax;
        private short adcMin;
        private double realMax;
        private double realMin;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public GaugeType Type
        {
            get { return type; }
            set { type = value; }
        }
        public ScaleType Scale
        {
            get { return scale; }
            set { scale = value; }
        }
        public TagUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public double FilterGain
        {
            get { return filterGain; }
            set
            {
                if (value < 0.1) filterGain = 0.1;
                else if (value > 1.0) filterGain = 1.0;
                else filterGain = value;
            }
        }
        public short AdcMax
        {
            get { return adcMax; }
            set { adcMax = value; }
        }
        public short AdcMin
        {
            get { return adcMin; }
            set { adcMin = value; }
        }
        public double RealMax
        {
            get { return realMax; }
            set { realMax = value; }
        }
        public double RealMin
        {
            get { return realMin; }
            set { realMin = value; }
        }


        public void Clone(TagCalibrationInfo info)
        {
            this.name = info.name;
            this.type = info.type;
            this.scale = info.scale;
            this.unit = info.unit;
            this.filterGain = info.filterGain;
            this.adcMax = info.adcMax;
            this.adcMin = info.adcMin;
            this.realMax = info.realMax;
            this.realMin = info.realMin;
        }

        public TagCalibrationInfo()
        {

        }

        public TagCalibrationInfo(GaugeType type, string name)
        {
            this.name = name;
            this.type = type;

            switch (type)
            {
                case GaugeType.CKDFlow:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 200.0;
                    realMax = 4000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.DW605:
                    unit = new TagUnit(UnitType.Pa);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 750.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.GsEuv:
                    unit = new TagUnit(UnitType.mW);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 385.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.JLK25:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 130.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.ULK25:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 130.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.LevelGauge:
                    unit = new TagUnit(UnitType.L);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 1000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.PG35:
                    unit = new TagUnit(UnitType.kPa);
                    adcMin = 0x0CCC;
                    adcMax = 0x3000;
                    realMin = 0.0;
                    realMax = 1000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.PT100:
                    unit = new TagUnit(UnitType.Celsius);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 4000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.WaterResistance:
                    unit = new TagUnit(UnitType.Mohm);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 4000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.HpmjCurrent://2009.09.21 kimgun
                    unit = new TagUnit(UnitType.A);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 4000.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.Sensys:
                    unit = new TagUnit(UnitType.Pa);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 750.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ColorBoard:
                    unit = new TagUnit(UnitType.None);
                    adcMin = 0;     //0V
                    adcMax = 16383; //5V
                    realMin = 0.0;
                    realMax = 256.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.HPMJPress:
                    unit = new TagUnit(UnitType.Bar);
                    adcMin = 6553;     //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 200.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ApN2Flow:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;      //4mA
                    adcMax = 32767;     //20mA
                    realMin = 0.0;
                    realMax = 500.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ApCDAFlow:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;      //4mA
                    adcMax = 32767;     //20mA
                    realMin = 0.0;
                    realMax = 3.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ApPCWFlow:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 2.0;
                    realMax = 16.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ApVoltage:
                    unit = new TagUnit(UnitType.kV);
                    adcMin = 0;     //0V
                    adcMax = 16383; //5V
                    realMin = 0.0;
                    realMax = 13.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;
                case GaugeType.ApWatt: // Mr.Kang
                    unit = new TagUnit(UnitType.KW);
                    adcMin = 0;     //0V
                    adcMax = 16383; //5V
                    realMin = 0.0;
                    realMax = 12.0;
                    scale = ScaleType.Common;
                    filterGain = 0.3;
                    break;

                case GaugeType.SEBA_SCTUF_PC_030:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 30.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.SEBA_SCTUF_PC_050:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 50.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.SEBA_SCTUF_PC_080:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 80.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.SEBA_SVF_PC_120:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 120.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.densitometer:
                    unit = new TagUnit(UnitType.Percent);
                    adcMin = 6553;  //4mA
                    adcMax = 32767; //20mA
                    realMin = 0.0;
                    realMax = 5.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.PCG550:
                    unit = new TagUnit(UnitType.Torr);
                    adcMin = 1998;  // +1.2  V
                    adcMax = 32767; // +8.68 V
                    realMin = 0.001;
                    realMax = 10.21;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.RfPos:
                    unit = new TagUnit(UnitType.Percent);
                    adcMin = 1;     // +0  V
                    adcMax = 16383; // +5.0 V
                    realMin = 0;
                    realMax = 100;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.RfPower:
                    unit = new TagUnit(UnitType.KW);
                    adcMin = 1;     // +0  V
                    adcMax = 16383; // +5.0 V
                    realMin = 0;
                    realMax = 15;   // 15KW
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.MfcFlow:
                    unit = new TagUnit(UnitType.ccm);
                    adcMin = 1;     // +0  V
                    adcMax = 16383; // +5.0 V
                    realMin = 0;
                    realMax = 1;   // 1000 ccm
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                case GaugeType.AP_DIW_Flow:
                case GaugeType.AP_CDA_Flow:
                case GaugeType.AP_LFC_Flow:
                    unit = new TagUnit(UnitType.lpm);
                    adcMin = 0;     //  
                    adcMax = 4095;  //  Ap 데이터 하위 12bit
                    realMin = 0;
                    realMax = 4095; //  BM 일단 그대로 표기
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.AP_DIW_Press:
                case GaugeType.AP_CDA_Press:
                case GaugeType.AP_SmartDamper_Pressure:
                case GaugeType.AP_LFC_Press:
                    unit = new TagUnit(UnitType.Pa);
                    adcMin = 0;     //  
                    adcMax = 4095;  //  Ap 데이터 하위 12bit
                    realMin = 0;
                    realMax = 4095; //  BM 일단 그대로 표기
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.AP_SmartDamper_ValveAngle:
                    unit = new TagUnit(UnitType.Degree);
                    adcMin = 0;
                    adcMax = 90;   //  Ap 데이터 하위 12bit지만 실제값대로 오기 때문에 90으로 지정
                    realMin = 0;
                    realMax = 90;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.AP_LFC_OpenRate:
                    unit = new TagUnit(UnitType.Percent);
                    adcMin = 0;
                    adcMax = 100;   //  Ap 데이터 하위 12bit지만 실제값대로 오기 때문에 100으로 지정
                    realMin = 0;
                    realMax = 100;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;
                case GaugeType.AP_Manometer_Exhaust:
                    unit = new TagUnit(UnitType.Pa);
                    adcMin = 0;     //  
                    adcMax = 4095;  //  Ap 데이터 하위 12bit
                    realMin = 0;
                    realMax = 4095; //  BM 일단 그대로 표기
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                default:
                    unit = new TagUnit(UnitType.None);
                    adcMin = 6553;
                    adcMax = 32767;
                    realMin = 0.0;
                    realMax = 100.0;
                    scale = ScaleType.Common;
                    filterGain = 0.5;
                    break;

                    // TODO : implement others
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


    [Serializable()]
    public class TagGaugeInterlock
    {
        private string name;
        private bool use;
        private double lowAlarm;
        private double lowWarning;
        private double settingvalue; // 10.10.29 minhan
        private double highWarning;
        private double highAlarm;
        private double delayTime; // 11.01.27 minhan
        private TagUnit unit;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public bool Use
        {
            get { return use; }
            set { use = value; }
        }
        public double LowAlarm
        {
            get { return lowAlarm; }
            set { lowAlarm = value; }
        }
        public double LowWarning
        {
            get { return lowWarning; }
            set { lowWarning = value; }
        }
        public double SettingValue // 10.10.29 minhan
        {
            get { return settingvalue; }
            set { settingvalue = value; }
        }
        public double HighWarning
        {
            get { return highWarning; }
            set { highWarning = value; }
        }
        public double HighAlarm
        {
            get { return highAlarm; }
            set { highAlarm = value; }
        }
        public double DelayTime // 11.01.27 minhan
        {
            get { return delayTime; }
            set { delayTime = value; }
        }
        public TagUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }

        public void Clone(TagGaugeInterlock info)
        {
            this.name = info.name;
            this.use = info.use;
            this.unit = info.unit;
            this.lowAlarm = info.lowAlarm;
            this.lowWarning = info.lowWarning;
            this.settingvalue = info.settingvalue; // 10.10.29 minhan
            this.highWarning = info.highWarning;
            this.highAlarm = info.highAlarm;
            this.delayTime = info.delayTime; // 11.01.27 minhan
        }

        public TagGaugeInterlock()
        {

        }

        public TagGaugeInterlock(string name, UnitType unitType, double lowAlarm, double lowWarning, double settingvalue, double highWarning, double highAlarm, double delaytime, bool use) // 10.10.29 minhan
        {
            this.name = name;
            this.use = use;
            this.unit = new TagUnit(unitType);
            this.lowAlarm = lowAlarm;
            this.lowWarning = lowWarning;
            this.settingvalue = settingvalue;
            this.highWarning = highWarning;
            this.highAlarm = highAlarm;
            this.DelayTime = delayTime;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
