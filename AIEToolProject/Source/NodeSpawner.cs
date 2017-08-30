using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AIEToolProject.Source
{
    /*
    * class NodeSpawner
    * child object of BaseComponent
    * 
    * a component that can create additional node
    * objects when called upon
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    class NodeSpawner : BaseComponent
    {
        //radii of the spawned node's colliders
        public float nodeRadius = 35.0f;
        public float connRadius = 15.0f;

        //reference to the form that is using it
        public EditorForm form = null;

        /*
        * public NodeSpawner()
        * constructor, defines the nodes list
        */
        public NodeSpawner() { }
  

        /*
        * MousePressedCallback
        * 
        * called whenever the mouse gets pressed
        * (by an event handler)
        * 
        * @param EventListener eventListener - the component that called the call back
        * @returns void
        */
        public void MousePressedCallback(EventListener eventListener)
        {
            //don't do anything if the form that called this hasn't been referenced properly
            if (form == null)
            {
                return;
            }

            //check that the button pressed is the right button
            if (eventListener.mouseEventArgs.Button == MouseButtons.Left)
            {
                //set this to be an exclusive if there arent any already
                if (form.exclusives.Count == 0)
                {
                    form.exclusives.Add(eventListener);
                }
            }
        }


        /*
        * MouseReleasedCallback
        * 
        * called whenever the mouse gets released
        * (by an event handler)
        * 
        * @param EventListener eventListener - the component that called the call back
        * @returns void
        */
        public void MouseReleasedCallback(EventListener eventListener)
        {
            //don't do anything if the form that called this hasn't been referenced properly
            if (form == null)
            {
                return;
            }

            //remove this event listener from the list (only if it was already in there)
            if (form.exclusives.Contains(eventListener))
            {
                form.exclusives.Remove(eventListener);

                //spawn a node
                BaseObject obj = new BaseObject();

                Node nodeComp = new Node();

                //avoids warning CS1690
                Point scrollPosition = form.safeScrollPosition;

                //the true mouse position relative to the world space
                Point trueMousePosition = new Point(scrollPosition.X + eventListener.mouseEventArgs.X, scrollPosition.Y + eventListener.mouseEventArgs.Y);

                nodeComp.collider = new Circle(trueMousePosition.X, trueMousePosition.Y, nodeRadius);
                nodeComp.form = form;

                nodeComp.upperConn = new Circle(0, -(nodeRadius - connRadius), connRadius);

                if (form.spawnType != NodeType.ACTION && form.spawnType != NodeType.CONDITION)
                {
                    nodeComp.lowerConn = new Circle(0, nodeRadius - connRadius, connRadius);
                }

                NodeRenderer rendererComp = new NodeRenderer();

                //set the characteristics of the renderer
                rendererComp.node = nodeComp;
                rendererComp.node.type = form.spawnType;

                EventListener listenerComp = new EventListener();

                //link the callback references
                listenerComp.mousePressed = nodeComp.MousePressedCallback;
                listenerComp.mouseReleased = nodeComp.MouseReleasedCallback;
                listenerComp.mouseMoved = nodeComp.MouseMovedCallback;

                nodeComp.container = obj;
                rendererComp.container = obj;
                listenerComp.container = obj;

                //add all of the components to the object
                obj.components.Add(nodeComp as BaseComponent);
                obj.components.Add(rendererComp as BaseComponent);
                obj.components.Add(listenerComp as BaseComponent);

                form.objects.Insert(0, obj);
            }
        }

    }
}
