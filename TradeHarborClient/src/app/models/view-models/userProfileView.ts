import { UserProfile } from "../api-responses/userProfile";

/**
 * Represents a view for displaying and interacting with a user profile.
 */
export class UserProfileView {
    /**
     * The user profile data to be displayed.
     */
    userProfile: UserProfile = new UserProfile();

    /**
     * Indicates whether a decline friend request operation is in progress for the user profile.
     */
    declineRequestWaiting: boolean = false;

    /**
     * Indicates whether an accept friend request operation is in progress for the user profile.
     */
    acceptRequestWaiting: boolean = false;

    /**
     * Indicates whether a send friend request operation is in progress for the user profile.
     */
    sendRequestWaiting: boolean = false;

    /**
     * Indicates whether a delete friend operation is in progress for the user profile.
     */
    deleteFriendWaiting: boolean = false;
}
