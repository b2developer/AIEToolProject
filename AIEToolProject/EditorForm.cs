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
using AIEToolProject.Source.Selection;

namespace AIEToolProject
{

    public partial class EditorForm : Form
    {
        //container of behaviour trees
        public SnapshotContainer snapshots = null;

        //current selection being made
        public Selection currentSelection = null;

        //default radius of all nodes
        public float scalar = 45.0f;

        //proportional radius of the connector circles
        public float connectorRatio = 0.33f;

        //empty labels that define the scrollable area
        public Label topLeft = null;
        public Label bottomRight = null;

        //remembers the last valid scroll position
        public Point validScrollPos = new Point(0, 0);

        //data for a temporary line to draw when a connection is being formed
        public float tx1 = 0.0f;
        public float tx2 = 0.0f;
        public float ty1 = 0.0f;
        public float ty2 = 0.0f;

        public EditorForm()
        {
            InitializeComponent();

            //create the snapshot container and add a default tree
            snapshots = new SnapshotContainer();
            snapshots.Add(new BehaviourTree());
            snapshots.First.nodes = new List<BehaviourNode>();

            topLeft = new Label();
            topLeft.Size = new Size(0, 0);

            bottomRight = new Label();
            bottomRight.Size = new Size(0, 0);

            this.Controls.Add(topLeft);
            this.Controls.Add(bottomRight);

        }


        /*
        * Record
        * 
        * copies the current tree, allows the program 
        * to remember the previous state if any action
        * is performed
        * 
        * @returns void
        */
        public void Record()
        {
            snapshots.Add(snapshots.First);
        }


        /*
        * Undo
        * 
        * pops the latest addition to the tree
        * 
        * @returns void
        */
        public void Undo()
        {
            snapshots.Add(snapshots.First);
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
            if (snapshots.First.nodes.Count > 0)
            {
                topLeftBest = new Point((int)snapshots.First.nodes[0].collider.x, (int)snapshots.First.nodes[0].collider.y);
                bottomRightBest = new Point((int)snapshots.First.nodes[0].collider.x, (int)snapshots.First.nodes[0].collider.y);
            }

            //iterate through all nodes in the nodes container
            foreach (BehaviourNode b in snapshots.First.nodes)
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
        * GenerateSelection 
        * 
        * creates the appropriate selection given
        * the position of the mouse relative to the 
        * nodes
        * 
        * @param mouseEventArgs mouseE - the mouse state arguments
        * @param Point trueMousePos - position of the mouse relative to the screen
        * @returns Selection - the new selection object
        */
        private Selection GenerateSelection(MouseEventArgs mouseE, Point trueMousePos)
        {
            //iterate through all nodes, checking for collision with the mouse
            foreach (BehaviourNode b in snapshots.First.nodes)
            {
                //test if the circle is intersecting the mouse position
                if (b.collider.IntersectingPoint(trueMousePos.X, trueMousePos.Y))
                {
                    //remember the selected node
                    BehaviourNode s = b;

                    //circles representing the connector
                    Circle parentCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[1], b.collider.radius * connectorRatio);
                    Circle childCircle = new Circle(b.collider.x, b.collider.y + b.connectorOffsets[0], b.collider.radius * connectorRatio);

                    //check for intersections with the connector circles before accepting the node
                    if (childCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y))
                    {
                        currentSelection = new ConnectionSelection();

                        currentSelection.mouseArgs = mouseE;

                        //down cast the selection
                        ConnectionSelection trueSelection = (currentSelection as ConnectionSelection);

                        //give the selection it's values
                        trueSelection.form = this;
                        trueSelection.node = s;
                        trueSelection.type = ConnectionType.CHILDREN;

                        //check which mouse button is being pressed
                        if (mouseE.Button == MouseButtons.Left)
                        {
                            currentSelection.LeftDown();
                        }
                        else if (mouseE.Button == MouseButtons.Right)
                        {
                            currentSelection.RightDown();
                        }

                        return currentSelection;

                    }   
                    else if (parentCircle.IntersectingPoint(trueMousePos.X, trueMousePos.Y) && b.type != BehaviourType.CONDITION && b.type != BehaviourType.ACTION)
                    {
                        currentSelection = new ConnectionSelection();

                        //down cast the selection
                        ConnectionSelection trueSelection = (currentSelection as ConnectionSelection);

                        currentSelection.mouseArgs = mouseE;

                        //give the selection it's values
                        trueSelection.form = this;
                        trueSelection.node = s;
                        trueSelection.type = ConnectionType.PARENT;

                        //check which mouse button is being pressed
                        if (mouseE.Button == MouseButtons.Left)
                        {
                            currentSelection.LeftDown();
                        }
                        else if (mouseE.Button == MouseButtons.Right)
                        {
                            currentSelection.RightDown();

                            currentSelection = null;

                            return null;
                        }

                        return currentSelection;
                    }
                    else
                    {
                        currentSelection = new NodeSelection();

                        currentSelection.mouseArgs = mouseE;

                        //down cast the selection
                        NodeSelection trueSelection = (currentSelection as NodeSelection);

                        //give the selection it's values
                        trueSelection.form = this;
                        trueSelection.node = s;

                        //check which mouse button is being pressed
                        if (mouseE.Button == MouseButtons.Left)
                        {
                            currentSelection.LeftDown();
                        }
                        else if (mouseE.Button == MouseButtons.Right)
                        {
                            currentSelection.RightDown();

                            //remove the node's link to it's children
                            s.children.Clear();

                            //remove the node's parent link (if it has one)
                            if (s.parent != null)
                            {
                                s.parent.children.Remove(s);
                            }

                            //remove the node from the tree
                            snapshots.First.nodes.Remove(s);

                            //don't create a new selection
                            trueSelection.node = null;

                            return null;
                        }

                        return currentSelection;
                    }
                }
            }

