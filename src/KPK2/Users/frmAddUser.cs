using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.Users
{
  // редактируем/добавляем сотрудника
  public partial class frmAddUser : Form
  {
    public bool isShow = true; // показать форму
    private bool isActiv; // сотрудник доступен
    private bool isPictur; // выбрали картинку

    /// <summary>
    /// выход по Escape
    /// </summary>
    private bool isEscape;

    /// <summary>
    /// ID сотрудника для редактирования; 
    /// "-1"-добавление нового сотрудника
    /// </summary>
    private int executorID;

    private string str = ""; // полученный табельный номер при редактировании для сравнения
    private Byte[] bPicture; // [] для рисунка
    private DataGridViewRow currentRow;

    public frmAddUser()
    {
      InitializeComponent();
      FillPosts();
    }

    private void FillPosts()
    {
      const string sId = "Id";
      const string sPost = "Post";
      DataTable dt = new DataTable();
      dt.Columns.Add(sId);
      dt.Columns.Add(sPost);
      dt.Columns.Add();
      dt.Rows.Add(0, string.Empty);
      DataManager.Instance.GetPosts(ref dt);
      cbPost.DataSource = dt;
      cbPost.DisplayMember = sPost;
      cbPost.ValueMember = sId;
    }

    private const int ColNumber = 1;
    private const int ColSurname = 2;
    private const int ColName = 3;
    private const int ColLastName = 4;
    private const int ColPost = 5;
    private const int ColIsActive = 6;
    private const int ColNote = 7;

    public frmAddUser(int id, DataGridViewRow dr)
      : this()
    {
      executorID = id;
      currentRow = dr;
    }

    // закрываемся + 
    private void btnClose_Click(object sender, EventArgs e)
    {
      this.Close();
    }

    // выбрать фото
    private void button1_Click(object sender, EventArgs e)
    {
      if (openFileDialog.ShowDialog() != DialogResult.OK)
        return;
      ResizePicture();
      if (executorID != -1)
        if (!DataManager.Instance.UpdateEmployeePicture(executorID, bPicture))
          MessageBox.Show(Resources.CantSaveDbParam, Resources.ErrorCaption);
    }

    public bool ThumbnailCallback()
    {
      return false;
    }

    /// <summary>
    /// Функция уменьшает картинку и приводит ее к byte
    /// </summary>
    /// <returns></returns>
    private void ResizePicture()
    {
      try
      {
        if (isPictur)
        {
          string path = openFileDialog.FileName;
          Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
          Bitmap myBitmap = new Bitmap(path);
          int height = myBitmap.Size.Height; // размеры картинки
          int width = myBitmap.Size.Width;
          // обрезали 
          double kWidth = (double) width/pictureBox.Size.Width;
          double kHeight = (double) height/pictureBox.Size.Height;
          double k;

          k = height/kWidth > pictureBox.Size.Height ? kHeight : kWidth;

          height = (int) (height/k);
          width = (int) (width/k);

          Image ima = myBitmap.GetThumbnailImage(width, height, myCallback, IntPtr.Zero);

          pictureBox.Image = ima;
          isPictur = false;

          MemoryStream mms = new MemoryStream();
          pictureBox.Image.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
          bPicture = mms.ToArray();
        }
      }
      catch
      {
        MessageBox.Show(Resources.ErrorPictureFormat, Resources.ErrorCaption);
      }
    }

    // сохраняем введенные данные
    private void btnAdd_Click(object sender, EventArgs e)
    {
      if (!CheckFields())
        return;
      if (executorID == -1) // добавление нового сотрудника
      {
        if (DataManager.Instance.IsEmployeeExist(EmployeeName, EmployeeSurname, EmployeeLastName))
        {
          MessageBox.Show(Resources.EmployeeAlreadyExist, Resources.ErrorCaption);
          return;
        }

        if (
          !DataManager.Instance
                      .InsertEmployee(tbNumber.Text, EmployeeSurname, EmployeeName, EmployeeLastName,
                                      Convert.ToInt32(cbPost.SelectedValue), true, tbNote.Text, bPicture))
          MessageBox.Show(Resources.CantSaveDbParam, Resources.ErrorCaption);
        else
          Close();
      }
      else // редактирование данных
      {
        if (
          !DataManager.Instance
                      .UpdateEmployee(executorID, tbNumber.Text, EmployeeSurname, EmployeeName, EmployeeLastName,
                                      Convert.ToInt32(cbPost.SelectedValue), chckBox.Checked, tbNote.Text))
          MessageBox.Show(Resources.CantSaveDbParam, Resources.ErrorCaption);
        else
          Close();
      }
    }

    private string EmployeeSurname
    {
      get { return tbSurname.Text.Trim(); }
    }

    private string EmployeeName
    {
      get { return tbName.Text.Trim(); }
    }

    private string EmployeeLastName
    {
      get { return tbLastName.Text.Trim(); }
    }

    private bool CheckFields()
    {
      bool result = true;
      if (String.IsNullOrEmpty(EmployeeSurname))
      {
        errProvider.SetError(tbSurname, string.Format(Resources.FieldNRequired, lSurname.Text));
        result =  false;
      }
      if (String.IsNullOrEmpty(EmployeeName))
      {
        errProvider.SetError(tbName, string.Format(Resources.FieldNRequired, lName.Text));
        result = false;
      }
      return result;
    }

    /// <summary>
    /// Берем картинку из БД, вставляем в PictureBox
    /// </summary>
    /// <param name="id"></param>
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

    // работает/не работает
    private void chckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (chckBox.FlatStyle == FlatStyle.Standard)
      {
        chckBox.FlatStyle = FlatStyle.Flat;
        chckBox.Text = Resources.Work;
        chckBox.ForeColor = SystemColors.ControlText;
      }
      else
      {
        chckBox.FlatStyle = FlatStyle.Standard;
        chckBox.Text = Resources.NotWork;
        chckBox.ForeColor = Color.Red;
      }
    }

    private void openFileDialog_FileOk(object sender, CancelEventArgs e)
    {
      isPictur = true; // выбрали фотку
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
      if (keyData == Keys.Escape)
        isEscape = true;
      return base.ProcessCmdKey(ref msg, keyData);
    }

    #region Обработка textBox-ов

    // табельный №        
    private void txtBox_Number_Validating(object sender, CancelEventArgs e)
    {
      if (btnClose.Focused || isEscape)
        return;
      // уникальность Табельного номера || executorID != -1
      string strFilter = "EmployeeNomber = '" + tbNumber.Text + "'";
      DataRow[] r = KPK2DataSet.Tables["Executors"].Select(strFilter);
      bool b = executorID == -1 && r.Length != 0 ||
               executorID != -1 && !str.Equals(tbNumber.Text) && r.Length == 1;
      if (b && !btnClose.Focused && !isEscape &&
          tbNumber.Text != "")
      {
        MessageBox.Show(Resources.EmployeeNumberAlreadyExist, Resources.KPK2, MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        tbNumber.Focus();
        tbNumber.Select(0, tbNumber.Text.Length);
      }
    }

    #endregion

    private void TextBox_TextChanged(object sender, EventArgs e)
    {
      TextBox tbCurrent = (TextBox) sender;
      tbCurrent.Text = tbCurrent.Text.Trim();
    }

    private void frmAddUser_Load(object sender, EventArgs e)
    {
      if (executorID == -1) // добавляем сотрудника
      {
        Text = Resources.EditNewEmployee;
        chckBox.Visible = false;
        lbl7.Visible = false;
        btn_AddPicture.Text = Resources.AddFoto;
      }
      else // редактируем
      {
        Text = Resources.EditEmployee;
        btn_AddPicture.Text = Resources.ChangeFoto;
        
        tbNumber.Text = currentRow.Cells[ColNumber].Value.ToString();
        tbSurname.Text = currentRow.Cells[ColSurname].Value.ToString();
        tbName.Text = currentRow.Cells[ColName].Value.ToString();
        tbLastName.Text = currentRow.Cells[ColLastName].Value.ToString();
        cbPost.Text = currentRow.Cells[ColPost].Value.ToString();
        isActiv = Convert.ToBoolean(currentRow.Cells[ColIsActive].Value);
        tbNote.Text = currentRow.Cells[ColNote].Value.ToString();

        chckBox.Checked = isActiv;
        if (!isActiv)
        {
          chckBox.Text = Resources.NotWork;
          chckBox.ForeColor = Color.Red;
        }
        else
          chckBox.Text = Resources.Work;
        SelectPicture(executorID);
      }
    }

    private void tb_TextChanged(object sender, EventArgs e)
    {
      errProvider.Clear();
    }
  }
}
