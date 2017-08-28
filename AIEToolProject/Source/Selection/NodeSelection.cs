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
using AIEToolProject.Source.Selection;


namespace AIEToolProject.Source.Selection
{
    /*
    * class NodeSelection
    * child class of Selection
    * 
    * selection object for handling mouse events on a node
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class NodeSelection : Selection
    {
        //the form being used to select the node
        public EditorForm form = null;

        //the node that is being selected
        public BehaviourNode node = null;

        /*
        * LeftDown 
        * overrides Selection's LeftDown()
        * 
        * called when the user first presses the left mouse button
        * 
        * @returns void
        */
        public override void LeftDown()
        {

        }


        /*
        * LeftUp 
        * overrides Selection's LeftUp()
        * 
        * called when the user first releases the left mouse button
        * 
        * @returns void
        */
        public override void LeftUp()
        {

        }


        /*
        * RightDown 
        * overrides Selection's RightDown()
        * 
        * called when the user first presses the right mouse button
        * 
        * @returns void
        */
        public override void RightDown()
        {

        }


        /*
        * RightUp 
        * overrides Selection's RightUp()
        * 
        * called when the user first releases the right mouse button
        * 
        * @returns void
        */
        public override void RightUp()
        {

        }


        /*
        * LeftClick 
        * overrides Selection's LeftClick()
        * 
        * called when the user clicks the left mouse button
        * 
        * @returns void
        */
        public override void LeftClick()
        {

        }


        /*
        * RightClick 
        * overrides Selection's RightClick()
        * 
        * called when the user clicks the right mouse button
        * 
        * @returns void
        */
        public override void RightClick()
        {

        }


        /*
        * Move 
        * overrides Selection's Move()
        * 
        * called when the user moves the mouse
        * 
        * @returns void
        */
        public override void Move()
        {
            //cast the event to it's true type
            MouseEventArgs mouseE = (mouseArgs as MouseEventArgs);

            //get the amount of scroll that is offsetting the window
            Point scrollPos = form.validScrollPos;

            Point trueMousePos = new Point(mouseE.Location.X + scrollPos.X, mouseE.Location.Y + scrollPos.Y);

            node.collider.x = trueMousePos.X;
            node.collider.y = trueMousePos.Y;

        }
    }

}

