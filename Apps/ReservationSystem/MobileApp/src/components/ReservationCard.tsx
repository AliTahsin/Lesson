import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet } from 'react-native';

interface Reservation {
  id: number;
  reservationNumber: string;
  hotelName: string;
  roomNumber: string;
  roomType: string;
  checkInDate: string;
  checkOutDate: string;
  nightCount: number;
  guestCount: number;
  totalPrice: number;
  status: string;
  paymentStatus: string;
}

interface Props {
  reservation: Reservation;
  onPress: (id: number) => void;
  onCancel?: (id: number) => void;
}

export const ReservationCard: React.FC<Props> = ({ reservation, onPress, onCancel }) => {
  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Confirmed': return '#16a34a';
      case 'Pending': return '#f59e0b';
      case 'CheckedIn': return '#3b82f6';
      case 'CheckedOut': return '#6b7280';
      case 'Cancelled': return '#dc2626';
      default: return '#6b7280';
    }
  };

  const getPaymentStatusColor = (status: string) => {
    switch (status) {
      case 'Paid': return '#16a34a';
      case 'Partial': return '#f59e0b';
      case 'Pending': return '#dc2626';
      default: return '#6b7280';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  const canCancel = reservation.status === 'Confirmed' && 
    new Date(reservation.checkInDate) > new Date();

  return (
    <TouchableOpacity onPress={() => onPress(reservation.id)} style={styles.card}>
      <View style={styles.header}>
        <View>
          <Text style={styles.hotelName}>{reservation.hotelName}</Text>
          <Text style={styles.reservationNumber}>{reservation.reservationNumber}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(reservation.status) }]}>
          <Text style={styles.statusText}>{reservation.status}</Text>
        </View>
      </View>

      <View style={styles.roomInfo}>
        <Text style={styles.roomType}>{reservation.roomType}</Text>
        <Text style={styles.roomNumber}>Oda {reservation.roomNumber}</Text>
      </View>

      <View style={styles.dateInfo}>
        <View style={styles.dateItem}>
          <Text style={styles.dateLabel}>Giriş</Text>
          <Text style={styles.dateValue}>{formatDate(reservation.checkInDate)}</Text>
        </View>
        <View style={styles.dateArrow}>→</View>
        <View style={styles.dateItem}>
          <Text style={styles.dateLabel}>Çıkış</Text>
          <Text style={styles.dateValue}>{formatDate(reservation.checkOutDate)}</Text>
        </View>
      </View>

      <View style={styles.details}>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>Gece</Text>
          <Text style={styles.detailValue}>{reservation.nightCount}</Text>
        </View>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>Misafir</Text>
          <Text style={styles.detailValue}>{reservation.guestCount}</Text>
        </View>
        <View style={styles.detailItem}>
          <Text style={styles.detailLabel}>Ödeme</Text>
          <Text style={[styles.detailValue, { color: getPaymentStatusColor(reservation.paymentStatus) }]}>
            {reservation.paymentStatus}
          </Text>
        </View>
      </View>

      <View style={styles.priceRow}>
        <Text style={styles.priceLabel}>Toplam</Text>
        <Text style={styles.priceValue}>€{reservation.totalPrice.toFixed(2)}</Text>
      </View>

      {canCancel && onCancel && (
        <TouchableOpacity
          style={styles.cancelButton}
          onPress={() => onCancel(reservation.id)}
        >
          <Text style={styles.cancelButtonText}>İptal Et</Text>
        </TouchableOpacity>
      )}
    </TouchableOpacity>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 8,
    padding: 16,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  hotelName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  reservationNumber: {
    fontSize: 12,
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
  roomInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 12,
  },
  roomType: {
    fontSize: 14,
    color: '#666',
  },
  roomNumber: {
    fontSize: 14,
    fontWeight: '500',
    color: '#007AFF',
  },
  dateInfo: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: '#f5f5f5',
    borderRadius: 8,
    padding: 12,
    marginBottom: 12,
  },
  dateItem: {
    alignItems: 'center',
  },
  dateLabel: {
    fontSize: 11,
    color: '#888',
    marginBottom: 4,
  },
  dateValue: {
    fontSize: 13,
    fontWeight: '500',
    color: '#333',
  },
  dateArrow: {
    fontSize: 16,
    color: '#999',
  },
  details: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    borderTopWidth: 1,
    borderBottomWidth: 1,
    borderColor: '#f0f0f0',
    paddingVertical: 12,
    marginBottom: 12,
  },
  detailItem: {
    alignItems: 'center',
  },
  detailLabel: {
    fontSize: 11,
    color: '#888',
    marginBottom: 4,
  },
  detailValue: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  priceLabel: {
    fontSize: 14,
    color: '#666',
  },
  priceValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  cancelButton: {
    marginTop: 12,
    backgroundColor: '#dc2626',
    borderRadius: 8,
    paddingVertical: 10,
    alignItems: 'center',
  },
  cancelButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
});