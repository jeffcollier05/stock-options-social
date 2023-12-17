import { PostComment } from "./postComment";

/**
 * Represents a trade post with user and comment information.
 */
export class TradePost {
  /**
   * The unique identifier for the user who created the trade post.
   */
  userId: string = '';

  /**
   * The unique identifier for the trade post.
   */
  tradeId: string = '';

  /**
   * The ticker symbol associated with the trade post.
   */
  ticker: string = '';

  /**
   * The position associated with the trade post.
   */
  position: string = '';

  /**
   * The type of option associated with the trade post.
   */
  option: string = '';

  /**
   * The strike price associated with the trade post.
   */
  strikeprice: string = '';

  /**
   * Comments about the trade post from poster.
   */
  comment: string = '';

  /**
   * The timestamp when the trade post was created.
   */
  timestamp: string = '';

  /**
   * The first name of the user who created the trade post.
   */
  firstName: string = '';

  /**
   * The last name of the user who created the trade post.
   */
  lastName: string = '';

  /**
   * The username of the user who created the trade post.
   */
  username: string = '';

  /**
   * The URL of the user's profile picture.
   */
  profilePictureUrl: string = '';

  /**
   * The number of votes received on the trade post.
   */
  votes: number = 0;

  /**
   * The user's reaction towards the trade post.
   */
  userReaction: string = '';

  /**
   * The comments associated with the trade post.
   */
  comments: PostComment[] = [];
}
