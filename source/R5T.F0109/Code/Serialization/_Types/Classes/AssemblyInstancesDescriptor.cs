using System;

using R5T.T0142;


namespace R5T.F0109.Serialization
{
    [DataTypeMarker]
    public class AssemblyInstancesDescriptor
    {
        /// <inheritdoc cref="F0109.AssemblyInstancesDescriptor.Instances"/>
        public T0170.Serialization.InstanceDescriptor[] Instances { get; set; }

        /// <inheritdoc cref="F0109.AssemblyInstancesDescriptor.InstanceTypes"/>
        public InstanceTypeDescriptor[] InstanceTypes { get; set; }
    }
}
