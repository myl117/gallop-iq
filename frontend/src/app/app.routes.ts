import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./features/dashboard/dashboard.component').then(
        (m) => m.DashboardComponent
      ),
  },
  {
    path: 'race/:id',
    loadComponent: () =>
      import('./features/race-detail/race-detail.component').then(
        (m) => m.RaceDetailComponent
      ),
  },
  { path: '**', redirectTo: '' },
];
