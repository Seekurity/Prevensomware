using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Prevensomware.Dto;
using Prevensomware.Logic;

namespace Prevensomware.GUI
{
    public partial class RevertChanges : Form
    {
        private BoLog boLog;
        private IEnumerable<DtoLog> logList; 
        public RevertChanges()
        {
            InitializeComponent();
            boLog = new BoLog();
        }

        private void RevertChanges_Load(object sender, EventArgs e)
        {
            logList =  boLog.GetList();
            if (!logList.Any())
            {
                MessageBox.Show("No Logs Found.");
                Close();
            }
            gridLogs.DataSource = logList;
            gridLogs.Columns.Remove("RegistryKeyList");
            gridLogs.Columns.Remove("FileList");
            gridLogs.Columns["Oid"].Visible = false;
            gridLogs.Columns["CreateDateTime"].DisplayIndex = 0;
            gridLogs.Columns["CreateDateTime"].HeaderText = "Created At";
            gridLogs.Columns["IsReverted"].HeaderText = "Is Reverted";

            gridLogs.Columns["Payload"].DisplayIndex = 1;
            gridLogs.Columns["Payload"].HeaderText = "Query";
            gridLogs.Columns["SearchPath"].DisplayIndex = 1;
            gridLogs.Columns["SearchPath"].HeaderText = "Search Path";

        }

        private void btnRevertAll_Click(object sender, EventArgs e)
        {
            boLog.RevertAll();
            MessageBox.Show("All changes are reverted.");
            Close();
        }

        private void btnRevert_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow selectedRow in gridLogs.SelectedRows)
            {
                 var dtoLog =logList.Single(x => x.Oid == Convert.ToInt32(selectedRow.Cells["Oid"].Value));
                if (dtoLog.IsReverted)
                {
                    MessageBox.Show("Can't Revert Already Reverted Log.");
                    return;
                }
                 boLog.Revert(dtoLog);
            }
            MessageBox.Show("Changes are reverted.");
            Close();
        }

        private void gridLogs_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            gridLogs.Rows[e.RowIndex].Selected = true;
        }
    }
}
