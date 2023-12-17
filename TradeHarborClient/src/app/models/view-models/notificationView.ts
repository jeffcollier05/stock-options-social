import { Notification } from 'src/app/models/api-responses/notification';

export class NotificationView {
    notification: Notification = new Notification();
    deleteWaiting: boolean = false;
}