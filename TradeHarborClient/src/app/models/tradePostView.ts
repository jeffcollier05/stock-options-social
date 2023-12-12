import { TradePost } from "./tradePost";

export class TradePostView {
    tradePost: TradePost = new TradePost();
    deleteWaiting: boolean = false;
    writeComment: string = '';
    writeCommentWaiting: boolean = false;
}