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


        /*
        * SortTree 
        * 
        * sorts the nodes in the tree
        * with their brothers (nodes with the same parent)
        * 
        * @returns void
        */
        public void SortTree()
        {
            //get the size of the nodes list
            int nodeSize = nodes.Count;

            //iterate through all of the nodes, sorting each of their children
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temp value for performance and readability
                Node node = nodes[i];

                //sort the children
                node.children.Sort();
            }
        }


        /*
        * GetRoot 
        * 
        * recursively gets the uppermost parent of a node
        * 
        * @param Node node - the node to get the uppermost parent of
        * @returns Node - reference to the uppermost parent
        */
        public Node GetRoot(Node node)
        {
            //call the function again on the parent until one doesn't exist
            if (node.parent != null)
            {
                return GetRoot(node.parent);
            }

            return node;
        }


        /*
        * GetMostCommonRoot 
        * 
        * gets the node that is most likely the root of the tree
        * 
        * this is necessary since there are circumstances
        * where the tree structure can be broken and it
        * isn't clear which node is the root
        * 
        * @returns Node - the node that is most likely the root of the tree
        */
        public Node GetMostCommonRoot()
        {
            //containers to count how many children (not just immediate children) each node has
            List<int> scores = new List<int>();
            List<Node> roots = new List<Node>();

            //get the size of the nodes list
            int nodeSize = nodes.Count;

            //(stored here to be used in multiple scopes)
            int rootSize = 0;

            //iterate through all of the nodes, sorting each of their children
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temp value for performance and readability
                Node node = nodes[i];

                //get the root of the node
                Node root = GetRoot(node);

                //flag indicating that the root is already in the roots container
                bool found = false;

                //get the size of the roots list
                rootSize = roots.Count;

                //iterate through the roots list, searching for the root of the current node
                for (int j = 0; j < rootSize; j++)
                {
                    //store in a temp value for performance and readability
                    Node candidate = roots[j];

                    //compare the search key
                    if (candidate == root)
                    { 
                        scores[j]++;
                        found = true;
                        break;
                    }
                }

                //check if the node was found
                if (!found)
                {
                    //add a new score and reference for the root
                    scores.Add(0);
                    roots.Add(root);
                }
            }

            //get the size of the roots list
            rootSize = roots.Count;

            //track the best index and score
            int bestScore = 0;
            int bestI = 0;

            //iterate through the roots list, getting the highest corresponding score
            for (int i = 0; i < rootSize; i++)
            {
                //check if the score is higher
                if (scores[i] > bestScore)
                {
                    //set the new best index and score
                    bestScore = scores[i];
                    bestI = i;
                }
            }

            return roots[bestI];
        }
    }
}
