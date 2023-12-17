/**
 * Represents a user profile with basic information and friendship status.
 */
export class UserProfile {
    /**
     * The unique identifier for the user.
     */
    userId: string = '';
  
    /**
     * The first name of the user.
     */
    firstName: string = '';
  
    /**
     * The last name of the user.
     */
    lastName: string = '';
  
    /**
     * The username of the user.
     */
    username: string = '';
  
    /**
     * The URL of the user's profile picture.
     */
    profilePictureUrl: string = '';
  
    /**
     * Indicates whether the user has sent a friend request to you.
     */
    sentFriendRequestToYou: boolean = false;
  
    /**
     * Indicates whether you have received a friend request from the user.
     */
    receivedFriendRequestFromYou: boolean = false;
  
    /**
     * Indicates whether the user is a friend.
     */
    isFriend: boolean = false;
  }
  