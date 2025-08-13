using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
	[Serializable()]
	public class IoNodeMelsecEtherNet : IoNodeMelsec
	{
		#region Constructor
        public IoNodeMelsecEtherNet()
        {
        }

		public IoNodeMelsecEtherNet(FieldBusType busType) 
			: base(busType)
        {
		}
		#endregion
	}
}
