import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import QRCode from 'react-native-qrcode-svg';
import Icon from 'react-native-vector-icons/Ionicons';
import { DigitalKey } from '../types/customer';
import { useLanguageContext } from '../context/LanguageContext';

interface Props {
  digitalKey: DigitalKey;
}

export const DigitalKeyCard: React.FC<Props> = ({ digitalKey }) => {
  const { t } = useLanguageContext();
  const isValid = digitalKey.isActive && !digitalKey.isUsed && new Date(digitalKey.validUntil) > new Date();

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
  };

  return (
    <View style={[styles.card, !isValid && styles.cardExpired]}>
      <View style={styles.header}>
        <Text style={styles.roomNumber}>{digitalKey.roomNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: isValid ? '#10b981' : '#ef4444' }]}>
          <Text style={styles.statusText}>
            {isValid ? t('key_active') : digitalKey.isUsed ? t('key_used') : t('key_expired')}
          </Text>
        </View>
      </View>

      <View style={styles.qrContainer}>
        <QRCode
          value={digitalKey.qrCode}
          size={180}
          backgroundColor="white"
          color="#000"
        />
      </View>

      <View style={styles.footer}>
        <View style={styles.infoRow}>
          <Icon name="calendar-outline" size={14} color="#666" />
          <Text style={styles.infoText}>
            {t('key_valid_until')}: {formatDate(digitalKey.validUntil)}
          </Text>
        </View>
        <View style={styles.infoRow}>
          <Icon name="key-outline" size={14} color="#666" />
          <Text style={styles.infoText}>Oda: {digitalKey.roomNumber}</Text>
        </View>
      </View>

      {digitalKey.isUsed && digitalKey.usedAt && (
        <View style={styles.usedInfo}>
          <Icon name="checkmark-circle" size={16} color="#10b981" />
          <Text style={styles.usedText}>
            Kullanıldı: {new Date(digitalKey.usedAt).toLocaleString()}
          </Text>
        </View>
      )}
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 16,
    margin: 16,
    padding: 20,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  cardExpired: {
    opacity: 0.7,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 20,
  },
  roomNumber: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 20,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  qrContainer: {
    alignItems: 'center',
    marginVertical: 20,
  },
  footer: {
    marginTop: 16,
    gap: 8,
  },
  infoRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  infoText: {
    fontSize: 13,
    color: '#666',
  },
  usedInfo: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    marginTop: 16,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  usedText: {
    fontSize: 12,
    color: '#10b981',
  },
});