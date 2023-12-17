import { TradePost } from "../api-responses/tradePost";

export class TradePostView {
    tradePost: TradePost = new TradePost();
    deleteWaiting: boolean = false;
    writeComment: string = '';
    writeCommentWaiting: boolean = false;
    showWriteComment: boolean = false;
    showComments: boolean = false;
}