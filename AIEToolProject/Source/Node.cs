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
using System.Xml;
using System.Xml.Serialization;
using AIEToolProject.Source.Reference;

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
    * implements ICloneable
    * 
    * a data structure that builds a tree
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class Node : BaseComponent, ICloneable
    {

        //the given name of the node
        public string name = "";

        [XmlIgnore]
        //reference to the form that this resides in
        public EditorForm form = null;

        //main collider
        public Circle collider = null;

        //connection colliders
        public Circle upperConn = null;
        public Circle lowerConn = null;

        //size of the text that the node displays
        public int textSize = 8;

        //used by file I/O / copying to maintain the tree structure
        public int index = 0;

        //the structure of the node in the tree
        [XmlIgnore]
        public Node parent = null;
        public List<Node> children;

        //the type of node
        public NodeType type = NodeType.ACTION;

        //event handling mode of the type
        public NodeFocusType focus = NodeFocusType.NONE;

        //point that the mouse clicked on the node from
        public Point mousePivot = new Point(0, 0);

        //first point that the node was dragged from
        public Point dragPivot = new Point(0, 0);

        //minimum drag distance to be considered a drag and not a rename
        public float minDragDistance = 2.5f;

        //temp variable for max drag distance
        public float maxDrag = 0.0f;

        //temporary line rendering variables when attempting to connect the node
        public bool lineEnabled = false;

        public float lx1 = 0.0f;
        public float lx2 = 0.0f;
        public float ly1 = 0.0f;
        public float ly2 = 0.0f;

        /*
        * public Node() 
        * constructor, defines the new list
        */
        public Node()
        {
            children = new List<Node>();
        }


        /*
        * Clone 
        * implement's ICloneable's Clone()
        * creates another object identical to this
        * 
        * @param object - the object with matching member variables
        */
        public object Clone()
        {
            //create a new node
            Node other = new Node();

            other.name = name;
            other.form = form;

            //deep-copy the circles
            other.collider = collider.Clone() as Circle;
            other.upperConn = upperConn.Clone() as Circle;

            //create a new lower connection only if one exists
            if (lowerConn != null)
            {
                other.collider = collider.Clone() as Circle;
            }

            other.textSize = textSize;
            other.index = index;

            //these don't get copied properly in a tree structure
            other.parent = parent;
            other.children = children;

            other.type = type;
            other.focus = focus;
            other.minDragDistance = minDragDistance;

            return other as object;
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
                    else if (eventListener.mouseEventArgs.Button == MouseButtons.Right)
                    {
                        //disconnect the parent
                        if (parent != null)
                        {
                            parent.children.Remove(this);
                            parent = null;
                        }
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
                    else if (eventListener.mouseEventArgs.Button == MouseButtons.Right)
                    {
                        //disconnect all of the children
                        foreach (Node child in children)
                        {
                            child.parent = null;
                        }

                        children.Clear();
                    }
                }
                //is the mouse clicking the base
                else if (collider.IntersectingPoint(trueMousePosition.X, trueMousePosition.Y))
                {
                    if (eventListener.mouseEventArgs.Button == MouseButtons.Left)
                    {
                        form.exclusives.Add(eventListener);

                        dragPivot = new Point((int)collider.x, (int)collider.y);
                        maxDrag = 0.0f;

                        mousePivot = new Point((int)(trueMousePosition.X - collider.x), (int)(trueMousePosition.Y - collider.y));

                        focus = NodeFocusType.BASE;
                    }
                    else if (eventListener.mouseEventArgs.Button == MouseButtons.Right)
                    {
                        //remove the node from the tree and form

                        form.objects.Remove(this.container);

                        //disconnect the parent
                        if (parent != null)
                        {
                            parent.children.Remove(this);
                        }

                        //disconnect all of the children
                        foreach (Node child in children)
                        {
                            child.parent = null;
                        }
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

                //if the base of the node was selected
                if (focus == NodeFocusType.BASE)
                {
                    //check if the node wasn't dragged far
                    if (maxDrag < minDragDistance)
                    {
                        //create a naming dialog and set the result to this node's name
                        NameDialog nameDialog = new NameDialog();

                        //create an object to transfer to another scope
                        StringReference reference = new StringReference();
                        reference.data = name;

                        nameDialog.immutable = reference;

                        //initialise the name dialog
                        nameDialog.SetDisplayText(name);
                        nameDialog.ShowDialog();

                        name = reference.data;
                    }
                }
                //if the connections were selected, check for another connection that has been intersected
                else if (focus == NodeFocusType.UPPER || focus == NodeFocusType.LOWER)
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

                        Circle trueUpperConn = new Circle(other.collider.x + other.upperConn.x, other.collider.y + other.upperConn.y, other.upperConn.radius);
                        Circle trueLowerConn = null;

                        //check that the node has a lower connection
                        if (other.lowerConn != null)
                        {
                            trueLowerConn = new Circle(other.collider.x + other.lowerConn.x, other.collider.y + other.lowerConn.y, other.lowerConn.radius);
                        }

                        //is the mouse clicking the upper connection circle
                        if (trueUpperConn.IntersectingPoint(lx2, ly2))
                        {
                            //test that the connection wont break the tree structure
                            if (focus == NodeFocusType.LOWER && !TreeHelper.SharedRoot(this, other) && other.parent == null)
                            {
                                //connect this node as a parent of the other
                                other.parent = this;
                                children.Add(other);

                                break;
                            }
                        }
                        //is the mouse clicking the lower connection circle
                        else if (trueLowerConn != null && trueLowerConn.IntersectingPoint(lx2, ly2) && this.parent == null)
                        {
                            //test that the connection wont break the tree structure
                            if (focus == NodeFocusType.UPPER && !TreeHelper.SharedRoot(this, other))
                            {
                                //connect this node as a child of other
                                this.parent = other;
                                other.children.Add(this);

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
                    collider.x = trueMousePosition.X - mousePivot.X;
                    collider.y = trueMousePosition.Y - mousePivot.Y;

                    //test how far the node has been dragged
                    Point dragPoint = new Point((int)(dragPivot.X - collider.x), (int)(dragPivot.Y - collider.y));
                    float dragDist = dragPoint.X * dragPoint.X + dragPoint.Y * dragPoint.Y;

                    if (dragDist > maxDrag)
                    {
                        maxDrag = dragDist;
                    }
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
