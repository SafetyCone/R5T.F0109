using System;
using System.Linq;
using System.Reflection;

using R5T.T0131;


namespace R5T.F0109
{
    [ValuesMarker]
    public partial interface IInstanceNameGenerators : IValuesMarker
    {
        /// <summary>
        /// Just use the method-specific code.
        /// </summary>
        public InstanceDescriptor[] For_MethodsAsProperties(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Methods(typeInfo)
                .Where(Instances.ReflectionOperator.IsFunctionMethod)
                .Select(methodInfo =>
                {
                    var methodIsObsolete = Instances.MethodOperator.IsObsolete(methodInfo);

                    var isObsolete = typeIsObsolete || methodIsObsolete;

                    var signature = Instances.SignatureOperator.Get_Signature(methodInfo);

                    var identityString = Instances.SignatureOperator.Get_IdentityString(signature);
                    var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

                    var output = new InstanceDescriptor
                    {
                        IsObsolete = isObsolete,
                        IdentityString = identityString,
                        SignatureString = signatureString,
                    };
                    return output;
                })
                .ToArray();

            return output;
        }

        public InstanceDescriptor[] For_StaticReadOnlyObjects(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Fields_StaticReadonly_Object(typeInfo)
                .Select(fieldInfo =>
                {
                    var propertyIsObsolete = Instances.FieldOperator.IsObsolete(fieldInfo);

                    var isObsolete = typeIsObsolete || propertyIsObsolete;

                    var signature = Instances.SignatureOperator.Get_Signature(fieldInfo);

                    var identityString = Instances.SignatureOperator.Get_IdentityString(signature);
                    var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

                    var output = new InstanceDescriptor
                    {
                        IsObsolete = isObsolete,
                        IdentityString = identityString,
                        SignatureString = signatureString,
                    };
                    return output;
                })
                .ToArray();

            return output;
        }

        public InstanceDescriptor[] For_PropertiesOfType(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Properties(typeInfo)
                .Where(Instances.ReflectionOperator.IsValueProperty)
                .Select(propertyInfo =>
                {
                    var propertyIsObsolete = Instances.PropertyOperator.IsObsolete(propertyInfo);

                    var isObsolete = typeIsObsolete || propertyIsObsolete;

                    var signature = Instances.SignatureOperator.Get_Signature(propertyInfo);

                    var identityString = Instances.SignatureOperator.Get_IdentityString(signature);
                    var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

                    var output = new InstanceDescriptor
                    {
                        IsObsolete = isObsolete,
                        IdentityString = identityString,
                        SignatureString = signatureString,
                    };
                    return output;
                })
                .ToArray();

            return output;
        }

        public InstanceDescriptor[] For_MethodsOfType(TypeInfo typeInfo)
        {
            var typeIsObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var output = Instances.ReflectionOperator.Get_Methods(typeInfo)
                .Where(Instances.ReflectionOperator.IsFunctionMethod)
                .Select(methodInfo =>
                {
                    var methodIsObsolete = Instances.MethodOperator.IsObsolete(methodInfo);

                    var isObsolete = typeIsObsolete || methodIsObsolete;

                    var signature = Instances.SignatureOperator.Get_Signature(methodInfo);

                    var identityString = Instances.SignatureOperator.Get_IdentityString(signature);
                    var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

                    var output = new InstanceDescriptor
                    {
                        IsObsolete = isObsolete,
                        IdentityString = identityString,
                        SignatureString = signatureString,
                    };
                    return output;
                })
                .ToArray();

            return output;
        }

        public InstanceDescriptor[] For_Type(TypeInfo typeInfo)
        {
            var isObsolete = Instances.TypeOperator.IsObsolete(typeInfo);

            var signature = Instances.SignatureOperator.Get_Signature(typeInfo);

            var identityString = Instances.SignatureOperator.Get_IdentityString(signature);
            var signatureString = Instances.SignatureOperator.Get_SignatureString(signature);

            // Need to return an array for the purposes of standardization across all instance varieties (which for some, like methods, there might be multiple per type).
            var output = new[]
            {
                new InstanceDescriptor
                {
                    IsObsolete = isObsolete,
                    IdentityString = identityString,
                    SignatureString = signatureString
                }
            };

            return output;
        }
    }
}
