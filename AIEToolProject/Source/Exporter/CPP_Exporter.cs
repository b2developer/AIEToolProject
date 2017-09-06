using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AIEToolProject.Source.Exporter
{

    /*
    * class CPP_Exporter
    * child object of BaseExporter
    *
    * defines a recipe for creating behaviour 
    * tree generating code in the C++ programming language
    *
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class CPP_Exporter : BaseExporter
    {

        //file stream to write to
        FileStream stream = null;

        /*
        * Write
        * 
        * writes a string to the stream
        * 
        * @param string data - the string to write
        * @returns void
        */
        public void Write(string data)
        {
            stream.SetLength(stream.Length + data.Length);

            //convert string to byte array
            byte[] bytes = Encoding.ASCII.GetBytes(data);

            //write to the stream
            stream.Write(bytes, 0, data.Length);
        }


        /*
        * Initialise 
        * overrides BaseExporter's Initialise()
        * 
        * creates a new file
        * sets up a stream to write data in
        * 
        * @returns void
        */
        public override void Initialise()
        {
            stream = File.Create(exportingPath + "\\export.cpp");

            //start of the program, #define statements and namespace definitions
            Write("#include <vector>\r\n");
            Write("\r\n");
            Write("#include \"export.h\"\r\n");
            Write("#include \"Action.h\"\r\n");
            Write("#include \"Condition.h\"\r\n");
            Write("#include \"Selector.h\"\r\n");
            Write("#include \"Sequence.h\"\r\n");
            Write("#include \"Decorator.h\"\r\n");
            Write("\r\n");
            Write("#define BT BehaviourTree\r\n");
            Write("\r\n");
        }


        /*
        * FormatForFunctionName
        * overrides BaseExporter's FormatForFunctionName()
        * 
        * formats a string so that it can be a function name
        * 
        * @string data - the string to format
        * @returns string - the function safe string
        */
        public override string FormatForFunctionName(string data)
        {
            //unnamed functions are not allowed
            if (data.Length == 0)
            {
                return "unnamed";
            }

            //string of all acceptable characters
            string alpha = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            //string of all acceptable numbers
            string nums = "1234567890";

            //get the individual characters of the string
            char[] chars = data.ToCharArray();

            //string to append safe chars to
            string build = "";

            //iterate through all chars, adding safe chars to the build string
            for (int i = 0; i < chars.Length; i++)
            {
                //store in a temporary value for performance and readability
                char c = chars[i];

                if (alpha.Contains(c)) //characters are always allowed
                {
                    build += c;
                }
                else if (nums.Contains(c) && build.Length > 0) //numbers are allowed only if they aren't the first in the name
                {
                    build += c;
                }
                else if (c == ' ') //white space is converted to an underscore
                {
                    build += '_';
                }
            }

            return build;
        }


        /*
        * CreateInputClass 
        * overrides BaseExporter's CreateInputClass()
        * 
        * writes code for a derived input class
        * for the behaviour tree instance to use
        * and a behaviour tree instance
        * 
        * @returns void
        */
        public override void CreateInputClass()
        {
            //remember the cpp stream
            FileStream cppStream = stream;

            //create a new header
            FileStream headerStream = File.Create(exportingPath + "\\export.h");
            stream = headerStream;

            Write("#include \"BehaviourTree.h\"\r\n");
            Write("\r\n");
            Write("#define BT BehaviourTree\r\n");
            Write("\r\n");

            //input class
            Write("class DerivedInput : public BT::Input\r\n");
            Write("{\r\n");
            Write("\r\n");
            Write("};\r\n");

            Write("\r\n");

            Write("class TreeBuilder\r\n");
            Write("{\r\n");
            Write("public:\r\n");
            Write("\r\n");
            Write("\tBT::BehaviourTree* CreateTree();\r\n");
            Write("\r\n");
            Write("};\r\n");

            Write("\r\n");

            headerStream.Close();

            stream = cppStream;
        }


        /*
        * CreatefunctionReferences 
        * overrides BaseExporter's CreateFunctionReferences()
        * 
        * writes code for all required function references
        * 
        * @returns void
        */
        public override void CreateFunctionReferences()
        {
            //get the size of the nodes list
            int nodeSize = input.nodes.Count;

            //iterate through all of the nodes, writing function references for unique ones
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                //don't write a function for selector or sequence nodes as they don't require one
                if (node.type == NodeType.SELECTOR || node.type == NodeType.SEQUENCE)
                {
                    continue;
                }


                //check if the node already has a function
                if (existingNames.Contains(node.name))
                {
                    continue;
                }

                //add the name and type to the lists of nodes known
                existingNames.Add(node.name);
                existingTypes.Add(node.type);

                //array of strings to add to the function name, one function is made per additive
                List<string> additives = new List<string>();

                additives.Add("");

                //additives are required for the decorator
                if (node.type == NodeType.DECORATOR)
                {
                    additives.Clear();

                    additives.Add("pre");
                    additives.Add("post");
                }

                //function-proof the name
                string safeName = FormatForFunctionName(node.name);

                for (int j = 0; j < additives.Count; j++)
                {

                    //start of the function
                    Write("BT::BehaviourReturn ");
                    Write(safeName + "F" + additives[j] + "(");

                    //each node requires a different type of function
                    switch (node.type)
                    {
                        case NodeType.ACTION: Write("BT::Action* node)"); break;
                        case NodeType.CONDITION: Write("BT::Condition* node)"); break;
                        case NodeType.DECORATOR: Write("BT::Decorator* node, std::vector<BT::Behaviour*> children)"); break;
                    }

                    Write("\r\n");
                    Write("{\r\n");
                    Write("\treturn BT::BehaviourReturn::NONE;\r\n");
                    Write("}\r\n");
                    Write("\r\n");
                    Write("\r\n");
                }
            }
        }


        /*
        * DefineTree 
        * overrides BaseExporter's DefineTree()
        * 
        * writes code that creates an derived input instance and behaviour tree instance
        * 
        * @returns void
        */
        public override void DefineTree()
        {
            Write("BT::BehaviourTree* TreeBuilder::CreateTree()\r\n");
            Write("{\r\n");
            Write("\r\n");
            Write("\tBT::Input* input = new DerivedInput();\r\n");
            Write("\r\n");
            Write("\tBT::BehaviourTree* tree = new BT::BehaviourTree();\r\n");
            Write("\r\n");
            Write("\ttree->input = input;\r\n");
            Write("\r\n");
        }


        /*
        * DefineBehaviours 
        * overrides BaseExporter's DefineBehaviours()
        * 
        * writes code that defines all behaviour node instances
        * 
        * @returns void
        */
        public override void DefineBehaviours()
        {
            //get the size of the nodes list
            int nodeSize = input.nodes.Count;

            //iterate through all nodes, setting a duplicate identifier for each (2nd instance of a node gets an id of 1)
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                //count the nodes with the same name
                int count = 0;

                //iterate through all of the nodes before the current node
                for (int j = 0; j < i; j++)
                {
                    Node other = input.nodes[j];

                    //increment the count if the two unique nodes share the same name
                    if (node.name == other.name)
                    {
                        count++;
                    }
                }

                //store the count in the node's index
                node.index = count;
            }

            //iterate through all nodes, generating a node for each
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                Write("\t");

                //write the correct type
                switch (node.type)
                {
                    case NodeType.ACTION: Write("BT::Action*"); break;
                    case NodeType.CONDITION: Write("BT::Condition*"); break;
                    case NodeType.SELECTOR: Write("BT::Selector*"); break;
                    case NodeType.SEQUENCE: Write("BT::Sequence*"); break;
                    case NodeType.DECORATOR: Write("BT::Decorator*"); break;
                }

                //the name of the node with the index
                string safeName = FormatForFunctionName(node.name) + "_" + node.index.ToString();

                Write(" " + safeName + " = new ");

                //write the correct type
                switch (node.type)
                {
                    case NodeType.ACTION: Write("BT::Action"); break;
                    case NodeType.CONDITION: Write("BT::Condition"); break;
                    case NodeType.SELECTOR: Write("BT::Selector"); break;
                    case NodeType.SEQUENCE: Write("BT::Sequence"); break;
                    case NodeType.DECORATOR: Write("BT::Decorator"); break;
                }

                Write("();\r\n");

            }

            Write("\r\n");

        }


        /*
        * DefineConnections 
        * overrides BaseExporter's DefineConnections()
        * 
        * writes code that connects all behaviours to the tree
        * 
        * @returns void
        */
        public override void DefineConnections()
        {
            //get the best root
            Node bestRoot = input.GetMostCommonRoot();

            Write("\ttree->root = ");

            //the name of the node with the index
            string safeName = FormatForFunctionName(bestRoot.name) + "_" + bestRoot.index.ToString();

            Write(safeName + ";\r\n");

            //get the size of the nodes list
            int nodeSize = input.nodes.Count;

            //iterate through all nodes, generating a node for each
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                Write("\ttree->behaviours.push_back(");

                //the name of the node with the index
                safeName = FormatForFunctionName(node.name) + "_" + node.index.ToString();

                Write(safeName + ");\r\n");

            }

            Write("\ttree->linkNodes();\r\n");
            Write("\r\n");
        }


        /*
        * DefineStructure 
        * overrides BaseExporter's DefineStructure()
        * 
        * writes code that observes the tree and assigns children
        * 
        * @returns void
        */
        public override void DefineStructure()
        {
            //get the size of the nodes list
            int nodeSize = input.nodes.Count;

            //iterate through all nodes, generating a node for each
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                //the name of the node with the index
                string safeName = FormatForFunctionName(node.name) + "_" + node.index.ToString();

                //get the size of the children list
                int childSize = node.children.Count;

                //iterate through all of the children, generating code to add each to the parent
                for (int j = 0; j < childSize; j++)
                {
                    //store in a temporary value for performance and readability
                    Node child = node.children[j];

                    //the name of the child with the index
                    string safeChildName = FormatForFunctionName(child.name) + "_" + child.index.ToString();

                    Write("\t" + safeName + "->children.push_back(" + safeChildName + ");\r\n");
                }

                //add a new line if there was children added to the current node, or if this is the last node
                if (childSize > 0 || i == nodeSize - 1)
                {
                    Write("\r\n");
                }
            }

        }


        /*
        * AssignFunctionReferences 
        * overrides BaseExporter's AssignFunctionReferences()
        * 
        * writes code that assigns function references of the nodes to the actual functions
        * 
        * @returns void
        */
        public override void AssignFunctionReferences()
        {
            //get the size of the nodes list
            int nodeSize = input.nodes.Count;

            //iterate through all nodes, generating a node for each
            for (int i = 0; i < nodeSize; i++)
            {
                //store in a temporary value for performance and readability
                Node node = input.nodes[i];

                //the safe name of the node
                string formattedName = FormatForFunctionName(node.name);

                //the name of the node with the index
                string safeName = formattedName + "_" + node.index.ToString();

                if (node.type == NodeType.ACTION || node.type == NodeType.CONDITION)
                {
                    //the name of the function reference to link
                    string functionName = formattedName + "F";

                    Write("\t" + safeName + "->routine = &" + functionName + ";\r\n");
                    Write("\r\n");
                }
                else if (node.type == NodeType.DECORATOR)
                {
                    //the name of the function references to link
                    string preFunctionName = formattedName + "Fpre";
                    string postFunctionName = formattedName + "Fpost";

                    Write("\t" + safeName + "->preRoutine = &" + preFunctionName + ";\r\n");
                    Write("\t" + safeName + "->postRoutine = &" + postFunctionName + ";\r\n");
                    Write("\r\n");
                }

            }

            //final return statement
            Write("\treturn tree;\r\n");
        }


        /*
        * CleanUp
        * overrides BaseExporter's CleanUp
        * 
        * used for de-initialisation
        * 
        * @returns void
        */
        public override void CleanUp()
        {
            //closing braces
            Write("}\r\n");

            stream.Close();
        }

    }
}
