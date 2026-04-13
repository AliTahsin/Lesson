export interface PriceBreakdown {
  factor: string;
  description: string;
  multiplier: number;
  impact: number;
}

export interface DailyPrice {
  date: string;
  basePrice: number;
  finalPrice: number;
  breakdowns: PriceBreakdown[];
  demandScore: number;
  occupancyRate: number;
}

export interface PriceResponse {
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  nightCount: number;
  dailyPrices: DailyPrice[];
  totalPrice: number;
  averagePricePerNight: number;
  longStayDiscount: number;
  promoDiscount: number;
}

export interface PriceRequest {
  roomId: number;
  checkInDate: string;
  checkOutDate: string;
  guestCount: number;
  promoCode?: string;
}

export interface DynamicPrice {
  id: number;
  roomId: number;
  date: string;
  basePrice: number;
  finalPrice: number;
  occupancyMultiplier: number;
  demandMultiplier: number;
  seasonMultiplier: number;
  isActive: boolean;
}

export interface DemandFactor {
  id: number;
  hotelId: number;
  date: string;
  demandScore: number;
  expectedOccupancy: number;
  webSearchCount: number;
  bookingAttempts: number;
  events: string[];
}

export interface PriceRule {
  id: number;
  name: string;
  ruleType: string;
  minValue: number;
  maxValue: number;
  multiplier: number;
  description: string;
}

export interface Season {
  id: number;
  hotelId: number;
  name: string;
  startMonth: number;
  endMonth: number;
  multiplier: number;
  color: string;
}

export interface PricingStats {
  averagePrice: number;
  minPrice: number;
  maxPrice: number;
  averageDemandScore: number;
  priceVolatility: number;
  byDayOfWeek: { day: string; averagePrice: number; count: number }[];
  revenueForecast: number;
}