using OutlookIMExToolsAddIn1.Usecases;
using Microsoft.Office.Interop.Outlook;
using OutlookIMExToolsAddIn1.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace OutlookIMExToolsAddIn1.Forms
{
    public partial class ImForm : Form
    {
        public ImForm(ThunderbirdHelperUsecase thunderbirdHelperUsecase)
        {
            InitializeComponent();
        }
    }
}
