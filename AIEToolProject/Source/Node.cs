using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIEToolProject.Source
{

    //type for nodes
    public enum NodeType
    {
        ACTION,
        CONDITION,
        SELECTOR,
        SEQUENCE,
        DECORATOR,
    }


    //type for selections
    public enum NodeFocusType
    {
        NONE,
        BASE,
        UPPER,
        LOWER,
    }

    /*
    * class Node
    * child object of BaseComponent
    * 
    * a data structure that builds a tree
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class Node : BaseComponent
    {

        //reference to the form that this resides in
        public EditorForm form = null;

        //main collider
        public Circle collider = null;

        //connection colliders
        public Circle upperConn = null;
        public Circle lowerConn = null;

        //the structure of the node in the tree
        public Node parent;
        public List<Node> children;

        //event handling mode of the type
        public NodeFocusType focus = NodeFocusType.NONE;

        //temporary line rendering variables when attempting to connect the node
        public bool lineEnabled = false;

        public float lx1 = 0.0f;
        public float lx2 = 0.0f;
        public float ly1 = 0.0f;
        public float ly2 = 0.0f;

        /*
        * public Node() 
        * default constructor
        */
        public Node()
        {

        }


        /*
        * MousePressedCallback
        * 
        * called whenever the mouse gets pressed
        * (by an event handler)
        * 
        * @param EventListener eventListener - the component that called the call back
        * @returns void
        */
        public void MousePressedCallback(EventListener eventListener)
        {
            //don't do anything if the form that called this hasn't been referenced properly
            if (form == null)
            {
                return;
            }

            //avoids warning CS1690
            Point scrollPosition = form.safeScrollPosition;

            //the true mouse position relative to the world space
            Point trueMousePosition = new Point(scrollPosition.X + eventListener.mouseEventArgs.X, scrollPosition.Y + eventListener.mouseEventArgs.Y);

            //set this to be an exclusive if there aren't any already
            if (form.exclusives.Count == 0)
            {
                Circle trueUpperConn = new Circle(collider.x + upperConn.x, collider.y + upperConn.y, upperConn.radius);
                Circle trueLowerConn = null;

                //check that the node has a lower connection
                if (lowerConn != null)
                {
                    trueLowerConn = new Circle(collider.x + lowerConn.x, collider.y + lowerConn.y, lowerConn.radius);
                }

                //is the mouse clicking the upper connection circle
                if (trueUpperConn.IntersectingPoint(trueMousePosition.X, trueMousePosition.Y))
                {
                    if (eventListener.mouseEventArgs.Button == MouseButtons.Left)
                    {
                        form.exclusives.Add(eventListener);
                        focus = NodeFocusType.UPPER;

                        lx1 = trueUpperConn.x;
                        ly1 = trueUpperConn.y;
                        lx2 = trueMousePosition.X;
                        ly2 = trueMousePosition.Y;

                        lineEnabled = true;
                    }
                }
                //is the mouse clicking the lower connection circle
                else if (trueLowerConn != null && trueLowerConn.IntersectingPoint(trueMousePosition.X, trueMousePosition.Y))
                {
                    if (eventListener.mouseEventArgs.Button == MouseButtons.Left)
                    {
                        form.exclusives.Add(eventListener);
                        focus = NodeFocusType.LOWER;

                        lx1 = trueLowerConn.x;
                        ly1 = trueLowerConn.y;
                        lx2 = trueMousePosition.X;
                        ly2 = trueMousePosition.Y;

                        lineEnabled = true;
                    }
                }
                //is the mouse clicking the base
                else if (collider.IntersectingPoint(trueMousePosition.X, trueMousePosition.Y))
                {
                    if (eventListener.mouseEventArgs.Button == MouseButtons.Left)
                    {
                        form.exclusives.Add(eventListener);
                        focus = NodeFocusType.BASE;
                    }
                    else if (eventListener.mouseEventArgs.Button == MouseButtons.Right)
                    {
                        form.objects.Remove(this.container);
                    }
                }

              
            }
     
        }


        /*
        * MouseReleasedCallback
        * 
        * called whenever the mouse gets released
        * (by an event handler)
        * 
        * @param EventListener eventListener - the component that called the call back
        * @returns void
        */
        public void MouseReleasedCallback(EventListener eventListener)
        {
            //don't do anything if the form that called this hasn't been referenced properly
            if (form == null)
            {
                return;
            }

            //remove this event listener from the list (only if it was already in there)
            if (form.exclusives.Contains(eventListener))
            {
                form.exclusives.Remove(eventListener);

                //if the connections were selected, check for another connection that has been intersected
                if (focus == NodeFocusType.UPPER || focus == NodeFocusType.LOWER)
                {
                    List<Node> otherNodes = new List<Node>();

                    //get the size of the form's objects list
                    int size = form.objects.Count;

                    //iterate through all of the objects
                    for (int i = 0; i < size; i++)
                    {
                        //temp varaible for readability and performance
                        BaseObject obj = form.objects[i];

                        //get the size of the objects components list
                        int compSize = obj.components.Count;

                        //iterate through all of the components in the object
                        for (int j = 0; j < compSize; j++)
                        {
                            //temp variable for readablilty and performance
                            BaseComponent comp = obj.components[j];

                            //this object is a node, compare it's circles for structure extensions
                            if (comp is Node && (comp as Node) != this)
                            {
                                otherNodes.Add(comp as Node);
                            }
                        }
                    }

                    //get the size of the other nodes list
                    int otherSize = otherNodes.Count;

                    //iterate through all of the other nodes
                    for (int i = 0; i < otherSize; i++)
                    {
                        //store in temp value for readability and performance
                        Node other = otherNodes[i];

                        Circle trueUpperConn = new Circle(collider.x + upperConn.x, collider.y + upperConn.y, upperConn.radius);
                        Circle trueLowerConn = null;

                        //check that the node has a lower connection
                        if (lowerConn != null)
                        {
                            trueLowerConn = new Circle(collider.x + lowerConn.x, collider.y + lowerConn.y, lowerConn.radius);
                        }

                        //is the mouse clicking the upper connection circle
                        if (trueUpperConn.IntersectingPoint(lx2, ly2))
                        {
                            if (focus == NodeFocusType.LOWER)
                            {
                                //form connection
                                break;
                            }
                        }
                        //is the mouse clicking the lower connection circle
                        else if (trueLowerConn != null && trueLowerConn.IntersectingPoint(lx2, ly2))
                        {
                            if (focus == NodeFocusType.UPPER)
                            {
                                //form connection
                                break;
                            }
                        }

                    }
                }


                focus = NodeFocusType.NONE;
                lineEnabled = false;
            }
        }


        /*
        * MouseMovedCallback
        * 
        * called whenever the mouse gets pressed
        * (by an event handler)
        * 
        * @param EventListener eventListener - the component that called the call back
        * @returns void
        */
        public void MouseMovedCallback(EventListener eventListener)
        { 
            //don't do anything if the form that called this hasn't been referenced properly
            if (form == null)
            {
                return;
            }

            //remove this event listener from the list (only if it was already in there)
            if (form.exclusives.Contains(eventListener))
            { 

                //avoids warning CS1690
                Point scrollPosition = form.safeScrollPosition;

                //the true mouse position relative to the world space
                Point trueMousePosition = new Point(scrollPosition.X + eventListener.mouseEventArgs.X, scrollPosition.Y + eventListener.mouseEventArgs.Y);

                //test what type of event handling is being executed
                if (focus == NodeFocusType.BASE)
                {
                    //set the new position
                    collider.x = trueMousePosition.X;
                    collider.y = trueMousePosition.Y;
                }
                else if (focus == NodeFocusType.UPPER || focus == NodeFocusType.LOWER)
                {
                    lx2 = trueMousePosition.X;
                    ly2 = trueMousePosition.Y;
                }
            }
        }
    }
}
