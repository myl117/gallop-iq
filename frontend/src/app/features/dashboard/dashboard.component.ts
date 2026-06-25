import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { RaceService } from '../../core/services/race.service';
import { Race } from '../../core/models/race.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatChipsModule,
    MatIconModule,
    MatDividerModule,
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
})
export class DashboardComponent implements OnInit {
  races: Race[] = [];
  loading = true;
  error: string | null = null;
  today = new Date();
  skeletons = Array(6).fill(null);

  constructor(private raceService: RaceService) {}

  ngOnInit(): void {
    this.raceService.getTodaysRaces().subscribe({
      next: (races) => {
        this.races = races;
        this.loading = false;
      },
      error: (err) => {
        this.error =
          "Failed to load today's races. Please check your API configuration and ensure the backend is running.";
        this.loading = false;
        console.error(err);
      },
    });
  }

  getGoingColor(going: string | undefined): 'primary' | 'accent' | 'warn' | undefined {
    if (!going) return undefined;
    const g = going.toLowerCase();
    if (g.includes('firm') || g.includes('fast') || g.includes('hard')) return 'primary';
    if (g.includes('good')) return 'accent';
    if (g.includes('soft') || g.includes('heavy') || g.includes('yielding')) return 'warn';
    return undefined;
  }

  getAnimationDelay(index: number): string {
    return `${index * 50}ms`;
  }

  trackByRace(_: number, race: Race): string {
    return race.raceId;
  }
}
