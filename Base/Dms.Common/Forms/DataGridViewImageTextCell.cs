using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Dms.Common
{
    public class DataGridViewImageTextCell : DataGridViewImageCell
    {
        private string text;
        private Color textColor = Color.Black;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public DataGridViewImageTextCell()
        {
            base.ImageLayout = DataGridViewImageCellLayout.Normal;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            //Draw Image
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);

            //Draw Text
            SolidBrush brush = new SolidBrush(TextColor);
            if (value != base.DefaultNewRowValue && text != null)
            {
                //graphics.DrawString(this.text, Control.DefaultFont, Brushes.Black, cellBounds.X, cellBounds.Y, format);
                graphics.DrawString(this.text, System.Windows.Forms.Control.DefaultFont, brush, cellBounds.Width / 2 - ((Image)value).Width / 2, cellBounds.Y + 2);
            }
        }
    }
}
