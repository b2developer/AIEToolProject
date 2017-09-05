using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIEToolProject.Source;
using AIEToolProject.Source.Exporter;

namespace AIEToolProject
{
    public partial class ExportDialog : Form
    {

        //reference to the editor that called this export dialog
        public EditorForm caller = null;

        //the directory to export to
        public string selectedDirectory = "";

        public ExportDialog()
        {
            InitializeComponent();
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
            BaseExporter exporter = new CS_Exporter();

            //sort the tree's items so that left children are always executed first
            exportTree.SortTree();

            //give the exporter a tree
            exporter.input = exportTree;

            //set the directory of the exporter
            exporter.exportingPath = selectedDirectory;

            //call the exporter to create the file
            exporter.Initialise();
            exporter.CreateInputClass();
            exporter.CreateFunctionReferences();
            exporter.DefineTree();
            exporter.DefineBehaviours();
            exporter.DefineConnections();
            exporter.DefineStructure();
            exporter.AssignFunctionReferences();
            exporter.CleanUp();
        }
    }
}
