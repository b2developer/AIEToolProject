using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AIEToolProject.Source;


namespace AIEToolProject.Source
{
    //type for all call back functions
    public enum CallbackType
    {
        MOUSE_PRESSED,
        MOUSE_RELEASED,
        MOUSE_MOVED,
    }


    /*
    * class EventListener
    * child object of BaseComponent
    * 
    * triggers a call-back function whenever the active
    * form detects input
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class EventListener : BaseComponent
    {
        //define a function reference type
        public delegate void CallbackDelegate(EventListener eventListener);

        /*
        * public EventListener() 
        * constructor, defines default callbacks
        */
        public EventListener()
        {
            mousePressed = DefaultCallback;
            mouseReleased = DefaultCallback;
            mouseMoved = DefaultCallback;
        }


        /*
        * Clone 
        * overrides BaseComponent's Clone()
        * 
        * creates another object identical to this
        * 
        * @param object - the object with matching member variables
        */
        public override object Clone()
        {
            //create a new instance of the object
            EventListener other = new EventListener();

            other.index = index;

            other.mousePressed = mousePressed;
            other.mouseReleased = mouseReleased;
            other.mouseMoved = mouseMoved;

            return other as object;
        }


        /*
        * DefaultCallback
        * 
        * default call back function for all call back delegate objects
        * 
        * @param EventListener eventListener - the event listener that called the function
        * @returns void
        */
        public void DefaultCallback(EventListener eventListener)
        {

        }

        //object that describes the mouse's state
        public MouseEventArgs mouseEventArgs = null;

        //called when the mouse is pressed
        public CallbackDelegate mousePressed;

        //called when the mouse is released
        public CallbackDelegate mouseReleased;

        //called when the mouse is moved
        public CallbackDelegate mouseMoved;
    }
}
