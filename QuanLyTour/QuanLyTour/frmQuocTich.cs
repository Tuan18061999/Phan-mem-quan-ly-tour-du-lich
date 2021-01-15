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
    public partial class frmQuocTich : DevExpress.XtraEditors.XtraForm
    {
        public frmQuocTich()
        {
            InitializeComponent();
        }
        string TrangThai = "";
        private void frmQuocTich_Load(object sender, EventArgs e)
        {
            loadDgvQuocTich();
            TrangThaiBanDau();
        }
        private void loadDgvQuocTich()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstQuocTich = from QT in data.QuocTiches
                                  select new
                                  {
                                      QT.MaQuocGia,
                                      QT.TenQuocGia
                                  };
                dgvQuocTich.DataSource = lstQuocTich;
            }
        }

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            int MaQuocGia = int.Parse(gridView1.GetRowCellValue(e.RowHandle, "MaQuocGia").ToString().Trim());
            txtMaQuocGia.Text = gridView1.GetRowCellValue(e.RowHandle, "MaQuocGia").ToString().Trim();
            txtTenQuocGia.Text = gridView1.GetRowCellValue(e.RowHandle, "TenQuocGia").ToString().Trim();
            loadKhachHangTheoQuocTich(MaQuocGia);
            TrangThaiNhanView(); 
        }
        private void loadKhachHangTheoQuocTich(int maQuocGia)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstKhachhang = from kh in data.KhachHangs
                                   where (kh.MaQuocGia == maQuocGia)
                                   select new
                                   {
                                       kh.TenKhachHang,
                                       kh.GioiTinh,
                                       kh.CMND,
                                       kh.DiaChi,
                                       kh.SDT
                                   };
                dgvKhachHang.DataSource = lstKhachhang;
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
                r = MessageBox.Show("Bạn có muốn xóa quốc tịch này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    try
                    {
                        string maQuocGia = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaQuocGia").ToString();
                        QuocTich qt = data.QuocTiches.Where(t => t.MaQuocGia == int.Parse(maQuocGia)).FirstOrDefault();
                        data.QuocTiches.DeleteOnSubmit(qt);
                        data.SubmitChanges();
                        loadDgvQuocTich();
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
                    QuocTich QuocTich = new QuocTich();
                    QuocTich.TenQuocGia = txtTenQuocGia.Text;
                    data.QuocTiches.InsertOnSubmit(QuocTich);
                    data.SubmitChanges();
                    loadDgvQuocTich();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (TrangThai == "sua")
                {
                    int MaQuocGia = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaQuocGia").ToString());
                    QuocTich QuocTich = data.QuocTiches.Where(t => t.MaQuocGia == MaQuocGia).FirstOrDefault();
                    QuocTich.TenQuocGia = txtTenQuocGia.Text;
                    data.SubmitChanges();
                    loadDgvQuocTich();
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
            txtMaQuocGia.Text = "";
            txtTenQuocGia.Text = "";
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
            dgvKhachHang.Enabled = false;
            dgvQuocTich.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvKhachHang.Enabled = true;
            dgvQuocTich.Enabled = true;
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
            dgvKhachHang.Enabled = true;
            dgvQuocTich.Enabled = true;
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