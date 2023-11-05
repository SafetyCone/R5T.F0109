using System;

using R5T.L0062.T000;
using R5T.T0142;
using R5T.T0172;


namespace R5T.F0109
{
    [DataTypeMarker]
    public class InstanceTypeDescriptor
    {
        public IProjectFilePath ProjectFilePath { get; set; }
        public IIdentityString TypeIdentityString { get; set; }
        public bool IsObsolete { get; set; }
    }
}
