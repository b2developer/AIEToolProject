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

namespace AIEToolProject.Source
{
    //enum type for to identify the behaviour node
    public enum BehaviourType
    {
        CONDITION,
        ACTION,
        SELECTOR,
        SEQUENCE,
        DECORATOR,
    }
    


    /*
    * class BehaviourNode
    * utilises the IComparable interface
    * utilises the ICloneable interface
    * 
    * represents a node in a tree structure
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BehaviourNode : IComparable, ICloneable
    {
        //name of the node
        public string name;

        //index of the node in the nodes container
        public int index = 0;

        //the circle that represents the node
        public Circle collider;

        //array of offsets on the connectors
        public float[] connectorOffsets;

        //type of node that the behaviour node is
        public BehaviourType type = BehaviourType.ACTION;

        //link to the children that this node holds
        public List<BehaviourNode> children;

        [XmlIgnore]
        //link to the node that holds this node
        public BehaviourNode parent = null;

        /*
        * BehaviourNode()
        * default constructor
        */
        public BehaviourNode()
        {
            collider = new Circle();
            children = new List<BehaviourNode>();
        }


        /*
        * SortChildren
        * 
        * sorts children in order in the array from right to left
        * this is used to ensure that they run in the correct order
        * that they are displayed on the screen
        * 
        * @returns void
        */
        public void SortChildren()
        {
            //behaviour nodes have IComparable implemented, allowing children to be sorted this way
            children.Sort();
        }


        /*
        * Clone 
        * implements ICloneable's Clone()
        * 
        * clones the behaviour node
        * 
        * @returns object - the cloned behaviour node
        */
        public object Clone()
        {
            BehaviourNode clone = new BehaviourNode();

            clone.name = name;
            clone.index = index;

            //clone the collider
            clone.collider = new Circle();
            clone.collider.x = collider.x;
            clone.collider.y = collider.y;
            clone.collider.radius = collider.radius;

            clone.connectorOffsets = connectorOffsets;
            clone.type = type;
            clone.parent = parent;
            clone.children = children;

            return clone;
        }


        /*
        * CompareTo
        * implements IComparable's CompareTo(object obj)
        * 
        * compares two behaviour nodes by comparing 
        * the 'x' component of both of their positions
        * 
        * @param object obj - the other behaviour node in it's purest form
        * @returns void
        */
        public int CompareTo(object obj)
        {
            //case the object to it's true type
            BehaviourNode other = (obj as BehaviourNode);

            if (other.collider.x < collider.x)
            {
                //the other element should be before it
                return -1;
            }
            else if (other.collider.x == collider.x)
            {
                //the same
                return 0;
            }
            else
            {
                //this element should be before the other
                return 1;
            }
        }
    }
}
