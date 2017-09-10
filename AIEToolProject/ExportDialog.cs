using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using AIEToolProject.Source;
using AIEToolProject.Source.Exporter;
using ChecklistControl;

namespace AIEToolProject
{
    public partial class ExportDialog : Form
    {

        //reference to the editor that called this export dialog
        public EditorForm caller = null;

        //check list object that the exporter uses to check the button
        public CheckListControl checkList = null;

        //the directory to export to
        public string selectedDirectory = "";

        //the programming language selected to export
        public ProgrammingLanguage selectedLanguage = ProgrammingLanguage.C_Sharp;

        string filePathReason = "no filepath selected";
        string programmingReason = "programming language \nnot implemented";

        public ExportDialog()
        {
            InitializeComponent();
            checkList = this.checkListControl1;

            checkList.RegisterReason(filePathReason, false);
            checkList.RegisterReason(programmingReason, true);

            checkList.button = this.exportButton;

            checkList.button.Enabled = false;
        }


        /*
        * filePathButton_Click 
        * 
        * prompts for a new directory for the export
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void filePathButton_Click(object sender, EventArgs e)
        {
            //search for a folder to use
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();

            //check that a directory was found
            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                selectedDirectory = folderDialog.SelectedPath;

                //display the selected directory
                filePathTextBox.Text = selectedDirectory;

                checkList.ChangeReason(filePathReason, Directory.Exists(selectedDirectory));

            }
        }


        /*
        * exportButton_Click 
        * 
        * writes code bases for a behaviour tree
        * at the set directory
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void exportButton_Click(object sender, EventArgs e)
        {
            //don't attempt to export if the directory hasn't been set
            if (selectedDirectory == "")
            {
                return;
            }

            //get the tree from the editor form
            Tree exportTree = TreeHelper.extractTree(caller);

            //there is nothing to export
            if (exportTree.nodes.Count == 0)
            {
                return;
            }

            //create a new exporter to use
            BaseExporter exporter = null;

            //deduct the selected programming language and create an exporter for it
            if (csRadio.Checked)
            {
                exporter = new CS_Exporter();
            }
            else if (cppRadio.Checked)
            {
                exporter = new CPP_Exporter();
            }
            else if (pythonRadio.Checked)
            {
                throw new NotImplementedException();
            }

            //sort the tree's items so that left children are always executed first
            exportTree.SortTree();

            //give the exporter a tree
            exporter.input = exportTree;

            //set the directory of the exporter
            exporter.exportingPath = selectedDirectory;

            //call the exporter to create the file/s
            exporter.Initialise();
            exporter.CreateInputClass();
            exporter.CreateFunctionReferences();
            exporter.DefineTree();
            exporter.DefineBehaviours();
            exporter.DefineConnections();
            exporter.DefineStructure();
            exporter.AssignFunctionReferences();
            exporter.CleanUp();

            Close();
        }


        /*
        * csRadio_CheckedChanged 
        * 
        * called when the radio button gets changed
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void csRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (csRadio.Checked)
            {
                selectedLanguage = ProgrammingLanguage.C_Sharp;
                checkList.ChangeReason(programmingReason, true);
            }
        }


        /*
        * cppRadio_CheckedChanged 
        * 
        * called when the radio button gets changed
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void cppRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (cppRadio.Checked)
            {
                selectedLanguage = ProgrammingLanguage.C_PlusPlus;
                checkList.ChangeReason(programmingReason, true);
            }
        }


        /*
        * pythonRadio_CheckedChanged 
        * 
        * called when the radio button gets changed
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void pythonRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (pythonRadio.Checked)
            {
                selectedLanguage = ProgrammingLanguage.Python;
                checkList.ChangeReason(programmingReason, false);
            }
        }
    }
}
