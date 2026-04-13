export interface RoomType {
  id: number;
  name: string;
  code: string;
  description: string;
  defaultCapacity: number;
  defaultPrice: number;
  icon: string;
  standardAmenities: string[];
  imageUrl: string;
}

export interface Room {
  id: number;
  hotelId: number;
  hotelName: string;
  roomTypeId: number;
  roomTypeName?: string;
  roomNumber: string;
  floor: number;
  view: string;
  capacity: number;
  extraBedCapacity: number;
  basePrice: number;
  isAvailable: boolean;
  isClean: boolean;
  status: string;
  amenities: string[];
  description: string;
}

export interface RoomInventory {
  id: number;
  roomId: number;
  date: string;
  isAvailable: boolean;
  price: number;
  availableCount: number;
  bookedCount: number;
  maintenanceCount: number;
}

export interface RoomStats {
  totalRooms: number;
  availableRooms: number;
  occupiedRooms: number;
  maintenanceRooms: number;
  cleaningRooms: number;
  byRoomType: { roomType: string; count: number }[];
  averagePrice: number;
  todayRevenue: number;
}