using System;


namespace R5T.F0109
{
    public class InstanceNameGenerator : IInstanceNameGenerator
    {
        #region Infrastructure

        public static IInstanceNameGenerator Instance { get; } = new InstanceNameGenerator();


        private InstanceNameGenerator()
        {
        }

        #endregion
    }
}
