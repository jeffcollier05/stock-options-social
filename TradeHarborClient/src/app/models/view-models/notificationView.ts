import { Notification } from 'src/app/models/api-responses/notification';

/**
 * Represents a view for displaying a notification.
 */
export class NotificationView {
    /**
     * The notification data to be displayed.
     */
    notification: Notification = new Notification();

    /**
     * Indicates whether a delete operation is in progress for the notification.
     */
    deleteWaiting: boolean = false;
}
