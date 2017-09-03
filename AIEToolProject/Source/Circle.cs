using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace AIEToolProject.Source
{

    /*
    * class Circle
    * implements ICloneable
    * 
    * a circle data structure, holds a point and radius
    * includes collision-detection functions
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class Circle : ICloneable
    {
        //coordinates
        public float x = 0.0f;
        public float y = 0.0f;

        //size of the circle
        public float radius = 1.0f;


        /*
        * Circle()
        * default constructor
        */
        public Circle()
        {

        }


        /*
        * Circle(float _x, float _y, float _radius)
        * constructor, default assignments
        * 
        * @param float _x - input x coordinate of the circle
        * @param float _y - input y coordinate of the circle
        * @param float _radius - input radius of the circle
        */
        public Circle(float _x, float _y, float _radius)
        {
            x = _x;
            y = _y;
            radius = _radius;
        }


        /*
        * Clone 
        * implement's ICloneable's Clone()
        * creates another object identical to this
        * 
        * @param object - the object with matching member variables
        */
        public object Clone()
        {
            //create a new circle
            Circle other = new Circle();

            //copy member variables
            other.x = x;
            other.y = y;
            other.radius = radius;

            return other as object;
        }


        /*
        * IntersectingPoint 
        * 
        * tests if the circle is intersecting
        *  the point at (px, py)
        * 
        * 
        * @param px - the x coordinate of the point
        * @param py - the y coordinate of the point
        * @returns bool - intersection flag
        */
        public bool IntersectingPoint(float px, float py)
        {
            //relative coordinates
            float rx = px - x;
            float ry = py - y;

            //compare the square magnitude to the squared radius
            float sqrMag = rx * rx + ry * ry;

            return sqrMag < radius * radius;
        }


        /*
        * IntersectingCircle
        * 
        * tests if the circle is intersecting
        * another circle
        * 
        * 
        * @param Circle c - the other circle
        * @returns bool - intersection flag
        */
        public bool IntersectingCircle(Circle c)
        {
            //relative coordinates
            float rx = c.x - x;
            float ry = c.y - y;

            //compare the square magnitude to the squared sum of the radii
            float sqrMag = rx * rx + ry * ry;
            float combRadii = radius + c.radius;

            return sqrMag < (combRadii * combRadii);
        }


    }
}
