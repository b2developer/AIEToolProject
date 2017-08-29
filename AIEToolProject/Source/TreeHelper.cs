using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{
    /*
    * static class TreeHelper
    *
    * a singleton that calculates solutions to tree related
    * structural problems (eg. adding a new node breaking the tree)
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public static class TreeHelper
    {

        /*
        * SharedRoot 
        * 
        * tests if two nodes share the same root
        * 
        * @param Node n1 - the first node
        * @param Node n2 - the second node
        * @returns bool - flag indicating if they share the same root 
        */
        public static bool SharedRoot(Node n1, Node n2)
        {
            //recursively get the uppermost parent node of both nodes
            if (n1.parent != null)
            {
                return SharedRoot(n1.parent, n2);
            }
            else if (n2.parent != null)
            {
                return SharedRoot(n1, n2.parent);
            }

            //the uppermost parents have been found, compare them
            return n1 == n2;
        }
    }
}
