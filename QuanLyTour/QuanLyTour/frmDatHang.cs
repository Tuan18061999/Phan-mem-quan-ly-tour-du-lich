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
    public partial class frmDatHang : DevExpress.XtraEditors.XtraForm
    {
        string trangThai1 = "";
        string trangThai2 = "";
        public frmDatHang()
        {
            InitializeComponent();
        }

        private void frmDatHang_Load(object sender, EventArgs e)
        {
            loadDgvDatHang();
            loadCboKhachHang();
            loadCboTour();
        }
        private void loadDgvDatHang()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstDatHang = from dh in data.DatHangs
                                 join kh in data.KhachHangs
                                 on dh.MaSoKhachHang equals kh.MaSoKhachHang
                                 select new
                                 {
                                     dh.MaDatHang,
                                     kh.TenKhachHang,
                                     dh.NgayDat,
                                     dh.TinhTrang,
                                     dh.TongThanhTien
                                 };
                dgvDatHang.DataSource = lstDatHang;
            }
        }
        private void loadCboKhachHang()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstKhachHang = from kh in data.KhachHangs
                                   select new
                                   {
                                       kh.MaSoKhachHang,
                                       kh.TenKhachHang
                                   };
                cboKhachHang.Properties.DataSource = lstKhachHang;
                cboKhachHang.Properties.DisplayMember = "TenKhachHang";
                cboKhachHang.Properties.ValueMember = "MaSoKhachHang";
                cboKhachHang.Properties.NullText = "";
            }
        }

        private void gridView1_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            string maDH = gridView1.GetRowCellValue(e.RowHandle, "MaDatHang").ToString();
            cboKhachHang.Properties.NullText = gridView1.GetRowCellValue(e.RowHandle, "TenKhachHang").ToString();
            txtNgayDat.EditValue = DateTime.Parse(gridView1.GetRowCellValue(e.RowHandle, "NgayDat").ToString());
            txtTinhTrang.Text = gridView1.GetRowCellValue(e.RowHandle, "TinhTrang").ToString();
            txtTongThanhTien.Text = gridView1.GetRowCellValue(e.RowHandle, "TongThanhTien").ToString();
            txtTongThanhTienChu.Text = So_chu(double.Parse(txtTongThanhTien.Text));
            loadDgvChiTietDatHangTheoMa(int.Parse(maDH));
        }

        private void btnTaoDonHang_Click(object sender, EventArgs e)
        {
            trangThai1 = "them";
        }

        private void btnXoaDonHang_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maDH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDatHang").ToString();
                DatHang dh = data.DatHangs.Where(t => t.MaDatHang == int.Parse(maDH)).FirstOrDefault();
                try
                {
                    DialogResult r;
                    r = MessageBox.Show("Bạn có muốn xóa khách hàng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        data.DatHangs.DeleteOnSubmit(dh);
                        data.SubmitChanges();
                        MessageBox.Show("Xóa đơn hàng thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadDgvDatHang();
                    }
                    
                }
                catch
                {
                    MessageBox.Show("Không thể xóa đơn hàng này vì có dữ liệu liên quan", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        private void btnSuaDonHang_Click(object sender, EventArgs e)
        {
            trangThai1 = "sua";
        }

        private void btnLuuDonHang_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai1 == "them")
                {
                    DatHang dh = new DatHang();
                    dh.MaSoKhachHang = int.Parse(cboKhachHang.EditValue.ToString());
                    dh.NgayDat = DateTime.Parse(txtNgayDat.EditValue.ToString());
                    dh.TongThanhTien = "0";
                    dh.TinhTrang = txtTinhTrang.Text;
                    data.DatHangs.InsertOnSubmit(dh);
                    data.SubmitChanges();
                    loadDgvDatHang();
                    //ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                if (trangThai1 == "sua")
                {
                    string maDH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDatHang").ToString();
                    DatHang dh = data.DatHangs.Where(t => t.MaDatHang == int.Parse(maDH)).FirstOrDefault();
                    dh.MaSoKhachHang = int.Parse(cboKhachHang.EditValue.ToString());
                    dh.NgayDat = DateTime.Parse(txtNgayDat.EditValue.ToString());
                    dh.TinhTrang = txtTinhTrang.Text;
                    data.SubmitChanges();
                    loadDgvDatHang();
                    //ClearAll();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
        }
        private void loadDgvChiTietDatHangTheoMa(int maDH)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstChiTietDatHang = from ctdh in data.ChiTietDatHangs
                                        join t in data.TOURs
                                        on ctdh.MaSoTour equals t.MaSoTour
                                        where (ctdh.MaDatHang == maDH)
                                        select new
                                        {
                                            t.MaSoTour,
                                            t.TenTour,
                                            ctdh.SoLuongVe,
                                            t.Gia,
                                            ctdh.ThanhTien
                                        };
                dgvChiTietDatHang.DataSource = lstChiTietDatHang;
            }
            
        }
        private void loadCboTour()
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                var lstTour = from t in data.TOURs
                              select new
                              {
                                  t.MaSoTour,
                                  t.TenTour
                              };
                cboTour.Properties.DataSource = lstTour;
                cboTour.Properties.DisplayMember = "TenTour";
                cboTour.Properties.ValueMember = "MaSoTour";
                cboTour.Properties.NullText = "";
            }
        }

        #region HAM CHUYEN SO THANH CHU
        private static string Chu(string gNumber)
        {
            string result = "";
            switch (gNumber)
            {
                case "0":
                    result = "không";
                    break;
                case "1":
                    result = "một";
                    break;
                case "2":
                    result = "hai";
                    break;
                case "3":
                    result = "ba";
                    break;
                case "4":
                    result = "bốn";
                    break;
                case "5":
                    result = "năm";
                    break;
                case "6":
                    result = "sáu";
                    break;
                case "7":
                    result = "bảy";
                    break;
                case "8":
                    result = "tám";
                    break;
                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }
        private static string Donvi(string so)
        {
            string Kdonvi = "";

            if (so.Equals("1"))
                Kdonvi = "";
            if (so.Equals("2"))
                Kdonvi = "nghìn";
            if (so.Equals("3"))
                Kdonvi = "triệu";
            if (so.Equals("4"))
                Kdonvi = "tỷ";
            if (so.Equals("5"))
                Kdonvi = "nghìn tỷ";
            if (so.Equals("6"))
                Kdonvi = "triệu tỷ";
            if (so.Equals("7"))
                Kdonvi = "tỷ tỷ";

            return Kdonvi;
        }
        private static string Tach(string tach3)
        {
            string Ktach = "";
            if (tach3.Equals("000"))
                return "";
            if (tach3.Length == 3)
            {
                string tr = tach3.Trim().Substring(0, 1).ToString().Trim();
                string ch = tach3.Trim().Substring(1, 1).ToString().Trim();
                string dv = tach3.Trim().Substring(2, 1).ToString().Trim();
                if (tr.Equals("0") && ch.Equals("0"))
                    Ktach = " không trăm lẻ " + Chu(dv.ToString().Trim()) + " ";
                if (!tr.Equals("0") && ch.Equals("0") && dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm ";
                if (!tr.Equals("0") && ch.Equals("0") && !dv.Equals("0"))
                    Ktach = Chu(tr.ToString().Trim()).Trim() + " trăm lẻ " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (tr.Equals("0") && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = " không trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (tr.Equals("0") && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = " không trăm mười " + Chu(dv.Trim()).Trim() + " ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("0"))
                    Ktach = " không trăm mười ";
                if (tr.Equals("0") && ch.Equals("1") && dv.Equals("5"))
                    Ktach = " không trăm mười lăm ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi " + Chu(dv.Trim()).Trim() + " ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi ";
                if (Convert.ToInt32(tr) > 0 && Convert.ToInt32(ch) > 1 && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm " + Chu(ch.Trim()).Trim() + " mươi lăm ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && Convert.ToInt32(dv) > 0 && !dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười " + Chu(dv.Trim()).Trim() + " ";

                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("0"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười ";
                if (Convert.ToInt32(tr) > 0 && ch.Equals("1") && dv.Equals("5"))
                    Ktach = Chu(tr.Trim()).Trim() + " trăm mười lăm ";

            }


            return Ktach;

        }
        public static string So_chu(double gNum)
        {
            if (gNum == 0)
                return "Không đồng";

            string lso_chu = "";
            string tach_mod = "";
            string tach_conlai = "";
            double Num = Math.Round(gNum, 0);
            string gN = Convert.ToString(Num);
            int m = Convert.ToInt32(gN.Length / 3);
            int mod = gN.Length - m * 3;
            string dau = "[+]";

            // Dau [+ , - ]
            if (gNum < 0)
                dau = "[-]";
            dau = "";

            // Tach hang lon nhat
            if (mod.Equals(1))
                tach_mod = "00" + Convert.ToString(Num.ToString().Trim().Substring(0, 1)).Trim();
            if (mod.Equals(2))
                tach_mod = "0" + Convert.ToString(Num.ToString().Trim().Substring(0, 2)).Trim();
            if (mod.Equals(0))
                tach_mod = "000";
            // Tach hang con lai sau mod :
            if (Num.ToString().Length > 2)
                tach_conlai = Convert.ToString(Num.ToString().Trim().Substring(mod, Num.ToString().Length - mod)).Trim();

            ///don vi hang mod
            int im = m + 1;
            if (mod > 0)
                lso_chu = Tach(tach_mod).ToString().Trim() + " " + Donvi(im.ToString().Trim());
            /// Tach 3 trong tach_conlai

            int i = m;
            int _m = m;
            int j = 1;
            string tach3 = "";
            string tach3_ = "";

            while (i > 0)
            {
                tach3 = tach_conlai.Trim().Substring(0, 3).Trim();
                tach3_ = tach3;
                lso_chu = lso_chu.Trim() + " " + Tach(tach3.Trim()).Trim();
                m = _m + 1 - j;
                if (!tach3_.Equals("000"))
                    lso_chu = lso_chu.Trim() + " " + Donvi(m.ToString().Trim()).Trim();
                tach_conlai = tach_conlai.Trim().Substring(3, tach_conlai.Trim().Length - 3);

                i = i - 1;
                j = j + 1;
            }
            if (lso_chu.Trim().Substring(0, 1).Equals("k"))
                lso_chu = lso_chu.Trim().Substring(10, lso_chu.Trim().Length - 10).Trim();
            if (lso_chu.Trim().Substring(0, 1).Equals("l"))
                lso_chu = lso_chu.Trim().Substring(2, lso_chu.Trim().Length - 2).Trim();
            if (lso_chu.Trim().Length > 0)
                lso_chu = dau.Trim() + " " + lso_chu.Trim().Substring(0, 1).Trim().ToUpper() + lso_chu.Trim().Substring(1, lso_chu.Trim().Length - 1).Trim() + " đồng chẵn.";

            return lso_chu.ToString().Trim();

        }
        #endregion

        private void btnThemTour_Click(object sender, EventArgs e)
        {
            trangThai2 = "them";
        }

        private void btnXoaTour_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string maDH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDatHang").ToString();
                string maTour = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaSoTour").ToString();
                ChiTietDatHang ctdh = data.ChiTietDatHangs.Where(t => t.MaDatHang == int.Parse(maDH) && t.MaSoTour == int.Parse(maTour)).FirstOrDefault();
                DialogResult r;
                r = MessageBox.Show("Bạn có muốn xóa tour này khỏi đơn", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    data.ChiTietDatHangs.DeleteOnSubmit(ctdh);
                    data.SubmitChanges();
                    loadDgvChiTietDatHangTheoMa(int.Parse(maDH));
                    CapNhapTongThanhTien(int.Parse(maDH));
                }
            }
        }

        private void btnSuaTour_Click(object sender, EventArgs e)
        {
            trangThai2 = "sua";
        }

        private void btnLuuTour_Click(object sender, EventArgs e)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                if (trangThai2 == "them")
                {
                    string maDH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDatHang").ToString();
                    ChiTietDatHang ctdh = new ChiTietDatHang();
                    ctdh.MaDatHang = int.Parse(maDH);
                    ctdh.MaSoTour = int.Parse(cboTour.EditValue.ToString());
                    ctdh.SoLuongVe = int.Parse(txtSoLuongVe.Value.ToString());
                    ctdh.ThanhTien = int.Parse(txtThanhTienTour.Text.ToString());
                    data.ChiTietDatHangs.InsertOnSubmit(ctdh);
                    data.SubmitChanges();
                    loadDgvChiTietDatHangTheoMa(int.Parse(maDH));
                    //ClearAll();
                    MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    CapNhapTongThanhTien(int.Parse(maDH));
                }
                if (trangThai2 == "sua")
                {
                    string maDH = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MaDatHang").ToString();
                    string maTour = gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "MaSoTour").ToString();
                    ChiTietDatHang ctdh = data.ChiTietDatHangs.Where(t => t.MaDatHang == int.Parse(maDH) && t.MaSoTour == int.Parse(maTour)).FirstOrDefault();
                    ctdh.SoLuongVe = int.Parse(txtSoLuongVe.Value.ToString());
                    ctdh.ThanhTien = int.Parse(txtThanhTienTour.Text);
                    data.SubmitChanges();
                    loadDgvChiTietDatHangTheoMa(int.Parse(maDH));
                    //ClearAll();
                    MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    CapNhapTongThanhTien(int.Parse(maDH));
                }
            }
        }

        private void txtSoLuongVe_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                int value = int.Parse(txtDonGia.Text) * int.Parse(txtSoLuongVe.Value.ToString());
                txtThanhTienTour.Text = value.ToString();
            }
            catch { }
        }
        

        private void cboTour_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                using (DataClasses1DataContext data = new DataClasses1DataContext())
                {
                    string maTour = cboTour.EditValue.ToString();
                    TOUR tour = data.TOURs.Where(t => t.MaSoTour == int.Parse(maTour)).FirstOrDefault();
                    txtDonGia.Text = tour.Gia.ToString();
                }
            }
            catch { }
        }

        private void CapNhapTongThanhTien(int maDatHang)
        {
            using (DataClasses1DataContext data = new DataClasses1DataContext())
            {
                string value = data.ChiTietDatHangs.Where(t => t.MaDatHang == maDatHang).Sum(a => a.ThanhTien).ToString();
                DatHang dh = data.DatHangs.Where(t => t.MaDatHang == maDatHang).FirstOrDefault();
                dh.TongThanhTien = value;
                data.SubmitChanges();
                loadDgvDatHang();
                txtTongThanhTien.Text = value;
            }
        }

        private void txtTongThanhTien_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                txtTongThanhTienChu.Text = So_chu(double.Parse(txtTongThanhTien.Text));
            }
            catch { }
        }

        private void gridView2_CustomRowCellEditForEditing(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            cboTour.Properties.NullText = gridView2.GetRowCellValue(e.RowHandle, "TenTour").ToString();
            txtSoLuongVe.Value = decimal.Parse(gridView2.GetRowCellValue(e.RowHandle, "SoLuongVe").ToString());
            txtDonGia.Text = gridView2.GetRowCellValue(e.RowHandle, "Gia").ToString();
            txtThanhTienTour.Text = gridView2.GetRowCellValue(e.RowHandle, "ThanhTien").ToString();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn muốn đóng chức năng này", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
                this.Close();
        }

        private void btnReset1_Click(object sender, EventArgs e)
        {
            //TrangThaiBanDau1();
            ClearAll1();
        }

        private void btnReset2_Click(object sender, EventArgs e)
        {
            //TrangThaiBanDau2();
            ClearAll2();
        }
        private void ClearAll1()
        {
            cboKhachHang.Properties.NullText = "";
            cboKhachHang.EditValue = null;
            txtNgayDat.EditValue = null;
            txtTinhTrang.Text = "";
            txtTongThanhTien.Text = "";
            txtTongThanhTienChu.Text = "";
        }
        private void ClearAll2()
        {
            cboTour.Properties.NullText = "";
            cboTour.EditValue = null;
            txtSoLuongVe.Value = 0;
            txtDonGia.Text = "";
            txtThanhTienTour.Text = "";
        }

        

        
    }
}