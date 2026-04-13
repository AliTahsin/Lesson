export interface Chain {
  id: number;
  name: string;
  headquarters: string;
  foundedYear: number;
  totalHotels: number;
  website: string;
  logoUrl: string;
}

export interface Brand {
  id: number;
  name: string;
  chainName: string;
  segment: string;
  logoUrl: string;
  description: string;
  minStarRating: number;
  maxStarRating: number;
}

export interface Hotel {
  id: number;
  name: string;
  brandId: number;
  brandName?: string;
  chainName?: string;
  city: string;
  country: string;
  address: string;
  starRating: number;
  phone: string;
  email: string;
  website: string;
  description: string;
  totalRooms: number;
  status: string;
  amenities: string[];
  images: string[];
  location: {
    latitude: number;
    longitude: number;
  };
}

export interface HotelStats {
  totalHotels: number;
  totalBrands: number;
  totalChains: number;
  hotelsByCountry: { country: string; count: number }[];
  hotelsByStarRating: { starRating: number; count: number }[];
}