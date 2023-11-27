import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MenuBarComponent } from './components/menu-bar/menu-bar.component';
import { FeedViewComponent } from './components/feed-view/feed-view.component';

@NgModule({
  declarations: [
    AppComponent,
    MenuBarComponent,
    FeedViewComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
