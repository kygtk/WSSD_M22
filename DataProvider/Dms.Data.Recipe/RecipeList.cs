using System;
using System.Collections.Generic;
using System.Text;
using Dms.Common;

namespace Dms.Data
{
    public class RecipeList
    {
        #region Fields
        private List<TagRecipe> m_Items = new List<TagRecipe>();
        #endregion

        #region Properties
        public int Count
        {
            get { return m_Items.Count; }
        }
        #endregion    

        #region Constructor
        public RecipeList()
        {
        }
        #endregion

        #region Methods
 
        #endregion
    }
}
