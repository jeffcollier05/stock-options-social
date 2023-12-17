/**
 * Represents a request to create a trade post.
 */
export class CreateTradePostRequest {
    /**
     * Ticker symbol for the trade.
     */
    ticker: string = '';

    /**
     * Position for the trade.
     */
    position: string = '';

    /**
     * Option type for the trade.
     */
    option: string = '';

    /**
     * Strike price for the trade.
     */
    strikeprice: string = '';

    /**
     * Additional comments or notes related to the trade.
     */
    comment: string = '';
}
