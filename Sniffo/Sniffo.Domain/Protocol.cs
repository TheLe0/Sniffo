using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sniffo.Domain
{
    public abstract class Protocol
    {
        protected int Length { get; set; }

        public Protocol(int length)
        {
            Length = length;
        }
    }
}
