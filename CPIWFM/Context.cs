using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPIWFM
{
    class Context
    {
        private Strategy oStrategy;
        public Context(Strategy oStrategy)
        {
            this.oStrategy = oStrategy;
        }
        public void ContextInterface()
        {
            oStrategy.AlgorithmForTheStrategy();
        }
    }
}
