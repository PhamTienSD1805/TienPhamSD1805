using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BUS_QLBanHang;
using DTO_QLBanHang;

namespace TemplateProject1_QLBanHang
{
    public partial class FrmKhach : Form
    {
        BUS_Khach busKhach = new BUS_QLBanHang.BUS_Khach();
        string stremail = FrmMain.mail;//nhận email tư FrmMain
        bool isAdding = false;
        public FrmKhach()
        {
            InitializeComponent();
        }

        private void FrmKhach_Load(object sender, EventArgs e)
        {
            LoadGridview_Khach();
            ResetValues();
        }

        //Load danh sách san pham len datagridview
        private void LoadGridview_Khach()
        {
            dgvkhach.DataSource = busKhach.getKhach();
            dgvkhach.Columns[0].HeaderText = "Điện Thoại";
            dgvkhach.Columns[1].HeaderText = "Họ và Tên";
            dgvkhach.Columns[2].HeaderText = "Địa Chỉ";
            dgvkhach.Columns[3].HeaderText = "Giới Tính";
            dgvkhach.Columns[4].Visible = false;
        }

        //thiết lập trạng thái control khi form load
        private void ResetValues()
        {
            txtDiachi.Text = null;
            txtDienthoai.Text = null;
            txtTenkhach.Text = null;
            rbnam.Checked = false;
            rbnu.Checked = false;

            txtDiachi.Enabled = false;
            txtDienthoai.Enabled = false;
            txtTenkhach.Enabled = false;
            rbnu.Enabled = false;
            rbnam.Enabled = false;
            dgvkhach.Enabled = true;

            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnDong.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;

            txtDienthoai.Focus();
        }

        private void BtnDong_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnThem_Click(object sender, EventArgs e)
        {
            isAdding = true;

            txtDiachi.Text = null;
            txtDienthoai.Text = null;
            txtTenkhach.Text = null;
            rbnam.Checked = true;
            rbnu.Checked = false;

            txtDiachi.Enabled = true;
            txtDienthoai.Enabled = true;
            txtTenkhach.Enabled = true;
            rbnu.Enabled = true;
            rbnam.Enabled = true;
            dgvkhach.Enabled = false;

            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnDong.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        // Bỏ qua các hành động đang thao tác, gọi trạng thái load form ban dầu
        private void BtnBoqua_Click(object sender, EventArgs e)
        {
            ResetValues();
        }

        private void BtnLuu_Click(object sender, EventArgs e)
        {
            string phai = "Nam";
            if (rbnu.Checked == true)
                phai = "Nữ";
            if (txtDienthoai.TextLength != 10 || !txtDienthoai.Text.All(char.IsDigit))
            {
                MessageBox.Show("Số điện thoại phải có 10 số, số nguyên", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtDienthoai.Focus();
                return;
            }
            if (isAdding == true)
            {
                DTO_Khach kh = new DTO_Khach(txtDienthoai.Text, txtTenkhach.Text, txtDiachi.Text, phai, stremail);
                if (busKhach.InsertKhach(kh))
                {
                    MessageBox.Show("Thêm thành công");
                    ResetValues();
                    LoadGridview_Khach(); // refresh datagridview
                }
                else
                {
                    MessageBox.Show("Thêm ko thành công");
                }
            }
            else
            {
                DTO_Khach kh = new DTO_Khach(txtDienthoai.Text, txtTenkhach.Text, txtDiachi.Text, phai); // Vì ID tự tăng nên để ID số gì cũng dc
                if (MessageBox.Show("Bạn có chắc muốn chỉnh sửa", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (busKhach.UpdateKhach(kh))
                    {
                        MessageBox.Show("Cập nhật khách hàng thành công");
                        ResetValues();
                        LoadGridview_Khach(); // refresh datagridview
                    }
                    else
                    {
                        MessageBox.Show("Cập nhật khách hàng không thành công");
                    }
                }
                else
                {
                    //do something if NO
                    ResetValues();
                }
            }
        }

        private void Dgvkhach_Click(object sender, EventArgs e)
        {
            if (dgvkhach.Rows.Count > 1)
            {
                btnLuu.Enabled = false;
                txtDienthoai.Focus();
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
                txtDienthoai.Text = dgvkhach.CurrentRow.Cells[0].Value.ToString();
                txtTenkhach.Text = dgvkhach.CurrentRow.Cells[1].Value.ToString();
                txtDiachi.Text = dgvkhach.CurrentRow.Cells[2].Value.ToString();
                string phai = dgvkhach.CurrentRow.Cells[3].Value.ToString();
                if (phai == "Nam")
                    rbnam.Checked = true;
                else
                    rbnu.Checked = true;
            }
            else
            {
                MessageBox.Show("Bảng không tồn tại dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSua_Click(object sender, EventArgs e)
        {
            isAdding = false;

            txtDiachi.Enabled = true;
            txtDienthoai.Enabled = false;
            txtTenkhach.Enabled = true;
            rbnu.Enabled = true;
            rbnam.Enabled = true;
            dgvkhach.Enabled = false;

            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnDong.Enabled = true;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu khách hàng", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //do something if YES
                if (busKhach.DeleteKhach(txtDienthoai.Text))
                {
                    MessageBox.Show("Xóa dữ liệu khách hàng thành công");
                    ResetValues();
                    LoadGridview_Khach(); // refresh datagridview
                }
                else
                {
                    MessageBox.Show("Xóa dữ liệu khách hàng không thành công");
                }
            }
            else
            {
                //do something if NO
                ResetValues();
            }
        }

        private void BtnTimkiem_Click(object sender, EventArgs e)
        {
            DataTable ds = busKhach.SearchKhach(txttimKiem.Text);
            if (ds.Rows.Count > 0)
            {
                dgvkhach.DataSource = ds;
                dgvkhach.Columns[0].HeaderText = "Điện Thoại";
                dgvkhach.Columns[1].HeaderText = "Họ và Tên";
                dgvkhach.Columns[2].HeaderText = "Địa Chỉ";
                dgvkhach.Columns[3].HeaderText = "Giới Tính";
                dgvkhach.Columns[4].Visible = false;
            }
            else
            {
                MessageBox.Show("Không tìm thấy khách hàng nào phù hợp tiêu chí tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txttimKiem.Focus();
            }
            txttimKiem.Text = "Nhập số điện thoại khach hàng";
            txttimKiem.BackColor = Color.LightGray;
            ResetValues();
        }

        private void TxttimKiem_Click(object sender, EventArgs e)
        {
            txttimKiem.Text = null;
            txttimKiem.BackColor = Color.White;
        }

        private void btnDanhsach_Click(object sender, EventArgs e)
        {
            ResetValues();
            LoadGridview_Khach();
        }
    }
}
