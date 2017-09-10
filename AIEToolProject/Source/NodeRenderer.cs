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
    * class NodeRenderer
    * child object of BaseComponent
    * 
    * the object that renders a node
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class NodeRenderer : BaseComponent
    {

        //the node that is being rendered
        public Node node = null;

        /*
        * public NodeRenderer() 
        * default constructor
        */
        public NodeRenderer()
        {

        }


        /*
        * Clone 
        * overrides BaseComponent's Clone()
        * creates another object identical to this
        * 
        * @param object - the object with matching member variables
        */
        public override object Clone()
        {
            //create a new node renderer
            NodeRenderer other = new NodeRenderer();

            other.index = index;

            other.node = node;

            return other as object;
        }


        /*
        * Stitch
        * overrides BaseComponent's Stitch(List<BaseComponent> components)
        * 
        * re-links references after the base object is duplicated
        * 
        * @param List<BaseComponent> components - list of components
        * @returns void
        */
        public override void Stitch(List<BaseComponent> components)
        {
            node = components[node.index] as Node;
        }


        /*
        * Render() 
        * draws the node to the current form
        * 
        * @param EditorForm form - the form that called the render
        * @param Graphics g - the grpahics object that will render the node
        */
        public void Render(EditorForm form, Graphics g)
        {

            //the black pen is a commonly needed pen for the renderering
            Pen blackPen = new Pen(Color.Black, 1.0f);

            //deduct what type of node this
            //-------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------
            switch (node.type)
            {
                case NodeType.ACTION:

                    {
                        //create the pen and brush to draw the outlined circle with
                        Brush greenBrush = new SolidBrush(Color.Green);

                        //define a region to draw the node in
                        Rectangle region = new Rectangle((int)(node.collider.x - node.collider.radius), (int)(node.collider.y - node.collider.radius), 
                                                         (int)(node.collider.radius * 2), (int)(node.collider.radius * 2));

                        region = form.OffsetRectangle(region, form.safeScrollPosition);

                        //draw the node
                        g.FillPie(greenBrush, region, 0.0f, 360.0f);
                        g.DrawArc(blackPen, region, 0.0f, 360.0f);

                        greenBrush.Dispose();
                    }

                    break;

                case NodeType.CONDITION:

                    {
                        //create the pen and brush to draw the outlined circle with
                        Brush yellowBrush = new SolidBrush(Color.Yellow);

                        //define a region to draw the node in
                        Rectangle region = new Rectangle((int)(node.collider.x - node.collider.radius), (int)(node.collider.y - node.collider.radius),
                                                         (int)(node.collider.radius * 2), (int)(node.collider.radius * 2));

                        region = form.OffsetRectangle(region, form.safeScrollPosition);

                        //draw the node
                        g.FillPie(yellowBrush, region, 0.0f, 360.0f);
                        g.DrawArc(blackPen, region, 0.0f, 360.0f);

                        yellowBrush.Dispose();
                    }

                    break;

                case NodeType.SELECTOR:

                    {
                        //create the pen and brush to draw the diamond with
                        Brush blueBrush = new SolidBrush(Color.Aqua);

                        //define the polygon to draw
                        Point[] diamond = new Point[]
                        {
                            new Point((int)node.collider.x, (int)(node.collider.y + node.collider.radius)),
                            new Point((int)(node.collider.x + node.collider.radius), (int)node.collider.y),
                            new Point((int)node.collider.x, (int)(node.collider.y - node.collider.radius)),
                            new Point((int)(node.collider.x - node.collider.radius), (int)node.collider.y)
                        };

                        diamond = form.OffsetPolygon(diamond, form.safeScrollPosition);

                        //draw the node
                        g.FillPolygon(blueBrush, diamond);
                        g.DrawPolygon(blackPen, diamond);

                        blueBrush.Dispose();
                    }

                    break;

                case NodeType.SEQUENCE:

                    {
                        //create the pen and brush to draw the rectangle with
                        Brush blueBrush = new SolidBrush(Color.Aqua);

                        //define a region to draw the node in
                        Rectangle region = new Rectangle((int)(node.collider.x - node.collider.radius), (int)(node.collider.y - node.collider.radius * 0.83f), 
                                                         (int)(node.collider.radius * 2.0f), (int)(node.collider.radius * 1.66f));

                        region = form.OffsetRectangle(region, form.safeScrollPosition);

                        //draw the node
                        g.FillRectangle(blueBrush, region);
                        g.DrawRectangle(blackPen, region);

                        blueBrush.Dispose();
                    }

                    break;

                case NodeType.DECORATOR:

                    {
                        //create the pen and brush to draw the diamond with
                        Brush brownBrush = new SolidBrush(Color.RosyBrown);

                        //define the polygon to draw
                        Point[] hexagon = new Point[]
                        {
                            new Point((int)(node.collider.x - node.collider.radius), (int)node.collider.y),
                            new Point((int)(node.collider.x - node.collider.radius * 0.66f), (int)(node.collider.y + node.collider.radius)),
                            new Point((int)(node.collider.x + node.collider.radius * 0.66f), (int)(node.collider.y + node.collider.radius)),
                            new Point((int)(node.collider.x + node.collider.radius), (int)node.collider.y),
                            new Point((int)(node.collider.x + node.collider.radius * 0.66f), (int)(node.collider.y - node.collider.radius)),
                            new Point((int)(node.collider.x - node.collider.radius * 0.66f), (int)(node.collider.y - node.collider.radius)),
                        };

                        hexagon = form.OffsetPolygon(hexagon, form.safeScrollPosition);

                        //draw the node
                        g.FillPolygon(brownBrush, hexagon);
                        g.DrawPolygon(blackPen, hexagon);

                        brownBrush.Dispose();
                    }

                    break;

            }
            //-------------------------------------------------------------------------------------------------------------------------
            //-------------------------------------------------------------------------------------------------------------------------

            //draw the temporary line if one needs to be drawn
            if (node.lineEnabled)
            {
                //avoids warning CS1690
                Point scrollPosition = form.safeScrollPosition;

                g.DrawLine(blackPen, node.lx1 - scrollPosition.X, node.ly1 - scrollPosition.Y, node.lx2 - scrollPosition.X, node.ly2 - scrollPosition.Y);
            }

            //draw the connector circles       
            Brush whiteBrush = new SolidBrush(Color.White);

            //define a region to draw the node in
            Rectangle connRegion = new Rectangle((int)((node.collider.x + node.upperConn.x) - node.upperConn.radius), (int)((node.collider.y + node.upperConn.y) - node.upperConn.radius),
                                                 (int)(node.upperConn.radius * 2), (int)(node.upperConn.radius * 2));

            connRegion = form.OffsetRectangle(connRegion, form.safeScrollPosition);

            //draw the node
            g.FillPie(whiteBrush, connRegion, 0.0f, 360.0f);
            g.DrawArc(blackPen, connRegion, 0.0f, 360.0f);

            //check that the node has a lower connection
            if (node.lowerConn != null)
            {

                //define a region to draw the node in
                connRegion = new Rectangle((int)((node.collider.x + node.lowerConn.x) - node.upperConn.radius), (int)((node.collider.y + node.lowerConn.y) - node.lowerConn.radius),
                                           (int)(node.lowerConn.radius * 2), (int)(node.lowerConn.radius * 2));

                connRegion = form.OffsetRectangle(connRegion, form.safeScrollPosition);

                //draw the node
                g.FillPie(whiteBrush, connRegion, 0.0f, 360.0f);
                g.DrawArc(blackPen, connRegion, 0.0f, 360.0f);
            }

            blackPen.Dispose();
            whiteBrush.Dispose();


        }


        /*
        * PreRender() 
        * draws the connections going away from the current node to the current form
        * 
        * @param EditorForm form - the form that called the render
        * @param Graphics g - the grpahics object that will render the node
        */
        public void PreRender(EditorForm form, Graphics g)
        {
            //get the scroll position
            Point scrollPosition = form.safeScrollPosition;

            //create the pen to draw with
            Pen blackPen = new Pen(Color.Black, 1.0f);

            //iterate through all nodes, drawing each
            foreach (Node child in node.children)
            {
                g.DrawLine(blackPen, new Point((int)(node.collider.x + node.lowerConn.x - scrollPosition.X), (int)(node.collider.y + node.lowerConn.y - scrollPosition.Y)), 
                                     new Point((int)(child.collider.x + child.upperConn.x - scrollPosition.X), (int)(child.collider.y + child.upperConn.y - scrollPosition.Y)));
            }

            blackPen.Dispose();

        }


        /*
        * PostRender() 
        * draws the upper-most layers of the node ie. text
        * 
        * @param EditorForm form - the form that called the render
        * @param Graphics g - the grpahics object that will render the node
        */
        public void PostRender(EditorForm form, Graphics g)
        {
            //get the scroll position
            Point scrollPosition = form.safeScrollPosition;

            //define a font
            Font font = new Font("Arial", node.textSize);

            //create the brush to draw with
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            //create a format object
            StringFormat format = new StringFormat();

            //set the text to allign to the centre
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            //transform position relative to the window
            Point localPosition = new Point((int)(node.collider.x - scrollPosition.X), (int)(node.collider.y - scrollPosition.Y));

            //draw the text
            g.DrawString(node.name, font, blackBrush, localPosition, format);

            font.Dispose();
            blackBrush.Dispose();

        }

    }
}
