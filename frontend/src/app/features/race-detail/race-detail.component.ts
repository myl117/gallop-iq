import { Component, OnInit } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { RouterModule, ActivatedRoute, Router } from '@angular/router';
import { RaceService } from '../../core/services/race.service';
import { PredictionService } from '../../core/services/prediction.service';
import { RaceDetail } from '../../core/models/race.model';
import {
  RacePredictionResult,
  HorsePrediction,
} from '../../core/models/prediction.model';

@Component({
  selector: 'app-race-detail',
  standalone: true,
  imports: [CommonModule, RouterModule, DecimalPipe],
  templateUrl: './race-detail.component.html',
  styleUrls: ['./race-detail.component.scss'],
})
export class RaceDetailComponent implements OnInit {
  raceId = '';
  race: RaceDetail | null = null;
  predictionResult: RacePredictionResult | null = null;
  loadingRace = true;
  loadingPredictions = false;
  raceError: string | null = null;
  predictionError: string | null = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private raceService: RaceService,
    private predictionService: PredictionService
  ) {}

  ngOnInit(): void {
    this.raceId = this.route.snapshot.paramMap.get('id') ?? '';
    this.loadRace();
  }

  private loadRace(): void {
    this.raceService.getRaceById(this.raceId).subscribe({
      next: (race) => {
        this.race = race;
        this.loadingRace = false;
        this.loadPredictions();
      },
      error: () => {
        this.raceError = 'Failed to load race details.';
        this.loadingRace = false;
      },
    });
  }

  loadPredictions(): void {
    this.loadingPredictions = true;
    this.predictionError = null;
    this.predictionResult = null;
    this.predictionService.generatePredictions(this.raceId).subscribe({
      next: (result) => {
        this.predictionResult = result;
        this.loadingPredictions = false;
      },
      error: () => {
        this.predictionError =
          'Failed to generate AI predictions. Check your Gemini API key configuration.';
        this.loadingPredictions = false;
      },
    });
  }

  getPredictionForHorse(horseName: string): HorsePrediction | undefined {
    return this.predictionResult?.horses.find(
      (h) => h.horseName.toLowerCase() === horseName.toLowerCase()
    );
  }

  getProbabilityPercent(prob: number): string {
    return `${Math.round(prob * 100)}%`;
  }

  goBack(): void {
    this.router.navigate(['/']);
  }

  trackByHorse(_: number, h: HorsePrediction): string {
    return h.horseName;
  }

  trackByRunner(_: number, r: { horseName: string }): string {
    return r.horseName;
  }

  get sortedPredictions(): HorsePrediction[] {
    return (
      this.predictionResult?.horses
        .slice()
        .sort((a, b) => b.winProbability - a.winProbability) ?? []
    );
  }
}
