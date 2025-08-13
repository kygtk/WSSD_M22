namespace Dms.Control
{
    partial class CvUnit
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
            this.unit = new Dms.Control.Unit();
            this.glsDataL = new Dms.Control.GlsData();
            this.glsDataR = new Dms.Control.GlsData();
            this.glsSensorL = new Dms.Control.GlsSensor();
            this.glsSensorR = new Dms.Control.GlsSensor();
            this.cvMotorR = new Dms.Control.CvMotor();
            this.cvMotorL = new Dms.Control.CvMotor();
            this.leakSensor = new Dms.Control.LeakSensor();
            this.SuspendLayout();
            // 
            // unit
            // 
            this.unit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.unit.Location = new System.Drawing.Point(0, 16);
            this.unit.Name = "unit";
            this.unit.Size = new System.Drawing.Size(96, 32);
            this.unit.TabIndex = 0;
            this.unit.UnitName = "__ UNIT";
            // 
            // glsDataL
            // 
            this.glsDataL.Location = new System.Drawing.Point(16, 0);
            this.glsDataL.Name = "glsDataL";
            this.glsDataL.PositionId = 0;
            this.glsDataL.Size = new System.Drawing.Size(31, 15);
            this.glsDataL.TabIndex = 2;
            // 
            // glsDataR
            // 
            this.glsDataR.Location = new System.Drawing.Point(49, 0);
            this.glsDataR.Name = "glsDataR";
            this.glsDataR.PositionId = 0;
            this.glsDataR.Size = new System.Drawing.Size(31, 15);
            this.glsDataR.TabIndex = 3;
            // 
            // glsSensorL
            // 
            this.glsSensorL.Location = new System.Drawing.Point(0, 0);
            this.glsSensorL.Name = "glsSensorL";
            this.glsSensorL.Size = new System.Drawing.Size(15, 15);
            this.glsSensorL.TabIndex = 1;
            // 
            // glsSensorR
            // 
            this.glsSensorR.Location = new System.Drawing.Point(81, 0);
            this.glsSensorR.Name = "glsSensorR";
            this.glsSensorR.Size = new System.Drawing.Size(15, 15);
            this.glsSensorR.TabIndex = 4;
            // 
            // cvMotorR
            // 
            this.cvMotorR.AnimationUpdateInterval = 70;
            this.cvMotorR.CurMotorDirection = Dms.Control.CvMotor.MotorDirection.CwIsFw;
            this.cvMotorR.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cvMotorR.Location = new System.Drawing.Point(69, 51);
            this.cvMotorR.Name = "cvMotorR";
            this.cvMotorR.Size = new System.Drawing.Size(28, 27);
            this.cvMotorR.TabIndex = 7;
            // 
            // cvMotorL
            // 
            this.cvMotorL.AnimationUpdateInterval = 70;
            this.cvMotorL.CurMotorDirection = Dms.Control.CvMotor.MotorDirection.CwIsFw;
            this.cvMotorL.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cvMotorL.Location = new System.Drawing.Point(41, 51);
            this.cvMotorL.Name = "cvMotorL";
            this.cvMotorL.Size = new System.Drawing.Size(28, 27);
            this.cvMotorL.TabIndex = 6;
            // 
            // leakSensor
            // 
            this.leakSensor.Location = new System.Drawing.Point(0, 51);
            this.leakSensor.Name = "leakSensor";
            this.leakSensor.Size = new System.Drawing.Size(35, 7);
            this.leakSensor.TabIndex = 5;
            // 
            // CvUnit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.leakSensor);
            this.Controls.Add(this.cvMotorL);
            this.Controls.Add(this.cvMotorR);
            this.Controls.Add(this.glsSensorR);
            this.Controls.Add(this.glsSensorL);
            this.Controls.Add(this.glsDataR);
            this.Controls.Add(this.glsDataL);
            this.Controls.Add(this.unit);
            this.Name = "CvUnit";
            this.Size = new System.Drawing.Size(96, 78);
            this.ResumeLayout(false);

        }

        #endregion

        private Unit unit;
        private GlsData glsDataL;
        private GlsData glsDataR;
        private GlsSensor glsSensorL;
        private GlsSensor glsSensorR;
        private CvMotor cvMotorR;
        private CvMotor cvMotorL;
        private LeakSensor leakSensor;
    }
}
