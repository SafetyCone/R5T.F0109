using System;
using System.Collections.Generic;
using System.Linq;

using R5T.T0132;
using R5T.T0172;


namespace R5T.F0109
{
    [FunctionalityMarker]
    public partial interface IInstanceTypeDescriptorOperator : IFunctionalityMarker
    {
        /// <summary>
        /// <inheritdoc cref="Documentation.For_CompareRunInstances" path="/summary"/>
        /// </summary>
        /// <param name="runInstanceTypes">The instances from today's run.</param>
        /// <param name="priorToTodayInstanceTypes">The full set of instances accumulated over time.</param>
        /// <param name="projectsToIgnoreFilePathsHash">The list of projects that failed to build or to process during today's run.</param>
        public (
            InstanceTypeDescriptor[] newInstanceTypes,
            InstanceTypeDescriptor[] removedInstanceTypes,
            InstanceTypeDescriptor[] missingInstanceTypes)
        CompareRunInstances(
            IList<InstanceTypeDescriptor> runInstanceTypes,
            IList<InstanceTypeDescriptor> priorToTodayInstanceTypes,
            // Do not include "do not build" projects, so that any instances in them are advertised forever.
            HashSet<IProjectFilePath> projectsToIgnoreFilePathsHash)
        {
            // Use an equality comparer that does not care about the description (since the description being updated doesn't really create a new instance descriptor).
            var instanceDescriptorEqualityComparer = InstanceTypeDescriptorEqualityComparer.Instance;

            // Determine added instances: these are easy, it's just what instances exist in the "per-run" file that don't exist in the "prior-to" today file.
            var newInstances = runInstanceTypes.Except(
                priorToTodayInstanceTypes,
                instanceDescriptorEqualityComparer)
                .Now();

            // Determine removed instances: these are harder.
            // First determine what instances exist in the "prior-to" today file that do not exist in the "per-run" file.
            // Then load the build problems and processing problems file paths.
            // For any instances that are in projects in either of the build problems or processing problems files, remove them from the list.
            // Whatever instances remain, those instances have actually been removed.
            var missingInstances = priorToTodayInstanceTypes.Except(
                runInstanceTypes,
                instanceDescriptorEqualityComparer)
                .Now();

            var removedInstances = missingInstances
                .Where(instance =>
                {
                    var ignoreProject = projectsToIgnoreFilePathsHash.Contains(
                        instance.ProjectFilePath);

                    var output = !ignoreProject;
                    return output;
                })
                .Now();

            return (newInstances, removedInstances, missingInstances);
        }
    }
}
