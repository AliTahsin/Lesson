import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import DatePicker from 'react-native-datepicker';
import Icon from 'react-native-vector-icons/Ionicons';
import { LineChart } from 'victory-native';
import { aiApi } from '../services/aiApi';
import { DemandPrediction, RevenuePrediction, OccupancyPrediction } from '../types/ai';

export const PredictionScreen = () => {
  const [hotelId, setHotelId] = useState(1);
  const [date, setDate] = useState('');
  const [startDate, setStartDate] = useState('');
  const [endDate, setEndDate] = useState('');
  const [loading, setLoading] = useState(false);
  const [demandPrediction, setDemandPrediction] = useState<DemandPrediction | null>(null);
  const [revenuePrediction, setRevenuePrediction] = useState<RevenuePrediction | null>(null);
  const [occupancyPrediction, setOccupancyPrediction] = useState<OccupancyPrediction | null>(null);
  const [activeTab, setActiveTab] = useState<'demand' | 'revenue' | 'occupancy'>('demand');

  const hotels = [
    { id: 1, name: 'Marriott Istanbul' },
    { id: 2, name: 'Hilton Izmir' },
    { id: 3, name: 'Sofitel Bodrum' }
  ];

  const loadDemandPrediction = async () => {
    if (!date) {
      Alert.alert('Uyarı', 'Lütfen tarih seçin');
      return;
    }

    setLoading(true);
    try {
      const data = await aiApi.predictDemand(hotelId, date);
      setDemandPrediction(data);
    } catch (error) {
      Alert.alert('Hata', 'Tahmin yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const loadRevenuePrediction = async () => {
    if (!startDate || !endDate) {
      Alert.alert('Uyarı', 'Lütfen tarih aralığı seçin');
      return;
    }

    setLoading(true);
    try {
      const data = await aiApi.predictRevenue(hotelId, startDate, endDate);
      setRevenuePrediction(data);
    } catch (error) {
      Alert.alert('Hata', 'Tahmin yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const loadOccupancyPrediction = async () => {
    if (!date) {
      Alert.alert('Uyarı', 'Lütfen tarih seçin');
      return;
    }

    setLoading(true);
    try {
      const data = await aiApi.predictOccupancy(hotelId, date);
      setOccupancyPrediction(data);
    } catch (error) {
      Alert.alert('Hata', 'Tahmin yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handlePredict = () => {
    if (activeTab === 'demand') loadDemandPrediction();
    else if (activeTab === 'revenue') loadRevenuePrediction();
    else loadOccupancyPrediction();
  };

  const getConfidenceColor = (confidence: number) => {
    if (confidence >= 0.8) return '#10b981';
    if (confidence >= 0.6) return '#f59e0b';
    return '#ef4444';
  };

  const chartData = revenuePrediction?.dailyPredictions.map((item, index) => ({
    x: index,
    y: item.predictedRevenue,
    label: new Date(item.date).getDate().toString()
  })) || [];

  return (
    <ScrollView style={styles.container}>
      <View style={styles.hotelSelector}>
        <Text style={styles.label}>Otel Seçin</Text>
        <View style={styles.hotelButtons}>
          {hotels.map(hotel => (
            <TouchableOpacity
              key={hotel.id}
              style={[styles.hotelButton, hotelId === hotel.id && styles.hotelButtonActive]}
              onPress={() => setHotelId(hotel.id)}
            >
              <Text style={[styles.hotelButtonText, hotelId === hotel.id && styles.hotelButtonTextActive]}>
                {hotel.name.split(' ')[0]}
              </Text>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      <View style={styles.tabSelector}>
        <TouchableOpacity
          style={[styles.tab, activeTab === 'demand' && styles.tabActive]}
          onPress={() => setActiveTab('demand')}
        >
          <Text style={[styles.tabText, activeTab === 'demand' && styles.tabTextActive]}>Talep</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.tab, activeTab === 'revenue' && styles.tabActive]}
          onPress={() => setActiveTab('revenue')}
        >
          <Text style={[styles.tabText, activeTab === 'revenue' && styles.tabTextActive]}>Gelir</Text>
        </TouchableOpacity>
        <TouchableOpacity
          style={[styles.tab, activeTab === 'occupancy' && styles.tabActive]}
          onPress={() => setActiveTab('occupancy')}
        >
          <Text style={[styles.tabText, activeTab === 'occupancy' && styles.tabTextActive]}>Doluluk</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.inputCard}>
        {activeTab === 'demand' && (
          <View style={styles.dateField}>
            <Text style={styles.label}>Tarih</Text>
            <DatePicker
              style={styles.datePicker}
              date={date}
              mode="date"
              placeholder="Seçiniz"
              format="YYYY-MM-DD"
              confirmBtnText="Tamam"
              cancelBtnText="İptal"
              onDateChange={(d) => setDate(d)}
            />
          </View>
        )}

        {activeTab === 'revenue' && (
          <View style={styles.dateRange}>
            <View style={styles.dateField}>
              <Text style={styles.label}>Başlangıç</Text>
              <DatePicker
                style={styles.datePicker}
                date={startDate}
                mode="date"
                placeholder="Seçiniz"
                format="YYYY-MM-DD"
                confirmBtnText="Tamam"
                cancelBtnText="İptal"
                onDateChange={(d) => setStartDate(d)}
              />
            </View>
            <View style={styles.dateField}>
              <Text style={styles.label}>Bitiş</Text>
              <DatePicker
                style={styles.datePicker}
                date={endDate}
                mode="date"
                placeholder="Seçiniz"
                format="YYYY-MM-DD"
                confirmBtnText="Tamam"
                cancelBtnText="İptal"
                onDateChange={(d) => setEndDate(d)}
              />
            </View>
          </View>
        )}

        {activeTab === 'occupancy' && (
          <View style={styles.dateField}>
            <Text style={styles.label}>Tarih</Text>
            <DatePicker
              style={styles.datePicker}
              date={date}
              mode="date"
              placeholder="Seçiniz"
              format="YYYY-MM-DD"
              confirmBtnText="Tamam"
              cancelBtnText="İptal"
              onDateChange={(d) => setDate(d)}
            />
          </View>
        )}

        <TouchableOpacity style={styles.predictButton} onPress={handlePredict}>
          <Text style={styles.predictButtonText}>Tahmin Et</Text>
        </TouchableOpacity>
      </View>

      {loading && (
        <View style={styles.loader}>
          <ActivityIndicator size="large" />
        </View>
      )}

      {demandPrediction && activeTab === 'demand' && (
        <View style={styles.resultCard}>
          <Text style={styles.resultTitle}>Talep Tahmini</Text>
          <Text style={styles.resultValue}>{demandPrediction.predictedDemand}</Text>
          <Text style={styles.resultUnit}>rezervasyon</Text>
          
          <View style={styles.confidenceContainer}>
            <Text style={styles.confidenceLabel}>Güven Seviyesi</Text>
            <View style={styles.confidenceBar}>
              <View style={[styles.confidenceFill, { width: `${demandPrediction.confidence * 100}%`, backgroundColor: getConfidenceColor(demandPrediction.confidence) }]} />
            </View>
            <Text style={styles.confidenceText}>%{(demandPrediction.confidence * 100).toFixed(0)}</Text>
          </View>

          <Text style={styles.factorsTitle}>Etkileyen Faktörler</Text>
          {demandPrediction.factors.map((factor, index) => (
            <View key={index} style={styles.factorItem}>
              <Icon name="checkmark-circle" size={16} color="#10b981" />
              <Text style={styles.factorText}>{factor}</Text>
            </View>
          ))}
        </View>
      )}

      {revenuePrediction && activeTab === 'revenue' && (
        <View style={styles.resultCard}>
          <Text style={styles.resultTitle}>Gelir Tahmini</Text>
          <Text style={styles.resultValue}>€{revenuePrediction.predictedRevenue.toLocaleString()}</Text>
          <Text style={styles.resultUnit}>{revenuePrediction.dailyPredictions.length} günlük tahmin</Text>
          
          <View style={styles.chartContainer}>
            <LineChart
              data={chartData}
              width={300}
              height={200}
              padding={{ left: 40, top: 20, right: 20, bottom: 30 }}
              style={{
                labels: { fontSize: 10, fill: '#666' }
              }}
            />
          </View>
          
          <View style={styles.confidenceContainer}>
            <Text style={styles.confidenceLabel}>Güven Seviyesi</Text>
            <View style={styles.confidenceBar}>
              <View style={[styles.confidenceFill, { width: `${revenuePrediction.confidence * 100}%`, backgroundColor: getConfidenceColor(revenuePrediction.confidence) }]} />
            </View>
            <Text style={styles.confidenceText}>%{(revenuePrediction.confidence * 100).toFixed(0)}</Text>
          </View>
        </View>
      )}

      {occupancyPrediction && activeTab === 'occupancy' && (
        <View style={styles.resultCard}>
          <Text style={styles.resultTitle}>Doluluk Tahmini</Text>
          <Text style={styles.resultValue}>{occupancyPrediction.predictedOccupancyRate.toFixed(1)}%</Text>
          <Text style={styles.resultUnit}>doluluk oranı</Text>
          
          <View style={styles.occupancyDetails}>
            <View style={styles.occupancyItem}>
              <Text style={styles.occupancyLabel}>Tahmini Satılan Oda</Text>
              <Text style={styles.occupancyValue}>{occupancyPrediction.predictedSoldRooms}</Text>
            </View>
            <View style={styles.occupancyItem}>
              <Text style={styles.occupancyLabel}>Toplam Oda</Text>
              <Text style={styles.occupancyValue}>{occupancyPrediction.totalRooms}</Text>
            </View>
          </View>
          
          <View style={styles.confidenceContainer}>
            <Text style={styles.confidenceLabel}>Güven Seviyesi</Text>
            <View style={styles.confidenceBar}>
              <View style={[styles.confidenceFill, { width: `${occupancyPrediction.confidence * 100}%`, backgroundColor: getConfidenceColor(occupancyPrediction.confidence) }]} />
            </View>
            <Text style={styles.confidenceText}>%{(occupancyPrediction.confidence * 100).toFixed(0)}</Text>
          </View>
        </View>
      )}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  hotelSelector: {
    backgroundColor: 'white',
    margin: 16,
    padding: 16,
    borderRadius: 12,
  },
  label: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
  },
  hotelButtons: {
    flexDirection: 'row',
    gap: 8,
  },
  hotelButton: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
    backgroundColor: '#f0f0f0',
  },
  hotelButtonActive: {
    backgroundColor: '#007AFF',
  },
  hotelButtonText: {
    color: '#333',
  },
  hotelButtonTextActive: {
    color: 'white',
  },
  tabSelector: {
    flexDirection: 'row',
    marginHorizontal: 16,
    marginBottom: 12,
  },
  tab: {
    flex: 1,
    paddingVertical: 10,
    alignItems: 'center',
    borderBottomWidth: 2,
    borderBottomColor: 'transparent',
  },
  tabActive: {
    borderBottomColor: '#007AFF',
  },
  tabText: {
    color: '#666',
    fontWeight: '500',
  },
  tabTextActive: {
    color: '#007AFF',
  },
  inputCard: {
    backgroundColor: 'white',
    marginHorizontal: 16,
    marginBottom: 16,
    padding: 16,
    borderRadius: 12,
  },
  dateField: {
    marginBottom: 16,
  },
  dateRange: {
    flexDirection: 'row',
    gap: 12,
    marginBottom: 16,
  },
  datePicker: {
    width: '100%',
  },
  predictButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 14,
    alignItems: 'center',
  },
  predictButtonText: {
    color: 'white',
    fontWeight: 'bold',
    fontSize: 16,
  },
  loader: {
    marginVertical: 20,
  },
  resultCard: {
    backgroundColor: 'white',
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 20,
    borderRadius: 12,
    alignItems: 'center',
  },
  resultTitle: {
    fontSize: 14,
    color: '#888',
    marginBottom: 8,
  },
  resultValue: {
    fontSize: 48,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  resultUnit: {
    fontSize: 14,
    color: '#888',
    marginTop: 4,
  },
  confidenceContainer: {
    width: '100%',
    marginTop: 20,
  },
  confidenceLabel: {
    fontSize: 12,
    color: '#666',
    marginBottom: 4,
  },
  confidenceBar: {
    height: 6,
    backgroundColor: '#e0e0e0',
    borderRadius: 3,
    overflow: 'hidden',
  },
  confidenceFill: {
    height: '100%',
    borderRadius: 3,
  },
  confidenceText: {
    fontSize: 11,
    color: '#666',
    marginTop: 4,
    textAlign: 'right',
  },
  factorsTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 20,
    marginBottom: 12,
    alignSelf: 'flex-start',
  },
  factorItem: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
    marginBottom: 8,
    alignSelf: 'flex-start',
  },
  factorText: {
    fontSize: 13,
    color: '#555',
  },
  chartContainer: {
    marginVertical: 20,
    alignItems: 'center',
  },
  occupancyDetails: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    width: '100%',
    marginVertical: 20,
  },
  occupancyItem: {
    alignItems: 'center',
  },
  occupancyLabel: {
    fontSize: 12,
    color: '#888',
  },
  occupancyValue: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 4,
  },
});