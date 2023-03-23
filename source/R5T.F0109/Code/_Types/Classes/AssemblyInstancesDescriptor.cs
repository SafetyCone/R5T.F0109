using System;

using R5T.T0142;
using R5T.T0170;


namespace R5T.F0109
{
    /// <summary>
    /// The result data from surveying an assembly.
    /// </summary>
    [DataTypeMarker]
    public class AssemblyInstancesDescriptor
    {
        /// <summary>
        /// Instances that were found.
        /// Note: Includes both obsolete and non-obsolete instances (<see cref="InstanceDescriptor.IsObsolete"/>).
        /// </summary>
        public InstanceDescriptor[] Instances { get; set; }
        /// <summary>
        /// All instance-containing types found, even if they are empty (contain no instances).
        /// </summary>
        public InstanceTypeDescriptor[] InstanceTypes { get; set; }
    }
}
