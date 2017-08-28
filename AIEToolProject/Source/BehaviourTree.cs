using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace AIEToolProject.Source
{
    /*
    * class BehaviourTree
   * utilises the IComparable interface
    * 
    * container for a tree structure of behaviour nodes,
    * implements XML saving and loading
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BehaviourTree : ICloneable
    {

        //list of nodes that the tree contains
        public List<BehaviourNode> nodes = null;

        /*
        * BehaviourTree
        * default constructor
        */
        public BehaviourTree()
        {

        }


        /*
        * Clone
        * implements ICloneable's Clone() 
        * 
        * deep copies the behaviour tree
        * 
        * @returns object - the cloned behaviour tree
        */
        public object Clone()
        {
            BehaviourTree clone = new BehaviourTree();

            //store the size of the nodes list
            int size = nodes.Count;

            //index each node
            for (int i = 0; i < size; i++)
            {
                nodes[i].index = i;
            }

            //iterate through each node, copying it
            for (int i = 0; i < size; i++)
            {
                clone.nodes.Add((nodes[i].Clone() as BehaviourNode));
            }

            //reconstruct the tree
            clone.Relink();

            return clone;
        }


        /*
        * Relink 
        * 
        * serialisation/cloning causes the behaviour nodes
        * to lose the references to objects that they originally
        * had links to, this function uses the stored indices to
        * establish those links once again
        * 
        * @returns void 
        */
        public void Relink()
        {
            //store the size of the nodes list
            int size = nodes.Count;

            //index each node
            for (int i = 0; i < size; i++)
            {
                //store for performance and readability
                BehaviourNode node = nodes[i];

                //count the children
                int childSize = node.children.Count;

                //iterate through each child, properly reassigning each child
                for (int j = 0; j < childSize; j++)
                {

                    //reset the reference
                    node.children[j] = nodes[node.children[j].index];

                    BehaviourNode child = node.children[j];

                    child.parent = node;
                }
            }
        }


        /*
        * Serialise 
        * 
        * converts the tree into XML data and saves it to name
        * 
        * @param string path - the file path of the XML file to save
        * @returns void
        */
        public void Serialise(string path)
        {
            //store the size of the nodes list
            int size = nodes.Count;

            //index each node
            for (int i = 0; i < size; i++)
            {
                nodes[i].index = i;
            }

            //create a serialiser object for the tree
            XmlSerializer serialiser = new XmlSerializer(typeof(BehaviourTree));

            //create a stream writer object for the xml writer
            StreamWriter streamWriter = new StreamWriter(path);

            //write the object to the stream
            serialiser.Serialize(streamWriter, this);

            streamWriter.Close();
        }
    }
}
