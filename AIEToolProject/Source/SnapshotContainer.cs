using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{
    /*
    * class SnapshotContainer
    * 
    * records the state of a form, used to undo the current state
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public class SnapshotContainer
    {
        //maximum amount of trees that can be undone to
        public int maxTrees = 10;

        //the last 'n' trees in the container
        public List<BehaviourTree> trees = null;

        /*
        * SnapshotContainer() 
        * constructor, creates a new list
        */
        public SnapshotContainer()
        {
            trees = new List<BehaviourTree>();
        }


        /*
        * Add 
        * 
        * adds a new tree to the list of trees
        * 
        * @param BehaviourTree tree - the tree to add
        * @returns void
        */
        public void Add(BehaviourTree tree)
        {
            //check if a tree can be added without breaking the limit on trees allowed
            if (trees.Count < maxTrees)
            {
                trees.Insert(0, tree);
            }
        }


        /*
        * Pop 
        * 
        * removes the latest tree from the list
        * 
        * @returns void
        */
        public void Pop()
        {
            //check there is a tree to remove
            if (trees.Count > 1)
            {
                trees.RemoveAt(0);
            }
        }


        /*
        * First
        * property 
        *
        * -get 
        * @returns BehaviourTree - the first tree in the array 
        * 
        * -set
        * @sets the value
        */
        public BehaviourTree First
        {
            get
            {
                //check if there is a tree in the array
                if (trees.Count > 0)
                {
                    //return the first tree
                    return trees[0];
                }
                else
                {
                    //there are no trees, return nothing
                    return null;
                }
            }

            set
            {
                trees[0] = value;
            }
        }

    }
}
