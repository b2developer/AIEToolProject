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
    * class BaseComponent
    * 
    * the base component that gives base objects functionality
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BaseComponent
    {
        [XmlIgnore]
        //reference to the game object holding the component
        public BaseObject container = null;

        /*
        * public BaseComponent() 
        * default constructor
        * 
        */
        public BaseComponent()
        {

        }
    }
}
