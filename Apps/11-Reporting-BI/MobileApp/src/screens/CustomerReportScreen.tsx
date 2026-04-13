import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { reportApi } from '../services/reportApi';
import { CustomerReport, TopCustomer } from '../types/reporting';

export const CustomerReportScreen = ({ route }: any) => {
  const { hotelId, startDate, endDate } = route.params;
  const [report, setReport] = useState<CustomerReport | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadReport();
  }, []);

  const loadReport = async () => {
    setLoading(true);
    try {
      const data = await reportApi.getCustomerReport(hotelId, startDate, endDate);
      setReport(data);
    } catch (error) {
      console.error('Error loading report:', error);
      Alert.alert('Hata', 'Rapor yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleExport = async () => {
    try {
      const filePath = await reportApi.exportToExcel({
        hotelId,
        reportType: 'Customer',
        startDate,
        endDate,
        format: 'Excel'
      });
      await reportApi.downloadAndShare(filePath);
    } catch (error) {
      Alert.alert('Hata', 'Rapor indirilemedi');
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!report) {
    return (
      <View style={styles.center}>
        <Text>Rapor bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.hotelName}>{report.hotelName}</Text>
        <Text style={styles.period}>
          {new Date(report.startDate).toLocaleDateString('tr-TR')} - {new Date(report.endDate).toLocaleDateString('tr-TR')}
        </Text>
      </View>

      <View style={styles.summaryCard}>
        <Text style={styles.summaryTitle}>Özet</Text>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Toplam Müşteri</Text>
          <Text style={styles.summaryValue}>{report.totalCustomers}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Yeni Müşteri</Text>
          <Text style={styles.summaryValue}>{report.newCustomers}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Tekrar Eden</Text>
          <Text style={styles.summaryValue}>{report.repeatCustomers}</Text>
        </View>
        <View style={styles.summaryRow}>
          <Text style={styles.summaryLabel}>Müşteri Memnuniyeti</Text>
          <Text style={styles.summaryValue}>{report.customerSatisfactionScore.toFixed(1)}%</Text>
        </View>
      </View>

      <View style={styles.tableCard}>
        <Text style={styles.tableTitle}>En Çok Harcama Yapan 10 Müşteri</Text>
        <View style={styles.tableHeader}>
          <Text style={[styles.tableCell, styles.nameCol]}>Müşteri</Text>
          <Text style={[styles.tableCell, styles.staysCol]}>Konaklama</Text>
          <Text style={[styles.tableCell, styles.spentCol]}>Harcama</Text>
        </View>
        {report.topCustomers.map((customer: TopCustomer, index: number) => (
          <View key={index} style={styles.tableRow}>
            <Text style={[styles.tableCell, styles.nameCol]}>{customer.customerName}</Text>
            <Text style={[styles.tableCell, styles.staysCol]}>{customer.totalStays}</Text>
            <Text style={[styles.tableCell, styles.spentCol]}>€{customer.totalSpent.toLocaleString()}</Text>
          </View>
        ))}
      </View>

      <TouchableOpacity style={styles.exportButton} onPress={handleExport}>
        <Icon name="download" size={20} color="white" />
        <Text style={styles.exportButtonText}>Excel Olarak İndir</Text>
      </TouchableOpacity>
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
    backgroundColor: '#f59e0b',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  hotelName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
  },
  period: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  summaryCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  summaryTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  summaryRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginBottom: 8,
  },
  summaryLabel: {
    fontSize: 14,
    color: '#666',
  },
  summaryValue: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  tableCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  tableTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  tableHeader: {
    flexDirection: 'row',
    paddingBottom: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  tableRow: {
    flexDirection: 'row',
    paddingVertical: 8,
    borderBottomWidth: 1,
    borderBottomColor: '#f5f5f5',
  },
  tableCell: {
    fontSize: 12,
    color: '#555',
  },
  nameCol: {
    width: '50%',
  },
  staysCol: {
    width: '25%',
    textAlign: 'right',
  },
  spentCol: {
    width: '25%',
    textAlign: 'right',
  },
  exportButton: {
    backgroundColor: '#10b981',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  exportButtonText: {
    color: 'white',
    fontSize: 14,
    fontWeight: 'bold',
  },
});