using System;


namespace R5T.F0109
{
    public static class Instances
    {
        public static IAssemblyInstancesDescriptorOperator AssemblyInstancesDescriptorOperator => F0109.AssemblyInstancesDescriptorOperator.Instance;
        public static F0000.IDictionaryOperator DictionaryOperator => F0000.DictionaryOperator.Instance;
        public static F0018.IFieldOperator FieldOperator => F0018.FieldOperator.Instance;
        public static F0017.F002.IIdentityNameProvider IdentityNameProvider => F0017.F002.IdentityNameProvider.Instance;
        public static IInstanceTypeDescriptorOperator InstanceTypeDescriptorOperator => F0109.InstanceTypeDescriptorOperator.Instance;
        public static F0032.IJsonOperator JsonOperator => F0032.JsonOperator.Instance;
        public static F0018.IMethodOperator MethodOperator => F0018.MethodOperator.Instance;
        public static F0017.F002.IParameterNamedIdentityNameProvider ParameterNamedIdentityNameProvider => F0017.F002.ParameterNamedIdentityNameProvider.Instance;
        public static F0018.IPropertyOperator PropertyOperator => F0018.PropertyOperator.Instance;
        public static IReflectionOperator ReflectionOperator => F0109.ReflectionOperator.Instance;
        public static F0018.IReflectionOperations ReflectionOperations => F0018.ReflectionOperations.Instance;
        public static F0018.ITypeOperator TypeOperator => F0018.TypeOperator.Instance;
    }
}