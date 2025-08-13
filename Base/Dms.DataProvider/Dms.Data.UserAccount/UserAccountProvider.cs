using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class UserAccountProvider
    {
        #region Fields
        private UserAccountAdapter m_Adapter;
        private TagUserAccount m_CurrentUserAccount;
        #endregion

        #region Properties
        public UserAccountAdapter Adapter
        {
            get { return m_Adapter; }
        }
        public TagUserAccount CurrentUserAccount
        {
            get { return m_CurrentUserAccount; }
            set { m_CurrentUserAccount = value; }
        }
        #endregion

        #region Singleton code...
        public static readonly UserAccountProvider Instance = new UserAccountProvider();
        #endregion

        #region Constructor
        private UserAccountProvider()
        {
            m_Adapter = new UserAccountAdapter();
        }
        #endregion

        #region Methods
        public bool CreateUser(TagUserAccount account)
        {
            return Adapter.Add(account);
        }

        public bool RemoveUser(string userID)
        {
            return Adapter.Remove(userID);
        }

        public bool Login(TagUserAccount account)
        {
            return Adapter.Login(account);
        }

        public bool ChangePassword(string userID, string curPass, string newPass1, string newPass2)
        {
            return Adapter.ChangePassword(userID, curPass, newPass1, newPass2);
        }

        public TagUserAccount LoginDefaultUser()
        {
            TagUserAccount account = new TagUserAccount();
            if (Adapter.GetDefaultUserAccout(ref account) == true)
            {
                Login(account);
            }
            else
            {
                account.UserID = "Default";
                account.UserLevel = UserLevels.Administrator;
                account.Password = "654321";
                CreateUser(account);
                Login(account);
            }
            return account;
        }
	    #endregion    
    }
}
