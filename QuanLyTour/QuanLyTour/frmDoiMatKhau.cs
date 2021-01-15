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
    public partial class frmDoiMatKhau : DevExpress.XtraEditors.XtraForm
    {
        public frmDoiMatKhau()
        {
            InitializeComponent();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            
        }
        private void XacNhan()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string tenDN = txtTenDN.Text;
                string mkCu = txtMatKhauCu.Text;
                string mkMoi = txtMatKhauMoi.Text;
                string xacNhanMK = txtXacNhanMatKhau.Text;
                TaiKhoan tk = data.TaiKhoans.Where(t => t.TenDangNhap == tenDN && t.MatKhau == mkCu).FirstOrDefault();
                if (tk == null)
                {
                    MessageBox.Show("Tài khoản không tồn tại");
                }
                else if (mkMoi != xacNhanMK)
                {
                    MessageBox.Show("Mật khẩu nhập lại phải giống mật khẩu mới");
                }
                else
                {
                    tk.MatKhau = mkMoi;
                    data.SubmitChanges();
                    MessageBox.Show("Đổi mật khẩu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            XacNhan();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        

        

        
    }
}