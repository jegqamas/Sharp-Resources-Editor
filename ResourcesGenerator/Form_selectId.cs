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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
namespace ResourcesGenerator
{
    public partial class Form_selectId : Form
    {
        public Form_selectId()
        {
            InitializeComponent();
            cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo inf in cultures)
            {
                comboBox1.Items.Add(inf.EnglishName + " (" + inf.Name + ")");
            }
            comboBox1.SelectedIndex = 0;
        }
        private CultureInfo[] cultures;
        public CultureInfo SelectedLanguage
        { get { return cultures[comboBox1.SelectedIndex]; } }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!comboBox1.Items.Contains(comboBox1.SelectedItem.ToString()))
            {
                MessageBox.Show("Please select a language !");
                return;
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            Close();
        }
    }
}
