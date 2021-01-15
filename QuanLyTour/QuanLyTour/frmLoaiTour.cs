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
    public partial class frmLoaiTour : DevExpress.XtraEditors.XtraForm
    {
        string trangThai = "";
        public frmLoaiTour()
        {
            InitializeComponent();
        }

        private void frmNhomTour_Load(object sender, EventArgs e)
        {
            loadDgvLoaiTour();
            TrangThaiBanDau();
        }
        private void loadDgvLoaiTour()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstLoaiTour = from loaiTour in data.LoaiTours
                                  select new
                                  {
                                      loaiTour.MaLoaiTour,
                                      loaiTour.TenLoai
                                  };
                dgvLoaiTour.DataSource = lstLoaiTour;
            }
        }
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtMaLoaiTour.Text = "";
            txtTenLoaiTour.Text = "";
            txtSoLuong.Text = "";
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
            dgvLoaiTour.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion
        private int DemSoTour(int maLoai)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                int tongSoTour = data.TOURs.Where(t => t.MaLoaiTour == maLoai).Count();
                return tongSoTour;
            }
        }
        private void loadTourTheoMaLoai(int maLoai)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstTour = from tour in data.TOURs
                              where (tour.MaLoaiTour == maLoai)
                              select new
                              {
                                  tour.TenTour,
                                  tour.NgayBatDau,
                                  tour.NgayKetThuc
                              };
                dgvTour.DataSource = lstTour;
            }
        }
        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            int maLoai = int.Parse(gridView1.GetRowCellValue(e.RowHandle, "MaLoaiTour").ToString().Trim());
            txtMaLoaiTour.Text = gridView1.GetRowCellValue(e.RowHandle, "MaLoaiTour").ToString().Trim();
            txtTenLoaiTour.Text = gridView1.GetRowCellValue(e.RowHandle, "TenLoai").ToString().Trim();
            txtSoLuong.Text = Convert.ToString(DemSoTour(maLoai));
            loadTourTheoMaLoai(maLoai);
            TrangThaiNhanView();
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
                r = MessageBox.Show("Bạn có muốn xóa loại tour này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    try
                    {
                        string maLoaiTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaLoaiTour").ToString();
                        LoaiTour loaiTour = data.LoaiTours.Where(t => t.MaLoaiTour == int.Parse(maLoaiTour)).FirstOrDefault();
                        data.LoaiTours.DeleteOnSubmit(loaiTour);
                        data.SubmitChanges();
                        loadDgvLoaiTour();
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
            trangThai = "sua";
            TrangThaiNhanSua();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai == "them")
                {
                    LoaiTour loaiTour = new LoaiTour();
                    loaiTour.TenLoai = txtTenLoaiTour.Text;
                    data.LoaiTours.InsertOnSubmit(loaiTour);
                    data.SubmitChanges();
                    loadDgvLoaiTour();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai == "sua")
                {
                    string maLoaiTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaLoaiTour").ToString();
                    LoaiTour loaiTour = data.LoaiTours.Where(t => t.MaLoaiTour == int.Parse(maLoaiTour)).FirstOrDefault();
                    loaiTour.TenLoai = txtTenLoaiTour.Text;
                    data.SubmitChanges();
                    loadDgvLoaiTour();
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
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvLoaiTour.Enabled = true;
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
            dgvLoaiTour.Enabled = true;
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