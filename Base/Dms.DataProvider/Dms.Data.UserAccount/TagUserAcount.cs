using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    //public enum UserLevels
    //{
    //    Operator,
    //    Technician,
    //    Engineer,
    //    Administrator
    //}

    [Serializable()]
    public class TagUserAccount
    {
        public UserLevels UserLevel;
        public string UserID = " ";
        public string Password = " ";

        public void Clone(TagUserAccount account)
        {
            this.UserLevel = account.UserLevel;
            this.UserID = account.UserID;
            this.Password = account.Password;
        }
    }
}
