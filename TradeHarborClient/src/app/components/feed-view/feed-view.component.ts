import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DeleteTradePostRequest } from 'src/app/models/social-requests/deleteTradePostRequest';
import { ErrorViewModel } from 'src/app/models/api-responses/errorViewModel';
import { PostCommentRequest } from 'src/app/models/social-requests/postCommentRequest';
import { PostReactionRequest } from 'src/app/models/social-requests/postReactionRequest';
import { TradePost } from 'src/app/models/api-responses/tradePost';
import { TradePostView } from 'src/app/models/view-models/tradePostView';
import { ApiService } from 'src/app/services/api.service';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-feed-view',
  templateUrl: './feed-view.component.html',
  styleUrls: ['./feed-view.component.scss']
})
export class FeedViewComponent implements OnInit {

  /** Array of trade post views to be displayed in the feed. */
  public tradePosts: TradePostView[] = []

  constructor(
    private apiService: ApiService,
    private authService: AuthenticationService,
    private snackbar: MatSnackBar
  ) { }

  ngOnInit(): void {
    this.getTradePosts();
  }

  /** Fetches trade posts from the API and populates the tradePosts array. */
  private getTradePosts(): void {
    this.apiService.getTradePosts().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        // Sort the response based on the timestamp in descending order
        resp.sort((x, y) => new Date(y.timestamp).getTime() - new Date(x.timestamp).getTime());

        // Map the response to TradePostView array
        var tradePostViews: TradePostView[] = resp.map(post => ({
          tradePost: post,
          deleteWaiting: false,
          writeComment: '',
          writeCommentWaiting: false,
          showWriteComment: false,
          showComments: false
        }));

        // Update the tradePosts array with the mapped tradePostViews
        this.tradePosts = tradePostViews;
      }
    });
  }

  /**
   * Calculates and returns a formatted string indicating the time elapsed since the given timestamp.
   */
  public getLapsedTime(timestamp: string): string {
    // Gets the timestamps
    var currentTimestamp = Date.now();
    var postTimestamp = new Date(timestamp);

    // Calculate the time difference between the current timestamp and the post timestamp
    var timeDifference = currentTimestamp - postTimestamp.getTime();

    // Calculate elapsed time in seconds, minutes, hours, and days
    var seconds = Math.floor(timeDifference / 1000);
    var minutes = Math.floor(seconds / 60);
    var hours = Math.floor(minutes / 60);
    var days = Math.floor(hours / 24);

    // Return elapsed time
    if (seconds < 60) {
      return `${seconds} second${seconds > 1 ? 's' : ''} ago`;
    } else if (minutes < 60) {
      return `${minutes} minute${minutes > 1 ? 's' : ''} ago`;
    } else if (hours < 24) {
      return `${hours} hour${hours > 1 ? 's' : ''} ago`;
    } else {
      return `${days} day${days > 1 ? 's' : ''} ago`;
    }
  }

  /**
   * Converts a timestamp to a formatted date and time string of local time.
   */
  public getDateAndTime(timestamp: string): string {
    var dateTimestamp = new Date(timestamp);

    const dateOptions = {
      month: 'numeric',
      day: 'numeric',
      year: 'numeric',
      timeZone: 'UTC'
    };

    const timeOptions = {
      hour: 'numeric',
      minute: 'numeric',
      hour12: true,
      timeZone: 'UTC'
    };

    // Format the date and time using the specified options
    var date = dateTimestamp.toLocaleString('en-US', dateOptions as Intl.DateTimeFormatOptions);
    var time = dateTimestamp.toLocaleString('en-US', timeOptions as Intl.DateTimeFormatOptions);
    return `${time} \u2022 ${date}`;
  }

  /**
   * Deletes a trade post using the provided TradePostView.
   */
  public deleteTradePost(tradePostView: TradePostView): void {
    var request: DeleteTradePostRequest = {
      tradeId: tradePostView.tradePost.tradeId
    };

    // Set deleteWaiting flag to true to indicate deletion in progress
    tradePostView.deleteWaiting = true;

    // Call the API service to delete the trade post
    this.apiService.deleteTradePost(request).subscribe(resp => {
      tradePostView.deleteWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        // Remove the deleted trade post from the local list
        this.tradePosts = this.tradePosts.filter(x => x.tradePost.tradeId != tradePostView.tradePost.tradeId);
        this.snackbar.open('Post was removed.', 'Close');
      }
    });
  }

  /**
   * Checks if the provided trade post belongs to the currently authenticated user.
   */
  public isUserPost(post: TradePost): boolean {
    var userId = this.authService.getUserIdFromJwt();
    return post.userId == userId;
  }

  /**
   * Reacts to a trade post by sending the reaction to the server.
   */
  public reactToPost(post: TradePost, newReaction: string): void {
    // Determine the action to be taken based on the current and new reactions
    var reactionAction = this.getVoteAction(post.userReaction, newReaction);
    
    var request: PostReactionRequest = {
      postId: post.tradeId,
      reactionType: reactionAction
    };
    
    // Call the API service to send the reaction to the server
    this.apiService.reactToPost(request).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        // Update the post's vote count and set the new user reaction
        this.updatePostVoteCount(post.userReaction, reactionAction, post);
        post.userReaction = reactionAction;
      }
    });
  }

  /**
   * Determines the action to be taken based on the current and new reaction.
   */
  public getVoteAction(oldReaction: string, newReaction: string): string {
    var reactionAction = '';
    
    // Check if the old and new reaction clicked are the same, if so we need to remove the reaction as the buttons are toggles
    if ((oldReaction == 'UPVOTE' && newReaction == 'UPVOTE')
      || (oldReaction == 'DOWNVOTE' && newReaction == 'DOWNVOTE')
      ) {
      reactionAction = 'NO-VOTE';
    } else {
      reactionAction = newReaction;
    }

    return reactionAction;
  }

  /**
   * Updates the vote count of a trade post locally based on the old and new reactions.
   */
  public updatePostVoteCount(oldReaction: string, newReaction: string, post: TradePost): void {
    if (newReaction == 'UPVOTE' &&  oldReaction == 'DOWNVOTE') {
      post.votes += 2;
    } else if (newReaction == 'DOWNVOTE' && oldReaction == 'UPVOTE') {
      post.votes -= 2;
    } else if (newReaction == 'NO-VOTE' &&  oldReaction == 'DOWNVOTE') {
      post.votes++;
    } else if (newReaction == 'NO-VOTE' &&  oldReaction == 'UPVOTE') {
      post.votes--;
    } else if (newReaction == 'UPVOTE') {
      post.votes++;
    } else if (newReaction == 'DOWNVOTE') {
      post.votes--;
    }
  }

  /**
   * Posts a comment on a trade post using the provided TradePostView.
   */
  public commentOnPost(view: TradePostView): void {
    var request: PostCommentRequest = {
      postId: view.tradePost.tradeId,
      comment: view.writeComment,
      postOwnerUserId: view.tradePost.userId
    };

    // Set writeCommentWaiting flag to true to indicate comment submission in progress
    view.writeCommentWaiting = true;

    // Call the API service to post a comment on the trade post
    this.apiService.commentOnPost(request).subscribe(resp => {
      view.writeCommentWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        //todo
      }
    });
  }

  /**
   * Toggles the visibility of the comment input for a trade post.
   */
  public toggleWriteComment(view: TradePostView): void {
    view.showWriteComment = !view.showWriteComment;
  }

  /**
   * Toggles the visibility of the comments for a trade post.
   */
  public toggleShowComments(view: TradePostView): void {
    view.showComments = !view.showComments;
  }
}
