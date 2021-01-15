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
    public partial class frmPhuongTien : DevExpress.XtraEditors.XtraForm
    {
        public frmPhuongTien()
        {
            InitializeComponent();
        }

        DataClasses1DataContext data = new DataClasses1DataContext();
        string trangThai = "";

        private void frmPhuongTien_Load(object sender, EventArgs e)
        {
            loadDgvPhuongTien();
            loadCboLoaiPhuongTien();
            TrangThaiBanDau();
        }

        private void loadDgvPhuongTien()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstPhuongTien = from pt in data.PHUONGTIENs
                                    select new
                                    {
                                        pt.MaSoPhuongTien,
                                        pt.TenPhuongTien,
                                        pt.LoaiPhuongTien
                                    };
                dgvPhuongTien.DataSource = lstPhuongTien;
            }
        }
        private void loadCboLoaiPhuongTien()
        {
            List<String> lstLoaiPT = new List<string> { "Đường thủy", "Đường bộ", "Hàng không" };
            cboLoaiPhuongTien.Properties.DataSource = lstLoaiPT;
            cboLoaiPhuongTien.Properties.NullText = "";
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
                r = MessageBox.Show("Bạn có muốn xóa phương tiện này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    string maPT = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoPhuongTien").ToString();
                    PHUONGTIEN pt = data.PHUONGTIENs.Where(t => t.MaSoPhuongTien == int.Parse(maPT)).FirstOrDefault();
                    data.PHUONGTIENs.DeleteOnSubmit(pt);
                    data.SubmitChanges();
                    loadDgvPhuongTien();
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
                    PHUONGTIEN pt = new PHUONGTIEN();
                    pt.TenPhuongTien = txtTenPhuongTien.Text;
                    pt.LoaiPhuongTien = cboLoaiPhuongTien.EditValue.ToString();
                    data.PHUONGTIENs.InsertOnSubmit(pt);
                    data.SubmitChanges();
                    loadDgvPhuongTien();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai == "sua")
                {
                    string maPT = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoPhuongTien").ToString();
                    PHUONGTIEN pt = data.PHUONGTIENs.Where(t => t.MaSoPhuongTien == int.Parse(maPT)).FirstOrDefault();
                    pt.TenPhuongTien = txtTenPhuongTien.Text;
                    pt.LoaiPhuongTien = cboLoaiPhuongTien.EditValue.ToString();
                    data.SubmitChanges();
                    loadDgvPhuongTien();
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

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            txtMaPhuongTien.Text = gridView1.GetRowCellValue(e.RowHandle, "MaSoPhuongTien").ToString().Trim();
            txtTenPhuongTien.Text = gridView1.GetRowCellValue(e.RowHandle, "TenPhuongTien").ToString().Trim();
            cboLoaiPhuongTien.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "LoaiPhuongTien").ToString().Trim();
            TrangThaiNhanView();
        }
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtMaPhuongTien.Text = "";
            txtTenPhuongTien.Text = "";
            cboLoaiPhuongTien.Properties.NullText = "";
            cboLoaiPhuongTien.EditValue = null;
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
            dgvPhuongTien.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvPhuongTien.Enabled = true;
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
            dgvPhuongTien.Enabled = true;
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