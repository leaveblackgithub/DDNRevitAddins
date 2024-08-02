using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDNGeneralLibrary
{
    public static class GuidExtension
    {
        public static Guid NewGUID()
        {
            return Guid.NewGuid();
        }
    }
}
