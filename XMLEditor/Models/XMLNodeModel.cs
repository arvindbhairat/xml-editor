using System;
using System.Collections.Generic;

namespace XMLEditor.Models
{
    public class XMLNodeModel
    {
        public string NodeName { get; set; }
        public string NodeID { get; set; } = Guid.NewGuid().ToString("N").Substring(0, 10);
        public Dictionary<string, string> Attributes { get; set; } = new Dictionary<string, string>();
        public List<XMLNodeModel> ChildNodes { get; set; } = new List<XMLNodeModel>();

    }
}