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
    public partial class frmDangKy : DevExpress.XtraEditors.XtraForm
    {
        public frmDangKy()
        {
            InitializeComponent();
        }

        private void frmDangKy_Load(object sender, EventArgs e)
        {
            loadCboNhanVien();
        }
        private void loadCboNhanVien()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                cboNhanVien.Properties.NullText = "Chọn ở đây";
                var lstNhanVien = from nv in data.NhanViens
                                  select new
                                  {
                                      nv.MaSoNhanVien,
                                      nv.TenNhanVien
                                  };
                cboNhanVien.Properties.DataSource = lstNhanVien;
                cboNhanVien.Properties.DisplayMember = "TenNhanVien";
                cboNhanVien.Properties.ValueMember = "MaSoNhanVien";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                TaiKhoan tk = data.TaiKhoans.Where(t => t.TenDangNhap == txtTenDN.Text).FirstOrDefault();
                if (tk != null)
                {
                    MessageBox.Show("Tên tài khoản đã tồn tại vui lòng chọn tài khoản khác");
                    return;
                }
                else if(txtMatKhau.Text!=txtXacNhanMatKhau.Text)
                {
                    MessageBox.Show("Mật khẩu xác nhận phải giống vói mật khẩu chính");
                    return;
                }
                TaiKhoan tkMoi = new TaiKhoan();
                tkMoi.TenDangNhap = txtTenDN.Text;
                tkMoi.MaSoNV = int.Parse(cboNhanVien.EditValue.ToString());
                tkMoi.MatKhau = txtMatKhau.Text;
                tkMoi.MaLoaiTaiKhoan = "2";
                data.TaiKhoans.InsertOnSubmit(tkMoi);
                data.SubmitChanges();
                MessageBox.Show("Đăng ký tài khoản thành công", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn chuyển tới trang đăng nhập", "Điều hướng", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    frmDangNhap fDangNhap = new frmDangNhap();
                    this.Visible = false;
                    fDangNhap.ShowDialog();
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            frmDangNhap fDangNhap = new frmDangNhap();
            fDangNhap.Show();
        }
    }
}