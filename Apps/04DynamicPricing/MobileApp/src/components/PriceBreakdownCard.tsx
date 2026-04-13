import React from 'react';
import { View, Text, StyleSheet } from 'react-native';
import { PriceBreakdown } from '../types/pricing';

interface Props {
  date: string;
  basePrice: number;
  finalPrice: number;
  breakdowns: PriceBreakdown[];
  demandScore: number;
  occupancyRate: number;
}

export const PriceBreakdownCard: React.FC<Props> = ({
  date,
  basePrice,
  finalPrice,
  breakdowns,
  demandScore,
  occupancyRate
}) => {
  const formatDate = (dateString: string) => {
    const d = new Date(dateString);
    return `${d.getDate()} ${getMonthName(d.getMonth())} ${d.getFullYear()}`;
  };

  const getMonthName = (month: number) => {
    const months = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 
                    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];
    return months[month];
  };

  const getDemandColor = (score: number) => {
    if (score >= 80) return '#dc2626';
    if (score >= 60) return '#f59e0b';
    if (score >= 40) return '#eab308';
    return '#16a34a';
  };

  const getOccupancyColor = (rate: number) => {
    if (rate >= 85) return '#dc2626';
    if (rate >= 70) return '#f59e0b';
    if (rate >= 50) return '#eab308';
    return '#16a34a';
  };

  return (
    <View style={styles.card}>
      <Text style={styles.date}>{formatDate(date)}</Text>
      
      <View style={styles.metricsRow}>
        <View style={styles.metric}>
          <Text style={styles.metricLabel}>Talep Skoru</Text>
          <Text style={[styles.metricValue, { color: getDemandColor(demandScore) }]}>
            {demandScore}/100
          </Text>
        </View>
        <View style={styles.metric}>
          <Text style={styles.metricLabel}>Doluluk Oranı</Text>
          <Text style={[styles.metricValue, { color: getOccupancyColor(occupancyRate) }]}>
            %{occupancyRate}
          </Text>
        </View>
      </View>

      <View style={styles.priceRow}>
        <Text style={styles.basePriceLabel}>Baz Fiyat</Text>
        <Text style={styles.basePrice}>€{basePrice.toFixed(2)}</Text>
      </View>

      {breakdowns.map((item, index) => (
        <View key={index} style={styles.breakdownRow}>
          <View style={styles.breakdownLeft}>
            <Text style={styles.factor}>{item.factor}</Text>
            <Text style={styles.description}>{item.description}</Text>
          </View>
          <View style={styles.breakdownRight}>
            <Text style={styles.multiplier}>x{item.multiplier}</Text>
            <Text style={[
              styles.impact,
              item.impact > 0 ? styles.positive : styles.negative
            ]}>
              {item.impact > 0 ? '+' : ''}€{Math.abs(item.impact).toFixed(2)}
            </Text>
          </View>
        </View>
      ))}

      <View style={styles.finalPriceRow}>
        <Text style={styles.finalLabel}>Gecelik Fiyat</Text>
        <Text style={styles.finalValue}>€{finalPrice.toFixed(2)}</Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  card: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginHorizontal: 16,
    marginVertical: 8,
    elevation: 3,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.1,
    shadowRadius: 4,
  },
  date: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
    textAlign: 'center',
  },
  metricsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    backgroundColor: '#f5f5f5',
    borderRadius: 8,
    padding: 12,
    marginBottom: 12,
  },
  metric: {
    alignItems: 'center',
  },
  metricLabel: {
    fontSize: 12,
    color: '#888',
    marginBottom: 4,
  },
  metricValue: {
    fontSize: 16,
    fontWeight: 'bold',
  },
  priceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  basePriceLabel: {
    fontSize: 14,
    color: '#666',
  },
  basePrice: {
    fontSize: 14,
    color: '#666',
  },
  breakdownRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
  },
  breakdownLeft: {
    flex: 1,
  },
  factor: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  description: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  breakdownRight: {
    alignItems: 'flex-end',
  },
  multiplier: {
    fontSize: 12,
    color: '#666',
  },
  impact: {
    fontSize: 13,
    fontWeight: '500',
    marginTop: 2,
  },
  positive: {
    color: '#dc2626',
  },
  negative: {
    color: '#16a34a',
  },
  finalPriceRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginTop: 12,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#e0e0e0',
  },
  finalLabel: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  finalValue: {
    fontSize: 22,
    fontWeight: 'bold',
    color: '#007AFF',
  },
});