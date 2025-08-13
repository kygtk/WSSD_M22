using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;
using System.Drawing;

namespace Dms.Data
{
    public enum SensorInterlockType
    {
        Emo,
        Door,
        Cover,
        Leak,
        FFU,
        Broken,
        Area,
        Etc,
    }

    [Serializable()]
    public class TagSetupInfo
    {
        #region Fields
        private string m_Name;
        private OptionType m_Type;
        private OptionFormat m_Format;
        private TagUnit m_Unit;
        private string m_Val;
        private string m_LoTerm = " ";
        private string m_HiTerm = " ";
        #endregion

        #region Properties
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }
        public OptionType Type
        {
            get { return m_Type; }
            set { m_Type = value; }
        }
        public OptionFormat Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }
        public TagUnit Unit
        {
            get { return m_Unit; }
            set { m_Unit = value; }
        }
        public string UnitName
        {
            get { return m_Unit.Name; }
        }
        public string Val
        {
            get { return m_Val; }
            set { m_Val = value; }
        }
        public string LoTerm
        {
            get { return m_LoTerm; }
            set { m_LoTerm = value; }
        }
        public string HiTerm
        {
            get { return m_HiTerm; }
            set { m_HiTerm = value; }
        }
        #endregion

        #region Constructor
        public TagSetupInfo()
        {

        }

        public TagSetupInfo(string name, OptionType type, OptionFormat format, UnitType unit, string val)
        {
            m_Name = name;
            m_Type = type;
            m_Format = format;
            m_Unit = new TagUnit(unit);
            m_Val = val;
        }

        public TagSetupInfo(string name, OptionType type, OptionFormat format, UnitType unit, string val, string loTerm, string hiTerm)
        {
            m_Name = name;
            m_Type = type;
            m_Format = format;
            m_Unit = new TagUnit(unit);
            m_Val = val;
            m_LoTerm = loTerm;
            m_HiTerm = hiTerm;
        }
        #endregion

        #region Methods
        public void Clone(TagSetupInfo item)
        {
            m_Name = item.m_Name;
            m_Type = item.m_Type;
            m_Format = item.m_Format;
            m_Unit = item.m_Unit;
            m_Val = item.m_Val;
            m_LoTerm = item.m_LoTerm;
            m_HiTerm = item.m_HiTerm;
        }

        // jemoon : value형을 object형으로 전환하게 되면 boxing/unboxing으로 인해 성능저하 초래
        // jemoon : C#에는 아래처럼 Template를 사용한다. -> Generics
        // 참고 : http://msdn.microsoft.com/ko-kr/library/ms379564(VS.80).aspx
        public T GetValue<T>()
        {
            return OptionFormatHelper.GetValue<T>(m_Format, m_Val);
        }

        public override string ToString()
        {
            return m_Name;
        }
        #endregion
    }


    [Serializable()]
    public class TagSetupCvInfo
    {
        #region Fields
        private string name;
        private double velRatio;
        private double gearRatio;
        private double diaMeter;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public double VelRatio
        {
            get { return velRatio; }
            set { velRatio = value; }
        }
        public double GearRatio
        {
            get { return gearRatio; }
            set { gearRatio = value; }
        }
        public double DiaMeter
        {
            get { return diaMeter; }
            set { diaMeter = value; }
        }
        #endregion

        #region Constructor
        public TagSetupCvInfo()
        {

        }

        public TagSetupCvInfo(int id, string name, double velRatio, double gearRatio, double diaMeter)
        {
            this.name = name;
            this.velRatio = velRatio;
            this.gearRatio = gearRatio;
            this.diaMeter = diaMeter;
        }

        public TagSetupCvInfo(string name, double velRatio, double gearRatio, double diaMeter)
        {
            this.name = name;
            this.velRatio = velRatio;
            this.gearRatio = gearRatio;
            this.diaMeter = diaMeter;
        }
        #endregion

        #region Methods
        public void Clone(TagSetupCvInfo item)
        {
            this.name = item.name;
            this.velRatio = item.velRatio;
            this.gearRatio = item.gearRatio;
            this.diaMeter = item.diaMeter;
        }

        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }

    [Serializable()]
    public class TagSetupSenSorInterlock
    {
        #region Fields
        private string name;
        private bool use;
        private SensorInterlockType type;
        #endregion

        #region Properties
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

        public SensorInterlockType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
        #endregion

        #region Constructor
        public TagSetupSenSorInterlock()
        {

        }

        public TagSetupSenSorInterlock(string name, bool use, SensorInterlockType enSensorParam)
        {
            this.name = name;
            this.use = use;
            this.type = enSensorParam;
        }
        #endregion

        #region Methods
        public void Clone(TagSetupSenSorInterlock item)
        {
            this.name = item.name;
            this.use = item.use;
            this.type = item.type;
        }

        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }

    // ECID, CEID 때문에 추가 //Sangseo 2009.9.29
    [Serializable()]
    public class TagHsmsSetupInfo
    {
        #region Fields
        private int id;
        private string name;
        private OptionType type;
        private OptionFormat format;
        private TagUnit unit;
        private string val;
        private string loTerm = " ";
        private string hiTerm = " ";
        private GridViewMode mode;
        #endregion

        #region Properties
        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public OptionType Type
        {
            get { return type; }
            set { type = value; }
        }
        public OptionFormat Format
        {
            get { return format; }
            set { format = value; }
        }
        public TagUnit Unit
        {
            get { return unit; }
            set { unit = value; }
        }
        public string UnitName
        {
            get { return unit.Name; }
        }
        public string Val
        {
            get { return val; }
            set { val = value; }
        }
        public string LoTerm
        {
            get { return loTerm; }
            set { loTerm = value; }
        }
        public string HiTerm
        {
            get { return hiTerm; }
            set { hiTerm = value; }
        }
        public GridViewMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        #endregion

        #region Constructor
        public TagHsmsSetupInfo()
        {

        }

        public TagHsmsSetupInfo(int id, string name, OptionType type, OptionFormat format, UnitType unit, string val, GridViewMode mode)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.format = format;
            this.unit = new TagUnit(unit);
            this.val = val;
            this.mode = mode;

        }

        public TagHsmsSetupInfo(int id, string name, OptionType type, OptionFormat format, UnitType unit, string val, string loTerm, string hiTerm, GridViewMode mode)
        {
            this.id = id;
            this.name = name;
            this.type = type;
            this.format = format;
            this.unit = new TagUnit(unit);
            this.val = val;
            this.loTerm = loTerm;
            this.hiTerm = hiTerm;
            this.mode = mode;
        }
        #endregion

        #region Methods
        public void Clone(TagHsmsSetupInfo item)
        {
            this.id = item.id;
            this.name = item.name;
            this.type = item.type;
            this.format = item.format;
            this.unit = item.unit;
            this.val = item.val;
            this.loTerm = item.loTerm;
            this.hiTerm = item.hiTerm;
            this.mode = item.mode;
        }

        // jemoon : value형을 object형으로 전환하게 되면 boxing/unboxing으로 인해 성능저하 초래
        // jemoon : C#에는 아래처럼 Template를 사용한다. -> Generics
        // 참고 : http://msdn.microsoft.com/ko-kr/library/ms379564(VS.80).aspx
        public T GetValue<T>()
        {
            return OptionFormatHelper.GetValue<T>(format, val);
        }

        public override string ToString()
        {
            return this.Name;
        }
        #endregion
    }

}
