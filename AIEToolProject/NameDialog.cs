using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIEToolProject.Source.Reference;

namespace AIEToolProject
{
    public partial class NameDialog : Form
    {

        //object that transfers data out of this dialog
        public StringReference immutable = null;

        public NameDialog()
        {
            InitializeComponent();
        }

        /*
        * SetDisplayText 
        * 
        * sets the text displayed in
        * the input box
        * 
        * @param string data
        * @returns void
        */
        public void SetDisplayText(string data)
        {
            inputBox.Text = data;
        }


        private void inputBox_TextChanged(object sender, EventArgs e)
        {
            immutable.data = inputBox.Text;
        }


        private void okButton_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }
    }
}
