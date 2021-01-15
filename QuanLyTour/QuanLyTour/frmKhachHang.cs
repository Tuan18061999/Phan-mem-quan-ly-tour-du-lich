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
    public partial class frmKhachHang : DevExpress.XtraEditors.XtraForm
    {
        DataClasses1DataContext data = new DataClasses1DataContext();
        string trangThai = "";
        public frmKhachHang()
        {
            InitializeComponent();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            loadDgvKhachHang();
            loadCboQuocTich();
            loadCboGioiTinh();
            TrangThaiBanDau();
        }
        #region LOAD DU LIEU
        private void loadDgvKhachHang()
        {
            var lstKhachHang1 = from kh in data.KhachHangs
                                join qt in data.QuocTiches
                                on kh.MaQuocGia equals qt.MaQuocGia
                                select new
                                {
                                    kh.MaSoKhachHang,
                                    kh.TenKhachHang,
                                    kh.GioiTinh,
                                    kh.CMND,
                                    kh.DiaChi,
                                    kh.SDT,
                                    qt.TenQuocGia
                                };
            dgvKhachHang.DataSource = lstKhachHang1;
        }
        private void loadCboQuocTich()
        {
            List<QuocTich> qt = data.QuocTiches.ToList();
            cboQuocTich.Properties.DataSource = qt;
            cboQuocTich.Properties.DisplayMember = "TenQuocGia";
            cboQuocTich.Properties.ValueMember = "MaQuocGia";
            cboQuocTich.Properties.NullText = "";
        }
        private void loadCboGioiTinh()
        {
            List<String> lstGioiTinh = new List<string> { "Nam", "Nu" };
            cboGioiTinh.Properties.DataSource = lstGioiTinh;
            cboGioiTinh.Properties.NullText = "";
        }
        #endregion

        #region EVENT
        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            TrangThaiNhanView();
            txtTenKhachHang.Text = gridView1.GetRowCellValue(e.RowHandle, "TenKhachHang").ToString().Trim();
            txtCMND.Text = gridView1.GetRowCellValue(e.RowHandle, "CMND").ToString().Trim();
            txtDiaChi.Text = gridView1.GetRowCellValue(e.RowHandle, "DiaChi").ToString().Trim();
            txtSDT.Text = gridView1.GetRowCellValue(e.RowHandle, "SDT").ToString().Trim();
            cboQuocTich.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "TenQuocGia").ToString().Trim();
            cboGioiTinh.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "GioiTinh").ToString().Trim();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearAll();
            trangThai = "them";
            TrangThaiNhanThem();
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (trangThai == "them")
            {
                KhachHang kh = new KhachHang();
                kh.TenKhachHang = txtTenKhachHang.Text;
                kh.CMND = txtCMND.Text;
                kh.DiaChi = txtDiaChi.Text;
                kh.SDT = txtSDT.Text;
                kh.GioiTinh = cboGioiTinh.EditValue.ToString();
                kh.MaQuocGia = int.Parse(cboQuocTich.EditValue.ToString());
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                loadDgvKhachHang();
                ClearAll();
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            if (trangThai == "sua")
            {
                string maKH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoKhachHang").ToString();
                KhachHang kh = data.KhachHangs.Where(t => t.MaSoKhachHang == int.Parse(maKH)).FirstOrDefault();
                kh.TenKhachHang = txtTenKhachHang.Text;
                kh.CMND = txtCMND.Text;
                kh.DiaChi = txtDiaChi.Text;
                kh.SDT = txtSDT.Text;
                kh.GioiTinh = cboGioiTinh.EditValue.ToString();
                kh.MaQuocGia = int.Parse(cboQuocTich.EditValue.ToString());
                data.SubmitChanges();
                loadDgvKhachHang();
                ClearAll();
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            TrangThaiBanDau();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            TrangThaiBanDau();
            string maKH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoKhachHang").ToString();
            KhachHang kh = data.KhachHangs.Where(t => t.MaSoKhachHang == int.Parse(maKH)).FirstOrDefault();
            DialogResult r;
            r = MessageBox.Show("Bạn có muốn xóa khách hàng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                data.KhachHangs.DeleteOnSubmit(kh);
                data.SubmitChanges();
                loadDgvKhachHang();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            TrangThaiNhanSua();
            trangThai = "sua";

            
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
        #endregion

        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtTenKhachHang.Text = "";
            txtCMND.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            cboQuocTich.Properties.NullText = "";
            cboQuocTich.EditValue = null;
            cboGioiTinh.Properties.NullText = "";
            cboGioiTinh.EditValue = null;
        }


        private void EnableAll()
        {
            foreach (Control ctr in panelKhachHang.Controls)
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
            dgvKhachHang.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvKhachHang.Enabled = true;
            btnThem.Enabled = true;
        }
        private void TrangThaiNhanThem()
        {
            EnableAll();
            foreach (Control ctr in panelKhachHang.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit))
                    ctr.Enabled = true;
            btnLuu.Enabled = true;
        }
        private void TrangThaiNhanView()
        {
            EnableAll();
            dgvKhachHang.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }
        private void TrangThaiNhanSua()
        {
            EnableAll();
            btnLuu.Enabled = true;
            foreach (Control ctr in panelKhachHang.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit))
                    ctr.Enabled = true;
            cboQuocTich.Properties.NullText = "";
            cboQuocTich.EditValue = null;
            cboGioiTinh.Properties.NullText = "";
            cboGioiTinh.EditValue = null;
        }
        #endregion

    }
}