using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Dms.Data;

namespace Dms.HMI
{
    public partial class RecipeTabOneRecipe : UserControl
    {
        #region Properties
        public ViewRecipe ViewRecipe
        {
            get { return this.viewRecipe1; }
        } 
        #endregion

        #region Constructor
        public RecipeTabOneRecipe()
        {
            InitializeComponent();

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.CacheText, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        } 
        #endregion

        #region Methods
        public void Initialize(RecipeProvider provider, bool vertical, bool onlyCurrent)
        {
            this.viewRecipe1.InitDataView(provider, vertical, onlyCurrent);
        } 
        #endregion
    }
}
