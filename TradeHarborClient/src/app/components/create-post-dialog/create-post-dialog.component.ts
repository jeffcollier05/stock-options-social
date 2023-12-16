import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CreateTradePostRequest } from 'src/app/models/createTradePostRequest';
import { ErrorViewModel } from 'src/app/models/errorViewModel';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-create-post-dialog',
  templateUrl: './create-post-dialog.component.html',
  styleUrls: ['./create-post-dialog.component.scss']
})
export class CreatePostDialogComponent {
  /** The request object for creating a trade post. */
  public createTradePostRequest = new CreateTradePostRequest();

  /** Indicates whether the component is waiting for the create post operation. */
  public createPostWaiting: boolean = false;

  constructor(
    private apiService: ApiService,
    private dialogRef: MatDialogRef<CreatePostDialogComponent>,
    private snackbar: MatSnackBar
  ) { }

  /**
   * Creates a new trade post.
   */
  public createTradePost(): void {
    this.createPostWaiting = true;
    this.apiService.createTradePost(this.createTradePostRequest).subscribe(resp => {
      this.createPostWaiting = false;
      if (!(resp instanceof ErrorViewModel)) {
        this.dialogRef.close();
        this.snackbar.open('Trade post was created!', 'Close');
      }
    });
  }
}
