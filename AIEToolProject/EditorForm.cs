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
    //enum type for to identify the type of selection made
    public enum SelectionType
    {
        NULL,
        NODE,
        PARENT, //parent connector
        CHILD, //child connector
    }



    public partial class EditorForm : Form
    {
        //container of behaviour nodes
        public List<BehaviourNode> nodes;

        //default radius of all nodes
        public float scalar = 25.0f;

        //empty labels that define the scrollable area
        public Label topLeft = null;
        public Label bottomRight = null;

        //remembers the last valid scroll position
        public Point validScrollPos = new Point(0, 0);

        //the type of selection being made (node movement, connection)
        public SelectionType selection = SelectionType.NULL;

        //the node being selected (or one of it's connections)
        public BehaviourNode selectedNode = null;

        //data for a temporary line to draw when a connection is being formed
        public float tx1 = 0.0f;
        public float tx2 = 0.0f;
        public float ty1 = 0.0f;
        public float ty2 = 0.0f;

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
                topLeftBest = new Point((int)nodes[0].collider.x, (int)nodes[0].collider.y);
                bottomRightBest = new Point((int)nodes[0].collider.x, (int)nodes[0].collider.y);
            }

            //iterate through all nodes in the nodes container
            foreach (BehaviourNode b in nodes)
            {
                //move the left limit if it too close
                if (topLeftBest.X > (int)b.collider.x)
                {
                    topLeftBest = new Point((int)b.collider.x, topLeftBest.Y);
                }

                //move the right limit if it too close
                if (topLeftBest.Y > (int)b.collider.y)
                {
                    topLeftBest = new Point(topLeftBest.X, (int)b.collider.y);
                }

                //move the left limit if it too close
                if (bottomRightBest.X < (int)b.collider.x)
                {
                    bottomRightBest = new Point((int)b.collider.x, bottomRightBest.Y);
                }

                //move the right limit if it too close
                if (bottomRightBest.Y < (int)b.collider.y)
                {
                    bottomRightBest = new Point(bottomRightBest.X, (int)b.collider.y);
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
                    //test if the circle is intersecting the mouse position
                    if (b.collider.IntersectingPoint(trueMousePos.X, trueMousePos.Y))
                    {

                        selectedNode = b;

                        if (mouseE.Button == MouseButtons.Left)
                        {

                            //circles representing the connector
                            Circle parentCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[0], b.collider.radius * 0.33f);
                            Circle childCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[1], b.collider.radius * 0.33f);

                            //check for intersections with the connector circles before accepting the node
                            if (childCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y) && b.type != BehaviourType.CONDITION && b.type != BehaviourType.ACTION)
                            {
                                selection = SelectionType.CHILD;
                            }
                            else if (parentCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y))
                            {
                                selection = SelectionType.PARENT;
                            }
                            else
                            {
                                selection = SelectionType.NODE;
                            }
                            
                        }
                        else if (mouseE.Button == MouseButtons.Right)
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
                        //nothing was selected, the user is trying to create a node
                        if (selection == SelectionType.NULL)
                        {
                            //create a new node
                            BehaviourNode node = new BehaviourNode();

                            //set the position
                            node.collider.x = mouseE.X + scrollPos.X;
                            node.collider.y = mouseE.Y + scrollPos.Y;

                            //set the circle radius
                            node.collider.radius = scalar;

                            node.type = (MdiParent as MainForm).selectedType;

                            node.connectorOffsets = new float[] { -node.collider.radius * 0.9f, node.collider.radius * 0.9f};

                            nodes.Add(node);
                        }
                    }
                }
                else
                {
                    if (selection == SelectionType.CHILD)
                        {
                            //check for collision with all parent connectors
                            foreach (BehaviourNode b in nodes)
                            {
                                //don't check for self collisions
                                if (b == selectedNode)
                                {
                                    continue;
                                }

                                //get the circle of the parent connector
                                Circle parentCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[0], b.collider.radius * 0.33f);

                                //coordinates of the mouse in global space
                                float mx = mouseE.X + scrollPos.X;
                                float my = mouseE.Y + scrollPos.Y;

                                //if the parent connector is intersecting
                                if (parentCircle.IntersectingPoint(mx, my))
                                {
                                    //form a connection
                                    selectedNode.children.Add(b);
                                    b.parent = selectedNode;

                                    break;
                                }
                            }

                        }
                        else if (selection == SelectionType.PARENT)
                        {
                            //check for collision with all child connectors
                            foreach (BehaviourNode b in nodes)
                            {
                                //don't check for self collisions
                                if (b == selectedNode)
                                {
                                    continue;
                                }

                                //get the circle of the child connector
                                Circle childCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[1], b.collider.radius * 0.33f);

                                //coordinates of the mouse in global space
                                float mx = mouseE.X + scrollPos.X;
                                float my = mouseE.Y + scrollPos.Y;

                                //if the child connector is intersecting
                                if (childCircle.IntersectingPoint(mx, my))
                                {
                                    //form a connection
                                    selectedNode.parent = b;
                                    b.children.Add(selectedNode);

                                    break;
                                }
                            }
                        }

                    //deselect the node
                    selectedNode = null;

                }

                //reset the selection
                selection = SelectionType.NULL;

                this.Refresh();

                //recalculate area after movement
                SetScrollableArea();

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

                //respond to the mouse movement as something is selected
                if (selectedNode != null)
                {
                    //a node is being moved
                    if (selection == SelectionType.NODE)
                    {
                        selectedNode.collider.x = trueMousePos.X;
                        selectedNode.collider.y = trueMousePos.Y;
                    }
                    //a connection is being potentially formed
                    else if (selection == SelectionType.CHILD)
                    {
                        tx1 = selectedNode.collider.x - scrollPos.X;
                        ty1 = selectedNode.collider.y + selectedNode.connectorOffsets[1] - scrollPos.Y;

                        tx2 = trueMousePos.X - scrollPos.X;
                        ty2 = trueMousePos.Y - scrollPos.Y;
                    }
                    else if (selection == SelectionType.PARENT)
                    {
                        tx1 = selectedNode.collider.x - scrollPos.X;
                        ty1 = selectedNode.collider.y + selectedNode.connectorOffsets[0] - scrollPos.Y;

                        tx2 = trueMousePos.X - scrollPos.X;
                        ty2 = trueMousePos.Y - scrollPos.Y;
                    }

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
            drawConnections(e);
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
                            Rectangle region = new Rectangle((int)(b.collider.x - b.collider.radius), (int)(b.collider.y - b.collider.radius), (int)(b.collider.radius * 2), (int)(b.collider.radius * 2));

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillPie(greenBrush, region, 0.0f, 360.0f);
                            g.DrawArc(blackPen, region, 0.0f, 360.0f);

                            blackPen.Dispose();
                            greenBrush.Dispose();
                        }

                        break;

                    case BehaviourType.CONDITION:

                        {
                            //create the pen and brush to draw the outlined circle with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush yellowBrush = new SolidBrush(Color.Yellow);

                            //define a region to draw the node in
                            Rectangle region = new Rectangle((int)(b.collider.x - b.collider.radius), (int)(b.collider.y - b.collider.radius), (int)(b.collider.radius * 2), (int)(b.collider.radius * 2));

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillPie(yellowBrush, region, 0.0f, 360.0f);
                            g.DrawArc(blackPen, region, 0.0f, 360.0f);

                            blackPen.Dispose();
                            yellowBrush.Dispose();
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
                                new Point((int)b.collider.x, (int)(b.collider.y + b.collider.radius)),
                                new Point((int)(b.collider.x + b.collider.radius), (int)b.collider.y),
                                new Point((int)b.collider.x, (int)(b.collider.y - b.collider.radius)),
                                new Point((int)(b.collider.x - b.collider.radius), (int)b.collider.y)
                            };

                            diamond = OffsetPolygon(diamond, validScrollPos);

                            //draw the node
                            g.FillPolygon(blueBrush, diamond);
                            g.DrawPolygon(blackPen, diamond);

                            blackPen.Dispose();
                            blueBrush.Dispose();
                        }

                        break;

                    case BehaviourType.SEQUENCE:

                        {
                            //create the pen and brush to draw the rectangle with
                            Pen blackPen = new Pen(Color.Black, 2.0f);
                            Brush blueBrush = new SolidBrush(Color.Aqua);

                            //define a region to draw the node in
                            Rectangle region = new Rectangle((int)(b.collider.x - b.collider.radius), (int)(b.collider.y - b.collider.radius * 0.83f), (int)(b.collider.radius * 2.0f), (int)(b.collider.radius * 1.66f));

                            region = OffsetRectangle(region, validScrollPos);

                            //draw the node
                            g.FillRectangle(blueBrush, region);
                            g.DrawRectangle(blackPen, region);

                            blackPen.Dispose();
                            blueBrush.Dispose();
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
                                new Point((int)(b.collider.x - b.collider.radius), (int)b.collider.y),
                                new Point((int)(b.collider.x - b.collider.radius * 0.66f), (int)(b.collider.y + b.collider.radius)),
                                new Point((int)(b.collider.x + b.collider.radius * 0.66f), (int)(b.collider.y + b.collider.radius)),
                                new Point((int)(b.collider.x + b.collider.radius), (int)b.collider.y),
                                new Point((int)(b.collider.x + b.collider.radius * 0.66f), (int)(b.collider.y - b.collider.radius)),
                                new Point((int)(b.collider.x - b.collider.radius * 0.66f), (int)(b.collider.y - b.collider.radius)),
                            };

                            hexagon = OffsetPolygon(hexagon, validScrollPos);

                            //draw the node
                            g.FillPolygon(brownBrush, hexagon);
                            g.DrawPolygon(blackPen, hexagon);

                            blackPen.Dispose();
                            brownBrush.Dispose();
                        }

                        break;

                }
            }
        }



        /*
        * drawConnections
        * 
        * draws the lines that represent connections
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        * 
        */
        public void drawConnections(PaintEventArgs e)
        {
            //get the amount of scroll that is offsetting the window
            this.AutoScrollPosition = validScrollPos;

            //get the graphics object to paint with
            Graphics g = e.Graphics;

            //create the pen and brush to draw the outlined circle with
            Pen blackPen = new Pen(Color.Black, 2.0f);

            //iterate through all nodes in the node list
            foreach (BehaviourNode b in nodes)
            {
                //position of the child connector
                float cx = b.collider.x - validScrollPos.X;
                float cy = b.collider.y + b.connectorOffsets[1] - validScrollPos.Y;

                //get the size of the children array
                int size = b.children.Count;

                for (int i = 0; i < size; i++)
                {
                    //store the child in a temporary variable for performance and readability
                    BehaviourNode child = b.children[i];

                    //position of the child's parent connector
                    float px = child.collider.x - validScrollPos.X;
                    float py = child.collider.y + child.connectorOffsets[0] - validScrollPos.Y;

                    //draw a line between them
                    g.DrawLine(blackPen, cx, cy, px, py);
                }

            }

            //is a connection being currently formed?
            if (selection == SelectionType.CHILD || selection == SelectionType.PARENT)
            {
                //draw the line
                g.DrawLine(blackPen, tx1, ty1, tx2, ty2);
            }

            blackPen.Dispose();
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

                int size = b.connectorOffsets.Count();

                for (int i = 0; i < size; i++)
                {
                    //define a region to draw the node in
                    Rectangle region = new Rectangle((int)(b.collider.x - b.collider.radius * 0.33f), (int)(b.collider.y + b.connectorOffsets[i] - b.collider.radius * 0.33f), (int)(b.collider.radius * 0.66f), (int)(b.collider.radius * 0.66));

                    region = OffsetRectangle(region, validScrollPos);

                    //draw the node
                    g.FillPie(whiteBrush, region, 0.0f, 360.0f);
                    g.DrawArc(blackPen, region, 0.0f, 360.0f);

                    //actions and conditions don't have children
                    if (b.type == BehaviourType.CONDITION || b.type == BehaviourType.ACTION)
                    {
                        break;
                    }
                }

                blackPen.Dispose();
                whiteBrush.Dispose();

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