            //there were no cirumstances in which to generate a selection
            return null;
        }


        /*
        * EditorForm_MouseDown
        * 
        * callback when the form is clicked on (down and then up)
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

                if (currentSelection == null)
                {
                    currentSelection = GenerateSelection(mouseE, trueMousePos);
                }
                else
                {
                    //set the mouse arguments
                    currentSelection.mouseArgs = e;

                    //check which mouse button is being pressed
                    if (mouseE.Button == MouseButtons.Left)
                    {
                        currentSelection.LeftDown();

                    }
                    else if (mouseE.Button == MouseButtons.Right)
                    {

                        currentSelection.RightDown();
                    }
                }
            }

        }


        /*
        * EditorForm_Click 
        * 
        * callback when the form is clicked (down)
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

                if (currentSelection == null)
                {

                    if (mouseE.Button == MouseButtons.Left)
                    {
                        //nothing was selected, the user is trying to create a node
                        BehaviourNode node = new BehaviourNode();

                        //set the position
                        node.collider.x = mouseE.X + scrollPos.X;
                        node.collider.y = mouseE.Y + scrollPos.Y;

                        //set the circle radius
                        node.collider.radius = scalar;

                        node.type = (MdiParent as MainForm).selectedType;

                        node.connectorOffsets = new float[] { -node.collider.radius * 0.7f, node.collider.radius * 0.7f };

                        snapshots.First.nodes.Add(node);

                    }

                }
                else
                {
                    //set the mouse arguments
                    currentSelection.mouseArgs = e;

                    if (mouseE.Button == MouseButtons.Left)
                    {
                        currentSelection.LeftUp();

                    }
                    else if (mouseE.Button == MouseButtons.Right)
                    {
                        currentSelection.RightUp();
                    }

                    //reset the selection
                    currentSelection = null;

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

                if (currentSelection != null)
                {
                    //set the mouse arguments
                    currentSelection.mouseArgs = e;

                    currentSelection.Move();
                }

                this.Refresh();

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
            foreach (BehaviourNode b in snapshots.First.nodes)
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
            foreach (BehaviourNode b in snapshots.First.nodes)
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
            if (currentSelection != null && currentSelection.GetType() == (new ConnectionSelection()).GetType())
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
            foreach (BehaviourNode b in snapshots.First.nodes)
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

            //force the form to re-draw itself
            Invalidate();
            this.Refresh();
        }
    }
}
