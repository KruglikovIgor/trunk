using KPK2.Properties;
using Sparc.KPK2.Config.Hardware;
using Sparc.KPK2.NFunctions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Globals = KPK2.Common.Globals;

namespace KPK2.GeneralForms.ServiceForm
{
  public partial class frmCalibrate : Form
  {
    private DataTable dtDev = new DataTable();
    private Dictionary<int, string> devices;
    private const string colName = "ParamName";
    private const string colNote = "Note";
    private const string colKoef = "Koef";
    private const string colDevice = "DeviceId";
    private const string colCommand = "Command";
    private const string colMin = "MinValue";
    private const string colMax = "MaxValue";
    private const string colPrecision = "Precision";
    private const string colId = "ParamId";
    private const string colAddCommand = "AddCommand";


    private const int numColName = 2;
    private const int numColNote = 1;
    private const int numColKoef = 7;
    private const int numColDevice = 3;
    private const int numColCommand = 8;
    private const int numColAddCommand = 9;
    private const int numColMin = 4;
    private const int numColMax = 5;
    private const int numColPrecision = 6;
    private const int numColId = 0;

    private const string CalibrationXmlFileName = "KPK2CalibrationCoefficients.xml";

        public frmCalibrate()
    {
      InitializeComponent();
    }

    private void frmCalibrate_Load(object sender, EventArgs e)
    {
      FillList();
      FillMainTable();
#if (emulation)
      dgvParams.ReadOnly = true;
      dgvParams.AllowUserToAddRows = true;
      dgvParams.ReadOnly = false;
#endif
    }

    private void FillMainTable()
    {
      dgvParams.ReadOnly = true;
      DataTable parameters = DataManager.Instance.GetAllParams();
      DataGridViewComboBoxColumn cDevices = (DataGridViewComboBoxColumn) dgvParams.Columns["cDevice"];
      if (cDevices == null)
        return;
      cDevices.DataSource = dtDev;
      cDevices.DisplayMember = "Name";
      cDevices.ValueMember = "Id";

      foreach (DataRow dtr in parameters.Rows)
      {
        dgvParams.Rows.Add(dtr[colId], dtr[colNote], dtr[colName], dtr[colDevice], dtr[colMin], dtr[colMax],
                           dtr[colPrecision], dtr[colKoef], dtr[colCommand], dtr[colAddCommand]);
        if (ContainOftenParams(dtr[colName].ToString()))
          oftenParamsRows.Add(dgvParams.Rows.Count - 1);
      }
      dgvParams.ReadOnly = false;
    }

    private bool ContainOftenParams(string name)
    {
      foreach (string str in oftenParams)
        if (str == name)
          return true;
      return false;
    }

    private string[] oftenParams =
      {
        "Wx_PKV",
        "Wy_PKV", "Wz_PKV", "Wx_NKV", "Wy_NKV", "Wz_NKV", "Wx_SPSV", "Wy_SPSV", "Wz_SPSV", "Wx_Tester",
        "Wy_Tester", "Wz_Tester", "Alfa", "W_7V", "HV"
      };

    private string AttestatinSign = "[АТТ]";

    private List<int> oftenParamsRows = new List<int>();

    private void ShowOftenMainTable()
    {
      dgvParams.ReadOnly = true;
      dgvParams.Rows.Clear();
      DataTable parameters = DataManager.Instance.GetAllParams();
      foreach (DataRow dtr in parameters.Rows)
      {
        if (dtr[colNote].ToString().Contains(AttestatinSign))
          dgvParams.Rows.Add(dtr[colId], dtr[colNote], dtr[colName], dtr[colDevice], dtr[colMin], dtr[colMax],
                             dtr[colPrecision], dtr[colKoef], dtr[colCommand], dtr[colAddCommand]);
      }
      //foreach (DataGridViewRow row in dgvParams.Rows)
      //  row.Visible = oftenParamsRows.Contains(row.Index);
      dgvParams.ReadOnly = false;
    }

