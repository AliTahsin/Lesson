import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert,
  Share
} from 'react-native';
import { paymentApi } from '../services/paymentApi';
import { Invoice, InvoiceItem } from '../types/payment';

export const InvoiceScreen = ({ route }: any) => {
  const { reservationId } = route.params;
  const [invoice, setInvoice] = useState<Invoice | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadInvoice();
  }, []);

  const loadInvoice = async () => {
    setLoading(true);
    try {
      const data = await paymentApi.getInvoiceByReservation(reservationId);
      setInvoice(data);
    } catch (error) {
      console.error('Error loading invoice:', error);
      Alert.alert('Hata', 'Fatura yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleDownloadPdf = async () => {
    if (!invoice) return;
    
    try {
      const pdfUrl = await paymentApi.downloadInvoicePdf(invoice.id);
      await Share.share({
        url: pdfUrl,
        message: `Fatura ${invoice.invoiceNumber}`
      });
    } catch (error) {
      Alert.alert('Hata', 'PDF indirilemedi');
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Paid': return '#10b981';
      case 'Issued': return '#3b82f6';
      case 'Draft': return '#6b7280';
      case 'Cancelled': return '#ef4444';
      default: return '#6b7280';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!invoice) {
    return (
      <View style={styles.center}>
        <Text>Fatura bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.invoiceTitle}>FATURA</Text>
        <Text style={styles.invoiceNumber}>{invoice.invoiceNumber}</Text>
        {invoice.eInvoiceNumber && (
          <Text style={styles.eInvoiceNumber}>e-Fatura No: {invoice.eInvoiceNumber}</Text>
        )}
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(invoice.status) }]}>
          <Text style={styles.statusText}>{invoice.status}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Fatura Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Fatura Tarihi:</Text>
          <Text style={styles.infoValue}>{formatDate(invoice.issueDate)}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Son Ödeme Tarihi:</Text>
          <Text style={styles.infoValue}>{formatDate(invoice.dueDate)}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Müşteri Bilgileri</Text>
        <Text style={styles.customerName}>{invoice.customerName}</Text>
        <Text style={styles.customerTaxId}>Vergi No: {invoice.customerTaxId}</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Fatura Kalemleri</Text>
        <View style={styles.itemsHeader}>
          <Text style={styles.itemDesc}>Açıklama</Text>
          <Text style={styles.itemQty}>Adet</Text>
          <Text style={styles.itemPrice}>Birim</Text>
          <Text style={styles.itemTotal}>Toplam</Text>
        </View>
        
        {invoice.items.map((item: InvoiceItem, index: number) => (
          <View key={index} style={styles.itemRow}>
            <Text style={styles.itemDescText}>{item.description}</Text>
            <Text style={styles.itemQtyText}>{item.quantity}</Text>
            <Text style={styles.itemPriceText}>€{item.unitPrice.toFixed(2)}</Text>
            <Text style={styles.itemTotalText}>€{item.totalPrice.toFixed(2)}</Text>
          </View>
        ))}
      </View>

      <View style={styles.totalSection}>
        <View style={styles.totalRow}>
          <Text style={styles.totalLabel}>Ara Toplam</Text>
          <Text style={styles.totalValue}>€{invoice.subTotal.toFixed(2)}</Text>
        </View>
        <View style={styles.totalRow}>
          <Text style={styles.totalLabel}>KDV (%{invoice.taxRate})</Text>
          <Text style={styles.totalValue}>€{invoice.taxAmount.toFixed(2)}</Text>
        </View>
        <View style={styles.grandTotalRow}>
          <Text style={styles.grandTotalLabel}>GENEL TOPLAM</Text>
          <Text style={styles.grandTotalValue}>€{invoice.totalAmount.toFixed(2)}</Text>
        </View>
      </View>

      <View style={styles.footer}>
        <TouchableOpacity style={styles.downloadButton} onPress={handleDownloadPdf}>
          <Text style={styles.downloadButtonText}>📄 PDF İndir</Text>
        </TouchableOpacity>
        <Text style={styles.footerText}>e-Fatura yasal bir belgedir.</Text>
        <Text style={styles.footerText}>GİB onaylıdır.</Text>
      </View>
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  headerCard: {
    backgroundColor: '#1a1a2e',
    borderRadius: 16,
    margin: 16,
    padding: 20,
    alignItems: 'center',
  },
  invoiceTitle: {
    color: 'white',
    fontSize: 20,
    fontWeight: 'bold',
    marginBottom: 8,
  },
  invoiceNumber: {
    color: '#fbbf24',
    fontSize: 18,
    fontWeight: 'bold',
    marginBottom: 4,
  },
  eInvoiceNumber: {
    color: '#9ca3af',
    fontSize: 12,
    marginBottom: 8,
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 100,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  customerName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 4,
  },
  customerTaxId: {
    fontSize: 14,
    color: '#666',
  },
  itemsHeader: {
    flexDirection: 'row',
    backgroundColor: '#f5f5f5',
    padding: 10,
    borderRadius: 8,
    marginBottom: 8,
  },
  itemDesc: {
    flex: 3,
    fontSize: 12,
    fontWeight: 'bold',
    color: '#666',
  },
  itemQty: {
    flex: 1,
    fontSize: 12,
    fontWeight: 'bold',
    color: '#666',
    textAlign: 'center',
  },
  itemPrice: {
    flex: 1.5,
    fontSize: 12,
    fontWeight: 'bold',
    color: '#666',
    textAlign: 'right',
  },
  itemTotal: {
    flex: 1.5,
    fontSize: 12,
    fontWeight: 'bold',
    color: '#666',
    textAlign: 'right',
  },
  itemRow: {
    flexDirection: 'row',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  itemDescText: {
    flex: 3,
    fontSize: 13,
    color: '#333',
  },
  itemQtyText: {
    flex: 1,
    fontSize: 13,
    color: '#333',
    textAlign: 'center',
  },
  itemPriceText: {
    flex: 1.5,
    fontSize: 13,
    color: '#333',
    textAlign: 'right',
  },
  itemTotalText: {
    flex: 1.5,
    fontSize: 13,
    fontWeight: '500',
    color: '#333',
    textAlign: 'right',
  },
  totalSection: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  totalRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  totalLabel: {
    fontSize: 14,
    color: '#666',
  },
  totalValue: {
    fontSize: 14,
    color: '#333',
  },
  grandTotalRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 8,
    paddingTop: 8,
    borderTopWidth: 1,
    borderTopColor: '#e0e0e0',
  },
  grandTotalLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  grandTotalValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  footer: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
    alignItems: 'center',
  },
  downloadButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 20,
    paddingVertical: 10,
    marginBottom: 12,
  },
  downloadButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
  footerText: {
    fontSize: 11,
    color: '#888',
    marginBottom: 2,
  },
});