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

namespace AIEToolProject
{
    public partial class EditorForm : Form
    {
        //container of behaviour nodes
        List<BehaviourNode> nodes;

        //size of the nodes when drawn
        public float scalar = 25.0f;

        //empty labels that define the scrollable area
        Label topLeft = null;
        Label bottomRight = null;

        //remembers the last valid scroll position
        Point validScrollPos = new Point(0, 0);

        BehaviourNode selectedNode = null;

        public EditorForm()
        {
            InitializeComponent();

            nodes = new List<BehaviourNode>();

            topLeft = new Label();
            topLeft.Size = new Size(0, 0);

            bottomRight = new Label();
            bottomRight.Size = new Size(0, 0);

            this.Controls.Add(topLeft);
            this.Controls.Add(bottomRight);
        }


        /*
        * SetScrollableArea
        * 
        * iterates through every node, setting a rectangle
        * (topLeft and bottomRight) that surrounds all nodes
        *
        * @returns void
        */
        public void SetScrollableArea()
        {

            Point scrollPos = new Point(this.HorizontalScroll.Value, this.VerticalScroll.Value);

            //track the best position for the rectangle as the list of nodes is iterated through
            Point topLeftBest = new Point();
            Point bottomRightBest = new Point();

            //default setting
            if (nodes.Count > 0)
            {
                topLeftBest = nodes[0].position;
                bottomRightBest = nodes[0].position;
            }

            //iterate through all nodes in the nodes container
            foreach (BehaviourNode b in nodes)
            {
                //move the left limit if it too close
                if (topLeftBest.X > b.position.X)
                {
                    topLeftBest = new Point(b.position.X, topLeftBest.Y);
                }

                //move the right limit if it too close
                if (topLeftBest.Y > b.position.Y)
                {
                    topLeftBest = new Point(topLeftBest.X, b.position.Y);
                }

                //move the left limit if it too close
                if (bottomRightBest.X < b.position.X)
                {
                    bottomRightBest = new Point(b.position.X, bottomRightBest.Y);
                }

                //move the right limit if it too close
                if (bottomRightBest.Y < b.position.Y)
                {
                    bottomRightBest = new Point(bottomRightBest.X, b.position.Y);
                }
            }

            //apply the padding
            topLeft.Location = new Point(topLeftBest.X - scrollPos.X, topLeftBest.Y - scrollPos.Y);
            bottomRight.Location = new Point(bottomRightBest.X - scrollPos.X, bottomRightBest.Y - scrollPos.Y);

        }



        /*
        * EditorForm_MouseDown
        * 
        * callback when the form is clicked on
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void EditorForm_MouseDown(object sender, EventArgs e)
        {

            //cast the event to it's true type
            MouseEventArgs mouseE = (e as MouseEventArgs);

            //get the amount of scroll that is offsetting the window
            Point scrollPos = validScrollPos;

            //failsafe
            if (mouseE != null)
            {
                //get the mouse coordinates in global space
                Point trueMousePos = new Point(mouseE.Location.X + scrollPos.X, mouseE.Location.Y + scrollPos.Y);

                //iterate through all nodes, checking for collision with the mouse
                foreach (BehaviourNode b in nodes)
                {
                    //relative vector from the node to the mouse
                    Point relative = new Point(trueMousePos.X - b.position.X, trueMousePos.Y - b.position.Y);

                    //square magnitude for circle to point
                    float sqrMag = relative.X * relative.X + relative.Y * relative.Y;

                    //if the square magnitude is less than the squared scalar, the mouse is clicking on this node
                    if (sqrMag < scalar * scalar)
                    {
                        selectedNode = b;

                        if (mouseE.Button == MouseButtons.Right)
                        {
                            nodes.Remove(selectedNode);
                        }

                        break;
                    }
                }

            }

        }



        /*
        * EditorForm_Click 
        * 
        * callback when the form is clicked
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void EditorForm_Click(object sender, EventArgs e)
        {

            //cast the event to it's true type
            MouseEventArgs mouseE = (e as MouseEventArgs);

            //get the amount of scroll that is offsetting the window
            Point scrollPos = validScrollPos;

            //failsafe
            if (mouseE != null)
            {

                //is the user dragging an existing node
                if (selectedNode == null)
                {
                    if (mouseE.Button == MouseButtons.Left)
                    {

                        //create a new node
                        BehaviourNode node = new BehaviourNode();

                        //set the position
                        node.position = new Point(mouseE.X + scrollPos.X, mouseE.Y + scrollPos.Y);
                        node.type = (MdiParent as MainForm).selectedType;
                        nodes.Add(node);

                        this.Refresh();

                        //recalculate area after movement
                        SetScrollableArea();
                    }
                }
                else
                {
                    //deselect the node
                    selectedNode = null;

                    this.Refresh();

                    //recalculate area after movement
                    SetScrollableArea();

                }

            }
          
        }



        /*
        * EditorForm_MouseMove 
        * 
        * callback when the mouse moves over the form
        * 
        * @param object sender - the object that sent the event
        * @param EventArgs e - description of the event
        * @returns void
        */
        private void EditorForm_MouseMove(object sender, MouseEventArgs e)
        {
            //cast the event to it's true type
            MouseEventArgs mouseE = (e as MouseEventArgs);

            //get the amount of scroll that is offsetting the window
            Point scrollPos = validScrollPos;

            //failsafe
            if (mouseE != null)
            {
                Point trueMousePos = new Point(mouseE.Location.X + scrollPos.X, mouseE.Location.Y + scrollPos.Y);

                //move the node if one is selected
                if (selectedNode != null)
                {
                    selectedNode.position = trueMousePos;

                    this.Refresh();
                }
            }
        }



