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
    public partial class frmTinh : DevExpress.XtraEditors.XtraForm
    {
        string trangThai = "";
        public frmTinh()
        {
            InitializeComponent();
        }

        private void frmTinh_Load(object sender, EventArgs e)
        {
            loadDgvTinh();
            TrangThaiBanDau();
        }
        #region LOAD DU LIEU
        private void loadDgvTinh()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstTinh = from t in data.Tinhs
                              select new
                              {
                                  t.MaSoTinh,
                                  t.TenTinh
                              };
                dgvTinh.DataSource = lstTinh;
            }
        }
        #endregion
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtMaTinh.Text = "";
            txtTenTinh.Text = "";
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
            dgvTinh.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        private void btnThem_Click(object sender, EventArgs e)
        {
            trangThai = "them";
            ClearAll();
            TrangThaiNhanThem();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa dịch vụ này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    string maSoTinh = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTinh").ToString();
                    Tinh tinh = data.Tinhs.Where(t => t.MaSoTinh == int.Parse(maSoTinh)).FirstOrDefault();
                    data.Tinhs.DeleteOnSubmit(tinh);
                    data.SubmitChanges();
                    loadDgvTinh();
                }
                TrangThaiBanDau();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            trangThai = "sua";
            TrangThaiNhanSua();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai == "them")
                {
                    Tinh tinh = new Tinh();
                    tinh.TenTinh = txtTenTinh.Text;
                    data.Tinhs.InsertOnSubmit(tinh);
                    data.SubmitChanges();
                    loadDgvTinh();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai == "sua")
                {
                    string maSoTinh = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTinh").ToString();
                    Tinh tinh = data.Tinhs.Where(t => t.MaSoTinh == int.Parse(maSoTinh)).FirstOrDefault();
                    tinh.TenTinh = txtTenTinh.Text;
                    data.SubmitChanges();
                    loadDgvTinh();
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

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            int maTinh = int.Parse(gridView1.GetRowCellValue(e.RowHandle, "MaSoTinh").ToString().Trim());
            txtMaTinh.Text = gridView1.GetRowCellValue(e.RowHandle, "MaSoTinh").ToString().Trim();
            txtTenTinh.Text = gridView1.GetRowCellValue(e.RowHandle, "TenTinh").ToString().Trim();
            loadTourTheoMaTinh(maTinh);
            TrangThaiNhanView();
        }
        private void loadTourTheoMaTinh(int maTinh)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstTour = from tour in data.TOURs
                              where (tour.MaSoTinh == maTinh)
                              select new
                              {
                                  tour.TenTour,
                                  tour.NgayBatDau,
                                  tour.NgayKetThuc
                              };
                dgvTour.DataSource = lstTour;
            }
        }
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvTinh.Enabled = true;
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
            dgvTinh.Enabled = true;
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