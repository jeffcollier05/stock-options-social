import { Component } from '@angular/core';
import { CreateTradePostRequest } from 'src/app/models/createTradePostRequest';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-create-post-dialog',
  templateUrl: './create-post-dialog.component.html',
  styleUrls: ['./create-post-dialog.component.scss']
})
export class CreatePostDialogComponent {
  public createTradePostRequest = new CreateTradePostRequest();

  constructor(private apiService: ApiService) { }

  public createTradePost(): void {
    this.apiService.createTradePost(this.createTradePostRequest).subscribe();
  }

}
