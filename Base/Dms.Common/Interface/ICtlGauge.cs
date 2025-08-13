using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Dms.Common
{
    public interface ICtlGauge
    {
        [Browsable(false), XmlIgnore()]
        double CurValue
        {
            get;
            set;
        }
    }
}
