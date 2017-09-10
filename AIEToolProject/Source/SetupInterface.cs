using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEToolProject.Source
{
    /*
    * interface IInitialised
    * 
    * custom interface for defining initialisation related method templates
    * 
    * author: Bradley Booth, Academy of Interactive Entertainment, 2017
    */
    public interface IInitialised
    {
        //functions that require implementing if this interface is used
        void Initialise();
        void DeInitialise();
    }
}
