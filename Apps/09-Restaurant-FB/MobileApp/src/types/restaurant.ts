export interface Restaurant {
  id: number;
  name: string;
  hotelId: number;
  cuisineType: string;
  description: string;
  openingTime: string;
  closingTime: string;
  totalTables: number;
  totalCapacity: number;
  averagePricePerPerson: number;
  isActive: boolean;
  images: string[];
}

export interface MenuItem {
  id: number;
  name: string;
  description: string;
  category: string;
  price: number;
  currency: string;
  isVegetarian: boolean;
  isVegan: boolean;
  isGlutenFree: boolean;
  preparationTimeMinutes: number;
  calories: number;
  ingredients: string[];
  allergens: string[];
  imageUrl: string;
  isAvailable: boolean;
}

export interface CartItem {
  menuItemId: number;
  name: string;
  quantity: number;
  price: number;
  totalPrice: number;
  specialInstructions: string;
}

export interface Order {
  id: number;
  orderNumber: string;
  restaurantId: number;
  tableId?: number;
  tableNumber?: string;
  customerName: string;
  orderType: string;
  status: string;
  subTotal: number;
  taxAmount: number;
  totalAmount: number;
  currency: string;
  specialInstructions: string;
  orderTime: string;
  items: OrderItem[];
}

export interface OrderItem {
  id: number;
  itemName: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  specialInstructions: string;
  status: string;
}

export interface CreateOrderDto {
  restaurantId: number;
  tableId?: number;
  tableNumber?: string;
  customerId?: number;
  customerName: string;
  roomId?: number;
  roomNumber?: string;
  orderType: string;
  specialInstructions: string;
  items: CreateOrderItemDto[];
}

export interface CreateOrderItemDto {
  menuItemId: number;
  quantity: number;
  specialInstructions: string;
}

export interface TableReservation {
  id: number;
  reservationNumber: string;
  restaurantId: number;
  tableNumber: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  guestCount: number;
  reservationDate: string;
  reservationTime: string;
  status: string;
}

export interface Table {
  id: number;
  tableNumber: string;
  capacity: number;
  location: string;
  status: string;
  qrCodeUrl: string;
}