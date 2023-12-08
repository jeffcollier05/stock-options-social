import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuBarComponent } from './components/menu-bar/menu-bar.component';
import { FeedViewComponent } from './components/feed-view/feed-view.component';
import { MatIconModule } from '@angular/material/icon';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { AuthenticationInterceptor } from './services/interceptor';
import { RegisterPageComponent } from './pages/register-page/register-page.component';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ActionBarComponent } from './components/action-bar/action-bar.component';
import { CreatePostDialogComponent } from './components/create-post-dialog/create-post-dialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS, MatSnackBarConfig, MatSnackBarModule } from '@angular/material/snack-bar';

const matSnackbarDefaultConfig: MatSnackBarConfig = {
  verticalPosition: 'bottom',
  horizontalPosition: 'center',
  duration: 3000,
};

@NgModule({
  declarations: [
    AppComponent,
    MenuBarComponent,
    FeedViewComponent,
    HomePageComponent,
    LoginPageComponent,
    RegisterPageComponent,
    ActionBarComponent,
    CreatePostDialogComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    MatIconModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    MatButtonModule,
    MatProgressSpinnerModule,
    MatDialogModule,
    CommonModule,
    MatSelectModule,
    MatFormFieldModule,
    MatInputModule,
    MatSnackBarModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthenticationInterceptor,
      multi: true
    },
    {
      provide: MAT_SNACK_BAR_DEFAULT_OPTIONS,
      useValue: matSnackbarDefaultConfig,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
