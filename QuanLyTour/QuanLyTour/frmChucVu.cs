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
    public partial class frmChucVu : DevExpress.XtraEditors.XtraForm
    {
        string TrangThai = "";
        public frmChucVu()
        {
            InitializeComponent();
        }
        private void frmChucVu_Load(object sender, EventArgs e)
        {
            loadDgvChucVu();
            TrangThaiBanDau();
        }
        private void loadDgvChucVu()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstChucVu = from CV in data.ChucVus
                                  select new
                                  {
                                      CV.MaSoChucVu,
                                      CV.TenChucVu
                                  };
                dgvChucVu.DataSource = lstChucVu;
            }
        }

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            int MaSoChucVu = int.Parse(gridView1.GetRowCellValue(e.RowHandle, "MaSoChucVu").ToString().Trim());
            txtMaChucVu.Text = gridView1.GetRowCellValue(e.RowHandle, "MaSoChucVu").ToString().Trim();
            txtTenChucVu.Text = gridView1.GetRowCellValue(e.RowHandle, "TenChucVu").ToString().Trim();
            loadNhanVienTheoChucVu(MaSoChucVu);
            TrangThaiNhanView();
        }
        private void loadNhanVienTheoChucVu(int maChucVu)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstNhanVien = from nv in data.NhanViens
                                   where (nv.MaSoChucVu == maChucVu)
                                   select new
                                   {
                                       nv.TenNhanVien,
                                       nv.NgaySinh,
                                       nv.GioiTinh,
                                       nv.DiaChi,
                                       nv.SoDienThoai
                                   };
                dgvNhanVien.DataSource = lstNhanVien;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            TrangThai = "them";
            ClearAll();
            TrangThaiNhanThem();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa chức vụ này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    try
                    {
                        string maChucVu = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoChucVu").ToString();
                        ChucVu cv = data.ChucVus.Where(t => t.MaSoChucVu == int.Parse(maChucVu)).FirstOrDefault();
                        data.ChucVus.DeleteOnSubmit(cv);
                        data.SubmitChanges();
                        loadDgvChucVu();
                    }
                    catch
                    {
                        MessageBox.Show("Bạn không thể xóa trường dữ liệu này vì có dữ liệu liên quan", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            TrangThaiBanDau();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TrangThai = "sua";
            TrangThaiNhanSua();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (TrangThai == "them")
                {
                    ChucVu ChucVu = new ChucVu();
                    ChucVu.TenChucVu = txtTenChucVu.Text;
                    data.ChucVus.InsertOnSubmit(ChucVu);
                    data.SubmitChanges();
                    loadDgvChucVu();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (TrangThai == "sua")
                {
                    int MaChucVu = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoChucVu").ToString());
                    ChucVu ChucVu = data.ChucVus.Where(t => t.MaSoChucVu == MaChucVu).FirstOrDefault();
                    ChucVu.TenChucVu = txtTenChucVu.Text;
                    data.SubmitChanges();
                    loadDgvChucVu();
                    ClearAll();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            TrangThaiBanDau();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearAll();
            TrangThaiBanDau();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn muốn đóng chức năng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                this.Close();
        }
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtMaChucVu.Text = "";
            txtTenChucVu.Text = "";
        }


        private void EnableAll()
        {
            foreach (Control ctr in panelText.Controls)
            {
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit) || ctr.GetType() == typeof(SimpleButton))
                {
                    ctr.Enabled = false;
                }
            }
            foreach (Control ctr in panelButton.Controls)
            {
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit) || ctr.GetType() == typeof(SimpleButton))
                {
                    ctr.Enabled = false;
                }
            }
            dgvNhanVien.Enabled = false;
            dgvChucVu.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvNhanVien.Enabled = true;
            dgvChucVu.Enabled = true;
            btnThem.Enabled = true;
        }
        private void TrangThaiNhanThem()
        {
            EnableAll();
            foreach (Control ctr in panelText.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit))
                    ctr.Enabled = true;
            btnLuu.Enabled = true;
        }
        private void TrangThaiNhanView()
        {
            EnableAll();
            dgvNhanVien.Enabled = true;
            dgvChucVu.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }
        private void TrangThaiNhanSua()
        {
            EnableAll();
            btnLuu.Enabled = true;
            foreach (Control ctr in panelText.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit))
                    ctr.Enabled = true;
        }
        #endregion       
    }
}