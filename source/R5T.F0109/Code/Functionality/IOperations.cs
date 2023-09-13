using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using R5T.F0099.T000;
using R5T.L0053.Extensions;
using R5T.T0132;
using R5T.T0161;
using R5T.T0161.Extensions;
using R5T.T0162;
using R5T.T0162.Extensions;
using R5T.T0169;
using R5T.T0170;
using R5T.T0171;
using R5T.T0172;


namespace R5T.F0109
{
    [FunctionalityMarker]
    public partial interface IOperations : IFunctionalityMarker
    {
        /// <summary>
        /// Special method required to check the "is-X" value (if the value present) for marker attribute constructors.
        /// </summary>
        public bool Has_AttributeOfTypeWithNonFalseConstructorValue(
            TypeInfo typeInfo,
            InstanceVarietyDescriptor instanceVarietyDescriptor)
        {
            var hasAttributeOfType = Instances.TypeOperator.HasAttributeOfType(
                typeInfo,
                instanceVarietyDescriptor.MarkerAttributeTypeName.Value);

            // All marker attributes have a parameter to indicated that even though they are applied to a type, the type they are applied to is NOT to be included.
            if (hasAttributeOfType.Result?.ConstructorArguments.Any() ?? false)
            {
                // The first contructor argument is always used, it might be named or not, and this handles named arguments.
                var constructorArgument = hasAttributeOfType.Result.ConstructorArguments.FirstOrDefault();

                // The value is always an "is X", so if false, we don't include the type.
                var isMarked = Convert.ToBoolean(constructorArgument.Value);
                if (!isMarked)
                {
                    return false;
                }
            }

            var output = true
               // Does the type have the attribute?
               && hasAttributeOfType
               ;

            return output;
        }

        public Dictionary<IInstanceVarietyName, Func<TypeInfo, bool>> Get_TypeIsOfInterestPredicates(
            IDictionary<IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName)
        {
            var typeIsOfInterestPredicatesByInstanceVarietyName = new Dictionary<IInstanceVarietyName, Func<TypeInfo, bool>>();

            foreach (var instanceVarietyDescriptor in instanceVarietyDescriptorsByName.Values)
            {
                bool predicate(TypeInfo typeInfo) => this.Has_AttributeOfTypeWithNonFalseConstructorValue(
                        typeInfo,
                        instanceVarietyDescriptor);

                typeIsOfInterestPredicatesByInstanceVarietyName.Add(
                    instanceVarietyDescriptor.Name,
                    predicate);
            }

            return typeIsOfInterestPredicatesByInstanceVarietyName;
        }

        /// <summary>
        /// Match variety targets (type, methods of type, etc.) to functions that will generate names for instances of each target.
        /// </summary>
        /// <returns></returns>
        public Dictionary<InstanceVarietyTarget, Func<TypeInfo, (IIdentityName, IKindMarkedFullMemberName, bool)[]>>
            Get_InstanceNameGeneratorFunctions()
        {
            // Build instance generator functions for each variety target.
            var instanceNameGeneratorFunctionsByInstanceVarietyTarget = new Dictionary<InstanceVarietyTarget, Func<TypeInfo, (IIdentityName, IKindMarkedFullMemberName, bool)[]>>
            {
                {
                    InstanceVarietyTarget.MethodsAsProperties,
                    Instances.InstanceNameGenerators.For_MethodsAsProperties
                },
                {
                    InstanceVarietyTarget.MethodsOfType,
                    Instances.InstanceNameGenerators.For_MethodsOfType
                },
                {
                    InstanceVarietyTarget.PropertiesOfType,
                    Instances.InstanceNameGenerators.For_PropertiesOfType
                },
                {
                    InstanceVarietyTarget.StaticReadOnlyObjects,
                    Instances.InstanceNameGenerators.For_StaticReadOnlyObjects
                },
                {
                    InstanceVarietyTarget.Type,
                    Instances.InstanceNameGenerators.For_Type
                }
            };

            return instanceNameGeneratorFunctionsByInstanceVarietyTarget;
        }

