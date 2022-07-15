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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
namespace ResourcesGenerator
{
    class Program
    {
        private static CultureInfo[] cultures;
        private static string inputFolder;
        private static string outputFolder;
        private static string languageId;
        [STAThread]
        static void Main(string[] args)
        {
            bool copyOnly = false;
            bool dontCopyEditor = false;
            if (args != null)
            {
                copyOnly = args.Contains("/copy");// copy all resources mode. No language id choosing.
                dontCopyEditor = args.Contains("/noeditor");// If presented, the Resource Editor will not be copied into target folder.
            }
            cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            // Select input folder
            FolderBrowserDialog inputFolderChoose = new FolderBrowserDialog();
            inputFolderChoose.Description = "Select the source folder which contain the resource files (*.resx files)";
            DialogResult res = inputFolderChoose.ShowDialog();
            if (res == DialogResult.OK)
            {
                inputFolder = inputFolderChoose.SelectedPath;
                Console.WriteLine("Input folder= " + inputFolder);
            }

            else
            {
                return;
            }
            // Select output folder
            FolderBrowserDialog outputFolderChoose = new FolderBrowserDialog();
            outputFolderChoose.Description = "Select where to save the generated/copied source files and the editor";
            res = outputFolderChoose.ShowDialog();
            if (res == DialogResult.OK)
            {
                outputFolder = outputFolderChoose.SelectedPath;
                Console.WriteLine("Output folder= " + outputFolder);
            }
            else
            {
                return;
            }
            // Select id !
            if (!copyOnly)
            {
                Form_selectId frm = new Form_selectId();
                res = frm.ShowDialog();
                if (res == DialogResult.Ignore)
                {
                    Console.WriteLine("Language ID= IGNORE");
                    languageId = "";
                }
                else
                {
                    languageId = frm.SelectedLanguage.Name;
                    Console.WriteLine("Language ID= " + languageId);
                }
            }
            // Do the generation !
            string[] folders = Directory.GetDirectories(inputFolder, "*.*", SearchOption.AllDirectories);
            foreach (string inFolder in folders)
            {
                string TargetFolder = inFolder.Replace(inputFolder, outputFolder + "\\Source\\");
                Directory.CreateDirectory(TargetFolder);
                string[] files = Directory.GetFiles(inFolder, "*.resx", SearchOption.TopDirectoryOnly);
                foreach (string file in files)
                {
                    if (copyOnly)
                    {
                        string target = TargetFolder + "\\" + Path.GetFileName(file);
                        Console.WriteLine("Copying file: " + target);
                        if (File.Exists(target))
                        {
                            File.Delete(target);
                            Console.WriteLine("->File is already exist, overwriting: " + target);
                        }
                        File.Copy(file, target);
                    }
                    else if (languageId != "")
                    {
                        string target = "";
                        if (file.Contains("en-US"))
                        {
                            // this is it !
                            target = TargetFolder + "\\" + Path.GetFileNameWithoutExtension(file).Replace(".en-US", "." + languageId) + ".resx";

                            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                            //proc.StartInfo.FileName = @"C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools\ResGen.exe";
                            // proc.StartInfo.Arguments = "\"" + file + "\"" + " " + "\"" + target + "\"";
                            // proc.Start();
                            Console.WriteLine("Copying file: " + target);
                            if (File.Exists(target))
                            {
                                File.Delete(target);
                                Console.WriteLine("->File is already exist, overwriting: " + target);
                            }
                            File.Copy(file, target);
                        }
                        else if (!IsItForCulture(file))
                        {
                            target = TargetFolder + "\\" + Path.GetFileNameWithoutExtension(file) + "." + languageId + ".resx";
                            Console.WriteLine("Copying file: " + target);
                            if (File.Exists(target))
                            {
                                File.Delete(target);
                                Console.WriteLine("->File is already exist, overwriting: " + target);
                            }
                            File.Copy(file, target);
                        }
                    }
                    else
                    {
                        if (!IsItForCulture(file))
                        {
                            string target = TargetFolder + "\\" + Path.GetFileName(file);
                            Console.WriteLine("Copying file: " + target);
                            if (File.Exists(target))
                            {
                                File.Delete(target);
                                Console.WriteLine("->File is already exist, overwriting: " + target);
                            }
                            File.Copy(file, target);
                        }
                    }
                }
            }
            // Copy the editor
            if (File.Exists("SharpResourcesEditor.exe") && !dontCopyEditor)
            {
                Console.WriteLine("Copying the editor ...");

                string currentFolder = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                // Get all files 
                string[] files = Directory.GetFiles(currentFolder, "*", SearchOption.AllDirectories);
                string fileOutPath = "";
                // Copy them
                foreach (string file in files)
                {
                    if (Path.GetFileName(file) != "ResourcesGenerator.exe")
                    {
                        fileOutPath = outputFolder + file.Replace(currentFolder, "");
                        Directory.CreateDirectory(Path.GetDirectoryName(fileOutPath));
                        if (!File.Exists(fileOutPath))
                        {
                            File.Copy(file, fileOutPath);
                            Console.WriteLine("->File copied: " + Path.GetFileName(file));
                        }
                    }
                }

                // Run
                try
                {
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = Path.Combine(outputFolder, "SharpResourcesEditor.exe");
                    proc.StartInfo.WorkingDirectory = outputFolder;
                    proc.Start();
                }
                catch { }
            }
        }
        private static bool IsItForCulture(string file)
        {
            foreach (CultureInfo inf in cultures)
            {
                if (file.Contains(inf.Name))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
