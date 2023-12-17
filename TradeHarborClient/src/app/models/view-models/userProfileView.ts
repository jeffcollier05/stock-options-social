import { UserProfile } from "../api-responses/userProfile";

export class UserProfileView {
    userProfile: UserProfile = new UserProfile();
    declineRequestWaiting: boolean = false;
    acceptRequestWaiting: boolean = false;
    sendRequestWaiting: boolean = false;
    deleteFriendWaiting: boolean = false;
}