export interface PaymentRequest {
  reservationId: number;
  customerId: number;
  amount: number;
  currency: string;
  paymentMethod: string;
  cardNumber: string;
  cardHolderName: string;
  expiryMonth: number;
  expiryYear: number;
  cvv: string;
  installment: number;
  paymentSource: string;
}

export interface PaymentResponse {
  id: number;
  paymentNumber: string;
  amount: number;
  currency: string;
  paymentMethod: string;
  cardBrand: string;
  maskedCardNumber: string;
  installment: string;
  status: string;
  isSuccess: boolean;
  message: string;
  paymentDate: string;
}

export interface RefundRequest {
  paymentId: number;
  reservationId: number;
  amount: number;
  reason: string;
  notes?: string;
}

export interface Invoice {
  id: number;
  invoiceNumber: string;
  eInvoiceNumber?: string;
  reservationId: number;
  customerName: string;
  customerTaxId: string;
  subTotal: number;
  taxAmount: number;
  taxRate: number;
  totalAmount: number;
  currency: string;
  issueDate: string;
  dueDate: string;
  status: string;
  pdfUrl?: string;
  items: InvoiceItem[];
}

export interface InvoiceItem {
  description: string;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  taxRate: number;
  taxAmount: number;
}