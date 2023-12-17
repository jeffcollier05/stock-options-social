/**
 * Represents a notification with basic information.
 */
export class Notification {
    /**
     * The main message of the notification.
     */
    message: string = '';
  
    /**
     * The unique identifier for the notification.
     */
    notificationId: string = '';
  
    /**
     * The timestamp when the notification was created.
     */
    createdTimestamp: Date = new Date();
  }
  