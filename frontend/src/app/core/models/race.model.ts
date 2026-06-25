export interface Race {
  raceId: string;
  courseName: string;
  raceName: string;
  offTime: string;
  distance?: string;
  going?: string;
  raceClass?: string;
  regionCode?: string;
  runnerCount: number;
  date: string;
}

export interface Runner {
  horseName: string;
  jockey?: string;
  trainer?: string;
  age?: string;
  form?: string;
  number?: number;
  odds?: string;
  lbs?: string;
}

export interface RaceDetail extends Race {
  runners: Runner[];
}
