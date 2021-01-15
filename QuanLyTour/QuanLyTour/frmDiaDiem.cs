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
    public partial class frmDiaDiem : DevExpress.XtraEditors.XtraForm
    {
        public frmDiaDiem()
        {
            InitializeComponent();
        }
        string trangThai = "";
        string tenAnh = null;
        private void frmDiaDiem_Load(object sender, EventArgs e)
        {
            loadDgvTour();
            TrangThaiBanDau();
        }
        private void loadDgvTour()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstTour = from tour in data.TOURs
                              join ltour in data.LoaiTours
                              on tour.MaLoaiTour equals ltour.MaLoaiTour
                              join t in data.Tinhs
                              on tour.MaSoTinh equals t.MaSoTinh
                              select new
                              {
                                  tour.MaSoTour,
                                  tour.TenTour,
                                  tour.NgayBatDau,
                                  tour.NgayKetThuc,
                                  tour.MoTaTour,
                                  tour.HinhAnhTour,
                                  tour.Gia,
                                  ltour.TenLoai,
                                  t.TenTinh
                              };
                dgvTour.DataSource = lstTour;
            }
        }
        private void loadDgvDiaDiemTheoMa(int ma)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstDiaDiem = from dd in data.DiaDiemThamQuans
                                 where (dd.MaSoTour == ma)
                                 select new
                                 {
                                     dd.MaSoDiaDiem,
                                     dd.TenDiaDiem,
                                     dd.MoTaDiaDiem,
                                     dd.HinhAnhDiaDiem
                                 };
                dgvDiaDiem.DataSource = lstDiaDiem;
            }
        }

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            string ma = gridView1.GetRowCellValue(e.RowHandle, "MaSoTour").ToString();
            loadDgvDiaDiemTheoMa(int.Parse(ma));
            
        }

        private void gridView2_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            ClearAll();
            try
            {
                string tenAnh = gridView2.GetRowCellValue(e.RowHandle, "HinhAnhDiaDiem").ToString();
                Image image = Image.FromFile("AnhDoAn/" + tenAnh);
                pHinhAnhDiaDiem.Image = resizeImage(image, pHinhAnhDiaDiem.Width, pHinhAnhDiaDiem.Height);
            }
            catch
            {
            }
            txtMaDiaDiem.Text = gridView2.GetRowCellValue(e.RowHandle, "MaSoDiaDiem").ToString();
            txtTenDiaDiem.Text = gridView2.GetRowCellValue(e.RowHandle, "TenDiaDiem").ToString();
            txtMoTaDiaDiem.Text = gridView2.GetRowCellValue(e.RowHandle, "MoTaDiaDiem").ToString();
            TrangThaiNhanView();
           
        }
        public Image resizeImage(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage((Image)b);

            g.DrawImage(img, 0, 0, width, height);
            g.Dispose();

            return (Image)b;
        }
        

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearAll();
            trangThai = "them";
            TrangThaiNhanThem();
            
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                string maDiaDiem = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaSoDiaDiem").ToString();
                DiaDiemThamQuan ddtq = data.DiaDiemThamQuans.Where(t => t.MaSoTour == int.Parse(maTour) && t.MaSoDiaDiem == int.Parse(maDiaDiem)).FirstOrDefault();
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa địa điểm này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    data.DiaDiemThamQuans.DeleteOnSubmit(ddtq);
                    data.SubmitChanges();
                    loadDgvDiaDiemTheoMa(int.Parse(maTour));
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
                    DiaDiemThamQuan ddtq = new DiaDiemThamQuan();
                    ddtq.MaSoTour = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString());
                    ddtq.MaSoDiaDiem = int.Parse(txtMaDiaDiem.Text);
                    ddtq.TenDiaDiem = txtTenDiaDiem.Text;
                    ddtq.MoTaDiaDiem = txtMoTaDiaDiem.Text;
                    try
                    {
                        ddtq.HinhAnhDiaDiem = tenAnh;
                    }
                    catch { }
                    data.DiaDiemThamQuans.InsertOnSubmit(ddtq);
                    data.SubmitChanges();
                    loadDgvDiaDiemTheoMa(int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString()));
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai == "sua")
                {
                    string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                    string maDiaDiem = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaSoDiaDiem").ToString();
                    DiaDiemThamQuan ddtq = data.DiaDiemThamQuans.Where(t => t.MaSoTour == int.Parse(maTour) && t.MaSoDiaDiem == int.Parse(maDiaDiem)).FirstOrDefault();
                    ddtq.MaSoTour = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString());
                    ddtq.MaSoDiaDiem = int.Parse(txtMaDiaDiem.Text);
                    ddtq.TenDiaDiem = txtTenDiaDiem.Text;
                    ddtq.MoTaDiaDiem = txtMoTaDiaDiem.Text;
                    try
                    {
                        ddtq.HinhAnhDiaDiem = tenAnh;
                    }
                    catch { }
                    data.SubmitChanges();
                    loadDgvTour();
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

        private void pHinhAnhDiaDiem_Properties_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (oFile.ShowDialog() == DialogResult.OK)
            {
                string duongDanAnh = oFile.FileName;
                Image image = Image.FromFile(duongDanAnh);
                pHinhAnhDiaDiem.Image = resizeImage(image, pHinhAnhDiaDiem.Width, pHinhAnhDiaDiem.Height);
                tenAnh = System.IO.Path.GetFileName(oFile.FileName);


            }
        }

        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            tenAnh = null;
            txtMaDiaDiem.Text = "";
            txtMoTaDiaDiem.Text = "";
            txtTenDiaDiem.Text = "";
            pHinhAnhDiaDiem.EditValue = null;

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
            dgvTour.Enabled = false;
            dgvDiaDiem.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion

        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvTour.Enabled = true;
            dgvDiaDiem.Enabled = true;
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
            dgvTour.Enabled = true;
            dgvDiaDiem.Enabled = true;
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