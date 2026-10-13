using System;
using System.Data;
using System.Windows.Forms;
using KPK2.Properties;
using Sparc.KPK2.NFunctions;

namespace KPK2.Users
{
  /// <summary>
  /// Должности: добавление, редактирование и их просмотр 
  /// </summary>
  public partial class frmPost : Form
  {
    /// <summary>
    /// доступность должности
    /// </summary>
    private bool isAvailablePosts = true;

    /// <summary>
    /// показывать форму - есть соединение с БД
    /// </summary>
    public bool isShow = true;

    /// <summary>
    /// ID должности
    /// </summary>
    private int postID;

    public frmPost()
    {
      InitializeComponent();
      DataTable dt = new DataTable();
      for (int i = 0; i < 3; i++)
        dt.Columns.Add();
      DataManager.Instance.GetPosts(ref dt);
      FillGrid(dt);
    }

    private void FillGrid(DataTable dt)
    {
      dgvPost.Rows.Clear();
      foreach (DataRow dr in dt.Rows)
        dgvPost.Rows.Add(dr.ItemArray);
    }

    // закрываем
    private void btnClose_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void RenewGrid()
    {
      DataTable dt = new DataTable();
      for (int i = 0; i < 3; i++)
        dt.Columns.Add();
      if (!isAvailablePosts)
      {
        btnAllPost.Text = Resources.ShowAvailablePosts;
        if (!DataManager.Instance.GetAllPosts(ref dt))
          MessageBox.Show(Resources.ErrorLoadFromDb, Resources.ErrorCaption);
      }
      else
      {
        btnAllPost.Text = Resources.ShowAllPosts;
        if (!DataManager.Instance.GetPosts(ref dt))
          MessageBox.Show(Resources.ErrorLoadFromDb, Resources.ErrorCaption);
      }
      FillGrid(dt);
      btnEdit.Enabled = dt.Rows.Count > 0;
    }

    // добавить новую должность 
    private void btnAdd_Click(object sender, EventArgs e)
    {
      frmEditPost frm = new frmEditPost(-1, "", true);
      frm.ShowDialog(); // запуск формы
      // обновили DataGridView 
      RenewGrid();
    }

    // редактировать и удалять (не доступна)
    private void btnEdit_Click(object sender, EventArgs e)
    {
      if (dgvPost.CurrentRow == null)
        return;
      frmEditPost frm = new frmEditPost(postID, dgvPost.CurrentRow.Cells[1].Value.ToString(),
                                        Convert.ToBoolean(dgvPost.CurrentRow.Cells[2].Value));
      if (frm.isShow) frm.ShowDialog();
      RenewGrid();
    }

    // закрываемся 
    private void frmPost_FormClosing(object sender, FormClosingEventArgs e)
    {
      // разблокировать меню 
      if (ParentForm != null)
        ((frmMain) ParentForm).EnableMenu(true);
    }

    // показать все должности 
    private void btnAllPost_Click(object sender, EventArgs e)
    {
      isAvailablePosts = !isAvailablePosts;
      DataTable dt = new DataTable();
      for (int i = 0; i < 3; i++)
        dt.Columns.Add();
      if (!isAvailablePosts)
      {
        btnAllPost.Text = Resources.ShowAvailablePosts;
        DataManager.Instance.GetAllPosts(ref dt);
      }
      else
      {
        btnAllPost.Text = Resources.ShowAllPosts;
        DataManager.Instance.GetPosts(ref dt);
      }
      FillGrid(dt);
      btnEdit.Enabled = dt.Rows.Count > 0;
    }

    // при изменении-определяем ID 
    private void dgv_SelectionChanged(object sender, EventArgs e)
    {
      if (dgvPost.CurrentRow == null)
        return;
      try
      {
        postID = Convert.ToInt32(dgvPost.CurrentRow.Cells["cId"].Value);
      }
      catch (NullReferenceException)
      {
      }
    }
  }
}