using System;
using System.Linq;

using R5T.T0132;
using R5T.T0160;
using R5T.T0170.Extensions;

using R5T.F0109.Extensions;

using DeserializedType = R5T.F0109.AssemblyInstancesDescriptor;
using SerializedType = R5T.F0109.Serialization.AssemblyInstancesDescriptor;


namespace R5T.F0109
{
    public partial interface IAssemblyInstancesDescriptorOperator : IFunctionalityMarker
    {
        public DeserializedType Deserialize_Synchronous(
            JsonFilePath jsonFilePath)
        {
            var output = Instances.JsonOperator.Deserialize_Synchronous<SerializedType>(jsonFilePath)
                .ToDeserializedType();

            return output;
        }

        public void Serialize_Synchronous(
            JsonFilePath jsonFilePath,
            DeserializedType instances)
        {
            var serializable = instances.ToSerializedType();

            Instances.JsonOperator.Serialize(
                jsonFilePath.Value,
                serializable);
        }

        public SerializedType ToSerializedType(DeserializedType deserializedType)
        {
            var output = new SerializedType
            {
                InstanceTypes = deserializedType.InstanceTypes.Select(x => x.ToSerializedType()).Now(),
                Instances = deserializedType.Instances.Select(x => x.ToSerializedType()).Now(),
            };

            return output;
        }

        public DeserializedType ToDeserializedType(SerializedType serializedType)
        {
            var output = new DeserializedType
            {
                InstanceTypes = serializedType.InstanceTypes.Select(x => x.ToDeserializedType()).Now(),
                Instances = serializedType.Instances.Select(x => x.ToDeserializedType()).Now(),
            };

            return output;
        }
    }
}