import { Component, OnInit, Input, SimpleChanges } from '@angular/core';
import { IMovie } from '../../model/IMovie';
import { MoviesService } from '../../services/movies.service';

@Component({
  selector: 'app-display-movies',
  templateUrl: './display-movies.component.html',
  styleUrls: ['./display-movies.component.scss']
})

export class DisplayMoviesComponent implements OnInit {

  movies: IMovie[];
  page: number;
  availablePages: number;

  @Input() Group: string;
  @Input() Level: number;
  @Input() IsDirty: boolean;

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
    this.goFirstPage();
  }

  busy = false;

  ngOnChanges(changes: SimpleChanges) {
    for (const propName in changes) {

      switch (propName) {
        case "Group":
          console.log("### change detected : " + this.Group);
          if (this.busy) {
            return;
          }
          this.busy = true;
          this.moviesService.getMovies(this.Group, 1)
            .subscribe(m => {
              console.log("got movies");
              this.movies = m.movies;
              this.page = 1;
              this.availablePages = m.availablePages;
              this.busy = false;
            });
      }
    }

  }

  //#region --- Movements -----------------------------------------------------
  notFirstPage() {
    return this.page > 1;
  }

  notLastPage() {
    return this.page < this.availablePages;
  }

  goFirstPage() {
    this.moviesService.getMovies(this.Group, 1)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = 1;
        this.availablePages = m.availablePages;
      });
  }

  goLastPage() {
    this.moviesService.getMovies(this.Group, this.availablePages)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.availablePages;
        this.availablePages = m.availablePages;
      });
  }

  goPreviousPage() {
    this.moviesService.getMovies(this.Group, this.page - 1)
      .subscribe(m => {
        this.movies = m.movies;
        this.page = m.page;
        this.availablePages = m.availablePages;
      });
  }

  goNextPage() {
    this.moviesService.getMovies(this.Group, this.page + 1)
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
