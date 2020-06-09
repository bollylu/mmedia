import { Component, OnInit, Input } from '@angular/core';
import { IMovie } from '../../model/IMovie';
import { MoviesService } from '../../services/movies.service';

@Component({
  selector: 'app-display-movie',
  templateUrl: './display-movie.component.html',
  styleUrls: ['./display-movie.component.scss']
})
export class DisplayMovieComponent implements OnInit {

  @Input() 
  Movie: IMovie;

  constructor(private moviesService: MoviesService) { }

  ngOnInit(): void {
  }

  getPicture() {
    return this.moviesService.getPictureLocation(this.Movie.picture);
  }

  Cleanup(text: string) {
    while (text.endsWith("/")) {
      text = text.substr(0, text.length - 1);
    }
    return text;
  }
}
