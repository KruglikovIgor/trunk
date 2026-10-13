using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.Users
{
    // сотрудники 
  public partial class frmUsers : Form
  {
    public bool isShow = true; // показывать форму
    //int executorID; // ID текущего сотрудника
    private bool isExec; // доступность сотрудников
    private int k = 0; // для блокировки редактирования при "0" сотрудников

    public frmUsers()
    {
      InitializeComponent();
      if (k == 0)
      {
        btnEdit.Enabled = false;
        lbl_IsWorker.Text = "";
      }
      else btnEdit.Enabled = true;
    }

    // закрываемся 
    private void btnClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    // добавить нового сотрудника
    private void btnAdd_Click(object sender, EventArgs e)
    {
      frmAddUser frm = new frmAddUser(-1, null);
      if (frm.isShow) frm.ShowDialog();
      else
      {
        MessageBox.Show(Resources.CantConnectDb, Resources.ErrorCaption, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        frm.Close();
        return;
      }
      RenewGrid();
      btnEdit.Enabled = true;
    }

    private void RenewGrid()
    {
      DataTable dt = new DataTable();
      if (isExec)
        DataManager.Instance.GetAllEmployees(ref dt);
      else
        DataManager.Instance.GetWorkingEmployees(ref dt);

      FillGrid(dt);
    }

    // редактировать данные о сотруднике
    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (dgvEmployees.CurrentRow == null)
        return;
      frmAddUser frm = new frmAddUser(Convert.ToInt32(dgvEmployees.CurrentRow.Cells[0].Value), dgvEmployees.CurrentRow);
      if (frm.isShow) frm.ShowDialog(this);
      RenewGrid();
      btnEdit.Enabled = true;
    }

    private void executorsDataGridView_SelectionChanged(object sender, EventArgs e)
    {
      if (dgvEmployees.CurrentRow != null)
      {
        btnEdit.Enabled = true;
        ShowData();
      }
    }

    /// <summary>
    /// Отражает развернутую информацию о сотруднике
    /// </summary>
    private void ShowData()
    {
      if (dgvEmployees.CurrentRow == null)
        return;
      txtBox_Number.Text = dgvEmployees.CurrentRow.Cells["cNumber"].Value.ToString();
      txtBox_FirstName.Text = dgvEmployees.CurrentRow.Cells["cSurname"].Value.ToString();
      txtBox_SecondName.Text = dgvEmployees.CurrentRow.Cells["cName"].Value.ToString();
      txtBox_LastName.Text = dgvEmployees.CurrentRow.Cells["cLastName"].Value.ToString();
      txtBox_Note.Text = dgvEmployees.CurrentRow.Cells["cNote"].Value.ToString();
      txtBox_Post.Text = dgvEmployees.CurrentRow.Cells["cPost"].Value.ToString();
      if (Convert.ToBoolean(dgvEmployees.CurrentRow.Cells["cIsActive"].Value))
      {
        lbl_IsWorker.Text = Resources.UserWorking;
        lbl_IsWorker.ForeColor = Color.Black;
      }
      else
      {
        lbl_IsWorker.Text = Resources.UserInactive;
        lbl_IsWorker.ForeColor = Color.Red;
      }
      SelectPicture(Convert.ToInt32(dgvEmployees.CurrentRow.Cells["cId"].Value));
    }

    /// <summary>
    /// Берем картинку из БД, вставляем в PictureBox
    /// </summary>
    /// <param name="id">ID выделенной строки</param>
    private void SelectPicture(int id)
    {
      MemoryStream ms = DataManager.Instance.GetEmployeePicture(id);
      if (ms == null)
        pictureBox.Image = pictureBox.ErrorImage;
      else
        try
        {
          pictureBox.Image = Image.FromStream(ms);
        }
        catch
        {
          pictureBox.Image = pictureBox.ErrorImage;
        }
    }

    // закрываемся
    private void frmUsers_FormClosing(object sender, FormClosingEventArgs e)
    {
      if (ParentForm != null)
        ((frmMain) ParentForm).EnableMenu(true);
    }

    // все сотрудники 
    private void btnAllExec_Click(object sender, EventArgs e)
    {
      DataTable dt = new DataTable();
      if (!isExec)
      {
        isExec = !isExec;
        btnAllExec.Text = Resources.ShowAllActiveUsers;
        DataManager.Instance.GetAllEmployees(ref dt);
      }
      else
      {
        isExec = !isExec;
        btnAllExec.Text = Resources.ShowAllUsers;
        DataManager.Instance.GetWorkingEmployees(ref dt);
      }
      btnEdit.Enabled = dt.Rows.Count > 0;
      FillGrid(dt);
    }

    private void frmUsers_Load(object sender, EventArgs e)
    {
      DataTable dt = new DataTable();
      DataManager.Instance.GetWorkingEmployees(ref dt);
      FillGrid(dt);
      dgvEmployees.Visible = true;
    }

    private void FillGrid(DataTable dt)
    {
      dgvEmployees.Rows.Clear();
      foreach (DataRow dr in dt.Rows)
        dgvEmployees.Rows.Add(dr.ItemArray);
    }
  }
}