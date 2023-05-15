using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using R5T.F0000.Extensions;
using R5T.T0131;
using R5T.T0161;
using R5T.T0161.Extensions;
using R5T.T0162;
using R5T.T0162.Extensions;


namespace R5T.F0109
{
    [ValuesMarker]
    public partial interface IInstanceNameGenerators : IValuesMarker
    {
        /// <summary>
        /// Just use the method-specific code.
        /// </summary>
        public (IdentityName, IKindMarkedFullMemberName, bool)[] For_MethodsAsProperties(TypeInfo typeInfo)
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

        public (IdentityName, IKindMarkedFullMemberName, bool)[] For_StaticReadOnlyObjects(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Fields_StaticReadonly_Object(typeInfo)
                .Select(fieldInfo =>
                {
                    var propertyIsObsolete = Instances.FieldOperator.IsObsolete(fieldInfo);

                    var isObsolete = typeIsObsolete || propertyIsObsolete;

                    var identityName = Instances.IdentityNameProvider.GetIdentityName(fieldInfo);
                    // Because we have only selected objects, all instances will be of type System.Object.
                    // Thus, we do not need any field type information.
                    var kindMarkedFullMemberName = identityName;

                    // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                    var output = (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete);
                    return output;
                })
                .ToArray();

            return output;
        }

        public (IdentityName, IKindMarkedFullMemberName, bool)[] For_PropertiesOfType(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Properties(typeInfo)
                .Where(Instances.ReflectionOperator.IsValueProperty)
                .Select(propertyInfo =>
                {
                    var propertyIsObsolete = Instances.PropertyOperator.IsObsolete(propertyInfo);

                    var isObsolete = typeIsObsolete || propertyIsObsolete;

                    var identityName = Instances.IdentityNameProvider.GetIdentityName(propertyInfo);
                    var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(propertyInfo);

                    // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                    var output = (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete);
                    return output;
                })
                .ToArray();

            return output;
        }

        public (IdentityName, IKindMarkedFullMemberName, bool)[] For_MethodsOfType(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Methods(typeInfo)
                .Where(Instances.ReflectionOperator.IsFunctionMethod)
                .Select(methodInfo =>
                {
                    var methodIsObsolete = Instances.MethodOperator.IsObsolete(methodInfo);

                    var isObsolete = typeIsObsolete || methodIsObsolete;

                    var identityName = Instances.IdentityNameProvider.GetIdentityName(methodInfo);
                    if (identityName == "M:R5T.E0068.ISyntaxAnnotationOperator.AnnotateTokens``1(``0,System.Collections.Generic.IEnumerable{Microsoft.CodeAnalysis.SyntaxToken},System.Collections.Generic.Dictionary`2[[Microsoft.CodeAnalysis.SyntaxToken, Microsoft.CodeAnalysis, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35],[Microsoft.CodeAnalysis.SyntaxAnnotation, Microsoft.CodeAnalysis, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]&)")
                    {
                        Console.WriteLine("For debugging.");
                    }
                    var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(methodInfo);

                    // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
                    var output = (identityName.ToIdentityName(), kindMarkedFullMemberName.ToKindMarkedFullMemberName(), isObsolete);
                    return output;
                })
                .ToArray();

            return output;
        }

        public (IdentityName, IKindMarkedFullMemberName, bool)[] For_Type(TypeInfo typeInfo)
        {
            var isObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var identityName = Instances.IdentityNameProvider.GetIdentityName(typeInfo);
            var kindMarkedFullMemberName = Instances.ParameterNamedIdentityNameProvider.GetParameterNamedIdentityName(typeInfo);

            // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
            var output = new[]
            {
                (
                    identityName.ToIdentityName(),
                    kindMarkedFullMemberName.ToKindMarkedFullMemberName(),
                    isObsolete
                )
            };

            return output;
        }
    }
}
