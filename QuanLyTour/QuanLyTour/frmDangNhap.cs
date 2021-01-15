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
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string tenDN = txtDangNhap.Text;
                string matKhau = txtMatKhau.Text;
                TaiKhoan tk = data.TaiKhoans.Where(t => t.TenDangNhap == tenDN && t.MatKhau == matKhau).FirstOrDefault();
                if (tk != null)
                {
                    MessageBox.Show("Đăng nhập thành công");
                    moFormMain();
                    truyenDuLieuFormThongTin();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void hyperlinkLabelControl1_Click(object sender, EventArgs e)
        {
            frmDangKy fDangKy = new frmDangKy();
            this.Visible = false;
            fDangKy.ShowDialog();
        }
        private void moFormMain()
        {
            frmMain fMain = new frmMain();
            this.Visible = false;
            fMain.ShowDialog();
        }
        private void truyenDuLieuFormThongTin()
        {
            
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            
        }

        

        
    }
}