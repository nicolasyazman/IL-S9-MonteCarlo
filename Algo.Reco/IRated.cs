using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algo
{
    public interface IRatedBy<T>
    {
        Dictionary<T, int> Ratings { get; }
    }
}
