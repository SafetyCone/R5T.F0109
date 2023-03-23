using System;

using R5T.T0142;


namespace R5T.F0109.Serialization
{
    [DataTypeMarker]
    public class InstanceTypeDescriptor
    {
        public string ProjectFilePath { get; set; }
        public string TypeName { get; set; }
        public bool IsObsolete { get; set; }
    }
}
