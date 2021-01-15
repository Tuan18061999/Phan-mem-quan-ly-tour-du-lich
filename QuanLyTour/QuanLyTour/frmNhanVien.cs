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
    public partial class frmNhanVien : DevExpress.XtraEditors.XtraForm
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        string trangThai = "";
        public frmNhanVien()
        {
            InitializeComponent();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            loadDgvNhanVien();
            loadCboChucVu();
            loadCboGioiTinh();
            TrangThaiBanDau();
        }
        private void loadDgvNhanVien()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstNhanVien1 = from nv in data.NhanViens
                                    join cv in data.ChucVus
                                    on nv.MaSoChucVu equals cv.MaSoChucVu
                                    select new
                                    {
                                        nv.MaSoNhanVien,
                                        nv.TenNhanVien,                                        
                                        nv.NgaySinh,
                                        nv.GioiTinh,
                                        nv.DiaChi,
                                        nv.SoDienThoai,
                                        cv.TenChucVu
                                    };
                dgvNhanVien.DataSource = lstNhanVien1;
            }
        }
        private void loadCboChucVu()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                List<ChucVu> cv = data.ChucVus.ToList();
                cboChucVu.Properties.DataSource = cv;
                cboChucVu.Properties.DisplayMember = "TenChucVu";
                cboChucVu.Properties.ValueMember = "MaSoChucVu";
                cboChucVu.Properties.NullText = "";
            }
        }
        private void loadCboGioiTinh()
        {
            List<String> lstGioiTinh = new List<string> { "Nam", "Nu" };
            cboGioiTinh.Properties.DataSource = lstGioiTinh;
            cboGioiTinh.Properties.NullText = "";
        }

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            txtTenNhanVien.Text = gridView1.GetRowCellValue(e.RowHandle, "TenNhanVien").ToString().Trim();
            txtNgaySinh.EditValue = DateTime.Parse(gridView1.GetRowCellValue(e.RowHandle, "NgaySinh").ToString().Trim());
            txtDiaChi.Text = gridView1.GetRowCellValue(e.RowHandle, "DiaChi").ToString().Trim();
            txtSDT.Text = gridView1.GetRowCellValue(e.RowHandle, "SoDienThoai").ToString().Trim();
            cboChucVu.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "TenChucVu").ToString().Trim();
            cboGioiTinh.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "GioiTinh").ToString().Trim();
            TrangThaiNhanView();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn muốn đóng chức năng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            TrangThaiBanDau();
            ClearAll();
        }
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtTenNhanVien.Text = "";
            txtNgaySinh.EditValue = null;
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            cboChucVu.Properties.NullText = "";
            cboChucVu.EditValue = null;
            cboGioiTinh.Properties.NullText = "";
            cboGioiTinh.EditValue = null;
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
            txtNgaySinh.Enabled = false;
            dgvNhanVien.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearAll();
            trangThai = "them";
            TrangThaiNhanThem();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            trangThai = "sua";
            cboGioiTinh.EditValue = null;
            cboGioiTinh.Properties.NullText = "";
            cboChucVu.EditValue = null;
            cboChucVu.Properties.NullText = "";
            TrangThaiNhanSua();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {            
            string maNV = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoNhanVien").ToString();
            NhanVien nv = data.NhanViens.Where(t => t.MaSoNhanVien == int.Parse(maNV)).FirstOrDefault();
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn xóa khách hàng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                data.NhanViens.DeleteOnSubmit(nv);
                data.SubmitChanges();
                loadDgvNhanVien();
            }
            TrangThaiBanDau();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (trangThai == "them")
            {
                NhanVien nv = new NhanVien();
                nv.TenNhanVien = txtTenNhanVien.Text;
                nv.NgaySinh = DateTime.Parse(txtNgaySinh.EditValue.ToString());
                nv.DiaChi = txtDiaChi.Text;
                nv.SoDienThoai = txtSDT.Text;
                nv.GioiTinh = cboGioiTinh.EditValue.ToString();
                nv.MaSoChucVu = int.Parse(cboChucVu.EditValue.ToString());
                data.NhanViens.InsertOnSubmit(nv);
                data.SubmitChanges();
                loadDgvNhanVien();
                ClearAll();
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (trangThai == "sua")
            {
                string maNV = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoNhanVien").ToString();
                NhanVien nv = data.NhanViens.Where(t => t.MaSoNhanVien == int.Parse(maNV)).FirstOrDefault();
                nv.TenNhanVien = txtTenNhanVien.Text;
                nv.NgaySinh = DateTime.Parse(txtNgaySinh.EditValue.ToString());
                nv.DiaChi = txtDiaChi.Text;
                nv.SoDienThoai = txtSDT.Text;
                nv.GioiTinh = cboGioiTinh.EditValue.ToString();
                nv.MaSoChucVu = int.Parse(cboChucVu.EditValue.ToString());
                data.SubmitChanges();
                loadDgvNhanVien();
                ClearAll();
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            TrangThaiBanDau();
        }
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvNhanVien.Enabled = true;
            btnThem.Enabled = true;
        }
        private void TrangThaiNhanThem()
        {
            EnableAll();
            foreach (Control ctr in panelText.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit))
                    ctr.Enabled = true;
            txtNgaySinh.Enabled = true;
            btnLuu.Enabled = true;
        }
        private void TrangThaiNhanView()
        {
            EnableAll();
            dgvNhanVien.Enabled = true;
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
            txtNgaySinh.Enabled = true;
            cboChucVu.Properties.NullText = "";
            cboChucVu.EditValue = null;
            cboGioiTinh.Properties.NullText = "";
            cboGioiTinh.EditValue = null;
        }
        #endregion
    }
}