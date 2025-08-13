using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Util.IODefine
{
    [Serializable()]
    public class KL2408 : IoTerminal
    {
        const int _MaxChannel = 8;

        public KL2408()
        {
            this.IoType = IoType.DO;
            this.TerminalType = TerminalType.KL2408;
        }

        public KL2408 Create()
        {
            for (int i = 0; i < _MaxChannel; i++)
            {
                IoItem item = new IoItem();
                item.Channel = i;
                item.Name = "do__";
                item.IoType = this.IoType;
                this.Channels.Add(item);
            }

            return this;
        }
    }
}