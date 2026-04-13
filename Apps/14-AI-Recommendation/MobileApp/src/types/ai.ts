export interface Recommendation {
  id: number;
  userId: number;
  itemId: number;
  itemType: string;
  itemName?: string;
  score: number;
  algorithm: string;
  reason: string;
  isClicked: boolean;
  isBooked: boolean;
  recommendedAt: string;
}

export interface TrackInteractionRequest {
  userId: number;
  itemId: number;
  itemType: string;
  interactionType: string;
  rating?: number;
  review?: string;
}

export interface DemandPrediction {
  hotelId: number;
  date: string;
  predictedDemand: number;
  confidence: number;
  factors: string[];
}

export interface RevenuePrediction {
  hotelId: number;
  startDate: string;
  endDate: string;
  predictedRevenue: number;
  confidence: number;
  dailyPredictions: DailyRevenuePrediction[];
}

export interface DailyRevenuePrediction {
  date: string;
  predictedRevenue: number;
  confidence: number;
}

export interface OccupancyPrediction {
  hotelId: number;
  date: string;
  predictedOccupancyRate: number;
  predictedSoldRooms: number;
  totalRooms: number;
  confidence: number;
}

export interface SentimentResult {
  reviewId?: number;
  sentiment: string;
  positiveScore: number;
  negativeScore: number;
  neutralScore: number;
  confidence: number;
  keywords: string[];
  analyzedAt: string;
}

export interface SentimentSummary {
  itemId: number;
  itemType: string;
  totalReviews: number;
  positiveCount: number;
  negativeCount: number;
  positivePercentage: number;
  negativePercentage: number;
  averageRating: number;
}

export interface RecommendationMetrics {
  totalRecommendations: number;
  clickThroughRate: number;
  conversionRate: number;
  averageScore: number;
}