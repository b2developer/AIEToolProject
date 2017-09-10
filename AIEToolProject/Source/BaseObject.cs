using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{
    /*
    * class BaseObject
    * implements ICloneable
    * 
    * the base object that can be updated by the program
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BaseObject : ICloneable
    {
        //the list of components to update
        public List<BaseComponent> components;

        /*
        * public BaseObject() 
        * default constructor
        */
        public BaseObject()
        {
            components = new List<BaseComponent>();
        }


        public void Update()
        {

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
            return new object();
        }


        /*
        * Clone 
        * implement's ICloneable's Clone()
        * creates another object identical to this
        * 
        * @param List<BaseComponent> indexedComponents - list of components used to re-attach references
        * @returns object - the object with matching member variables
        */
        public object Clone(List<BaseComponent> indexedComponents)
        {
            //create a new base object
            BaseObject other = new BaseObject();

            //get the size of the original components list
            int compSize = components.Count;

            //iterate through all of the components, cloning each
            for (int i = 0; i < compSize; i++)
            {
                other.components.Add(indexedComponents[components[i].index]);
            }

            //iterate through all of the components, stitching them
            foreach (BaseComponent bc in other.components)
            {
                bc.container = other;
                bc.Stitch(indexedComponents);
            }

            return other as object;

        }

    }
}
