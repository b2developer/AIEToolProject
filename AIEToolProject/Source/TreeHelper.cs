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
    * static class TreeHelper
    *
    * a singleton that calculates solutions to tree related
    * structural problems (eg. adding a new node breaking the tree)
    * and serialisation problems (eg. converting XML to a tree)
    * 
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public static class TreeHelper
    {

        /*
        * SharedRoot 
        * 
        * tests if two nodes share the same root
        * 
        * @param Node n1 - the first node
        * @param Node n2 - the second node
        * @returns bool - flag indicating if they share the same root 
        */
        public static bool SharedRoot(Node n1, Node n2)
        {
            //recursively get the uppermost parent node of both nodes
            if (n1.parent != null)
            {
                return SharedRoot(n1.parent, n2);
            }
            else if (n2.parent != null)
            {
                return SharedRoot(n1, n2.parent);
            }

            //the uppermost parents have been found, compare them
            return n1 == n2;
        }


        /*
        * extractTree
        * 
        * gets the tree structure from the editor form's objects
        * 
        * @param EditorForm form - the form to extract the tree from
        * @returns Tree - the tree structure 
        */
        public static Tree extractTree(EditorForm form)
        {
            //define the object to serialise
            Tree tree = new Tree();

            //get the size of the objects list
            int objSize = form.objects.Count;

            //iterate through all objects looking for nodes
            for (int i = 0; i < objSize; i++)
            {
                //store in a temp variable for performance and readability
                BaseObject obj = form.objects[i];

                //get the size of the components list
                int compSize = obj.components.Count;

                //iterate through all components, searching for a node component
                for (int j = 0; j < compSize; j++)
                {
                    BaseComponent comp = obj.components[j];

                    //check that the component is a node
                    if (comp is Node)
                    {
                        //cast the node to it's true type
                        Node node = comp as Node;

                        //add the node to the tree
                        tree.nodes.Add(node);
                    }
                }
            }

            tree.Index();

            return tree;
        }


        /*
        * LoadState 
        * 
        * de-serialises an XML file onto an editor form
        * 
        * @param EditorForm form - the form to load the data to
        * @returns bool - indicates success/failure
        */
        public static bool LoadState(EditorForm form)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "XML Files (*.xml)|*.xml| All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            //check the the file dialog found a file to deserialise
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //set the name of the form
                form.Text = openFileDialog.SafeFileName.Split('.')[0];

                //create a serialiser object for the tree
                XmlSerializer serialiser = new XmlSerializer(typeof(Tree));

                //create a file reader for the tree
                StreamReader streamReader = new StreamReader(openFileDialog.FileName);

                try
                {
                    //deserialise the stream (XML file stream)
                    Tree tree = serialiser.Deserialize(streamReader) as Tree;

                    tree.ReferenceByIndex();

                    //iterate through all of the nodes, adding them to the form as components in BaseObjects
                    foreach (Node n in tree.nodes)
                    {
                        //create a new base object
                        BaseObject obj = new BaseObject();

                        n.form = form;      

                        //create a new renderer for the node
                        NodeRenderer renderer = new NodeRenderer();
                        renderer.node = n;
                        
                        //create a new event listener for the node
                        EventListener listener = new EventListener();

                        //link the callback references
                        listener.mousePressed = n.MousePressedCallback;
                        listener.mouseReleased = n.MouseReleasedCallback;
                        listener.mouseMoved = n.MouseMovedCallback;

                        n.container = obj;
                        renderer.container = obj;
                        listener.container = obj;

                        //add all components to the object
                        obj.components.Add(n);
                        obj.components.Add(renderer as BaseComponent);
                        obj.components.Add(listener as BaseComponent);

                        //add the object to the form
                        form.objects.Insert(0, obj);
                    }

                    //set the loaded path
                    form.loadedPath = openFileDialog.FileName;

                    streamReader.Close();

                    return true;
                }
                catch
                {
                    streamReader.Close();

                    //the open dialog failed to deserialise the file
                    return false;
                }
            }

            //the open dialog failed to get a XML file to deserialise
            return false;
        }


        /*
        * SaveState 
        * 
        * serialises an editor form into XML format
        * 
        * @param EditorForm form - the form to serialise
        * @param forceDialog - flag indicating to enable checking for previous saves
        * @returns void
        */
        public static void SaveState(EditorForm form, bool forceDialog)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";

            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            //check that the tree isn't being saved to a directory that already exists
            bool previousSave = !forceDialog && form.loadedPath != "";

            //check that the save worked
            if (previousSave || saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //get the previous path
                string fullPath = form.loadedPath;

                if (!previousSave)
                {
                    //string formatting to get the desired display name
                    fullPath = saveFileDialog.FileName;
                    string[] splitPath = fullPath.Split('\\');
                    string pathName = splitPath[splitPath.Length - 1];
                    string name = pathName.Split('.')[0];

                    //set the display name of the form
                    form.Text = name;
                }

                //create a serialiser object for the tree
                XmlSerializer serialiser = new XmlSerializer(typeof(Tree));

                //create a file writer for the tree
                StreamWriter streamWriter = new StreamWriter(fullPath);

                //define the object to serialise
                Tree tree = extractTree(form);

                //serialise the tree
                serialiser.Serialize(streamWriter, tree);

                //set the new path
                if (!previousSave)
                {
                    form.loadedPath = fullPath;
                }

                streamWriter.Close();

            }
        }
    }
}
