namespace ChatApplication.Server.Domain.Interfaces
{
    /// <summary>
    /// Interface for accessing user-related context information in the current HTTP context.
    /// </summary>
    public interface IHttpUserContextService
    {
        /// <summary>
        /// Retrieves the user ID from the token in the current HTTP context.
        /// </summary>
        /// <returns>The user ID as a string, or null if not found.</returns>
        string? GetUserId();
    }

}
