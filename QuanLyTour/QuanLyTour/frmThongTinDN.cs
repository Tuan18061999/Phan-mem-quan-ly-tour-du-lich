using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QuanLyTour
{
    public partial class frmThongTinDN : DevExpress.XtraEditors.XtraForm
    {
        public frmThongTinDN()
        {
            InitializeComponent();
        }

        private void frmThongTinDN_Load(object sender, EventArgs e)
        {

        }
        string strNhan;
        public frmThongTinDN(string giaTriNhan)
            : this()
        {
            strNhan = giaTriNhan;
            txtTen.Text = strNhan;
        }
    }
}