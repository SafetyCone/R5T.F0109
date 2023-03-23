using System;
using System.Linq;

using R5T.F0109.Extensions;
using R5T.T0132;
using R5T.T0160;
using R5T.T0161.Extensions;
using R5T.T0172.Extensions;

using DeserializedType = R5T.F0109.InstanceTypeDescriptor;
using SerializedType = R5T.F0109.Serialization.InstanceTypeDescriptor;


namespace R5T.F0109
{
    public partial interface IInstanceTypeDescriptorOperator : IFunctionalityMarker
    {
        public DeserializedType[] Deserialize_Synchronous(JsonFilePath jsonFilePath)
        {
            var output = Instances.JsonOperator.Deserialize_Synchronous<SerializedType[]>(jsonFilePath.Value)
                .Select(x => x.ToDeserializedType())
                .Now();

            return output;
        }

        public void Serialize_Synchronous(
            JsonFilePath jsonFilePath,
            DeserializedType[] instances)
        {
            var serializable = instances
                .Select(x => x.ToSerializedType())
                .Now();

            Instances.JsonOperator.Serialize_Synchronous(
                jsonFilePath.Value,
                serializable);
        }

        public SerializedType ToSerializedType(DeserializedType deserializedType)
        {
            var output = new SerializedType
            {
                ProjectFilePath = deserializedType.ProjectFilePath.Value,
                IsObsolete = deserializedType.IsObsolete,
                TypeName = deserializedType.TypeName,
            };

            return output;
        }

        public DeserializedType ToDeserializedType(SerializedType serializedType)
        {
            var output = new DeserializedType
            {
                ProjectFilePath = serializedType.ProjectFilePath.ToProjectFilePath(),
                IsObsolete = serializedType.IsObsolete,
                TypeName = serializedType.TypeName.ToKindMarkedFullMemberName(),
            };

            return output;
        }
    }
}
