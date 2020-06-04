import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { IGroup } from '../model/IGroup';
import { Observable } from 'rxjs';
import { IMovie } from '../model/IMovie';
import { IMovies } from '../model/IMovies';

@Injectable({
  providedIn: 'root'
})

export class MoviesService {

  constructor(private http: HttpClient) { }

  getGroups(group: string, level: number = 1): Observable<IGroup[]> {
    const params: HttpParams = new HttpParams()
      .set("group", encodeURIComponent(group))
      .set("level", encodeURIComponent(level));

    return this.http.get<IGroup[]>('/api/groups', { params });
  }

  getMovies(page: number = 1): Observable<IMovies> {

    const params: HttpParams = new HttpParams()
      .set("page", page.toString())
      .set("items", "20");

    return this.http.get<IMovies>('/api/movies', { params });
  }

  getForGroup(group: string = "", level: number = 1, page: number = 1): Observable<IMovies> {
    console.log('get for group ' + group);

    const params: HttpParams = new HttpParams()
      .set("group", encodeURIComponent(group))
      .set("level", level.toString())
      .set("page", page.toString())
      .set("items", "20");

    return this.http.get<IMovies>("/api/movies/forGroup", { params });
  }

  getPictureLocation(pathname: string): string {
    let completeName = '/api/movies/getPicture/' + encodeURIComponent(pathname);
    return completeName;
  }
}
