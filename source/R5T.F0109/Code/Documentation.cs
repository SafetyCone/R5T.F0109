using System;


namespace R5T.F0109
{
	/// <summary>
	/// Assembly functionality survey functionality.
	/// </summary>
	public static class Documentation
	{
        /// <summary>
        /// Compare the instances found in today's run with the full set of instances accumulated over time.
        /// Returns the set of new instances (in today's run but not in the prior set), missing instances (not in today's run, but in the prior set), and most importantly, removed instances: missing instances whose containing project was not in the build problems or processing problems project lists.
        /// The distinction between missing and removed instances is important since instances might be missing from today's run only because their containing projects failed to build or process.
        /// These instances should not be removed since we want to know of their existence.
        /// Thus we only remove instances that no longer exist in projects that have successfully built and processed (i.e. were actually removed).
        /// </summary>
        public static readonly object For_CompareRunInstances;
    }
}