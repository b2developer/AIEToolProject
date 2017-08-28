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
    //custom type for defining the type of connection selection being made
    public enum ConnectionType
    {
        PARENT,
        CHILDREN,
    }


    /*
    * class ConnectionSelection
    * child class of Selection
    * 
    * selection object for handling mouse events on a connection
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class ConnectionSelection : Selection
    {
        //the form being used to select the connection
        public EditorForm form = null;

        //the node that has a connection being selected
        public BehaviourNode node = null;

        //the type of connection being selected on the node
        public ConnectionType type;

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
            //get the amount of scroll that is offsetting the window
            Point scrollPos = form.validScrollPos;

            //cast the event to it's true type
            MouseEventArgs mouseE = (mouseArgs as MouseEventArgs);

            //get the mouse coordinates in global space
            Point trueMousePos = new Point(mouseE.Location.X + scrollPos.X, mouseE.Location.Y + scrollPos.Y);

            //check for collision with all parent connectors
            foreach (BehaviourNode b in form.snapshots.First.nodes)
            {
                //don't check for self collisions
                if (b == node)
                {
                    continue;
                }

                //declare a circle that one of the statements will define below
                Circle circle = null;

                if (type == ConnectionType.PARENT)
                {
                    //get the circle of the other child connector
                    circle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[0], b.collider.radius * form.connectorRatio);
                }
                else if (type == ConnectionType.CHILDREN)
                {
                    //get the circle of the other parent connector
                    circle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[1], b.collider.radius * form.connectorRatio);
                }

                //coordinates of the mouse in global space
                float mx = mouseE.X + scrollPos.X;
                float my = mouseE.Y + scrollPos.Y;

                //if the parent connector is intersecting
                if (circle.IntersectingPoint(mx, my))
                {
                    if (!TreeHelper.IsCyclic(node, b))
                    {
                        //form a connection
                        if (type == ConnectionType.PARENT)
                        {
                            node.children.Add(b);
                            b.parent = node;
                        }
                        else if (type == ConnectionType.CHILDREN)
                        {
                            node.parent = b;
                            b.children.Add(node);
                        }
                    }

                    break;
                }
            }
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
            //get the amount of scroll that is offsetting the window
            Point scrollPos = form.validScrollPos;

            //cast the event to it's true type
            MouseEventArgs mouseE = (mouseArgs as MouseEventArgs);

            //get the mouse coordinates in global space
            Point trueMousePos = new Point(mouseE.Location.X + scrollPos.X, mouseE.Location.Y + scrollPos.Y);

            //circles representing the connector
            Circle parentCircle = new Circle(node.collider.x, node.collider.y + node.connectorOffsets[0], node.collider.radius * form.connectorRatio);
            Circle childCircle = new Circle(node.collider.x, node.collider.y + node.connectorOffsets[1], node.collider.radius * form.connectorRatio);

            //check for intersections with the connector circles before removing the node
            if (childCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y) && node.type != BehaviourType.CONDITION && node.type != BehaviourType.ACTION)
            {
                //iterate through all of the children, disconnecting them from this parent
                foreach (BehaviourNode c in node.children)
                {
                    c.parent = null;
                }

                node.children.Clear();
            }
            else if (parentCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y))
            {
                //check that the node has a parent
                if (node.parent != null)
                {
                    node.parent.children.Remove(node);
                }

                node.parent = null;
            }
            else
            {
                TreeHelper.RemoveFromTree(form.snapshots.First.nodes, node);
            }
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

            if (type == ConnectionType.PARENT)
            {
                form.tx1 = node.collider.x - scrollPos.X;
                form.ty1 = node.collider.y + node.connectorOffsets[1] - scrollPos.Y;

                form.tx2 = trueMousePos.X - scrollPos.X;
                form.ty2 = trueMousePos.Y - scrollPos.Y;
            }
            else if (type == ConnectionType.CHILDREN)
            {
                form.tx1 = node.collider.x - scrollPos.X;
                form.ty1 = node.collider.y + node.connectorOffsets[0] - scrollPos.Y;

                form.tx2 = trueMousePos.X - scrollPos.X;
                form.ty2 = trueMousePos.Y - scrollPos.Y;
            }
        }

    }
}
