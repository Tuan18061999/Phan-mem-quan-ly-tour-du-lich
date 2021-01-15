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
    public partial class frmDichVu : DevExpress.XtraEditors.XtraForm
    {
        string trangThai = "";
        public frmDichVu()
        {
            InitializeComponent();
        }

        private void frmDichVu_Load(object sender, EventArgs e)
        {
            loadDgvDichVu();
            TrangThaiBanDau();
        }
        private void loadDgvDichVu()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstDichVu = from dv in data.DichVus
                                select new
                                {
                                    dv.MaDichVu,
                                    dv.TenDichVu
                                };
                dgvDichVu.DataSource = lstDichVu;
            }
        }

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
                    string maDV = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDichVu").ToString();
                    DichVu dv = data.DichVus.Where(t => t.MaDichVu == int.Parse(maDV)).FirstOrDefault();
                    data.DichVus.DeleteOnSubmit(dv);
                    data.SubmitChanges();
                    loadDgvDichVu();
                }
            }
            TrangThaiBanDau();
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
                    DichVu dv = new DichVu();
                    dv.TenDichVu = txtTenDichVu.Text;
                    data.DichVus.InsertOnSubmit(dv);
                    data.SubmitChanges();
                    loadDgvDichVu();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai == "sua")
                {
                    string maDV = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDichVu").ToString();
                    DichVu dv = data.DichVus.Where(t => t.MaDichVu == int.Parse(maDV)).FirstOrDefault();
                    dv.TenDichVu = txtTenDichVu.Text;
                    data.SubmitChanges();
                    loadDgvDichVu();
                    ClearAll();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                TrangThaiBanDau();
            }
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
            txtMaDichVu.Text = "";
            txtTenDichVu.Text = "";
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
            dgvDichVu.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            txtMaDichVu.Text = gridView1.GetRowCellValue(e.RowHandle, "MaDichVu").ToString().Trim();
            txtTenDichVu.Text = gridView1.GetRowCellValue(e.RowHandle, "TenDichVu").ToString().Trim();
            TrangThaiNhanView();
        }
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvDichVu.Enabled = true;
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
            dgvDichVu.Enabled = true;
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