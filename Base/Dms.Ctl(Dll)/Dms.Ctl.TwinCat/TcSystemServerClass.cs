using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dms.Ctl
{
    internal class TcSystemServerClass
    {
        public string AmsNetId { get; internal set; }
        public int SystemState { get; internal set; }

        internal void StartSystem(object param)
        {
            throw new NotImplementedException();
        }

        internal void StopSystem(object param)
        {
            throw new NotImplementedException();
        }
    }
}
