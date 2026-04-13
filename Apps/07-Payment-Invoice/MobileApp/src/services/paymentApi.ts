import axios from 'axios';
import { PaymentRequest, PaymentResponse, RefundRequest, Invoice } from '../types/payment';

const API_BASE_URL = 'http://localhost:5006/api';

export const paymentApi = {
  // Process payment
  processPayment: async (request: PaymentRequest): Promise<PaymentResponse> => {
    const response = await axios.post(`${API_BASE_URL}/payments/process`, request);
    return response.data;
  },

  // Get payment by ID
  getPaymentById: async (id: number): Promise<PaymentResponse> => {
    const response = await axios.get(`${API_BASE_URL}/payments/${id}`);
    return response.data;
  },

  // Get payments by reservation
  getPaymentsByReservation: async (reservationId: number): Promise<PaymentResponse[]> => {
    const response = await axios.get(`${API_BASE_URL}/payments/reservation/${reservationId}`);
    return response.data;
  },

  // Get payments by customer
  getPaymentsByCustomer: async (customerId: number): Promise<PaymentResponse[]> => {
    const response = await axios.get(`${API_BASE_URL}/payments/customer/${customerId}`);
    return response.data;
  },

  // Process refund
  processRefund: async (request: RefundRequest): Promise<any> => {
    const response = await axios.post(`${API_BASE_URL}/refunds/process`, request);
    return response.data;
  },

  // Get invoice by reservation
  getInvoiceByReservation: async (reservationId: number): Promise<Invoice> => {
    const response = await axios.get(`${API_BASE_URL}/invoices/reservation/${reservationId}`);
    return response.data;
  },

  // Download invoice PDF
  downloadInvoicePdf: async (invoiceId: number): Promise<string> => {
    const response = await axios.get(`${API_BASE_URL}/invoices/${invoiceId}/pdf`, {
      responseType: 'blob'
    });
    return URL.createObjectURL(response.data);
  },

  // Get payment statistics
  getPaymentStats: async (startDate: string, endDate: string): Promise<any> => {
    const response = await axios.get(`${API_BASE_URL}/payments/stats`, {
      params: { startDate, endDate }
    });
    return response.data;
  }
};