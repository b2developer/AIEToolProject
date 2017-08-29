using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{
    /*
    * class BaseObject
    * 
    * the base object that can be updated by the program
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class BaseObject
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

    }
}
