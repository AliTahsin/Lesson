export interface MeetingRoom {
  id: number;
  hotelId: number;
  name: string;
  roomCode: string;
  capacity: number;
  theaterCapacity: number;
  classroomCapacity: number;
  uShapeCapacity: number;
  boardroomCapacity: number;
  area: number;
  halfDayPrice: number;
  fullDayPrice: number;
  hasProjector: boolean;
  hasSoundSystem: boolean;
  hasWhiteboard: boolean;
  hasFlipChart: boolean;
  hasWiFi: boolean;
  hasNaturalLight: boolean;
  images: string[];
  floor: string;
  isActive: boolean;
}

export interface Event {
  id: number;
  eventNumber: string;
  hotelId: number;
  meetingRoomId: number;
  meetingRoomName: string;
  name: string;
  description: string;
  eventType: string;
  startDate: string;
  endDate: string;
  expectedAttendees: number;
  actualAttendees: number;
  totalBudget: number;
  actualCost: number;
  status: string;
  customerName: string;
  customerCompany: string;
  specialRequests: string;
  schedule: EventSchedule[];
}

export interface EventSchedule {
  id: number;
  title: string;
  description: string;
  startTime: string;
  endTime: string;
  speaker: string;
  location: string;
  order: number;
}

export interface Attendee {
  id: number;
  eventId: number;
  firstName: string;
  lastName: string;
  fullName: string;
  email: string;
  phone: string;
  company: string;
  title: string;
  checkInTime?: string;
  hasCheckedIn: boolean;
  dietaryRestrictions: string;
  status: string;
  qrCode: string;
}

export interface Equipment {
  id: number;
  hotelId: number;
  name: string;
  category: string;
  description: string;
  dailyPrice: number;
  weeklyPrice: number;
  totalQuantity: number;
  availableQuantity: number;
  isActive: boolean;
}

export interface AttendeeStats {
  eventId: number;
  eventName: string;
  totalRegistered: number;
  checkedIn: number;
  checkedInRate: number;
  noShow: number;
  byCompany: { company: string; count: number; checkedIn: number }[];
}