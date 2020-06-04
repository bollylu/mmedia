import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { MainTitleComponent } from './components/MainTitle/MainTitle.component';
import { DisplayGroupsComponent } from './components/display-groups/display-groups.component';
import { DisplayMovieComponent } from './components/display-movie/display-movie.component';
import { DisplayGroupComponent } from './components/display-group/display-group.component';
import { DisplayMoviesComponent } from './components/display-movies/display-movies.component';
import { MoviesFilterComponent } from './components/movies-filter/movies-filter.component';

@NgModule({
  declarations: [
    AppComponent,
    MainTitleComponent,
    DisplayGroupsComponent,
    DisplayMovieComponent,
    DisplayGroupComponent,
    DisplayMoviesComponent,
    MoviesFilterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})

export class AppModule { }
