using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTime.Core.Exceptions
{
    public class EntityInsertException : Exception
    {
        public EntityInsertException(string message) : base(message)
        {
        }

        public EntityInsertException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
