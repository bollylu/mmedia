import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IMovies } from '../model/IMovies';
import { IGroups } from "../model/IGroups";

@Injectable({
  providedIn: 'root'
})

export class MoviesService {

  constructor(private http: HttpClient) { }

  getGroups(group: string): Observable<IGroups> {
    const params: HttpParams = new HttpParams()
      .set("group", encodeURIComponent(group));

    return this.http.get<IGroups>('/api/groups', { params });
  }

  getMovies(group: string = "", page: number = 1): Observable<IMovies> {
    console.log('get for group ' + group);

    const params: HttpParams = new HttpParams()
      .set("group", encodeURIComponent(group))
      .set("page", page.toString())
      .set("items", "20");

    return this.http.get<IMovies>("/api/movies", { params });
  }

  getPictureLocation(pathname: string): string {
    const completeName = '/api/movies/getPicture/' + encodeURIComponent(pathname);
    return completeName;
  }
}
