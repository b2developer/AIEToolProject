using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChecklistControl
{
    public partial class CheckListControl : UserControl
    {

        //reference to the button to enable
        public Button button = null;

        //list of messages to display
        public List<string> reasons;

        //list of boolean values that match the messages
        public List<bool> switches;

        //size of the text
        public float textSize = 6.0f;

        public CheckListControl()
        {
            InitializeComponent();

            //create the new lists
            reasons = new List<string>();
            switches = new List<bool>();
        }


        /*
        * RegisterReason
        * 
        * adds a display name and a boolean value to change
        * 
        * @param string reason - the display message to add
        * @param bool defaultValue - the default value for the boolean
        * @returns void
        */
        public void RegisterReason(string reason, bool defaultValue)
        {
            reasons.Add(reason);
            switches.Add(defaultValue);
        }


        /*
        * ChangeReason
        * 
        * changes the boolean value associated with a reason to the specified value
        * 
        * @param string reason - the reaspnm
        * @param bool newValue - the new boolean value
        * @returns void
        */
        public void ChangeReason(string reason, bool newValue)
        {
            //get the size of the reasons list
            int reasonSize = reasons.Count;

            //iterate through all of the reasons, searching for the correct string
            for (int i = 0; i < reasonSize; i++)
            {
                //check if the reason matches
                if (reasons[i] == reason)
                {
                    //assign the new value and break
                    switches[i] = newValue;
                    break;
                }
            }

            //check if there is still a false condition
            bool foundFalse = false;

            //iterate through all of th ereasons, searching for a false value
            for (int i = 0; i < reasonSize; i++)
            {
                //break the 
                if (!switches[i])
                {
                    foundFalse = true;
                    break;
                }
            }

            button.Enabled = !foundFalse;

            Invalidate();
        }


        /*
        * OnPaint
        * protects Form's OnPaint(PaintEventArgs e)
        * 
        * the custom draw call of the control, lists
        * all incorrect reasons
        * 
        * @param PaintEventArgs e - the arguments provided to paint the custom textures
        * @returns void
        */
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //get the graphics object to draw with
            Graphics g = e.Graphics;

            //define a font
            Font font = new Font("Arial", textSize);

            //create the brush to draw with
            SolidBrush blackBrush = new SolidBrush(Color.Black);

            //get the size of the reasons list
            int reasonSize = reasons.Count;

            //track the line number
            int lineNum = 0;

            //iterate through all of the reasons, searching for reasons with false corresponding values
            for (int i = 0; i < reasonSize; i++)
            {
                if (!switches[i])
                {
                    //create a format object
                    StringFormat format = new StringFormat();

                    //set the text to allign to the centre
                    format.Alignment = StringAlignment.Center;

                    Point centrePosition = new Point((int)(this.Size.Width * 0.5f), (int)(textSize + textSize * lineNum * 2.0f));

                    //draw the text
                    g.DrawString(reasons[i], font, blackBrush, centrePosition, format);

                    lineNum++;
                }
            }

            font.Dispose();
            blackBrush.Dispose();
        }
    }
    
}
