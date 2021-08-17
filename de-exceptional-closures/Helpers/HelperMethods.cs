namespace de_exceptional_closures.Helpers
{
    /// <summary>
    /// Helper methods.
    /// </summary>
    public static class HelperMethods
    {
        /// <summary>
        /// Returns a string or null
        /// </summary>
        /// <param name="data">data.</param>
        /// <returns>string.</returns>
        public static string ApprovedOrNot(bool? data)
        {
            if (data == true)
            {
                return "Approved";
            }
            else if (data == false)
            {
                return "Not approved";
            }
            else
            {
                return "Decision required";
            }
        }
    }
}