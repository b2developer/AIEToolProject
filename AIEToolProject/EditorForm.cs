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
    public partial class EditorForm : Form
    {
        //container of behaviour nodes
        List<BehaviourNode> nodes;

        public EditorForm()
        {
            InitializeComponent();

            nodes = new List<BehaviourNode>();
        }

        /*
        * EditorForm_Click 
        * 
        * callback when the form is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void EditorForm_Click(object sender, EventArgs e)
        {

            //cast the event to it's true type
            MouseEventArgs mouseE = (e as MouseEventArgs);

            //failsafe
            if (mouseE != null)
            {
                //create a new node
                BehaviourNode node = new BehaviourNode();

                //set the position
                node.position = new Point(mouseE.X, mouseE.Y);
                node.type = BehaviourType.ACTION;

                nodes.Add(node);

                this.Refresh();
            }
          
        }

        /*
        * OnPaint
        * protects Form's OnPaint(PaintEventArgs e)
        * 
        * the custom draw call of the form, draws all nodes
        * along with their text and connections
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        */
        protected override void OnPaint(PaintEventArgs e)
        {

            //called the hidden function
            base.OnPaint(e);

            //get the graphics object to paint with
            Graphics g = e.Graphics;

            //iterate through all nodes in the nodes container
            foreach (BehaviourNode b in nodes)
            {
                //deduct what type of node it is
                switch (b.type)
                {
                    case BehaviourType.ACTION:

                        //create the pen and brush to draw the outlined circle with
                        Pen blackPen = new Pen(Color.Black, 2.0f);
                        Brush greenBrush = new SolidBrush(Color.Green);

                        //define a region to draw the node in
                        Rectangle region = new Rectangle(b.position.X - 50, b.position.Y - 50, 100, 100);

                        //draw the node
                        g.FillPie(greenBrush, region, 0.0f, 360.0f);
                        g.DrawArc(blackPen, region, 0.0f, 360.0f);

                    break;
                }
            }
        }

    }
}
