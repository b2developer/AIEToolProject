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
    * class BaseState
    * implements ICloneable
    * 
    * a holder for gameobjects in a static state
    * uses XML to deep-copy itself
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BaseState : ICloneable
    {

        //list of objects held by the state
        List<BaseObject> objects;

        /*
        * BaseState()
        * constructor, defines the new list
        */
        public BaseState()
        {
            objects = new List<BaseObject>();
        }


        public object Clone()
        {

        }
    }
}
