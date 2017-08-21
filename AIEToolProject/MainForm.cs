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

namespace AIEToolProject
{
    public partial class MainForm : Form
    {

        //enum variable to store the type of behaviour
        public BehaviourType selectedType;

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
            selectedType = BehaviourType.ACTION;
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
            selectedType = BehaviourType.CONDITION;
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
            selectedType = BehaviourType.SELECTOR;
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
            selectedType = BehaviourType.SEQUENCE;
        }



        /*
        * decoratorButton_Click 
        * 
        * callback when the sequence button is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void decoratorButton_Click(object sender, EventArgs e)
        {
            selectedType = BehaviourType.DECORATOR;
        }
    }
}
