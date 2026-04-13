import axios from 'axios';
import { MaintenanceIssue, CreateIssueDto } from '../types/housekeeping';

const API_BASE_URL = 'http://localhost:5007/api';

export const issueApi = {
  // Get all issues
  getAllIssues: async (): Promise<MaintenanceIssue[]> => {
    const response = await axios.get(`${API_BASE_URL}/issues`);
    return response.data;
  },

  // Get issue by ID
  getIssueById: async (id: number): Promise<MaintenanceIssue> => {
    const response = await axios.get(`${API_BASE_URL}/issues/${id}`);
    return response.data;
  },

  // Get issues by hotel
  getIssuesByHotel: async (hotelId: number): Promise<MaintenanceIssue[]> => {
    const response = await axios.get(`${API_BASE_URL}/issues/hotel/${hotelId}`);
    return response.data;
  },

  // Get critical issues
  getCriticalIssues: async (): Promise<MaintenanceIssue[]> => {
    const response = await axios.get(`${API_BASE_URL}/issues/critical`);
    return response.data;
  },

  // Report issue
  reportIssue: async (data: CreateIssueDto): Promise<MaintenanceIssue> => {
    const response = await axios.post(`${API_BASE_URL}/issues`, data);
    return response.data;
  },

  // Assign issue
  assignIssue: async (issueId: number, staffId: number): Promise<MaintenanceIssue> => {
    const response = await axios.post(`${API_BASE_URL}/issues/${issueId}/assign/${staffId}`);
    return response.data;
  },

  // Resolve issue
  resolveIssue: async (issueId: number, resolutionNotes: string, actualCost: number): Promise<MaintenanceIssue> => {
    const response = await axios.post(`${API_BASE_URL}/issues/${issueId}/resolve`, {
      resolutionNotes,
      actualCost
    });
    return response.data;
  },

  // Close issue
  closeIssue: async (issueId: number, notes: string): Promise<MaintenanceIssue> => {
    const response = await axios.post(`${API_BASE_URL}/issues/${issueId}/close`, null, {
      params: { notes }
    });
    return response.data;
  }
};