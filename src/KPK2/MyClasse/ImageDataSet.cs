using System.Data;
using System.IO;

namespace KPK2.MyClasse
{
    class ImageDataSet
    {
        private static DataSet ds;

        public static DataSet DSet
        {
            get { return ds;}
        }

        public static void CreateDataSet()
        {
            ds = new DataSet();
            DataTable table = new DataTable("ImageTable");
            DataColumn column = new DataColumn("Image");
            column.DataType = System.Type.GetType("System.Byte[]");
            column.AllowDBNull = true;
            column.Caption = "Image";
            table.Columns.Add(column);
            ds.Tables.Add(table);
        }

        public static void AddImage(string path)
        {
            try
            {
                DataRow dr = ds.Tables[0].NewRow();
                FileStream fs = new FileStream(path, FileMode.Open);
                BinaryReader br = new BinaryReader(fs);
                int len = (int) br.BaseStream.Length;
                byte[] mBitmap = br.ReadBytes(len);
                br.Close();
                fs.Close();
                dr["Image"] = mBitmap;
                ds.Tables[0].Rows.Add(dr);
            }
            catch{}
        }
        
        public static void WriteSchema()
        {
            ds.WriteXml(@"dataset.xml",XmlWriteMode.WriteSchema);
        }
    }
}
