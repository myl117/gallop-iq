import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RacePredictionResult } from '../models/prediction.model';

@Injectable({ providedIn: 'root' })
export class PredictionService {
  constructor(private http: HttpClient) {}

  generatePredictions(raceId: string): Observable<RacePredictionResult> {
    return this.http.post<RacePredictionResult>(`/predict/${raceId}`, {});
  }

  getPredictions(raceId: string): Observable<RacePredictionResult> {
    return this.http.get<RacePredictionResult>(`/predictions/${raceId}`);
  }
}
