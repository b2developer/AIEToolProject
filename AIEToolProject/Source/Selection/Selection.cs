using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source.Selection
{
    /*
    * abstract class Selection
    * 
    * base class for a selection that triggers responses to mouse input
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public abstract class Selection
    {

        //arguments of the event
        public EventArgs mouseArgs = null;

        /*
        * LeftDown 
        * virtual function
        * 
        * called when the user first presses the left mouse button
        * 
        * @returns void
        */
        public virtual void LeftDown() { }


        /*
        * LeftUp 
        * virtual function
        * 
        * called when the user first releases the left mouse button
        * 
        * @returns void
        */
        public virtual void LeftUp() { }


        /*
        * RightDown 
        * virtual function
        * 
        * called when the user first releases the right mouse button
        * 
        * @returns void
        */
        public virtual void RightDown() { }


        /*
        * RightUp 
        * virtual function
        * 
        * called when the user first releases the right mouse button
        * 
        * @returns void
        */
        public virtual void RightUp() { }
    

        /*
        * LeftClick 
        * virtual function
        * 
        * called when the user clicks the left mouse button
        * 
        * @returns void
        */
        public virtual void LeftClick() { }


        /*
        * RightClick 
        * virtual function
        * 
        * called when the user clicks the right mouse button
        * 
        * @returns void
        */
        public virtual void RightClick() { }


        /*
        * Move 
        * virtual function
        * 
        * called when the user moves the mouse
        * 
        * @returns void
        */
        public virtual void Move() { }

    }
}
