import React from 'react';
import { View, Text, Dimensions, StyleSheet } from 'react-native';
import { LineChart } from 'victory-native';
import { DailyReservation } from '../types/reporting';

const { width } = Dimensions.get('window');

interface Props {
  data: DailyReservation[];
}

export const ReservationChart: React.FC<Props> = ({ data }) => {
  const confirmedData = data.map((item, index) => ({
    x: index,
    y: item.confirmedReservations
  }));
  
  const cancelledData = data.map((item, index) => ({
    x: index,
    y: item.cancelledReservations
  }));

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Günlük Rezervasyonlar</Text>
      <LineChart
        data={[confirmedData, cancelledData]}
        width={width - 40}
        height={220}
        padding={{ left: 50, top: 20, right: 30, bottom: 30 }}
        style={{
          labels: { fontSize: 10, fill: '#666' },
          data: {
            stroke: ['#10b981', '#ef4444'],
            strokeWidth: 2
          }
        }}
      />
      <View style={styles.legend}>
        <View style={styles.legendItem}>
          <View style={[styles.legendColor, { backgroundColor: '#10b981' }]} />
          <Text style={styles.legendText}>Onaylanan</Text>
        </View>
        <View style={styles.legendItem}>
          <View style={[styles.legendColor, { backgroundColor: '#ef4444' }]} />
          <Text style={styles.legendText}>İptal Edilen</Text>
        </View>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginHorizontal: 16,
    marginVertical: 8,
    alignItems: 'center',
  },
  title: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
    alignSelf: 'flex-start',
  },
  legend: {
    flexDirection: 'row',
    justifyContent: 'center',
    marginTop: 12,
  },
  legendItem: {
    flexDirection: 'row',
    alignItems: 'center',
    marginHorizontal: 12,
  },
  legendColor: {
    width: 12,
    height: 12,
    borderRadius: 6,
    marginRight: 6,
  },
  legendText: {
    fontSize: 11,
    color: '#666',
  },
});