using System;

using DeserializedType = R5T.F0109.InstanceTypeDescriptor;
using SerializedType = R5T.F0109.Serialization.InstanceTypeDescriptor;


namespace R5T.F0109.Extensions
{
    public static class InstanceTypeDescriptorExtensions
    {
        public static SerializedType ToSerializedType(this DeserializedType deserializedType)
        {
            return Instances.InstanceTypeDescriptorOperator.ToSerializedType(deserializedType);
        }

        public static DeserializedType ToDeserializedType(this SerializedType serializedType)
        {
            return Instances.InstanceTypeDescriptorOperator.ToDeserializedType(serializedType);
        }
    }
}
