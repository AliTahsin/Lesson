export interface Customer {
  id: number;
  customerNumber: string;
  email: string;
  firstName: string;
  lastName: string;
  fullName: string;
  phoneNumber: string;
  country: string;
  city: string;
  address: string;
  dateOfBirth: string;
  registrationDate: string;
  lastActivityDate: string;
  status: string;
  totalStays: number;
  totalNights: number;
  totalSpent: number;
  loyaltyPoints: number;
  membershipLevel: string;
  preferredLanguage: string;
  notes: string;
}

export interface LevelBenefits {
  discountRate: number;
  pointsMultiplier: number;
  freeUpgradePerYear: number;
  lateCheckoutHours: number;
  earlyCheckinHours: number;
  freeBreakfast: boolean;
  airportTransfer: boolean;
  loungeAccess: boolean;
}

export interface LoyaltyInfo {
  customerId: number;
  customerName: string;
  currentPoints: number;
  currentLevel: string;
  pointsToNextLevel: number;
  nextLevel: string;
  levelBenefits: LevelBenefits;
  totalStays: number;
  totalNights: number;
  totalSpent: number;
}

export interface LoyaltyTransaction {
  id: number;
  transactionType: string;
  points: number;
  pointsBefore: number;
  pointsAfter: number;
  source: string;
  description: string;
  transactionDate: string;
  expiryDate: string;
}

export interface MembershipLevel {
  id: number;
  name: string;
  minPoints: number;
  maxPoints: number;
  pointsMultiplier: number;
  discountRate: number;
  freeUpgradePerYear: number;
  lateCheckoutHours: number;
  earlyCheckinHours: number;
  freeBreakfast: boolean;
  airportTransfer: boolean;
  loungeAccess: boolean;
  color: string;
  icon: string;
}

export interface Preference {
  id: number;
  preferenceType: string;
  preferenceValue: string;
  description: string;
}

export interface LoyaltyStats {
  totalCustomers: number;
  totalPointsEarned: number;
  totalPointsRedeemed: number;
  averagePointsPerCustomer: number;
  byMembershipLevel: { level: string; count: number; averagePoints: number }[];
  topCustomers: { customerNumber: string; firstName: string; lastName: string; loyaltyPoints: number; membershipLevel: string }[];
}