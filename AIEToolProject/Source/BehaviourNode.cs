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
    * 
    * represents a node in a tree structure
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BehaviourNode
    {
        //name of the node
        public string name;

        //position of the node in the window
        public Point position;

        //type of node that the behaviour node is
        public BehaviourType type = BehaviourType.ACTION;

        //link to the node that holds this node
        public BehaviourNode parent = null;

        //link to the children that this node holds
        public List<BehaviourNode> children;

        /*
        * BehaviourNode()
        * default constructor
        */
        public BehaviourNode()
        {
            position = new Point(0, 0);
        }
    }
}
