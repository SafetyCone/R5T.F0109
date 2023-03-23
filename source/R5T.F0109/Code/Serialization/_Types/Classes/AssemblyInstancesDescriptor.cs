using System;

using R5T.T0142;
using R5T.T0170.Serialization;


namespace R5T.F0109.Serialization
{
    [DataTypeMarker]
    public class AssemblyInstancesDescriptor
    {
        /// <inheritdoc cref="F0109.AssemblyInstancesDescriptor.Instances"/>
        public InstanceDescriptor[] Instances { get; set; }

        /// <inheritdoc cref="F0109.AssemblyInstancesDescriptor.InstanceTypes"/>
        public InstanceTypeDescriptor[] InstanceTypes { get; set; }
    }
}
