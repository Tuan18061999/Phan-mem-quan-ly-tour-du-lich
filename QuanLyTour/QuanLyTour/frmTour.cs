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
using System.Drawing.Imaging;

namespace QuanLyTour
{
    public partial class frmTour : DevExpress.XtraEditors.XtraForm
    {
        public frmTour()
        {
            InitializeComponent();
        }
        string trangThai1="";
        string trangThai2="";
        string trangThai3="";
        string tenAnh = "";
        private void frmTour_Load(object sender, EventArgs e)
        {
            loadDgvTour();
            loadLoaiTour();
            loadTinh();
            loadCboDichVu();
            loadCboDuocHuong();
            loadCboPhuongTien();
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
        private void loadLoaiTour()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                List<LoaiTour> lstLoaiTour = data.LoaiTours.ToList();
                cboLoaiTour.Properties.DataSource = lstLoaiTour;
                cboLoaiTour.Properties.DisplayMember = "TenLoai";
                cboLoaiTour.Properties.ValueMember = "MaLoaiTour";
                cboLoaiTour.Properties.NullText = "Chọn ở đây";
            }
        }
        private void loadTinh()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                List<Tinh> lstTinh = data.Tinhs.ToList();
                cboTinh.Properties.DataSource = lstTinh;
                cboTinh.Properties.DisplayMember = "TenTinh";
                cboTinh.Properties.ValueMember = "MaSoTinh";
                cboTinh.Properties.NullText = "Chọn ở đây";
            }
        }
        private void loadDgvDichVuTheoMa(int maTour)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstDichVu = from dv in data.DichVus
                                join ctdv in data.ChiTietDichVus
                                on dv.MaDichVu equals ctdv.MaDichVu
                                where ctdv.MaSoTour==maTour
                                select new
                                {
                                    dv.MaDichVu,
                                    dv.TenDichVu,
                                    ctdv.DuocHuong
                                };
                dgvDichVu.DataSource = lstDichVu;
            }
        }
        private void loadDgvPhuongTienTheoMa(int maTour)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstPhuongTien = from pt in data.PHUONGTIENs
                                    join ctpt in data.ChiTietPhuongTiens
                                    on pt.MaSoPhuongTien equals ctpt.MaSoPhuongTien
                                    where ctpt.MaSoTour == maTour
                                    select new
                                    {
                                        pt.MaSoPhuongTien,
                                        pt.TenPhuongTien
                                    };
                dgvPhuongTien.DataSource = lstPhuongTien;
            }
        }
        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            string maTour = gridView1.GetRowCellValue(e.RowHandle, "MaSoTour").ToString();
            string tenAnh = gridView1.GetRowCellValue(e.RowHandle, "HinhAnhTour").ToString();
            txtTenTour.Text = gridView1.GetRowCellValue(e.RowHandle, "TenTour").ToString();
            txtNgayBatDau.EditValue = DateTime.Parse(gridView1.GetRowCellValue(e.RowHandle, "NgayBatDau").ToString());
            txtNgayKetThuc.EditValue = DateTime.Parse(gridView1.GetRowCellValue(e.RowHandle, "NgayKetThuc").ToString());
            txtMoTa.Text = gridView1.GetRowCellValue(e.RowHandle, "MoTaTour").ToString();
            txtGia.Text = gridView1.GetRowCellValue(e.RowHandle, "Gia").ToString();
            cboLoaiTour.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "TenLoai").ToString();
            cboTinh.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "TenTinh").ToString();
            Image image = Image.FromFile("AnhDoAn/" + tenAnh);
            pHinhAnhTour.Image = resizeImage(image, pHinhAnhTour.Width, pHinhAnhTour.Height);
            loadDgvDichVuTheoMa(int.Parse(maTour));
            loadDgvPhuongTienTheoMa(int.Parse(maTour));
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

        private void gridView2_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            cboDichVu.EditValue = null;
            cboDuocHuong.EditValue = null;
            cboDichVu.Properties.NullText = gridView2.GetRowCellValue(e.RowHandle, "TenDichVu").ToString();
            string duocHuong = gridView2.GetRowCellValue(e.RowHandle, "DuocHuong").ToString();
            if (int.Parse(duocHuong) == 1)
                cboDuocHuong.Properties.NullText = "Có";
            else
                cboDuocHuong.Properties.NullText = "Không";
        }
        private void loadCboDichVu()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                List<DichVu> lstDichVu = data.DichVus.ToList();
                cboDichVu.Properties.DataSource = lstDichVu;
                cboDichVu.Properties.DisplayMember = "TenDichVu";
                cboDichVu.Properties.ValueMember = "MaDichVu";
                cboDichVu.Properties.NullText = "Chọn ở đây";
            }
        }
        private void loadCboDuocHuong()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                List<String> lstDuocHuong = new List<string> { "Có", "Không" };
                cboDuocHuong.Properties.DataSource = lstDuocHuong;
                cboDuocHuong.Properties.NullText = "Chọn ở đây";
            }
        }
        private void loadCboPhuongTien()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                
                List<PHUONGTIEN> lstPhuongTien = data.PHUONGTIENs.ToList();
                cboPhuongTien.Properties.DataSource = lstPhuongTien;
                cboPhuongTien.Properties.DisplayMember = "TenPhuongTien";
                cboPhuongTien.Properties.ValueMember = "MaSoPhuongTien";
                cboPhuongTien.Properties.NullText = "Chọn ở đây";
            }
        }

        private void gridView3_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            cboPhuongTien.EditValue = null;
            cboPhuongTien.Properties.NullText = gridView3.GetRowCellValue(e.RowHandle, "TenPhuongTien").ToString();
        }
        private void ClearAllDichVu()
        {
            cboDichVu.Properties.NullText = "";
            cboDichVu.EditValue = null;
            cboDuocHuong.Properties.NullText = "";
            cboDuocHuong.EditValue = null;
        }

        private void btnThemDV_Click(object sender, EventArgs e)
        {
            ClearAllDichVu();
            trangThai2 = "them";
        }

        private void btnXoaDichVu_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                string maDV = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaDichVu").ToString();
                ChiTietDichVu ctdv = data.ChiTietDichVus.Where(t => t.MaSoTour == int.Parse(maTour) && t.MaDichVu == int.Parse(maDV)).FirstOrDefault();
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa dịch vụ này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    data.ChiTietDichVus.DeleteOnSubmit(ctdv);
                    data.SubmitChanges();
                    loadDgvDichVuTheoMa(int.Parse(maTour));
                }
            }
        }

        private void btnSuaDV_Click(object sender, EventArgs e)
        {
            ClearAllDichVu();
            trangThai2 = "sua";
        }

        private void btnLuuDV_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai2 == "them")
                {
                    ChiTietDichVu ctdv = new ChiTietDichVu();
                    string maTour=gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                    ctdv.MaSoTour=int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString());
                    ctdv.MaDichVu = int.Parse(cboDichVu.EditValue.ToString());
                    string duocHuong = cboDuocHuong.EditValue.ToString();
                    if (duocHuong == "Có")
                        ctdv.DuocHuong = 1;
                    else
                        ctdv.DuocHuong = 0;
                    data.ChiTietDichVus.InsertOnSubmit(ctdv);
                    data.SubmitChanges();
                    loadDgvDichVuTheoMa(int.Parse(maTour));
                    ClearAllDichVu();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai2 == "sua")
                {
                    string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                    string maDV = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaDichVu").ToString();
                    ChiTietDichVu ctdv = data.ChiTietDichVus.Where(t => t.MaSoTour == int.Parse(maTour) && t.MaDichVu == int.Parse(maDV)).FirstOrDefault();
                    string duocHuong = cboDuocHuong.EditValue.ToString();
                    if (duocHuong == "Có")
                        ctdv.DuocHuong = 1;
                    else
                        ctdv.DuocHuong = 0;
                    data.SubmitChanges();
                    loadDgvDichVuTheoMa(int.Parse(maTour));
                    ClearAllDichVu();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            
        }

        private void btnThemPT_Click(object sender, EventArgs e)
        {
            ClearAllPhuongTien();
            trangThai3 = "them";
        }

        private void btnXoaPT_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                string maPT = gridView3.GetRowCellValue(gridView3.FocusedRowHandle, "MaSoPhuongTien").ToString();
                ChiTietPhuongTien ctpt = data.ChiTietPhuongTiens.Where(t => t.MaSoTour == int.Parse(maTour) && t.MaSoPhuongTien == int.Parse(maPT)).FirstOrDefault();
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa phương tiện này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    data.ChiTietPhuongTiens.DeleteOnSubmit(ctpt);
                    data.SubmitChanges();
                    loadDgvPhuongTienTheoMa(int.Parse(maTour));
                }
            }
        }

        private void btnLuuPT_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai3 == "them")
                {
                    ChiTietPhuongTien ctpt = new ChiTietPhuongTien();
                    string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                    ctpt.MaSoTour = int.Parse(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString());
                    ctpt.MaSoPhuongTien = int.Parse(cboPhuongTien.EditValue.ToString());
                    data.ChiTietPhuongTiens.InsertOnSubmit(ctpt);
                    data.SubmitChanges();
                    loadDgvPhuongTienTheoMa(int.Parse(maTour));
                    ClearAllDichVu();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        private void ClearAllPhuongTien()
        {
            cboPhuongTien.Properties.NullText = "";
            cboPhuongTien.EditValue = null;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            ClearAll();
            trangThai1 = "them";
            TrangThaiNhanThem();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                TOUR tour = data.TOURs.Where(t => t.MaSoTour == int.Parse(maTour)).FirstOrDefault();
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn tour này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    data.TOURs.DeleteOnSubmit(tour);
                    data.SubmitChanges();
                    loadDgvTour();
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            trangThai1 = "sua";
            TrangThaiNhanSua();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai1 == "them")
                {
                    TOUR tour = new TOUR();
                    tour.TenTour = txtTenTour.Text;
                    tour.NgayBatDau = DateTime.Parse(txtNgayBatDau.EditValue.ToString());
                    tour.NgayKetThuc = DateTime.Parse(txtNgayKetThuc.EditValue.ToString());
                    tour.MoTaTour = txtMoTa.Text;
                    tour.HinhAnhTour = tenAnh;
                    tour.Gia = int.Parse(txtGia.Text);
                    tour.MaLoaiTour = int.Parse(cboLoaiTour.EditValue.ToString());
                    tour.MaSoTinh = int.Parse(cboTinh.EditValue.ToString());
                    data.TOURs.InsertOnSubmit(tour);
                    data.SubmitChanges();
                    loadDgvTour();
                    ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai1 == "sua")
                {
                    string maTour = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaSoTour").ToString();
                    TOUR tour = data.TOURs.Where(t => t.MaSoTour == int.Parse(maTour)).FirstOrDefault();
                    tour.TenTour = txtTenTour.Text;
                    tour.NgayBatDau = DateTime.Parse(txtNgayBatDau.EditValue.ToString());
                    tour.NgayKetThuc = DateTime.Parse(txtNgayKetThuc.EditValue.ToString());
                    tour.MoTaTour = txtMoTa.Text;
                    //tour.HinhAnhTour = tenAnh;
                    tour.Gia = int.Parse(txtGia.Text);
                    tour.MaLoaiTour = int.Parse(cboLoaiTour.EditValue.ToString());
                    tour.MaSoTinh = int.Parse(cboTinh.EditValue.ToString());
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
        

        private void pHinhAnhTour_Properties_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog oFile = new OpenFileDialog();
            oFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
            if (oFile.ShowDialog() == DialogResult.OK)
            {
                string duongDanAnh = oFile.FileName;
                Image image = Image.FromFile(duongDanAnh);
                pHinhAnhTour.Image = resizeImage(image, pHinhAnhTour.Width, pHinhAnhTour.Height);
                tenAnh = System.IO.Path.GetFileName(oFile.FileName);
                
                
            }
        }
        #region HAM DUNG CHUNG
        private void ClearAll()
        {
            txtTenTour.Text = "";
            txtNgayBatDau.EditValue = null;
            txtNgayKetThuc.EditValue = null;
            txtMoTa.Text = "";
            pHinhAnhTour.Image = null;
            txtGia.Text = "";
            cboLoaiTour.Properties.NullText = "";
            cboLoaiTour.EditValue = null;
            cboTinh.Properties.NullText = "";
            cboTinh.EditValue = null;
            cboDichVu.Properties.NullText = "";
            cboDichVu.EditValue = null;
            cboDuocHuong.Properties.NullText = "";
            cboDuocHuong.EditValue = null;
            cboPhuongTien.Properties.NullText = "";
            cboPhuongTien.EditValue = null;
        }


        private void EnableAll()
        {
            foreach (Control ctr in panelText.Controls)
            {
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit) || ctr.GetType() == typeof(DateEdit))
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
            dgvDichVu.Enabled = false;
            dgvPhuongTien.Enabled = false;
            btnDong.Enabled = true;
            btnReset.Enabled = true;
        }
        #endregion
        #region TRANG THAI
        private void TrangThaiBanDau()
        {
            EnableAll();
            dgvTour.Enabled = true;
            btnThem.Enabled = true;
        }
        private void TrangThaiNhanThem()
        {
            EnableAll();
            foreach (Control ctr in panelText.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit) || ctr.GetType() == typeof(DateEdit))
                    ctr.Enabled = true;
            btnLuu.Enabled = true;
        }
        private void TrangThaiNhanView()
        {
            EnableAll();
            dgvTour.Enabled = true;
            dgvDichVu.Enabled = true;
            dgvPhuongTien.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
        }
        private void TrangThaiNhanSua()
        {
            EnableAll();
            btnLuu.Enabled = true;
            foreach (Control ctr in panelText.Controls)
                if (ctr.GetType() == typeof(TextEdit) || ctr.GetType() == typeof(LookUpEdit) || ctr.GetType() == typeof(DateEdit))
                    ctr.Enabled = true;
            cboLoaiTour.Properties.NullText = "";
            cboLoaiTour.EditValue = null;
            cboTinh.Properties.NullText = "";
            cboTinh.EditValue = null;
        }
        #endregion

        

        

        

        
        

        

        

        

        
    }
}