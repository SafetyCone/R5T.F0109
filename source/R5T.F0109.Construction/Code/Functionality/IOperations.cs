using System;

using R5T.T0132;
using R5T.T0172.Extensions;

using R5T.F0109.Serialization;


namespace R5T.F0109.Construction
{
    [FunctionalityMarker]
    public partial interface IOperations : IFunctionalityMarker
    {
        private static F0109.IOperations LibraryOperations => F0109.Operations.Instance;


        public void DeserializationOfAssemblyInstancesDescriptor()
        {
            /// Inputs.
            var jsonFilePath = Instances.FilePaths.OutputJsonFilePath;


            /// Run.
            var deserialization = Instances.JsonOperator.Deserialize_Synchronous<Serialization.AssemblyInstancesDescriptor>(jsonFilePath);

            // Works!
        }

        public void GetAssemblyInstances()
        {
            /// Inputs.
            var outputJsonFilePath = Instances.FilePaths.OutputJsonFilePath;


            /// Run.
            var projectFilePath = @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.T0167\source\R5T.T0167\R5T.T0167.csproj".ToProjectFilePath();

            var assemblyDocumentationFilePath = Instances.FilePaths.AllMarkerAttributeTypesAssemblyDocumentationFilePath;

            var documentationByMemberIdentityName = Instances.DocumentationOperations.GetDocumentationByMemberIdentityName(assemblyDocumentationFilePath);

            var instanceVarietyDescriptorsByName = Instances.InstanceVarietyDescriptors.AllByName;

            var assemblyInstancesDescriptor = default(AssemblyInstancesDescriptor);

            Instances.AssemblyReflectionContextOperator.InAssemblyReflectionContext(assembly =>
            {
                assemblyInstancesDescriptor = LibraryOperations.Get_InstanceDescriptors(
                    projectFilePath,
                    assembly,
                    documentationByMemberIdentityName,
                    instanceVarietyDescriptorsByName);
            });

            var serialization = assemblyInstancesDescriptor.ToSerializedType();

            Instances.JsonOperator.Serialize_Synchronous(
                outputJsonFilePath,
                serialization);

            Instances.NotepadPlusPlusOperator.Open(outputJsonFilePath);
        }
    }
}
