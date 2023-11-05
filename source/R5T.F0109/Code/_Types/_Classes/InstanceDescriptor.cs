using System;

using R5T.L0062.T000;
using R5T.L0063.T000;
using R5T.T0142;


namespace R5T.F0109
{
    /// <summary>
    /// Descripts an instance for use in assembly surveys.
    /// </summary>
    [DataTypeMarker]
    public class InstanceDescriptor
    {
        public bool IsObsolete { get; set; }
        public IIdentityString IdentityString { get; set; }
        public ISignatureString SignatureString { get; set; }
    }
}
