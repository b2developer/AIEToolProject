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
        public List<BaseObject> objects;
        public List<BaseComponent> tempComponents;

        /*
        * BaseState()
        * constructor, defines the new list
        */
        public BaseState()
        {
            objects = new List<BaseObject>();
            tempComponents = new List<BaseComponent>();
        }


        /*
        * Clone 
        * implement's ICloneable's Clone()
        * creates another object identical to this
        * 
        * @returns object - the object with matching member variables
        */
        public object Clone()
        {
            tempComponents.Clear();

            //count the index to give the next component
            int indexCounter = 0;

            //create a new object with the same type
            BaseState other = new BaseState();

            //iterate through all of the objects, cloning each
            foreach (BaseObject obj in objects)
            {
                //get the size of the components list
                int compSize = obj.components.Count;

                //iterate through all of the components, indexing all of them
                for (int i = 0; i < compSize; i++)
                {
                    //clone the component
                    BaseComponent clone = obj.components[i].Clone() as BaseComponent;

                    obj.components[i].index = indexCounter;
                    clone.index = indexCounter;

                    tempComponents.Add(clone);
                    indexCounter++;
                }
            }

            //iterate throwugh all of the objects, cloning each
            foreach (BaseObject obj in objects)
            {
                other.objects.Add(obj.Clone(tempComponents) as BaseObject);
            }

            return other;
        }
    }
}
