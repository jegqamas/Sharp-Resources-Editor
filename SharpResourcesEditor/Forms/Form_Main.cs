/* This file is part of Sharp Resources Editor
   A program that edit resources of .net project

   Copyright © Alaa Ibrahim Hadid 2013 - 2021

   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.

   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU General Public License for more details.

   You should have received a copy of the GNU General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel.Design;
using System.Resources;
using System.Reflection;

namespace SharpResourcesEditor
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
            this.Text = "Sharp Resources Editor v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            settings.Reload();
            // Load files
            currentResourcesFolderPath = settings.LatestFolder;
            ReloadIDS(currentResourcesFolderPath);
            // Load settings
            this.Location = settings.Win_Location;
            this.Size = settings.Win_size;
        }
        private string[] files;
        private ResXResourceReader currentResource;
        private string currentFilePath = "";
        private string currentResourcesFolderPath = "";
        private Properties.Settings settings = new Properties.Settings();
        private string targetLanguage = "English";
        private string sourceLanguage = "English";
        private bool isTranslating = false;
        private bool stopTranslating = false;

        private void ReloadIDS(string folder)
        {
            files = Directory.GetFiles(folder, "*.resx", SearchOption.AllDirectories);
            // Load filters
            toolStripComboBox1.Items.Clear();
            toolStripComboBox1.Items.Add("All");
            toolStripComboBox1.Items.Add("No ID");
            foreach (string file in files)
            {
                string[] ids = Path.GetFileNameWithoutExtension(file).Split(new char[] { '.' });
                if (ids.Length > 1)
                {
                    // the last element is the id
                    if (!toolStripComboBox1.Items.Contains(ids[ids.Length - 1]))
                        toolStripComboBox1.Items.Add(ids[ids.Length - 1]);
                }
            }
            toolStripComboBox1.SelectedIndex = 0;
        }
        private void RefreshFiles(string folder)
        {
            currentResourcesFolderPath = folder;
            folder_path.Text = folder;
            folder_path.ToolTipText = folder;

            currentResource = null;
            pathLabel.Text = "";
            dataGridView1.Rows.Clear();
            treeView1.Nodes.Clear();
            // Load files
            string[] files = Directory.GetFiles(currentResourcesFolderPath, "*.resx", SearchOption.AllDirectories);
            foreach (string file in files)
            {
                if (toolStripComboBox1.SelectedItem.ToString() == "All")
                {
                    TreeNode tr = new TreeNode();
                    tr.Text = Path.GetFileNameWithoutExtension(file);
                    tr.Tag = file;

                    treeView1.Nodes.Add(tr);
                }
                else if (toolStripComboBox1.SelectedItem.ToString() == "No ID")
                {
                    if (!Path.GetFileNameWithoutExtension(file).Contains('.'))
                    {
                        TreeNode tr = new TreeNode();
                        tr.Text = Path.GetFileNameWithoutExtension(file);
                        tr.Tag = file;

                        treeView1.Nodes.Add(tr);
                    }
                }
                else// Filtering ... 
                {
                    if (Path.GetFileNameWithoutExtension(file).Contains(toolStripComboBox1.SelectedItem.ToString()))
                    {
                        TreeNode tr = new TreeNode();
                        tr.Text = Path.GetFileNameWithoutExtension(file);
                        tr.Tag = file;

                        treeView1.Nodes.Add(tr);
                    }
                }
            }
        }
        private void OpenSelectedResource()
        {
            pathLabel.Text = "";
            dataGridView1.Rows.Clear();
            if (treeView1.SelectedNode == null)
            {
                currentResource = null;
                return;
            }
            currentFilePath = (string)treeView1.SelectedNode.Tag;
            pathLabel.Text = currentFilePath;
            //try
            {
                // Read it
                currentResource = new ResXResourceReader(currentFilePath);
                currentResource.UseResXDataNodes = true;

                foreach (DictionaryEntry ent in currentResource)
                {
                    if (((ResXDataNode)ent.Value).GetValueTypeName((ITypeResolutionService)null).Contains("System.String"))
                    {
                        //if (((ResXDataNode)ent.Value).Name.ToString().EndsWith(".Text") | ((ResXDataNode)ent.Value).Name.EndsWith(".ToolTip") | ((ResXDataNode)ent.Value).Name.EndsWith(".ToolTipText"))
                        string val = "";
                        try
                        {
                            val = ((ResXDataNode)ent.Value).GetValue((ITypeResolutionService)null).ToString();
                        }
                        catch { }
                        dataGridView1.Rows.Add(((ResXDataNode)ent.Value).Name, val, val);
                    }
                }
            }
        }
        private bool GetValue(string key, out string value)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value.ToString() == key)
                {
                    string val = dataGridView1.Rows[i].Cells[2].Value.ToString();
                    if (val != null)
                    { value = val; }
                    else
                    { value = ""; }
                    return true;
                }
            }
            value = null;
            return false;
        }
        // Save
        private void button1_Click(object sender, EventArgs e)
        {
            if (currentResource == null)
                return;
            // Create writer
            ResXResourceWriter writer = new ResXResourceWriter(currentFilePath + "_temp");
            // Add resources
            IDictionaryEnumerator dict = currentResource.GetEnumerator();
            dict.Reset();
            while (dict.MoveNext())
            {
                string val = "";
                if (GetValue(dict.Key.ToString(), out val))
                {
                    writer.AddResource(dict.Key.ToString(), val);
                }
                else
                {
                    writer.AddResource(dict.Key.ToString(), dict.Value);
                }
            }
            currentResource.Close();
            writer.Close();
            // Delete original
            File.Delete(currentFilePath);
            // Copy temp
            File.Copy(currentFilePath + "_temp", currentFilePath);
            // Delete temp
            File.Delete(currentFilePath + "_temp");
            // Refresh to see results
            OpenSelectedResource();
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OpenSelectedResource();
        }
        // Reload
        private void button2_Click(object sender, EventArgs e)
        {
            if (currentResource != null)
            {
                currentResource.Close();
            }
            OpenSelectedResource();
        }
        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshFiles(currentResourcesFolderPath);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            settings.Win_Location = this.Location;
            settings.Win_size = this.Size;
            settings.LatestFolder = currentResourcesFolderPath;
            settings.Save();
        }
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form_About frm = new Form_About();
            frm.ShowDialog(this);
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.S))
            {
                //Save
                button1_Click(this, null);
            }
            if (e.KeyData == (Keys.Control | Keys.R))
            {
                //Reload
                button2_Click(this, null);
            }
        }
       
        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fol = new FolderBrowserDialog();
            fol.Description = "Open the folder that contain the resources files (.resx files)";
            fol.SelectedPath = currentResourcesFolderPath;
            if (fol.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ReloadIDS(fol.SelectedPath);
                RefreshFiles(fol.SelectedPath);
            }
        }
        private void Status_Label_Click(object sender, EventArgs e)
        {
            if (isTranslating)
            {
                stopTranslating = true;
                Status_Label.Text = "Canceling ...";
            }
        }
    }
}
