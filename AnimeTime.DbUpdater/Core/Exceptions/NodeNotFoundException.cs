using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeTimeDbUpdater.Core.Exceptions
{
    public class NodeNotFoundException : Exception
    {
        public string Node { get; }
        public string Page { get; }

        public NodeNotFoundException(string node, string page) : base($"XML '{node}' node not found on page {page}.")
        {
            Node = node;
            Page = page;
        }
    }
}
