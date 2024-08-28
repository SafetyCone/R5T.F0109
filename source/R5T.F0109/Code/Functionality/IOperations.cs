using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using R5T.F0099.T000;
using R5T.L0053.Extensions;
using R5T.T0132;
using R5T.T0162.Extensions;
using R5T.T0169;
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
                // The first constructor argument is always used, it might be named or not, and this handles named arguments.
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

        public Dictionary<T0171.IInstanceVarietyName, Func<TypeInfo, bool>> Get_TypeIsOfInterestPredicates(
            IDictionary<T0171.IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName)
        {
            var typeIsOfInterestPredicatesByInstanceVarietyName = new Dictionary<T0171.IInstanceVarietyName, Func<TypeInfo, bool>>();

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
        public Dictionary<InstanceVarietyTarget, Func<TypeInfo, InstanceDescriptor[]>>
            Get_InstanceDescriptorGeneratorFunctions()
        {
            // Build instance generator functions for each variety target.
            var instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget = new Dictionary<InstanceVarietyTarget, Func<TypeInfo, InstanceDescriptor[]>>
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

            return instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget;
        }

        public AssemblyInstancesDescriptor Get_InstanceDescriptors(
            IProjectFilePath projectFilePath,
            Assembly assembly,
            DocumentationByMemberIdentityName documentationByMemberIdentityName,
            IDictionary<T0171.IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName)
        {
            var typeIsOfInterestPredicatesByInstanceVarietyName = this.Get_TypeIsOfInterestPredicates(instanceVarietyDescriptorsByName);
            var instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget = this.Get_InstanceDescriptorGeneratorFunctions();

            return this.Get_InstanceDescriptors(
                projectFilePath,
                assembly,
                documentationByMemberIdentityName,
                instanceVarietyDescriptorsByName,
                typeIsOfInterestPredicatesByInstanceVarietyName,
                instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget);
        }

        public AssemblyInstancesDescriptor Get_InstanceDescriptors(
            IProjectFilePath projectFilePath,
            Assembly assembly,
            DocumentationByMemberIdentityName documentationByMemberIdentityName,
            IDictionary<T0171.IInstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName,
            IDictionary<T0171.IInstanceVarietyName, Func<TypeInfo, bool>> typeIsOfInterestPredicatesByInstanceVarietyName,
            IDictionary<InstanceVarietyTarget, Func<TypeInfo, InstanceDescriptor[]>> instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget)
        {
            // Iterate over all types in the assembly.
            var typesOfInterestByInstanceVarietyName = new Dictionary<T0171.IInstanceVarietyName, List<TypeInfo>>();

            assembly.Foreach_Type(typeInfo =>
            {
                foreach (var pair in typeIsOfInterestPredicatesByInstanceVarietyName)
                {
                    var typeIsOfInterestToVariety = pair.Value(typeInfo);

                    if(typeIsOfInterestToVariety)
                    {
                        // Add the type to the instances types list.
                        Instances.DictionaryOperator.Add_Value(
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
                    var typeIdentityString = Instances.IdentityStringOperator.Get_IdentityString(typeInfo);

                    var isObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

                    var output = new InstanceTypeDescriptor
                    {
                        ProjectFilePath = projectFilePath,
                        IsObsolete = isObsolete,
                        TypeIdentityString = typeIdentityString,
                    };

                    return output;
                })
                .Now();

            // Now iterate over all interesting types.
            var fullInstanceDescriptors = new List<T0170.InstanceDescriptor>();

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
                        var instanceDescriptorGeneratorFunction = instanceDescriptorGeneratorFunctionsByInstanceVarietyTarget[instanceVarietyTarget];

                        var instanceDescriptors = instanceDescriptorGeneratorFunction(type);

                        foreach (var instanceDescriptor in instanceDescriptors)
                        {
                            var identityName = instanceDescriptor.IdentityString.Value.ToIdentityName();

                            var descriptionXml = documentationByMemberIdentityName.Value.GetValueOrDefault(identityName);

                            var fullInstanceDescriptor = new T0170.InstanceDescriptor
                            {
                                InstanceVarietyName = instanceVarietyName,
                                ProjectFilePath = projectFilePath,
                                IdentityString = instanceDescriptor.IdentityString,
                                SignatureString = instanceDescriptor.SignatureString,
                                DescriptionXml = descriptionXml,
                                IsObsolete = instanceDescriptor.IsObsolete,
                            };

                            fullInstanceDescriptors.Add(fullInstanceDescriptor);
                        }
                    }
                }
            }

            //Return the output.
            var output = new AssemblyInstancesDescriptor
            {
                Instances = fullInstanceDescriptors.ToArray(),
                InstanceTypes = instanceTypeKindMarkedFullMemberNames,
            };

            return output;
        }
    }
}
