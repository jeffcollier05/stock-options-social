/**
 * Represents a comment on a post with user information.
 */
export class PostComment {
    /**
     * The unique identifier for the comment.
     */
    commentId: string = '';
  
    /**
     * The unique identifier for the post to which the comment belongs.
     */
    postId: string = '';
  
    /**
     * The content of the comment.
     */
    comment: string = '';
  
    /**
     * The timestamp when the comment was created.
     */
    timestamp: string = '';
  
    /**
     * The username of the user who made the comment.
     */
    username: string = '';
  
    /**
     * The URL of the user's profile picture.
     */
    profilePictureUrl: string = '';
  
    /**
     * The first name of the user who made the comment.
     */
    firstName: string = '';
  
    /**
     * The last name of the user who made the comment.
     */
    lastName: string = '';
  
    /**
     * The unique identifier for the user who made the comment.
     */
    userId: string = '';
  }
  