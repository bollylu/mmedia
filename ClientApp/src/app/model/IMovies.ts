import { IMovie } from './IMovie';
import { Observable } from 'rxjs';

export interface IMovies {
  movies: IMovie[];
  page: number;
  availablePages: number;
  source: string;
}
