import axios from 'axios';
import * as FileSystem from 'react-native-fs';
import Share from 'react-native-share';
import {
  RevenueReport,
  OccupancyReport,
  ReservationReport,
  CustomerReport,
  ChannelReport,
  KPI,
  ReportRequest,
  ReportDto,
  ScheduledReport
} from '../types/reporting';

const API_BASE_URL = 'http://localhost:5010/api';

export const reportApi = {
  // Revenue Report
  getRevenueReport: async (hotelId: number, startDate: string, endDate: string): Promise<RevenueReport> => {
    const response = await axios.get(`${API_BASE_URL}/reports/revenue`, {
      params: { hotelId, startDate, endDate }
    });
    return response.data;
  },

  // Occupancy Report
  getOccupancyReport: async (hotelId: number, startDate: string, endDate: string): Promise<OccupancyReport> => {
    const response = await axios.get(`${API_BASE_URL}/reports/occupancy`, {
      params: { hotelId, startDate, endDate }
    });
    return response.data;
  },

  // Reservation Report
  getReservationReport: async (hotelId: number, startDate: string, endDate: string): Promise<ReservationReport> => {
    const response = await axios.get(`${API_BASE_URL}/reports/reservation`, {
      params: { hotelId, startDate, endDate }
    });
    return response.data;
  },

  // Customer Report
  getCustomerReport: async (hotelId: number, startDate: string, endDate: string): Promise<CustomerReport> => {
    const response = await axios.get(`${API_BASE_URL}/reports/customer`, {
      params: { hotelId, startDate, endDate }
    });
    return response.data;
  },

  // Channel Report
  getChannelReport: async (hotelId: number, startDate: string, endDate: string): Promise<ChannelReport> => {
    const response = await axios.get(`${API_BASE_URL}/reports/channel`, {
      params: { hotelId, startDate, endDate }
    });
    return response.data;
  },

  // Export to Excel
  exportToExcel: async (request: ReportRequest): Promise<string> => {
    const response = await axios.post(`${API_BASE_URL}/reports/export/excel`, request, {
      responseType: 'blob'
    });
    
    const filePath = `${FileSystem.DocumentDirectoryPath}/${request.reportType}_Report_${Date.now()}.xlsx`;
    await FileSystem.writeFile(filePath, response.data, 'base64');
    return filePath;
  },

  // Export to PDF
  exportToPdf: async (request: ReportRequest): Promise<string> => {
    const response = await axios.post(`${API_BASE_URL}/reports/export/pdf`, request, {
      responseType: 'blob'
    });
    
    const filePath = `${FileSystem.DocumentDirectoryPath}/${request.reportType}_Report_${Date.now()}.pdf`;
    await FileSystem.writeFile(filePath, response.data, 'base64');
    return filePath;
  },

  // Generate and save report
  generateAndSaveReport: async (request: ReportRequest): Promise<ReportDto> => {
    const response = await axios.post(`${API_BASE_URL}/reports/generate`, request);
    return response.data;
  },

  // Download and share report
  downloadAndShare: async (filePath: string) => {
    await Share.open({
      url: `file://${filePath}`,
      type: filePath.endsWith('.pdf') ? 'application/pdf' : 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
    });
  },

  // Get KPIs
  getKPIs: async (hotelId: number): Promise<KPI[]> => {
    const response = await axios.get(`${API_BASE_URL}/dashboards/kpis`, {
      params: { hotelId }
    });
    return response.data;
  },

  // Get scheduled reports
  getScheduledReports: async (hotelId: number): Promise<ScheduledReport[]> => {
    const response = await axios.get(`${API_BASE_URL}/scheduledreports`, {
      params: { hotelId }
    });
    return response.data;
  }
};