using System;
using System.Linq;

using R5T.F0124;
using R5T.T0132;
using R5T.T0172;


namespace R5T.F0109
{
    [FunctionalityMarker]
    public partial interface IAssemblyInstancesDescriptorOperator : IFunctionalityMarker
    {
        public IProjectFilePath[] Get_ProjectFilePaths(
            AssemblyInstancesDescriptor instances,
            bool orderAlphabetically = IValues.Default_Alphabetization_Constant)
        {
            var output = instances.Instances
                .Select(x => x.ProjectFilePath)
                .Append(instances.InstanceTypes
                    .Select(x => x.ProjectFilePath))
                .Distinct()
                .OrderAlphabetically_If(x => x.Value, orderAlphabetically)
                .Now();

            return output;
        }
    }
}
