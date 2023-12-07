import { Component } from '@angular/core';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { TradePost } from 'src/app/models/tradePost';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-feed-view',
  templateUrl: './feed-view.component.html',
  styleUrls: ['./feed-view.component.scss']
})
export class FeedViewComponent {

  public tradePosts: TradePost[] = []

  constructor(private apiService: ApiService) {
    this.getTradePosts();
  }

  private getTradePosts(): void {
    this.apiService.getTradePosts().subscribe(resp => {
      if (!(resp instanceof ErrorViewModel)) {
        this.tradePosts = resp;
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
    };

    const timeOptions = {
      hour: 'numeric',
      minute: 'numeric',
      hour12: true,
    };

    var date = dateTimestamp.toLocaleString('en-US', dateOptions as Intl.DateTimeFormatOptions);
    var time = dateTimestamp.toLocaleString('en-US', timeOptions as Intl.DateTimeFormatOptions);
    return `${time} \u2022 ${date}`;
  }
  
}
