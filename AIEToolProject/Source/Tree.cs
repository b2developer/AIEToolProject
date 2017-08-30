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

namespace AIEToolProject.Source
{

    /*
    * class Tree
    * 
    * data structure based on one parent and multiple children
    * creating a branching structure
    * 
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class Tree
    {
        public List<Node> nodes = new List<Node>();

        /*
        * Tree()
        * default constructor
        */
        public Tree() { }


        /*
        * Index 
        * 
        * gives each node the index that they have in this list
        * 
        * @returns void
        */
        public void Index()
        {
            //get the size of the nodes list
            int size = nodes.Count;

            //iterate through the list, indexing each node
            for (int i = 0; i < size; i++)
            {
                //store in a temp value for performance and readability
                Node n = nodes[i];

                n.index = i;
            }
        }


        /*
        * ReferenceByIndex
        * 
        * reassigns lost node references using indices from this list
        * 
        * @returns void
        */
        public void ReferenceByIndex()
        {
            //get the size of the nodes list
            int size = nodes.Count;

            //iterate through the list, relinking each node to it's parent and children
            for (int i = 0; i < size; i++)
            {
                //store in a temp value for performance and readability
                Node n = nodes[i];

                //get the size of the children list
                int childSize = n.children.Count;

                //iterate through the children, relinking them
                for (int j = 0; j < childSize; j++)
                {
   
                    //relink the reference
                    n.children[j] = nodes[n.children[j].index];

                    //relink the parent reference of the child
                    n.children[j].parent = n;
                }
            }
        }
    }
}
