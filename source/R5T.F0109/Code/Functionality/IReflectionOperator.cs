using System;
using System.Linq;
using System.Reflection;

using R5T.T0132;


namespace R5T.F0109
{
    [FunctionalityMarker]
    public partial interface IReflectionOperator : IFunctionalityMarker,
        F0018.IReflectionOperator
    {
        public bool IsFunctionMethod(MethodInfo methodInfo)
        {
            var output = true
                // Only public methods.
                && methodInfo.IsPublic
                // Must not be a property.
                && !Instances.ReflectionOperations.IsPropertyMethod(methodInfo)
                ;

            return output;
        }

        public bool IsValueProperty(PropertyInfo propertyInfo)
        {
            var output = true
                // Only properties with get methods.
                && propertyInfo.GetMethod is object
                // Only properties with public get methods.
                && propertyInfo.GetMethod.IsPublic
                // Only properties *without* set methods.
                && propertyInfo.SetMethod is null
                // Only properties that are *not* indexers (which is tested by seeing if the property has any index parameters).
                && propertyInfo.GetIndexParameters().None()
                ;

            return output;
        }
    }
}
