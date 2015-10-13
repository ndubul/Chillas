namespace Dev2.Common.Interfaces.Deploy
{
    public interface IDeployStatsViewer 
    {
        /// <summary>
        /// Services being deployed
        /// </summary>
        int Connectors { get; set; }
        /// <summary>
        /// Services Being Deployed
        /// </summary>
        int Services { get; set; }
        /// <summary>
        /// Sources being Deployed
        /// </summary>
        int Sources { get; set; }
        /// <summary>
        /// What is unknown is unknown to me
        /// </summary>
        int Unknown { get; set; }
        /// <summary>
        /// The count of new resources being deployed
        /// </summary>
        int NewResources { get; set; }
        /// <summary>
        /// The count of overidded resources
        /// </summary>
        int Overrides { get; set; }
        /// <summary>
        /// The status of the last deploy
        /// </summary>
        string Status { get; set; }
    }
}