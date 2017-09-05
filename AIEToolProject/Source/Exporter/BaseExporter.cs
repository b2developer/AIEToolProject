using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source.Exporter
{
    //type for programming languages that can be exported to
    public enum ProgrammingLanguage
    {
        C_Sharp,
        C_PlusPlus,
        Python,
    }

    /*
	* class BaseExporter
    * abstract class
	*
	* base object for that defines a recipe for
    * creating behaviour tree generating code
	*
	* author: Bradley Booth, Academy of Interactive Entertainment, 2017
	*/
    public abstract class BaseExporter
    {

        //reference to the tree to write generation code for
        public Tree input = null;

        //the path to export to
        public string exportingPath = "";

        //container of unique node names and their types
        public List<string> existingNames = new List<string>();
        public List<NodeType> existingTypes = new List<NodeType>();

        /*
        * BaseExporter() 
        * default constructor 
        */
        public BaseExporter() { }


        /*
        * Initialise 
        * abstract function
        * 
        * creates a new file
        * sets up a stream to write data in
        * 
        * @returns void
        */
        public abstract void Initialise();


        /*
        * FormatForFunctionName
        * abstract function
        * 
        * formats a string so that it can be a function name
        * 
        * @string data - the string to format
        * @returns string - the function safe string
        */
        public abstract string FormatForFunctionName(string data);


        /*
        * CreateInputClass 
        * abstract function
        * 
        * writes code for a derived input class
        * for the behaviour tree instance to use
        * 
        * @returns void
        */
        public abstract void CreateInputClass();


        /*
        * CreateFunctionReferences 
        * abstract function
        * 
        * writes code for all required function references
        * 
        * @returns void
        */
        public abstract void CreateFunctionReferences();


        /*
        * DefineTree 
        * abstract function
        * 
        * writes code that creates an derived input instance and behaviour tree instance
        * 
        * @returns void
        */
        public abstract void DefineTree();


        /*
        * DefineBehaviours 
        * abstract function
        * 
        * writes code that defines all behaviour node instances
        * 
        * @returns void
        */
        public abstract void DefineBehaviours();


        /*
        * DefineConnections 
        * abstract function
        * 
        * writes code that connects all behaviours to the tree
        * 
        * @returns void
        */
        public abstract void DefineConnections();


        /*
        * DefineStructure 
        * abstract function
        * 
        * writes code that observes the tree and assigns children
        * 
        * @returns void
        */
        public abstract void DefineStructure();


        /*
        * AssignFunctionReferences 
        * abstract function
        * 
        * writes code that assigns function references of the nodes to the actual functions
        * 
        * @returns void
        */
        public abstract void AssignFunctionReferences();


        /*
        * CleanUp
        * abstract function
        * 
        * used for de-initialisation
        * 
        * @returns void
        */
        public abstract void CleanUp();

    }
}
