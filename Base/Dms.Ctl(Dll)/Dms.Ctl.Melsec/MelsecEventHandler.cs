using System;
using System.Collections.Generic;
using System.Text;

namespace Dms.Ctl
{
    public delegate void MelsecStateChangeEventHandler(object obj, MelsecStateChangeEventArgs e);

    public class MelsecStateChangeEventArgs : EventArgs
    {
        private short nAddress;
        public short Address
        {
            get { return nAddress; }
            set { nAddress = value; }
        }

        public MelsecStateChangeEventArgs() { }

        public MelsecStateChangeEventArgs(short Address)
        {
            this.Address = Address;
        }
    }
}
