import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { DeleteTradePostRequest } from 'src/app/models/deleteTradePostRequest';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { PostCommentRequest } from 'src/app/models/postCommentRequest';
import { PostReactionRequest } from 'src/app/models/postReactionRequest';
import { TradePost } from 'src/app/models/tradePost';
import { TradePostView } from 'src/app/models/tradePostView';
import { ApiService } from 'src/app/services/api.service';
import { AuthenticationService } from 'src/app/services/authentication.services';

@Component({
  selector: 'app-feed-view',
  templateUrl: './feed-view.component.html',
  styleUrls: ['./feed-view.component.scss']
})
export class FeedViewComponent {

  public tradePosts: TradePostView[] = []

  constructor(
    private apiService: ApiService,
    private authService: AuthenticationService,
    private snackbar: MatSnackBar
    ) {
    this.getTradePosts();
  }

  private getTradePosts(): void {
    this.apiService.getTradePosts().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        resp.sort((x, y) => new Date(y.timestamp).getTime() - new Date( x.timestamp).getTime());
        var tradePostViews: TradePostView[] = resp.map(post => (
          { 
            tradePost: post,
            deleteWaiting: false,
            writeComment: '',
            writeCommentWaiting: false
          }
        ));
        this.tradePosts = tradePostViews;
      }
    });
  }

  public getLapsedTime(timestamp: string): string {
    var currentTimestamp = Date.now();
    var postTimestamp = new Date(timestamp);
    var timeDifference = currentTimestamp - postTimestamp.getTime();

    var seconds = Math.floor(timeDifference / 1000);
    var minutes = Math.floor(seconds / 60);
    var hours = Math.floor(minutes / 60);
    var days = Math.floor(hours / 24);

    if (seconds < 60) {
      return `${seconds}s`;
    } else if (minutes < 60) {
      return `${minutes}m`;
    } else if (hours < 24) {
      return `${hours}h`;
    } else {
      return `${days}d`;
    }
  }

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

    var date = dateTimestamp.toLocaleString('en-US', dateOptions as Intl.DateTimeFormatOptions);
    var time = dateTimestamp.toLocaleString('en-US', timeOptions as Intl.DateTimeFormatOptions);
    return `${time} \u2022 ${date}`;
  }

  public deleteTradePost(tradePostView: TradePostView): void {
    var request: DeleteTradePostRequest = {
      tradeId: tradePostView.tradePost.tradeId
    };

    tradePostView.deleteWaiting = true;
    this.apiService.deleteTradePost(request).subscribe(resp => {
      tradePostView.deleteWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        this.tradePosts = this.tradePosts.filter(x => x.tradePost.tradeId != tradePostView.tradePost.tradeId);
        this.snackbar.open('Post was removed.', 'Close');
      }
    });
  }

  public isUserPost(post: TradePost): boolean {
    var userId = this.authService.getUserIdFromJwt();
    return post.userId == userId;
  }

  public reactToPost(post: TradePost, newReaction: string): void {
    var reactionAction = this.getVoteAction(post.userReaction, newReaction);
    
    var request: PostReactionRequest = {
      postId: post.tradeId,
      reactionType: reactionAction
    };
    
    this.apiService.reactToPost(request).subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.updatePostVoteCount(post.userReaction, reactionAction, post);
        post.userReaction = reactionAction;
      }
    });
  }

  public getVoteAction(oldReaction: string, newReaction: string): string {
    var reactionAction = '';
    
    if ((oldReaction == 'UPVOTE' && newReaction == 'UPVOTE')
      || (oldReaction == 'DOWNVOTE' && newReaction == 'DOWNVOTE')
      ) {
      reactionAction = 'NO-VOTE';
    } else {
      reactionAction = newReaction;
    }

    return reactionAction;
  }

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

  public commentOnPost(view: TradePostView): void {
    var request: PostCommentRequest = {
      postId: view.tradePost.tradeId,
      comment: view.writeComment,
      postOwnerUserId: view.tradePost.userId
    };

    view.writeCommentWaiting = true;
    this.apiService.commentOnPost(request).subscribe(resp => {
      view.writeCommentWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        //todo
      }
    });
  }
}
