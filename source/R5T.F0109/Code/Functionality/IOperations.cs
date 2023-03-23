using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using R5T.F0018.Extensions;
using R5T.F0099.T000;
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
        public AssemblyInstancesDescriptor Get_InstanceDescriptors(
            IProjectFilePath projectFilePath,
            Assembly assembly,
            DocumentationByMemberIdentityName documentationByMemberIdentityName,
            IDictionary<InstanceVarietyName, InstanceVarietyDescriptor> instanceVarietyDescriptorsByName)
        {
            // Build type-is-of-interest predicate functions for each instance variety.
            var typeIsOfInterestPredicatesByInstanceVarietyName = new Dictionary<InstanceVarietyName, Func<TypeInfo, bool>>();

            foreach (var instanceVarietyDescriptor in instanceVarietyDescriptorsByName.Values)
            {
                //Func<TypeInfo, bool> predicate = Instances.TypeOperator.GetTypeByHasAttributeOfNamespacedTypeNamePredicate(
                //    instanceVarietyDescriptor.MarkerAttributeTypeName);

                /// Special method required to check the "is-X" value (if the value present) for marker attribute constructors.
                bool HasAttributeOfTypeWithNonFalseConstructorValue(TypeInfo typeInfo)
                {
                    var hasAttributeOfType = Instances.TypeOperator.HasAttributeOfType(
                        typeInfo,
                        instanceVarietyDescriptor.MarkerAttributeTypeName);

                    // All marker attributes have a parameter to indicated that even though they are applied to a type, the type they are applied to is NOT to be included.
                    if(hasAttributeOfType.Result?.ConstructorArguments.Any() ?? false)
                    {
                        var attributeData = typeInfo.GetCustomAttributesData();

                        // The first contructor argument is always used, it might be named or not, and this handles named arguments.
                        var constructorArgument = hasAttributeOfType.Result.ConstructorArguments.FirstOrDefault();

                        // The value is always an "is X", so if false, we don't include the type.
                        var isMarked = Convert.ToBoolean(constructorArgument.Value);
                        if(!isMarked)
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

                Func<TypeInfo, bool> predicate = HasAttributeOfTypeWithNonFalseConstructorValue;

                typeIsOfInterestPredicatesByInstanceVarietyName.Add(
                    instanceVarietyDescriptor.Name,
                    predicate);
            }

            // Build instance generator functions for each variety target.
            var instanceNameGeneratorFunctionsByInstanceVarietyTarget = new Dictionary<InstanceVarietyTarget, Func<TypeInfo, (IdentityName, KindMarkedFullMemberName, bool)[]>>
            {
                {
                    InstanceVarietyTarget.Type,
                    typeInfo =>
                    {
                        var isObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

                        var identityName = Instances.IdentityNameProvider.GetIdentityName(typeInfo);
                        var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(typeInfo);

                        // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                        var output = new[]
                        {
                            (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete)
                        };

                        return output;
                    }
                },
                {
                    InstanceVarietyTarget.MethodsOfType,
                    typeInfo =>
                    {
                        var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

                        var output = Instances.ReflectionOperator.Get_Methods(typeInfo)
                            .Where(Instances.ReflectionOperator.IsFunctionMethod)
                            .Select(methodInfo =>
                            {
                                var methodIsObsolete = Instances.MethodOperator.IsObsolete(methodInfo);

                                var isObsolete = typeIsObsolete || methodIsObsolete;

                                var identityName = Instances.IdentityNameProvider.GetIdentityName(methodInfo);
                                var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(methodInfo);

                                // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                                var output = (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete);
                                return output;
                            })
                            .ToArray();

                        return output;
                    }
                },
                {
                    InstanceVarietyTarget.PropertiesOfType,
                    typeInfo =>
                    {
                        var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

                        var output = Instances.ReflectionOperator.Get_Properties(typeInfo)
                            .Where(Instances.ReflectionOperator.IsValueProperty)
                            .Select(propertyInfo =>
                            {
                                var propertyIsObsolete = Instances.PropertyOperator.IsObsolete(propertyInfo);

                                var isObsolete = typeIsObsolete || propertyIsObsolete;

                                var identityName = Instances.IdentityNameProvider.GetIdentityName(propertyInfo);
                                var kindMarkedFullMemberName = identityName; // TODO Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(methodInfo);

                                // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                                var output = (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete);
                                return output;
                            })
                            .ToArray();

                        return output;
                    }
                }
            };

            // Iterate over all types in the assembly.
            var typesOfInterestByInstanceVarietyName = new Dictionary<InstanceVarietyName, List<TypeInfo>>();

            assembly.Foreach_TypeInAssembly(typeInfo =>
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

                var instanceVarietyTarget = instanceVarietyDescriptor.Target;

                var instanceNameGeneratorFunction = instanceNameGeneratorFunctionsByInstanceVarietyTarget[instanceVarietyTarget];

                foreach (var type in types)
                {
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
