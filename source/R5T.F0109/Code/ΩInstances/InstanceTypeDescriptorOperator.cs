using System;


namespace R5T.F0109
{
    public class InstanceTypeDescriptorOperator : IInstanceTypeDescriptorOperator
    {
        #region Infrastructure

        public static IInstanceTypeDescriptorOperator Instance { get; } = new InstanceTypeDescriptorOperator();


        private InstanceTypeDescriptorOperator()
        {
        }

        #endregion
    }
}
