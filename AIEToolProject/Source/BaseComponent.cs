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
    * implements ICloneable
    * 
    * the base component that gives base objects functionality
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BaseComponent : ICloneable, IInitialised
    {
        [XmlIgnore]
        //reference to the game object holding the component
        public BaseObject container = null;

        //used for copying
        public int index = 0;

        /*
        * public BaseComponent() 
        * default constructor
        * 
        */
        public BaseComponent()
        {

        }


        /*
        * Clone
        * implements ICloneable's Clone()
        * virtual function
        * 
        * creates an object with identical properties to this
        * 
        * @returns object - a clone of the object 
        */
        public virtual object Clone() { return new object(); }


        /*
        * Stitch 
        * virtual function
        * 
        * re-links references after the base object is duplicated
        * 
        * @param List<BaseComponent> components - list of components
        * @returns void
        */
        public virtual void Stitch(List<BaseComponent> components) { }


        /*
        * Initialise 
        * implements IInitialised's Initialise()
        * virtual function
        * 
        * called when the object is activated
        * 
        * @returns void
        */
        public virtual void Initialise() { }


        /*
        * DeInitialise
        * implements IInitialised's DeInitialise()
        * virtual function
        * 
        * called when the object is de-activated
        * 
        * @returns void
        */
        public virtual void DeInitialise() { }
    }
}
