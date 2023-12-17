/**
 * Represents a request to post a comment on a trade post.
 */
export class PostCommentRequest {
    /**
     * The unique identifier of the post where the comment will be posted.
     */
    postId: string = '';

    /**
     * The content of the comment to be posted.
     */
    comment: string = '';

    /**
     * The user ID of the owner of the post where the comment will be posted.
     */
    postOwnerUserId: string = '';
}
