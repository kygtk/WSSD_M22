using System;


namespace Dms.Common
{
    public class ServerNotAliveException : Exception
    {
        public override string Message
        {
            get
            {
                return "Server is not connected!";
            }
        }
    }

    public class InvalidHMIException : Exception
    {
        public override string Message
        {
            get
            {
                return "The HMI is not founnd!";
            }
        }
    }
}
