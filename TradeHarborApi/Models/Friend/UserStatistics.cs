/// <summary>
/// Represents statistics related to a user.
/// </summary>
public class UserStatistics
{
    /// <summary>
    /// Gets or sets the number of friends the user has.
    /// </summary>
    public int FriendCount { get; set; }

    /// <summary>
    /// Gets or sets the number of posts created by the user.
    /// </summary>
    public int PostCount { get; set; }

    /// <summary>
    /// Gets or sets the number of downvotes received by the user.
    /// </summary>
    public int Downvotes { get; set; }

    /// <summary>
    /// Gets or sets the number of upvotes received by the user.
    /// </summary>
    public int Upvotes { get; set; }
}
