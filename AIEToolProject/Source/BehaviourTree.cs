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
    * 
    * container for a tree structure of behaviour nodes,
    * implements XML saving and loading
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BehaviourTree
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
        * Serialise 
        * 
        * converts the tree into XML data and saves it to name
        * 
        * @param string path - the file path of the XML file to save
        * @returns void
        */
        public void Serialise(string path)
        {
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
