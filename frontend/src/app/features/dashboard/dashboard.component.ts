import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { RaceService } from '../../core/services/race.service';
import { Race } from '../../core/models/race.model';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterModule],
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

  getGoingClass(going: string | undefined): string {
    if (!going) return 'going-unknown';
    const g = going.toLowerCase();
    if (g.includes('firm') || g.includes('fast') || g.includes('hard'))
      return 'going-firm';
    if (g.includes('good')) return 'going-good';
    if (g.includes('soft') || g.includes('heavy') || g.includes('yielding'))
      return 'going-soft';
    return 'going-unknown';
  }

  getAnimationDelay(index: number): string {
    return `${index * 60}ms`;
  }

  trackByRace(_: number, race: Race): string {
    return race.raceId;
  }
}
