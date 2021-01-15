using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace QuanLyTour
{
    public partial class frmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }
        bool flag = false;
        private void frmMain_Load(object sender, EventArgs e)
        {
            Skin();
            loadFormFull();
        }
        private void Skin()
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            themes.LookAndFeel.SkinName = "Pumpkin";
        }
        private Form isActive(Type fType)
        {
            foreach (Form f in this.MdiChildren)
            {
                if (f.GetType() == fType)
                {
                    return f;
                }
            }
            return null;
        }
        
        #region Load Form
        private void loadDangNhap()
        {
            Form f = isActive(typeof(frmDangNhap));
            if (f == null)
            {
                frmDangNhap fDangNhap = new frmDangNhap();
                //fDangNhap.MdiParent = this;
                fDangNhap.Show();
            }
            else
                f.Activate();
        }
        private void loadDoiMK()
        {
            Form f = isActive(typeof(frmDoiMatKhau));
            if (f == null)
            {
                frmDoiMatKhau fDoiMK = new frmDoiMatKhau();
                fDoiMK.Show();
            }
        }
        private void loadKhachHang()
        {
            Form f = isActive(typeof(frmKhachHang));
            if (f == null)
            {
                frmKhachHang fKhachHang = new frmKhachHang();
                fKhachHang.MdiParent = this;
                fKhachHang.Show();
            }
        }
        private void loadNhanVien()
        {
            Form f = isActive(typeof(frmNhanVien));
            if (f == null)
            {
                frmNhanVien fNhanVien = new frmNhanVien();
                fNhanVien.MdiParent = this;
                fNhanVien.Show();
            }
        }
        private void loadLoaiTour()
        {
            Form f = isActive(typeof(frmLoaiTour));
            if (f == null)
            {
                frmLoaiTour fNhomTour = new frmLoaiTour();
                fNhomTour.MdiParent = this;
                fNhomTour.Show();
            }
        }
        private void loadTour()
        {
            Form f = isActive(typeof(frmTour));
            if (f == null)
            {
                frmTour fTour = new frmTour();
                fTour.MdiParent = this;
                fTour.Show();
            }
        }
        private void loadDichVu()
        {
            Form f = isActive(typeof(frmDichVu));
            if (f == null)
            {
                frmDichVu fDichVu = new frmDichVu();
                fDichVu.MdiParent = this;
                fDichVu.Show();
            }
        }
        private void loadTinh()
        {
            Form f = isActive(typeof(frmTinh));
            if (f == null)
            {
                frmTinh fTinh = new frmTinh();
                fTinh.MdiParent = this;
                fTinh.Show();
            }
        }
        private void loadFormFull()
        {
            if (flag == false)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                flag = true;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.WindowState = FormWindowState.Normal;
                flag = false;
            }
        }
        private void loadThongTinDN()
        {
            Form f = isActive(typeof(frmThongTinDN));
            if (f == null)
            {
                frmThongTinDN fThongTinDN = new frmThongTinDN();
                fThongTinDN.MdiParent = this;
                fThongTinDN.Show();
            }
        }
        private void loadDiaDiem()
        {
            Form f = isActive(typeof(frmDiaDiem));
            if (f == null)
            {
                frmDiaDiem fDiaDiem = new frmDiaDiem();
                fDiaDiem.MdiParent = this;
                fDiaDiem.Show();
            }
        }
        private void loadDatHang()
        {
            Form f = isActive(typeof(frmDatHang));
            if (f == null)
            {
                frmDatHang fDatHang = new frmDatHang();
                fDatHang.MdiParent = this;
                fDatHang.Show();
            }
        }
        private void loadChucVu()
        {
            Form f = isActive(typeof(frmChucVu));
            if (f == null)
            {
                frmChucVu fChucVu = new frmChucVu();
                fChucVu.MdiParent = this;
                fChucVu.Show();
            }
        }
        private void loadQuocTich()
        {
            Form f = isActive(typeof(frmQuocTich));
            if (f == null)
            {
                frmQuocTich fQuocTich = new frmQuocTich();
                fQuocTich.MdiParent = this;
                fQuocTich.Show();
            }

        }
        private void loadDangKy()
        {
            Form f = isActive(typeof(frmDangKy));
            if (f == null)
            {
                frmDangKy fDangKy = new frmDangKy();
                fDangKy.Show();
            }
        }
        private void loadPhuongTien()
        {
            Form f = isActive(typeof(frmPhuongTien));
            if (f == null)
            {
                frmPhuongTien fPhuongTien = new frmPhuongTien();
                fPhuongTien.MdiParent = this;
                fPhuongTien.Show();
            }
        }

        #endregion

        #region Event
        
        private void btnKhachHang_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadKhachHang();
        }
        private void btnNhanVien_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadNhanVien();
            
        }
        private void btnNhomTour_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadLoaiTour();
        }
        private void btnTour_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadTour();
        }
        private void btnNhomDichVu_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDichVu();
        }
        private void btnTinh_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadTinh();
        }
        private void btnDN_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDangNhap();
        }

        private void btnDoiMK_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDoiMK();
        }
        private void btnThongTinNV_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadThongTinDN();

        }
        private void btnDiaDiem_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDiaDiem();
        }
        private void btnDatHang_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDatHang();
        }
        private void btnChucVu_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadChucVu();
        }
        private void btnQuocTich_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadQuocTich();
        }
        private void btnDangKy_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadDangKy();
        }
        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r;
            r = MessageBox.Show("Bạn muốn thoát chương trình này không", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                Application.Exit();
                Application.ExitThread();
            }

        }
        private void btnPhuongTien_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadPhuongTien();
        }
        #endregion

        
        

        

        
        

        
        

        
        

        

        
        
        
        

        
        
        
        

        

        

        

        

        
        

        
        

        

        
        
        

        

        





    }
}