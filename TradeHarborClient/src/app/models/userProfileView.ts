import { UserProfile } from "./userProfile";

export class UserProfileView {
    userProfile: UserProfile = new UserProfile();
    deleteRequestWaiting: boolean = false;
    acceptRequestWaiting: boolean = false;
    sendRequestWaiting: boolean = false;
    deleteFriendWaiting: boolean = false;
}