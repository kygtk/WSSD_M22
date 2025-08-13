using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Dms.Control.Hpmj
{
    static class Hpmj
    {
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ViewHpmj());
        }
    }
}