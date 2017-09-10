using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{

    /*
    * class LivePosition
    * child object of BaseComponent
    * 
    * updated whenever positioninal updates of the form
    * that this component sits in occurs
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class LivePosition : BaseComponent
    {
        //position recorded
        public int x = 0;
        public int y = 0;

        /*
        * LivePosition 
        * default constructor 
        */
        public LivePosition() { }


        /*
        * Record
        * 
        * sets the position
        * 
        * @param int _x - the input x axis
        * @param int _y - the input y axis
        * @returns void
        */ 
        public void Record(int _x, int _y)
        {
            x = _x;
            y = _y;
        }


        /*
        * Clone 
        * implement's ICloneable's Clone()
        * creates another object identical to this
        * 
        * @returns object - the object with matching member variables
        */
        public override object Clone()
        {
            //create a new object
            LivePosition other = new LivePosition();

            other.index = index;

            other.x = x;
            other.y = y;

            return other as object;
        }
    }
}
