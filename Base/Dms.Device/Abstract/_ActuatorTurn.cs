using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Dms.Common;

namespace Dms.Device
{
    abstract public class _ActuatorTurn : _Actuator
    {
        #region Tag Descriptor
        protected static new TagDescriptorActuatorTurn tagDescriptor = new TagDescriptorActuatorTurn();
        #endregion

        #region Fields
        protected int m_RefPoint;
        protected int m_CurPoint = -1;
        protected string[] m_PointList;
        #endregion

        #region Properties
        public override Type FamilyType
        {
            get { return typeof(_ActuatorTurn); }
        }
        [Category("DMS : Setting - Position")]
        [Description("Target Point Name")]
        public string[] PointList
        {
            get { return m_PointList; }
            set { m_PointList = value; }
        }
        #endregion

        #region Methods
        abstract public int SetRefPoint(int pointId);
        abstract public int GetRefPoint();
        abstract public int GetCurPoint();
        abstract public void SetCurPoint(int pointId);
        abstract public bool GetPositionSensorState(int pointId, bool defaultValue);
        #endregion
    }
}
