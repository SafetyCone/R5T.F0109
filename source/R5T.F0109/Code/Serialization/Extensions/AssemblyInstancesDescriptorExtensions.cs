using System;

using DeserializedType = R5T.F0109.AssemblyInstancesDescriptor;
using SerializedType = R5T.F0109.Serialization.AssemblyInstancesDescriptor;


namespace R5T.F0109.Extensions
{
    public static class AssemblyInstancesDescriptorExtensions
    {
        public static SerializedType ToSerializedType(this DeserializedType deserializedType)
        {
            return Instances.AssemblyInstancesDescriptorOperator.ToSerializedType(deserializedType);
        }

        public static DeserializedType ToDeserializedType(this SerializedType serializedType)
        {
            return Instances.AssemblyInstancesDescriptorOperator.ToDeserializedType(serializedType);
        }
    }
}
