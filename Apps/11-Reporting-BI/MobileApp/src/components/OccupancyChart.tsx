import React from 'react';
import { View, Text, Dimensions, StyleSheet } from 'react-native';
import { BarChart } from 'victory-native';
import { DailyOccupancy } from '../types/reporting';

const { width } = Dimensions.get('window');

interface Props {
  data: DailyOccupancy[];
}

export const OccupancyChart: React.FC<Props> = ({ data }) => {
  const chartData = data.map((item, index) => ({
    x: index,
    y: item.occupancyRate,
    label: `${item.occupancyRate.toFixed(1)}%`
  }));

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Günlük Doluluk Oranı</Text>
      <BarChart
        data={chartData}
        width={width - 40}
        height={220}
        padding={{ left: 50, top: 20, right: 30, bottom: 30 }}
        style={{
          labels: { fontSize: 10, fill: '#666' },
          data: { fill: '#007AFF' }
        }}
      />
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
});