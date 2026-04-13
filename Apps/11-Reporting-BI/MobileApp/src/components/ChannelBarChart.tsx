import React from 'react';
import { View, Text, Dimensions, StyleSheet } from 'react-native';
import { BarChart } from 'victory-native';
import { ChannelPerformance } from '../types/reporting';

const { width } = Dimensions.get('window');

interface Props {
  data: ChannelPerformance[];
}

export const ChannelBarChart: React.FC<Props> = ({ data }) => {
  const chartData = data.map((item, index) => ({
    x: index,
    y: item.revenue,
    label: `€${item.revenue.toFixed(0)}`
  }));

  return (
    <View style={styles.container}>
      <Text style={styles.title}>Kanal Bazında Gelir</Text>
      <BarChart
        data={chartData}
        width={width - 40}
        height={220}
        padding={{ left: 50, top: 20, right: 30, bottom: 50 }}
        style={{
          labels: { fontSize: 10, fill: '#666', angle: -45 },
          data: { fill: '#8b5cf6' }
        }}
      />
      <View style={styles.labels}>
        {data.map((item, index) => (
          <Text key={index} style={styles.label}>
            {item.channelName.substring(0, 8)}
          </Text>
        ))}
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
  labels: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    width: '100%',
    marginTop: 8,
  },
  label: {
    fontSize: 10,
    color: '#666',
    textAlign: 'center',
  },
});