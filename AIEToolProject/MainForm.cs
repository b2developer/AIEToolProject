using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
