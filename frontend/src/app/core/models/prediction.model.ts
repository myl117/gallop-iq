export interface HorsePrediction {
  horseName: string;
  winProbability: number;
  confidence: 'high' | 'medium' | 'low';
  valueScore: number;
  reasoning: string;
  isBestPick: boolean;
}

export interface RacePredictionResult {
  raceId: string;
  generatedAt: string;
  horses: HorsePrediction[];
}
