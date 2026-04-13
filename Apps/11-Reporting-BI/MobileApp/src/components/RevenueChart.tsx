import React from 'react';
import { View, Text, Dimensions, StyleSheet } from 'react-native';
import { LineChart } from 'victory-native';
import { DailyRevenue } from '../types/reporting';

const { width } = Dimensions.get('window');

interface Props {
  data: DailyRevenue[];
}

export const RevenueChart: React.FC<Props> = ({ data }) => {
  const chartData = data.map((item, index) => ({
    x: index,
    y: item.revenue,
    label: new Date(item.date).getDate().toString()
  }));

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Günlük Gelir Trendi</Text>
      <LineChart
        data={chartData}
        width={width - 40}
        height={220}
        padding={{ left: 50, top: 20, right: 30, bottom: 30 }}
        xAxisLabel="Gün"
        yAxisLabel="€"
        style={{
          labels: { fontSize: 10, fill: '#666' }
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