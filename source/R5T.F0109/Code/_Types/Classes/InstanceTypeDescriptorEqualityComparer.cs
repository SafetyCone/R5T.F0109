using System;
using System.Collections.Generic;


namespace R5T.F0109
{
    public class InstanceTypeDescriptorEqualityComparer : IEqualityComparer<InstanceTypeDescriptor>
    {
        #region Static

        public static InstanceTypeDescriptorEqualityComparer Instance { get; } = new InstanceTypeDescriptorEqualityComparer();

        #endregion


        public bool Equals(InstanceTypeDescriptor x, InstanceTypeDescriptor y)
        {
            var output = true
                && x.TypeName.Equals(y.TypeName)
                && x.IsObsolete.Equals(y.IsObsolete)
                && x.ProjectFilePath.Equals(y.ProjectFilePath)
                ;

            return output;
        }

        public int GetHashCode(InstanceTypeDescriptor obj)
        {
            var hashCode = HashCode.Combine(
                obj.ProjectFilePath,
                obj.TypeName,
                obj.IsObsolete);

            return hashCode;
        }
    }
}