    private void ShowAllMainTable()
    {
      dgvParams.ReadOnly = true;
      dgvParams.Rows.Clear();
      DataTable parameters = DataManager.Instance.GetAllParams();
      foreach (DataRow dtr in parameters.Rows)
      {
        dgvParams.Rows.Add(dtr[colId], dtr[colNote], dtr[colName], dtr[colDevice], dtr[colMin], dtr[colMax],
                           dtr[colPrecision], dtr[colKoef], dtr[colCommand], dtr[colAddCommand]);
      }
      //foreach (DataGridViewRow row in dgvParams.Rows)
      //    row.Visible = true;
      dgvParams.ReadOnly = false;
    }

    private void FillList()
    {
      devices = DataManager.Instance.GetModules();
      dtDev.Columns.Add("Id");
      dtDev.Columns.Add("Name");
      dtDev.Rows.Add(0, Resources.Report);
      if (devices != null)
        foreach (KeyValuePair<int, string> dev in devices)
          dtDev.Rows.Add(dev.Key, dev.Value);
    }



    private void dgvParams_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
      if (((DataGridView) sender).ReadOnly)
        return;
      if (e.RowIndex >= 0)
      {
        ((DataGridView) sender).Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = ChangedColor;
        bSave.Enabled = true;
      }

    }

