using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{

    /*
    * class TreeHelper
    * 
    * a singleton which helps manage the tree stucture and ensures
    * that the tree structure is maintained (no cyclic connections, duplicate connections etc.)
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public static class TreeHelper
    {


        /*
        * GetRoot 
        * 
        * gets the root recursively given a node on the tree
        * 
        * @param BehaviourNode node - a node that belongs to that tree
        * @returns BehaviourNode - the root of the tree
        */
        public static BehaviourNode GetRoot(BehaviourNode node)
        {
            //check if the node has a parent
            if (node.parent != null)
            {
                //recursively get the root
                return TreeHelper.GetRoot(node.parent);
            }
            else
            {
                return node;
            }
        }


        /*
        * IsCyclic 
        * 
        * tests if creating two nodes will create a cycle
        * 
        * @param BehaviourNode start - the starting node of the planned connection
        * @param BehaviourNode end - the ending node of the planned connection
        * @returns bool - flag indicating if a connection would by cyclic
        */
        public static bool IsCyclic(BehaviourNode start, BehaviourNode end)
        {
            //the nodes are cyclic if they have the same root
            BehaviourNode root1 = TreeHelper.GetRoot(start);
            BehaviourNode root2 = TreeHelper.GetRoot(end);

            return root1 == root2;
        }


        /*
        * RemoveFromTree 
        * 
        * properly disconnects a node from a tree and removes it
        * 
        * @param List<BehaviourNode> nodes - the list of nodes
        * @param BehaviourNode node - the node to remove
        * @returns void
        */
        public static void RemoveFromTree(List<BehaviourNode> nodes, BehaviourNode node)
        {
            //disconnect all of the node's children
            foreach (BehaviourNode b in node.children)
            {
                b.parent = null;
            }

            //disconnect the parent from this node, if one exists
            if (node.parent != null)
            {
                node.parent.children.Remove(node);
            }

            nodes.Remove(node);
            
        }
    }
}
