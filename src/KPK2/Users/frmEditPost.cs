using System;
using System.Drawing;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.Users
{
  /// <summary>
  /// ƒобавление/–едактирование существующей должности
  /// </summary> 
  public partial class frmEditPost : Form
  {
    /// <summary>
    /// при открытии - нормально соединились с Ѕƒ
    /// - иначе с родительской формы закрыть
    /// </summary>
    public bool isShow = true;

    /// <summary>
    /// є записи в Ѕƒ
    /// </summary>
    private int postID;

    /// <summary>
    /// ƒолжность доступна/не доступна изначально
    /// </summary>
    private bool isPost;

    public frmEditPost()
    {
      InitializeComponent();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="k">ID должности</param>
    public frmEditPost(int k, string value, bool isActive)
      : this()
    {
      postID = k;
      if (k == -1) // добавл€ем должность
      {
        this.Text = Resources.AddPost;
        label2.Visible = chckBox.Visible = false;
      }
      else // редактируем должность
      {
        Text = Resources.EditPost;
        label2.Visible = chckBox.Visible = true;
        try
        {
          txtBox.Text = value;
          isPost = isActive;
        }
        catch
        {
          isShow = false;
        }
        chckBox.Visible = true;
        chckBox.Checked = isPost;
        if (isPost) chckBox.Text = Resources.Available;
        else
        {
          chckBox.Text = Resources.NotAvailable;
          chckBox.ForeColor = Color.Red;
        }
      }
    }

    // закрылись и ничего не сохранили
    private void btnClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private string Post
    {
      get { return txtBox.Text.Trim(); }
    }

    // внесли изменени€ /нова€ должность
    private void btnAdd_Click(object sender, EventArgs e)
    {
      bool result = true;
      if (postID == -1)
      {
        if (DataManager.Instance.IsPostExist(Post))
        {
          MessageBox.Show(Resources.PostAlreadyExist, Resources.ErrorCaption);
          return;
        }
        result &= DataManager.Instance.InsertPost(Post);
      }
      else
        result &= DataManager.Instance.UpdatePost(postID, Post, chckBox.Checked);

      if (result)
        Close();
      else
        MessageBox.Show(Resources.CantSaveDbParam, Resources.ErrorCaption);
    }

    // должность доступна/не доступна
    private void chckBox_CheckedChanged(object sender, EventArgs e)
    {
      if (chckBox.FlatStyle == FlatStyle.Standard)
      {
        chckBox.FlatStyle = FlatStyle.Flat;
        chckBox.Text = Resources.Available;
        chckBox.ForeColor = SystemColors.ControlText;
      }
      else
      {
        chckBox.FlatStyle = FlatStyle.Standard;
        chckBox.Text = Resources.NotAvailable;
        chckBox.ForeColor = Color.Red;
      }
    }

    private void txtBox_TextChanged(object sender, EventArgs e)
    {
      btnAdd.Enabled = !String.IsNullOrEmpty(Post);
    }
  }
}