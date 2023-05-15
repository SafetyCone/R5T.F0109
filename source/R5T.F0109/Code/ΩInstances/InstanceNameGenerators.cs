using System;


namespace R5T.F0109
{
    public class InstanceNameGenerators : IInstanceNameGenerators
    {
        #region Infrastructure

        public static IInstanceNameGenerators Instance { get; } = new InstanceNameGenerators();


        private InstanceNameGenerators()
        {
        }

        #endregion
    }
}
