using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace De02
{
    public partial class frmSanpham : Form
    {
        Model1 context = new Model1();
        public frmSanpham()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            LoadLoaiSP();
            LoadSanpham();
        }

        private void LoadLoaiSP()
        {
            var loaiSPs = context.LoaiSPs.ToList();
            cboLoaiSP.DataSource = loaiSPs;
            cboLoaiSP.DisplayMember = "TenLoai";
            cboLoaiSP.ValueMember = "MaLoai";
        }

        private void LoadSanpham()
        {
            var sanphams = context.Sanphams.Select(sp => new
            {
                sp.MaSP,
                sp.TenSP,
                sp.NgayNhap,
                TenLoai = sp.LoaiSP.TenLoai
            }).ToList();

            lvSanpham.Items.Clear();
            foreach (var sp in sanphams)
            {
                ListViewItem item = new ListViewItem(sp.MaSP);
                item.SubItems.Add(sp.TenSP);
                item.SubItems.Add(sp.NgayNhap.HasValue ? sp.NgayNhap.Value.ToString("dd/MM/yyyy") : ""); // Check for null
                item.SubItems.Add(sp.TenLoai);
                lvSanpham.Items.Add(item);
            }

        }

        private void frmSanpham_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void lvSanpham_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSanpham.SelectedItems.Count > 0)
            {
                ListViewItem item = lvSanpham.SelectedItems[0];
                txtMaSP.Text = item.SubItems[0].Text;
                txtTenSP.Text = item.SubItems[1].Text;

                if (DateTime.TryParse(item.SubItems[2].Text, out DateTime ngayNhap))
                {
                    dtNgaynhap.Value = ngayNhap;
                }
                else
                {
                    dtNgaynhap.Value = DateTime.Now;
                }
                cboLoaiSP.Text = item.SubItems[3].Text;
            }
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            AddOrUpdateSanpham();
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            
            var result = MessageBox.Show("Bạn đã chắc chắn muốn sửa các thông tin này chưa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
              
                    var maSP = txtMaSP.Text.Trim();
                    var sanpham = context.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);

                    if (sanpham != null)
                    {
                        
                        sanpham.TenSP = txtTenSP.Text.Trim();
                        sanpham.NgayNhap = dtNgaynhap.Value;
                        sanpham.MaLoai = (cboLoaiSP.SelectedItem as LoaiSP)?.MaLoai;
                 
                        context.SaveChanges();

                        LoadSanpham();

                        MessageBox.Show("Sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm với mã này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btXoa_Click(object sender, EventArgs e)
        {
            
            if (lvSanpham.SelectedItems.Count > 0)
            {              
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
              
                if (result == DialogResult.Yes)
                {                  
                    string maSP = lvSanpham.SelectedItems[0].SubItems[0].Text;                   
                    var sanpham = context.Sanphams.FirstOrDefault(sp => sp.MaSP == maSP);
                   
                    if (sanpham != null)
                    {                       
                        context.Sanphams.Remove(sanpham);
                        context.SaveChanges();                      
                        LoadSanpham();                 
                        MessageBox.Show("Bạn đã xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            else
            {
                
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btLuu_Click(object sender, EventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            var result = MessageBox.Show("Bạn đã thực sự muốn lưu chưa?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // Lưu các thay đổi vào cơ sở dữ liệu
                    context.SaveChanges();

                    // Hiển thị thông báo lưu thành công
                    MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // Hiển thị thông báo lỗi nếu có ngoại lệ xảy ra
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AddOrUpdateSanpham()
        {
            try
            {
                var sanpham = context.Sanphams.Find(txtMaSP.Text);

                if (sanpham != null)
                {
                    // Update existing product
                    sanpham.TenSP = txtTenSP.Text;
                    sanpham.NgayNhap = dtNgaynhap.Value;
                    sanpham.MaLoai = cboLoaiSP.SelectedValue.ToString();
                    MessageBox.Show("Sản phẩm đã được cập nhật.");
                }
                else
                {
                    // Add new product
                    sanpham = new Sanpham
                    {
                        MaSP = txtMaSP.Text,
                        TenSP = txtTenSP.Text,
                        NgayNhap = dtNgaynhap.Value,
                        MaLoai = cboLoaiSP.SelectedValue.ToString()
                    };
                    context.Sanphams.Add(sanpham);
                    MessageBox.Show("Sản phẩm mới đã được thêm.");
                }

                context.SaveChanges();
                LoadSanpham();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }
        private void btKLuu_Click(object sender, EventArgs e)
        {
            LoadSanpham();
            MessageBox.Show("Danh sách đã được load lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
       
    }

        private void btThoat_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận thoát", 
                                                       MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            // Nếu người dùng chọn "No" thì không hiện gì
        }

        private void btTim_Click(object sender, EventArgs e)
        {
            string tenSP = textBox1.Text;
           
            if (!string.IsNullOrEmpty(tenSP))
            {
                // Truy vấn cơ sở dữ liệu để tìm sản phẩm chứa tên sản phẩm trong TextBox
                var sanphams = context.Sanphams
                    .Where(sp => sp.TenSP.Contains(tenSP))
                    .Select(sp => new
                    {
                        sp.MaSP,
                        sp.TenSP,
                        sp.NgayNhap,
                        TenLoai = sp.LoaiSP.TenLoai
                    }).ToList();

                // Xóa các mục hiện tại trong ListView
                lvSanpham.Items.Clear();
                // Kiểm tra nếu có sản phẩm được tìm thấy
                if (sanphams.Count > 0)
                {
                   
                    foreach (var sp in sanphams)
                    {
                        ListViewItem item = new ListViewItem(sp.MaSP);
                        item.SubItems.Add(sp.TenSP);
                        item.SubItems.Add(sp.NgayNhap.HasValue ? sp.NgayNhap.Value.ToString("dd/MM/yyyy") : ""); // Check for null
                        item.SubItems.Add(sp.TenLoai);
                        lvSanpham.Items.Add(item);
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy sản phẩm nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm để tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void dtNgaynhap_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
