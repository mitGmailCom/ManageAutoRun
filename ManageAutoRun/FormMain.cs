using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ManageAutoRun
{
    public partial class FormMain : Form
    {
        private RegistryKey currentUser;
        private RegistryKey software;
        private RegistryKey microsoft;
        private RegistryKey windows;
        private RegistryKey currentVersion;
        private RegistryKey run;

        public FormMain()
        {
            InitializeComponent();
            Load += FormMain_Load;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            LoadPrograms();
        }

        /// <summary>
        /// Invoke OpenSubKeys and SetProgrammsToListBox
        /// </summary>
        private void LoadPrograms()
        {
            OpenSubKeys();
            SetProgrammsToListBox();
        }

        /// <summary>
        /// Open Autorun
        /// </summary>
        private void OpenSubKeys()
        {
            currentUser = Registry.CurrentUser;
            software = currentUser.OpenSubKey("SOFTWARE", true);
            microsoft = software.OpenSubKey("Microsoft", true);
            windows = microsoft.OpenSubKey("Windows", true);
            currentVersion = windows.OpenSubKey("CurrentVersion", true);
            run = currentVersion.OpenSubKey("Run", true);
        }

        /// <summary>
        /// Set programms from autorun in listbox
        /// </summary>
        private void SetProgrammsToListBox()
        {
            var mass = run.GetValueNames();
            listBoxProgramms.Items.AddRange(mass);
        }



        /// <summary>
        /// Click on "Delete"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            if (listBoxProgramms.Items.Count != 0)
            {
                if (listBoxProgramms.SelectedItem != null)
                    run.DeleteValue($"{listBoxProgramms.SelectedItem.ToString()}");
                else
                    MessageBox.Show("Select one programm to delete from auturun", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateListBox();
        }

        /// <summary>
        /// Click on "Add"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string path = GetPath();
            string name = path.Substring(path.LastIndexOf('\\') + 1);

            run.SetValue(name, path);
            UpdateListBox();
        }



        /// <summary>
        /// Get path to selected programm
        /// </summary>
        /// <returns></returns>
        private string GetPath()
        {
            string path = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Exe | *.exe";
            ofd.InitialDirectory = @"C:\Program Files";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                path = ofd.FileName;
            }
            return path;
        }

        /// <summary>
        /// Update values in listbox
        /// </summary>
        private void UpdateListBox()
        {
            listBoxProgramms.Items.Clear();
            SetProgrammsToListBox();
        }

        /// <summary>
        /// Close all subkeys
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            run.Close();
            currentVersion.Close();
            windows.Close();
            microsoft.Close();
            software.Close();
            currentUser.Close();
        }
    }
}
