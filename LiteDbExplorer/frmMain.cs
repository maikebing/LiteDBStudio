using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;

namespace LiteDBStudio
{
    public partial class frmMain : RibbonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnOpen_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
       
            if (openFileDialog.ShowDialog()== DialogResult.OK)
            {
                FileInfo fi = new FileInfo(openFileDialog.FileName);
                LiteDB.LiteDatabase liteDatabase = new LiteDB.LiteDatabase(fi.FullName);
                DataSet dataSet = new DataSet(fi.Name);
                liteDatabase.GetCollectionNames().ToList().ForEach(name =>
                {
                    var dt = dataSet.Tables.Add(name);
                    List<string> keys = new List<string>();
                    liteDatabase.GetCollection(name).FindAll().ToList().ForEach(item => keys =  item.Keys.Union(keys).ToList());
                    keys.ForEach(str => dt.Columns.Add(str, typeof(string)));
                    liteDatabase.GetCollection(name).FindAll().ToList().ForEach(item => dt.Rows.Add(item.Values.ToArray().Select(v => v.AsString).ToArray())
                    );
                });
                gcData.DataSource = dataSet;
            }
        }
    }
}
