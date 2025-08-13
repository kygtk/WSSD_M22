namespace Dms.Control
{
    partial class RecipeTabRecipes
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마십시오.
        /// </summary>
        private void InitializeComponent()
        {
            this.viewRecipe1 = new Dms.Data.ViewRecipe();
            this.SuspendLayout();
            // 
            // viewRecipe1
            // 
            this.viewRecipe1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.viewRecipe1.BackColor = System.Drawing.Color.Transparent;
            this.viewRecipe1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewRecipe1.Location = new System.Drawing.Point(3, 3);
            this.viewRecipe1.Name = "viewRecipe1";
            this.viewRecipe1.Size = new System.Drawing.Size(895, 527);
            this.viewRecipe1.SkipItems = 0;
            this.viewRecipe1.TabIndex = 0;
            this.viewRecipe1.TitleName = "Recipes";
            // 
            // RecipeTabRecipes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.viewRecipe1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "RecipeTabRecipes";
            this.Size = new System.Drawing.Size(903, 539);
            this.ResumeLayout(false);

        }

        #endregion

        private Dms.Data.ViewRecipe viewRecipe1;


    }
}