    private void dgvParams_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
      if (((DataGridView) sender).ReadOnly)
        return;
      foreach (DataGridViewCell cell in ((DataGridView) sender).Rows[e.RowIndex - 1].Cells)
        cell.Style.BackColor = ChangedColor;
    }

    public static readonly Color ChangedColor = Color.Red;
    public static readonly Color UnchangedColor = Color.White;

    private void RowSaved(int rowIndex)
    {
      foreach (DataGridViewCell cell in dgvParams.Rows[rowIndex].Cells)
        cell.Style.BackColor = UnchangedColor;
    }

    private bool SaveNewRow(int rowIndex)
    {
      DataGridViewCellCollection coll = dgvParams.Rows[rowIndex].Cells;
      foreach (DataGridViewCell cell in coll)
        if (cell.Value == null)
          cell.Value = "";
      coll[numColId].Value = DataManager.Instance.InsertNewParam(coll[numColName].Value.ToString(),
                                                                   Convert.ToInt32(coll[numColDevice].Value),
                                                                   coll[numColCommand].Value.ToString(),
                                                                   Globals.MoveToDouble(
                                                                     coll[numColKoef].Value.ToString()),
                                                                   Globals.MoveToDouble(coll[numColMin].Value.ToString()),
                                                                   Globals.MoveToDouble(coll[numColMax].Value.ToString()),
                                                                   Convert.ToInt32(coll[numColPrecision].Value),
                                                                   coll[numColNote].Value.ToString(),
                                                                   coll[numColAddCommand].Value.ToString());
      if (Convert.ToInt32(coll[numColId].Value) != 0)
      {
        RowSaved(rowIndex);
        return true;
      }
      return false;
    }

    private bool Save()
    {
      bool saved = true;
      foreach (DataGridViewRow row in dgvParams.Rows)
      {
        bool isNewRow = true;
        bool isChangedRow = false;
        foreach (DataGridViewCell cell in row.Cells)
          if (cell.Style.BackColor == ChangedColor)
            isChangedRow = true;
          else
            isNewRow = false;
        if (isNewRow)
          saved &= SaveNewRow(row.Index);
        else if (isChangedRow)
          saved &= SaveChangedRow(row.Index);
      }
      return saved;
    }

    private bool SaveChangedRow(int rowIndex)
    {
      DataGridViewCellCollection coll = dgvParams.Rows[rowIndex].Cells;
      foreach (DataGridViewCell cell in coll)
        if (cell.Value == null)
          cell.Value = "";
      try
      {
        if (DataManager.Instance.UpdateParam(Convert.ToInt32(coll[numColId].Value),
                                               coll[numColName].Value.ToString(),
                                               Convert.ToInt32(coll[numColDevice].Value),
                                               coll[numColCommand].Value.ToString(),
                                               Globals.MoveToDouble(coll[numColKoef].Value.ToString()),
                                               Globals.MoveToDouble(coll[numColMin].Value.ToString()),
                                               Globals.MoveToDouble(coll[numColMax].Value.ToString()),
                                               Convert.ToInt32(coll[numColPrecision].Value),
                                               coll[numColNote].Value.ToString(),
                                               coll[numColAddCommand].Value.ToString()))
        {
          RowSaved(rowIndex);
          return true;
        }
      }
      catch
      {
      }
      return false;
    }

    private void bSave_Click(object sender, EventArgs e)
    {
      if (!Save())
        MessageBox.Show(Resources.SaveNotCompleted);
      else
        bSave.Enabled = false;
    }

    private const int LTRDeviceId = 9;

    private void bGenSinSettings_Click(object sender, EventArgs e)
    {
      FrmGenSinSettings _frm = new FrmGenSinSettings();
      _frm.ShowDialog();
    }

    private void dgvParams_CellClick(object sender, DataGridViewCellEventArgs e)
    {
      if (e.ColumnIndex == numColCommand)
      {
        DataGridView curGrid = ((DataGridView) sender);
        if (Convert.ToInt32(curGrid.Rows[e.RowIndex].Cells[numColDevice].Value) != LTRDeviceId)
          return;
        frmLTRSignals frm = new frmLTRSignals();
        int currentSignal = 0;
        if ((curGrid.Rows[e.RowIndex].Cells[numColCommand].Value != null) &&
            (curGrid.Rows[e.RowIndex].Cells[numColCommand].Value.ToString() != ""))
          currentSignal = Convert.ToInt32(curGrid.Rows[e.RowIndex].Cells[numColCommand].Value);
        frm.CurSignal = currentSignal;
        frm.ShowDialog();
        if ((curGrid.CurrentRow != null) && (currentSignal != frm.CurSignal))
          curGrid.Rows[curGrid.CurrentRow.Index].Cells[numColCommand].Value = frm.CurSignal;
      }
    }

    private void cbOftenUsages_CheckedChanged(object sender, EventArgs e)
    {
      cbOftenUsages.Enabled = false;
      if (cbOftenUsages.Checked)
      {
          ShowOftenMainTable();
          bSaveToXMLFile.Enabled = false;
          bReadFromXMLFile.Enabled = false;
      }
      else
      {
          ShowAllMainTable();
          bSaveToXMLFile.Enabled = true;
          bReadFromXMLFile.Enabled = true;
      }
      cbOftenUsages.Enabled = true;
    }

        private void bSaveToXMLFile_Click(object sender, EventArgs e)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "Sparc", "KPK2");
            string fullPath = Path.Combine(folder, CalibrationXmlFileName);

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Сохранить уалибровочные коэффициенты в XML файл";
                sfd.Filter = "XML файл (*.xml)|*.xml|Все файлы (*.*)|*.*";
                sfd.DefaultExt = "xml";
                sfd.AddExtension = true;
                sfd.OverwritePrompt = true;
                sfd.FileName = "Params_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";

                sfd.InitialDirectory = Directory.Exists(folder) ? folder : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                sfd.FileName = CalibrationXmlFileName; // "KPK2CalibrationCoefficients.xml"

                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                try
                {
                    if (string.IsNullOrWhiteSpace(folder))
                        throw new InvalidOperationException(
                            "Не задан путь к общей папке (KPK2HardwareSettings.pathSharedFolder).");

                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    SaveGridToXml(fullPath);

                    MessageBox.Show(this,
                        "Калибровочные коэффициенты успешно сохранены:\n" + fullPath,
                        "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        "Ошибка сохранения:\n" + ex.Message,
                        "Экспорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveGridToXml(string fileName)
        {
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                NewLineChars = Environment.NewLine,
                NewLineHandling = NewLineHandling.Replace,
                Encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false) // без BOM
            };

            using (var writer = XmlWriter.Create(fileName, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("CalibrationParams");
                writer.WriteAttributeString("SavedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                writer.WriteAttributeString("Source", "frmCalibrate.dgvParams");
                writer.WriteAttributeString("RowCount", dgvParams.Rows.Count.ToString());

                // --- Секция с описанием колонок ---
                writer.WriteStartElement("Columns");
                foreach (DataGridViewColumn col in dgvParams.Columns)
                {
                    writer.WriteStartElement("Column");
                    writer.WriteAttributeString("Index", col.Index.ToString());
                    writer.WriteAttributeString("Name", col.Name ?? string.Empty);
                    writer.WriteAttributeString("HeaderText", col.HeaderText ?? string.Empty);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // Columns

                // --- Строки ---
                writer.WriteStartElement("Rows");
                foreach (DataGridViewRow row in dgvParams.Rows)
                {
                    if (row.IsNewRow) // пустая строка "для добавления" — пропускаем
                        continue;

                    writer.WriteStartElement("Row");
                    writer.WriteAttributeString("Index", row.Index.ToString());

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.ColumnIndex < 0 || cell.ColumnIndex >= dgvParams.Columns.Count)
                            continue;

                        string columnName = dgvParams.Columns[cell.ColumnIndex].Name;
                        if (string.IsNullOrEmpty(columnName))
                            columnName = "Column" + cell.ColumnIndex;

                        string value =
                            (cell.Value == null || cell.Value == DBNull.Value)
                                ? string.Empty
                                : cell.Value.ToString();

                        writer.WriteStartElement("Cell");
                        writer.WriteAttributeString("Column", columnName);
                        writer.WriteAttributeString("Index", cell.ColumnIndex.ToString());
                        writer.WriteValue(value);
                        writer.WriteEndElement(); // Cell
                    }

                    writer.WriteEndElement(); // Row
                }
                writer.WriteEndElement(); // Rows

                writer.WriteEndElement(); // CalibrationParams
                writer.WriteEndDocument();
            }
        }

        private void bReadFromXMLFile_Click(object sender, EventArgs e)
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments),
                "Sparc", "KPK2");
            string defaultFile = Path.Combine(folder, CalibrationXmlFileName);

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Загрузить калибровочные коэффициенты из XML";
                ofd.Filter = "XML файл (*.xml)|*.xml|Все файлы (*.*)|*.*";
                ofd.FilterIndex = 1;
                ofd.DefaultExt = "xml";
                ofd.CheckFileExists = true;
                ofd.CheckPathExists = true;
                ofd.InitialDirectory = Directory.Exists(folder)
                    ? folder
                    : Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                ofd.FileName = CalibrationXmlFileName; // KPK2CalibrationCoefficients.xml

                if (ofd.ShowDialog(this) != DialogResult.OK)
                    return;

                try
                {
                    ReadXmlToGrid(ofd.FileName);
                    MessageBox.Show(this,
                        "Данные успешно загружены из файла:\n" + ofd.FileName,
                        "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this,
                        "Ошибка загрузки:\n" + ex.Message,
                        "Импорт", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ReadXmlToGrid(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Не задан путь к файлу.", nameof(fileName));
            if (!File.Exists(fileName))
                throw new FileNotFoundException("Файл не найден.", fileName);

            // ---------- 1. Разбор XML ----------
            var doc = new XmlDocument();
            doc.Load(fileName);

            XmlElement root = doc.DocumentElement;
            if (root == null || root.Name != "CalibrationParams")
                throw new InvalidDataException(
                    "Корневой элемент файла должен быть <CalibrationParams>.");

            XmlNode rowsNode = root.SelectSingleNode("Rows");
            if (rowsNode == null)
                throw new InvalidDataException("В файле отсутствует секция <Rows>.");

            var parsedRows = new List<Dictionary<string, string>>();
            foreach (XmlNode rowNode in rowsNode.SelectNodes("Row"))
            {
                var row = new Dictionary<string, string>(StringComparer.Ordinal);
                foreach (XmlNode cellNode in rowNode.SelectNodes("Cell"))
                {
                    XmlAttribute colAttr = cellNode.Attributes?["Column"];
                    if (colAttr == null || string.IsNullOrEmpty(colAttr.Value))
                        continue;
                    row[colAttr.Value] = cellNode.InnerText ?? string.Empty;
                }
                parsedRows.Add(row);
            }

            if (parsedRows.Count == 0)
                throw new InvalidDataException("В файле нет ни одной строки <Row>.");

            // ---------- 2. Проверка соответствия колонок ----------
            var gridColumnNames = new HashSet<string>(StringComparer.Ordinal);
            foreach (DataGridViewColumn col in dgvParams.Columns)
                if (!string.IsNullOrEmpty(col.Name))
                    gridColumnNames.Add(col.Name);

            var unknownColumns = new HashSet<string>(StringComparer.Ordinal);
            foreach (var row in parsedRows)
                foreach (string name in row.Keys)
                    if (!gridColumnNames.Contains(name))
                        unknownColumns.Add(name);

            if (unknownColumns.Count > 0)
            {
                var answer = MessageBox.Show(this,
                    "В файле найдены колонки, которых нет в таблице:\n  " +
                    string.Join(", ", unknownColumns) +
                    "\n\nОни будут проигнорированы. Продолжить?",
                    "Импорт", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (answer != DialogResult.OK)
                    return;
            }

            // ---------- 3. Снимок текущего содержимого грида (по ParamId) ----------
            var oldById = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;

                var snapshot = new Dictionary<string, string>(StringComparer.Ordinal);
                string id = null;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex < 0 || cell.ColumnIndex >= dgvParams.Columns.Count)
                        continue;

                    string colName = dgvParams.Columns[cell.ColumnIndex].Name;
                    if (string.IsNullOrEmpty(colName)) continue;

                    string v = (cell.Value == null || cell.Value == DBNull.Value)
                        ? string.Empty
                        : cell.Value.ToString();

                    snapshot[colName] = v;

                    if (cell.ColumnIndex == numColId)   // <-- было: colName == colId
                        id = v;
                }

                if (!string.IsNullOrEmpty(id))
                    oldById[id] = snapshot;
            }

            // ---------- 4. Заполнение грида ----------
            dgvParams.ReadOnly = true;
            try
            {
                dgvParams.Rows.Clear();

                int colCount = dgvParams.Columns.Count;
                foreach (var newRow in parsedRows)
                {
                    var values = new object[colCount];
                    for (int i = 0; i < colCount; i++)
                    {
                        string colName = dgvParams.Columns[i].Name;
                        string text;
                        values[i] = (!string.IsNullOrEmpty(colName) && newRow.TryGetValue(colName, out text))
                            ? text
                            : string.Empty;
                    }
                    dgvParams.Rows.Add(values);
                }
            }
            finally
            {
                dgvParams.ReadOnly = false;
            }

            // ---------- 5. Пометка изменённых ячеек ----------
            bool anyChanged = false;

            foreach (DataGridViewRow row in dgvParams.Rows)
            {
                if (row.IsNewRow) continue;

                // Определяем ParamId текущей строки
                string id = null;
                var idCell = row.Cells[numColId];
                if (idCell.Value != null && idCell.Value != DBNull.Value)
                    id = idCell.Value.ToString();

                Dictionary<string, string> oldRow = null;                    // <-- было пропущено
                bool isNewRow = string.IsNullOrEmpty(id)
                                || !oldById.TryGetValue(id, out oldRow);

                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex < 0 || cell.ColumnIndex >= dgvParams.Columns.Count)
                        continue;

                    string colName = dgvParams.Columns[cell.ColumnIndex].Name;
                    if (string.IsNullOrEmpty(colName)) continue;

                    string newVal = (cell.Value == null || cell.Value == DBNull.Value)
                        ? string.Empty
                        : cell.Value.ToString();

                    bool changed;
                    if (isNewRow)
                    {
                        changed = true;
                    }
                    else
                    {
                        string oldVal;
                        bool hasOldValue = oldRow.TryGetValue(colName, out oldVal);
                        changed = !hasOldValue ||
                                  !string.Equals(oldVal, newVal, StringComparison.Ordinal);
                    }

                    if (changed)
                    {
                        cell.Style.BackColor = ChangedColor;
                        anyChanged = true;
                    }
                }
            }

            // ---------- 6. Кнопка «Сохранить» ----------
            bSave.Enabled = anyChanged;

            // ---------- 7. Пересборка списка «часто используемых» строк ----------
            oftenParamsRows.Clear();
            for (int i = 0; i < dgvParams.Rows.Count; i++)
            {
                if (dgvParams.Rows[i].IsNewRow) continue;

                var nameCell = dgvParams.Rows[i].Cells[numColName];
                if (nameCell.Value != null && ContainOftenParams(nameCell.Value.ToString()))
                    oftenParamsRows.Add(i);
            }
        }
    }
}