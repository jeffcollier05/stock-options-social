/**
 * Represents a request to post a reaction on a trade post.
 */
export class PostReactionRequest {
    /**
     * The unique identifier of the post where the reaction will be posted.
     */
    postId: string = '';

    /**
     * The type of reaction to be posted on the trade post.
     */
    reactionType: string = '';
}
