import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  Alert,
  TouchableOpacity
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { loggingApi } from '../services/loggingApi';
import { Trace } from '../types/logging';

export const TraceDetailScreen = ({ route }: any) => {
  const { traceId } = route.params;
  const [trace, setTrace] = useState<Trace | null>(null);
  const [loading, setLoading] = useState(true);
  const [expandedTags, setExpandedTags] = useState(false);

  useEffect(() => {
    loadTrace();
  }, []);

  const loadTrace = async () => {
    setLoading(true);
    try {
      const data = await loggingApi.getTraceById(traceId);
      setTrace(data);
    } catch (error) {
      console.error('Error loading trace:', error);
      Alert.alert('Hata', 'Trace detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}.${date.getMilliseconds()}`;
  };

  const getDurationColor = (durationMs: number) => {
    if (durationMs < 200) return '#10b981';
    if (durationMs < 1000) return '#f59e0b';
    return '#ef4444';
  };

  const copyToClipboard = (text: string) => {
    Alert.alert('Kopyalandı', `${text} panoya kopyalandı`);
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!trace) {
    return (
      <View style={styles.center}>
        <Text>Trace bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.operationName}>{trace.operationName}</Text>
        <Text style={[styles.duration, { color: getDurationColor(trace.durationMs) }]}>
          {trace.durationMs} ms
        </Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Genel Bilgiler</Text>
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Trace ID:</Text>
          <TouchableOpacity onPress={() => copyToClipboard(trace.traceId)}>
            <Text style={styles.infoValueLink}>{trace.traceId.substring(0, 20)}...</Text>
          </TouchableOpacity>
        </View>
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Span ID:</Text>
          <Text style={styles.infoValue}>{trace.spanId.substring(0, 16)}...</Text>
        </View>
        
        {trace.parentSpanId && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Parent Span ID:</Text>
            <Text style={styles.infoValue}>{trace.parentSpanId.substring(0, 16)}...</Text>
          </View>
        )}
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Servis:</Text>
          <Text style={styles.infoValue}>{trace.service}</Text>
        </View>
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Endpoint:</Text>
          <Text style={styles.infoValue}>{trace.endpoint}</Text>
        </View>
        
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Başlangıç:</Text>
          <Text style={styles.infoValue}>{formatDateTime(trace.startTime)}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <View style={styles.tagsHeader}>
          <Text style={styles.sectionTitle}>Etiketler (Tags)</Text>
          <TouchableOpacity onPress={() => setExpandedTags(!expandedTags)}>
            <Text style={styles.expandButton}>
              {expandedTags ? 'Daralt' : 'Genişlet'}
            </Text>
          </TouchableOpacity>
        </View>
        
        <View style={styles.tagsContainer}>
          {Object.entries(trace.tags).slice(0, expandedTags ? undefined : 5).map(([key, value]) => (
            <View key={key} style={styles.tagRow}>
              <Text style={styles.tagKey}>{key}:</Text>
              <Text style={styles.tagValue}>{value}</Text>
            </View>
          ))}
        </View>
      </View>

      <View style={styles.timelineSection}>
        <Text style={styles.sectionTitle}>Zaman Çizelgesi</Text>
        <View style={styles.timelineBar}>
          <View style={[styles.timelineFill, { width: `${Math.min(100, (trace.durationMs / 5000) * 100)}%` }]} />
        </View>
        <Text style={styles.timelineText}>Toplam Süre: {trace.durationMs} ms</Text>
      </View>

      <View style={styles.relatedSection}>
        <Text style={styles.sectionTitle}>İlişkili Loglar</Text>
        <TouchableOpacity
          style={styles.viewLogsButton}
          onPress={() => Alert.alert('Bilgi', 'Bu trace ile ilişkili loglar gösterilecek')}
        >
          <Text style={styles.viewLogsButtonText}>İlişkili Logları Görüntüle</Text>
        </TouchableOpacity>
      </View>
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
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  operationName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  duration: {
    fontSize: 18,
    fontWeight: 'bold',
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 100,
    fontSize: 13,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 13,
    color: '#333',
  },
  infoValueLink: {
    flex: 1,
    fontSize: 13,
    color: '#007AFF',
    textDecorationLine: 'underline',
  },
  tagsHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  expandButton: {
    color: '#007AFF',
    fontSize: 12,
  },
  tagsContainer: {
    gap: 6,
  },
  tagRow: {
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  tagKey: {
    fontSize: 12,
    fontWeight: 'bold',
    color: '#666',
    marginRight: 6,
  },
  tagValue: {
    fontSize: 12,
    color: '#333',
    flex: 1,
  },
  timelineSection: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  timelineBar: {
    height: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 4,
    overflow: 'hidden',
    marginBottom: 8,
  },
  timelineFill: {
    height: '100%',
    backgroundColor: '#007AFF',
    borderRadius: 4,
  },
  timelineText: {
    fontSize: 12,
    color: '#666',
    textAlign: 'right',
  },
  relatedSection: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
  },
  viewLogsButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
  },
  viewLogsButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});