import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Race, RaceDetail } from '../models/race.model';

@Injectable({ providedIn: 'root' })
export class RaceService {
  constructor(private http: HttpClient) {}

  getTodaysRaces(): Observable<Race[]> {
    return this.http.get<Race[]>('/races/today');
  }

  getRaceById(id: string): Observable<RaceDetail> {
    return this.http.get<RaceDetail>(`/races/${id}`);
  }
}
