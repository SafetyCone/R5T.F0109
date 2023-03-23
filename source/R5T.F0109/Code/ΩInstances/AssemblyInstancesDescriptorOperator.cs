using System;


namespace R5T.F0109
{
    public class AssemblyInstancesDescriptorOperator : IAssemblyInstancesDescriptorOperator
    {
        #region Infrastructure

        public static IAssemblyInstancesDescriptorOperator Instance { get; } = new AssemblyInstancesDescriptorOperator();


        private AssemblyInstancesDescriptorOperator()
        {
        }

        #endregion
    }
}
