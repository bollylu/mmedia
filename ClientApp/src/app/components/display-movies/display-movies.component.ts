import { Component, OnInit, Input, SimpleChanges, OnChanges } from '@angular/core';
import { IMovie } from '../../model/IMovie';
import { MoviesService } from '../../services/movies.service';
import { Observable } from 'rxjs';
import { IMovies } from '../../model/IMovies';


@Component({
  selector: 'app-display-movies',
  templateUrl: './display-movies.component.html',
  styleUrls: ['./display-movies.component.scss']
})

export class DisplayMoviesComponent implements OnInit, OnChanges {

  movies: IMovie[];
  page: number;
  availablePages: number;

  @Input() GroupNameFilter: string;
  @Input() Level: number;

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
    this.goFirstPage();
  }

  ngOnChanges(changes: SimpleChanges) {
    console.log('changes detected: ' + this.GroupNameFilter);
    this.moviesService.getForGroup(this.GroupNameFilter, this.Level, this.page)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }

  //#region --- Movements -----------------------------------------------------
  notFirstPage() {
    return this.page > 1;
  }

  notLastPage() {
    return this.page < this.availablePages;
  }

  goFirstPage() {
    this.moviesService.getMovies(1)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }

  goLastPage() {
    this.moviesService.getMovies(this.availablePages)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }

  goPreviousPage() {
    this.moviesService.getMovies(this.page - 1)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }

  goNextPage() {
    this.moviesService.getMovies(this.page + 1)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }
  //#endregion --- Movements --------------------------------------------------

  //#region --- CSS helpers----------------------------------------------------
  isVisible(test: boolean) {
    if (test) {
      return 'button-visble';
    } else {
      return 'button-hidden';
    }
  }
  //#endregion --- CSS helpers-------------------------------------------------

}
