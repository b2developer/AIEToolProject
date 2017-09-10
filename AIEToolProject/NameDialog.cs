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
using AIEToolProject.Source.Reference;

namespace AIEToolProject
{
    public partial class NameDialog : Form
    {
        //the string that is being changed
        public StringReference immutable = null;

        public NameDialog()
        {
            InitializeComponent();
        }

        /*
        * SetDisplayText 
        * 
        * sets the contents of the input box
        * 
        * @param string display - the string displayed in the input box
        * @returns void
        */
        public void SetDisplayText(string display)
        {
            this.inputBox.Text = display;
        }


        /*
        * okButton_Click 
        * 
        * callback when the ok button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            immutable.data = this.inputBox.Text;
            Close();
        }
    }
}
