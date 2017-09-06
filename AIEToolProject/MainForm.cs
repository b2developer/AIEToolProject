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
using System.Xml.Serialization;
using AIEToolProject.Source;

namespace AIEToolProject
{
    public partial class MainForm : Form
    {

        public MainForm()
        {
            InitializeComponent();
        }

        /*
        * newButton_Click 
        * 
        * callback when the new button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void newButton_Click(object sender, EventArgs e)
        {
            //create a new EditorForm
            EditorForm newChild = new EditorForm();

            //set the parent
            newChild.MdiParent = this;

            //display the newly created child
            newChild.Show();

            //send the MDI window to the front of the screen
            newChild.Select();
        }


        /*
        * actionButton_Click 
        * 
        * callback when the action button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void actionButton_Click(object sender, EventArgs e)
        {
            EditorForm activeChild = (this.ActiveMdiChild as EditorForm);
            activeChild.spawnType = NodeType.ACTION;
        }


        /*
        * conditionButton_Click 
        * 
        * callback when the condition button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void conditionButton_Click(object sender, EventArgs e)
        {
            EditorForm activeChild = (this.ActiveMdiChild as EditorForm);
            activeChild.spawnType = NodeType.CONDITION;
        }


        /*
        * selectorButton_Click 
        * 
        * callback when the selector button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void selectorButton_Click(object sender, EventArgs e)
        {
            EditorForm activeChild = (this.ActiveMdiChild as EditorForm);
            activeChild.spawnType = NodeType.SELECTOR;
        }

        /*
        * sequenceButton_Click 
        * 
        * callback when the sequence button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void sequenceButton_Click(object sender, EventArgs e)
        {
            EditorForm activeChild = (this.ActiveMdiChild as EditorForm);
            activeChild.spawnType = NodeType.SEQUENCE;
        }


        /*
        * decoratorButton_Click 
        * 
        * callback when the decorator button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void decoratorButton_Click(object sender, EventArgs e)
        {
            EditorForm activeChild = (this.ActiveMdiChild as EditorForm);
            activeChild.spawnType = NodeType.DECORATOR;
        }


        private void MainForm_Load(object sender, EventArgs e)
        {

        }
  

        /*
        * openButton_Click 
        * 
        * callback when the open button is clicked, opens an additional file dialog
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void openButton_Click(object sender, EventArgs e)
        {
            //create a new EditorForm
            EditorForm newChild = new EditorForm();

            //de-serialise a file 
            if (TreeHelper.LoadState(newChild))
            {

                newChild.MdiParent = this;

                //open the window
                newChild.Show();
                newChild.Select();
            }
        }


        /*
        * MainForm_DragDrop 
        * 
        * callback when data from other sources is dragged onto the form
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void MainForm_DragOver(object sender, DragEventArgs e)
        {
            //test that the data is a text file (XML)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }


        /*
        * MainForm_DragDrop 
        * 
        * callback when data from a drag and drop is completed
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            //test that the data is a text file (XML)
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //create a new EditorForm
                EditorForm newChild = new EditorForm();

                //get all of the file paths
                string[] paths = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (TreeHelper.LoadState(newChild, paths[0]))
                {
                    newChild.MdiParent = this;

                    //open the window
                    newChild.Show();
                    newChild.Select();
                }

            }
        }


        /*
        * saveButton_Click 
        * 
        * callback when the save button is clicked, can open an additional file dialog
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void saveButton_Click(object sender, EventArgs e)
        {
            //check that the program has a window open
            if (ActiveMdiChild != null)
            {
                //get the active window
                EditorForm activeChild = (this.ActiveMdiChild as EditorForm);

                //serialise to a file
                TreeHelper.SaveState(activeChild, false);
            }
        }


        /*
        * save_asButton_Click 
        * 
        * callback when the save_as button is clicked, opens an additional file dialog
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void save_asButton_Click(object sender, EventArgs e)
        {
            //check that the program has a window open
            if (ActiveMdiChild != null)
            {
                //get the active window
                EditorForm activeChild = (this.ActiveMdiChild as EditorForm);

                //serialise to a file
                TreeHelper.SaveState(activeChild, true);
            }
        }


        /*
        * exportButton_Click 
        * 
        * callback when the export button is clicked, opens an additional file dialog
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void exportButton_Click(object sender, EventArgs e)
        {
            //check that the program has a window open
            if (ActiveMdiChild != null)
            {
                //create a export menu
                ExportDialog ed = new ExportDialog();

                //set the caller
                ed.caller = ActiveMdiChild as EditorForm;

                ed.Show();
            }
        }

        private void undoButton_Click(object sender, EventArgs e)
        {

        }

        private void loopTimer_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

       
    }
}