        public AssemblyInstancesDescriptor Get_InstanceDescriptors(
            IProjectFilePath projectFilePath,
            Assembly assembly,
            DocumentationByMemberIdentityName documentationByMemberIdentityName,
            IDictionary<IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName)
        {
            var typeIsOfInterestPredicatesByInstanceVarietyName = this.Get_TypeIsOfInterestPredicates(instanceVarietyDescriptorsByName);
            var instanceNameGeneratorFunctionsByInstanceVarietyTarget = this.Get_InstanceNameGeneratorFunctions();

            return this.Get_InstanceDescriptors(
                projectFilePath,
                assembly,
                documentationByMemberIdentityName,
                instanceVarietyDescriptorsByName,
                typeIsOfInterestPredicatesByInstanceVarietyName,
                instanceNameGeneratorFunctionsByInstanceVarietyTarget);
        }

        public AssemblyInstancesDescriptor Get_InstanceDescriptors(
            IProjectFilePath projectFilePath,
            Assembly assembly,
            DocumentationByMemberIdentityName documentationByMemberIdentityName,
            IDictionary<IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName,
            IDictionary<IInstanceVarietyName, Func<TypeInfo, bool>> typeIsOfInterestPredicatesByInstanceVarietyName,
            IDictionary<InstanceVarietyTarget, Func<TypeInfo, (IIdentityName, IKindMarkedFullMemberName, bool)[]>> instanceNameGeneratorFunctionsByInstanceVarietyTarget)
        {
            // Iterate over all types in the assembly.
            var typesOfInterestByInstanceVarietyName = new Dictionary<IInstanceVarietyName, List<TypeInfo>>();

            assembly.Foreach_Type(typeInfo =>
            {
                foreach (var pair in typeIsOfInterestPredicatesByInstanceVarietyName)
                {
                    var typeIsOfInterestToVariety = pair.Value(typeInfo);

                    if(typeIsOfInterestToVariety)
                    {
                        // Add the type to the instances types list.
                        Instances.DictionaryOperator.AddValue(
                            typesOfInterestByInstanceVarietyName,
                            pair.Key,
                            typeInfo);
                    }
                }
            });

            // For all types that *are* instances, get their kind-marked full member names.
            var instanceTypesInAssembly = typesOfInterestByInstanceVarietyName
                .SelectMany(x => x.Value)
                ;

            var instanceTypeKindMarkedFullMemberNames = instanceTypesInAssembly
                .Select(typeInfo =>
                {
                    var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(typeInfo)
                        .ToKindMarkedFullMemberName();

                    var isObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

                    var output = new InstanceTypeDescriptor
                    {
                        ProjectFilePath = projectFilePath,
                        IsObsolete = isObsolete,
                        TypeName = kindMarkedFullMemberName,
                    };

                    return output;
                })
                .Now();

            // Now iterate over all interesting types.
            var instanceDescriptors = new List<InstanceDescriptor>();

            foreach (var pair in typesOfInterestByInstanceVarietyName)
            {
                var instanceVarietyName = pair.Key;
                var types = pair.Value;

                var instanceVarietyDescriptor = instanceVarietyDescriptorsByName[instanceVarietyName];

                var instanceVarietyTargets = instanceVarietyDescriptor.Targets;

                foreach (var type in types)
                {
                    foreach (var instanceVarietyTarget in instanceVarietyTargets)
                    {
                        var instanceNameGeneratorFunction = instanceNameGeneratorFunctionsByInstanceVarietyTarget[instanceVarietyTarget];

                        var names = instanceNameGeneratorFunction(type);

                        foreach (var name in names)
                        {
                            var (identityName, kindMarkedFullMemberName, isObsolete) = name;

                            var descriptionXml = documentationByMemberIdentityName.Value.GetValueOrDefault(identityName);

                            var instanceDescriptor = new InstanceDescriptor
                            {
                                InstanceVarietyName = instanceVarietyName,
                                ProjectFilePath = projectFilePath,
                                IdentityName = identityName,
                                KindMarkedFullMemberName = kindMarkedFullMemberName,
                                DescriptionXml = descriptionXml,
                                IsObsolete = isObsolete,
                            };

                            instanceDescriptors.Add(instanceDescriptor);
                        }
                    }
                }
            }

            //Return the output.
            var output = new AssemblyInstancesDescriptor
            {
                Instances = instanceDescriptors.ToArray(),
                InstanceTypes = instanceTypeKindMarkedFullMemberNames,
            };

            return output;
        }
    }
}
