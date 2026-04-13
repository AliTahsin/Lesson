import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { PaymentResponse } from '../types/payment';

interface Props {
  payment: PaymentResponse;
  onPress?: () => void;
}

export const PaymentCard: React.FC<Props> = ({ payment, onPress }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Success': return '#10b981';
      case 'Failed': return '#ef4444';
      case 'Refunded': return '#f59e0b';
      default: return '#6b7280';
    }
  };

  const getStatusIcon = (status: string) => {
    switch (status) {
      case 'Success': return '✓';
      case 'Failed': return '✗';
      case 'Refunded': return '↺';
      default: return '○';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  return (
    <View style={styles.card}>
      <View style={styles.header}>
        <View>
          <Text style={styles.paymentNumber}>{payment.paymentNumber}</Text>
          <Text style={styles.date}>{formatDate(payment.paymentDate)}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(payment.status) }]}>
          <Text style={styles.statusText}>
            {getStatusIcon(payment.status)} {payment.status}
          </Text>
        </View>
      </View>

      <View style={styles.details}>
        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Tutar</Text>
          <Text style={styles.amount}>
            {payment.currency} {payment.amount.toLocaleString()}
          </Text>
        </View>

        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Kart</Text>
          <Text style={styles.detailValue}>
            {payment.cardBrand} • {payment.maskedCardNumber}
          </Text>
        </View>

        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Taksit</Text>
          <Text style={styles.detailValue}>{payment.installment} taksit</Text>
        </View>

        <View style={styles.detailRow}>
          <Text style={styles.detailLabel}>Ödeme Yöntemi</Text>
          <Text style={styles.detailValue}>{payment.paymentMethod}</Text>
        </View>
      </View>

      {payment.isSuccess && (
        <View style={styles.successBadge}>
          <Text style={styles.successText}>✓ Ödeme Onaylandı</Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 1 },
    shadowOpacity: 0.05,
    shadowRadius: 2,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
    paddingBottom: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  paymentNumber: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
  },
  date: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 11,
    fontWeight: 'bold',
  },
  details: {
    gap: 8,
  },
  detailRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  detailLabel: {
    fontSize: 13,
    color: '#666',
  },
  detailValue: {
    fontSize: 13,
    color: '#333',
    fontWeight: '500',
  },
  amount: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  successBadge: {
    marginTop: 12,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
    alignItems: 'center',
  },
  successText: {
    color: '#10b981',
    fontSize: 12,
    fontWeight: 'bold',
  },
});