        /*
        * OffsetRectangle 
        * 
        * subtracts the relative vector formed by "point"
        * from the rectangle and forms a new rectangle
        * 
        * @param Rectangle local - the rectangle to offset
        * @param Point point - the offset to apply
        * 
        */
        public Rectangle OffsetRectangle(Rectangle local, Point point)
        {
            //create a new rectangle
            Rectangle transformed = new Rectangle();

            transformed.Location = new Point(local.Location.X - point.X, local.Location.Y - point.Y);
            transformed.Size = local.Size;

            return transformed;

        }



        /*
        * OffsetPolygon 
        * 
        * subtracts the relative vector formed by "point"
        * from the polygon and forms a new polygon
        * 
        * @param Point[] local - the polygon (list of points that builds it) to offset
        * @param Point point - the offset to apply
        * 
        */
        public Point[] OffsetPolygon(Point[] local, Point point)
        {
            int size = local.Count();
            
            //new array of transformed points
            Point[] transformed = new Point[size];

            //iterate through all of the points, applying the offset to all of them
            for (int i = 0; i < size; i++)
            {
                transformed[i] = new Point(local[i].X - point.X, local[i].Y - point.Y);
            }

            return transformed;

        }



        /*
        * OnPaint
        * protects Form's OnPaint(PaintEventArgs e)
        * 
        * the custom draw call of the form, draws all nodes
        * along with their text and connections
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        */
        protected override void OnPaint(PaintEventArgs e)
        {

            //called the hidden function
            base.OnPaint(e);

            drawNodes(e);
            drawConnectors(e);
        }



