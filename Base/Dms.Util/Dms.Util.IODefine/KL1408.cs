using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class KL1408 : IoTerminal
    {
        const int _MaxChannel = 8;

        public KL1408()
        {
            this.IoType = IoType.DI;
            this.TerminalType = TerminalType.KL1408;
        }

        public KL1408 Create()
        {
            for (int i = 0; i < _MaxChannel; i++)
            {
                IoItem item = new IoItemDI();
                item.Channel = i;
                item.Name = "di__";
                item.IoType = this.IoType;
                this.Channels.Add(item);
            }

            return this;
        }
    }
}