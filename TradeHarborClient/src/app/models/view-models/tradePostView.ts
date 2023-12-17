import { TradePost } from "../api-responses/tradePost";

/**
 * Represents a view for displaying and interacting with a trade post.
 */
export class TradePostView {
    /**
     * The trade post data to be displayed.
     */
    tradePost: TradePost = new TradePost();

    /**
     * Indicates whether a delete operation is in progress for the trade post.
     */
    deleteWaiting: boolean = false;

    /**
     * The content of the comment to be written for the trade post.
     */
    writeComment: string = '';

    /**
     * Indicates whether a write comment operation is in progress for the trade post.
     */
    writeCommentWaiting: boolean = false;

    /**
     * Indicates whether the section for writing a comment is visible.
     */
    showWriteComment: boolean = false;

    /**
     * Indicates whether the section for displaying comments is visible.
     */
    showComments: boolean = false;
}