        /*
        * drawNodes 
        * 
        * draws the main shapes that represent the behaviour nodes
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        * 
        */
        public void drawNodes(PaintEventArgs e)
        {
            //get the amount of scroll that is offsetting the window
            this.AutoScrollPosition = validScrollPos;

            //get the graphics object to paint with
            Graphics g = e.Graphics;

            //iterate through all nodes in the nodes container
            foreach (BehaviourNode b in nodes)
            {

                //deduct what type of node it is
                switch (b.type)
                {
                    case BehaviourType.ACTION:

                        {
                            //create the pen and brush to draw the outlined circle with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush greenBrush = new SolidBrush(Color.Green);

                            //define a region to draw the node in
                            Rectangle region = new Rectangle(b.position.X - (int)scalar, b.position.Y - (int)scalar, (int)scalar * 2, (int)scalar * 2);

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillPie(greenBrush, region, 0.0f, 360.0f);
                            g.DrawArc(blackPen, region, 0.0f, 360.0f);
                        }

                        break;

                    case BehaviourType.CONDITION:

                        {
                            //create the pen and brush to draw the outlined circle with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush yellowBrush = new SolidBrush(Color.Yellow);

                            //define a region to draw the node in
                            Rectangle region = new Rectangle(b.position.X - (int)scalar, b.position.Y - (int)scalar, (int)scalar * 2, (int)scalar * 2);

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillPie(yellowBrush, region, 0.0f, 360.0f);
                            g.DrawArc(blackPen, region, 0.0f, 360.0f);
                        }

                        break;

                    case BehaviourType.SELECTOR:

                        {
                            //create the pen and brush to draw the diamond with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush blueBrush = new SolidBrush(Color.Aqua);

                            //define the polygon to draw
                            Point[] diamond = new Point[]
                            {
                                new Point(0 + b.position.X, (int)scalar + b.position.Y),
                                new Point((int)scalar + b.position.X, 0 + b.position.Y),
                                new Point(0 + b.position.X, -(int)scalar + b.position.Y),
                                new Point(-(int)scalar + b.position.X, 0 + b.position.Y)
                            };

                            diamond = OffsetPolygon(diamond, validScrollPos);

                            //draw the node
                            g.FillPolygon(blueBrush, diamond);
                            g.DrawPolygon(blackPen, diamond);
                        }

                        break;

                    case BehaviourType.SEQUENCE:

                        {
                            //create the pen and brush to draw the rectangle with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush blueBrush = new SolidBrush(Color.Aqua);

                            //define a region to draw the node in
                            Rectangle region = new Rectangle(b.position.X - (int)scalar, b.position.Y - (int)(scalar * 0.66f), (int)scalar * 2, (int)(scalar * 1.66f));

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillRectangle(blueBrush, region);
                            g.DrawRectangle(blackPen, region);
                        }

                        break;

                    case BehaviourType.DECORATOR:

                        {
                            //create the pen and brush to draw the diamond with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush brownBrush = new SolidBrush(Color.RosyBrown);

                            //define the polygon to draw
                            Point[] hexagon = new Point[]
                            {
                                new Point(-(int)scalar + b.position.X, 0 + b.position.Y),
                                new Point(-(int)(scalar * 0.66f) + b.position.X, (int)scalar + b.position.Y),
                                new Point((int)(scalar * 0.66f) + b.position.X, (int)scalar + b.position.Y),
                                new Point((int)scalar + b.position.X, 0 + b.position.Y),
                                new Point((int)(scalar * 0.66f) + b.position.X, -(int)scalar + b.position.Y),
                                new Point(-(int)(scalar * 0.66f) + b.position.X, -(int)scalar + b.position.Y),
                            };

                            hexagon = OffsetPolygon(hexagon, validScrollPos);

                            //draw the node
                            g.FillPolygon(brownBrush, hexagon);
                            g.DrawPolygon(blackPen, hexagon);
                        }

                        break;

                }
            }
        }



        /*
        * drawConnectors 
        * 
        * draws the connection circles
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        * 
        */
        public void drawConnectors(PaintEventArgs e)
        {
            //get the amount of scroll that is offsetting the window
            this.AutoScrollPosition = validScrollPos;

            //get the graphics object to paint with
            Graphics g = e.Graphics;

            //iterate through all nodes in the node list
            foreach (BehaviourNode b in nodes)
            {
                //create the pen and brush to draw the outlined circle with
                Pen blackPen = new Pen(Color.Black, 2.0f);
                Brush whiteBrush = new SolidBrush(Color.White);

                //define a region to draw the node in
                Rectangle region = new Rectangle(b.position.X - (int)(scalar * 0.33f), b.position.Y, (int)(scalar * 0.66f), (int)(scalar * 0.66));

                region = OffsetRectangle(region, validScrollPos);

                //draw the node
                g.FillPie(whiteBrush, region, 0.0f, 360.0f);
                g.DrawArc(blackPen, region, 0.0f, 360.0f);
            }
        }



        private void EditorForm_Load(object sender, EventArgs e)
        {

        }



        private void EditorForm_Resize(object sender, EventArgs e)
        {
            SetScrollableArea();

            //force the form to re-draw itself
            Invalidate();          
            this.Refresh();
        }



        private void EditorForm_Scroll(object sender, ScrollEventArgs e)
        {
            validScrollPos = new Point(this.HorizontalScroll.Value, this.VerticalScroll.Value);

            SetScrollableArea();

            Invalidate();
            this.Refresh();
        }
    }
}
