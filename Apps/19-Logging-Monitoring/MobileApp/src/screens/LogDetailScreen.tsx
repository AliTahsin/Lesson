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
import { LevelBadge } from '../components/LevelBadge';
import { LogEntry } from '../types/logging';

export const LogDetailScreen = ({ route }: any) => {
  const { logId } = route.params;
  const [log, setLog] = useState<LogEntry | null>(null);
  const [loading, setLoading] = useState(true);
  const [expanded, setExpanded] = useState(false);

  useEffect(() => {
    loadLog();
  }, []);

  const loadLog = async () => {
    setLoading(true);
    try {
      const data = await loggingApi.getLogById(logId);
      setLog(data);
    } catch (error) {
      console.error('Error loading log:', error);
      Alert.alert('Hata', 'Log detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const formatDateTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}:${date.getSeconds().toString().padStart(2, '0')}`;
  };

  const copyToClipboard = (text: string) => {
    // Implement copy functionality
    Alert.alert('Kopyalandı', 'Log panoya kopyalandı');
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!log) {
    return (
      <View style={styles.center}>
        <Text>Log bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <View style={styles.headerRow}>
          <LevelBadge level={log.level} size="medium" />
          <Text style={styles.time}>{formatDateTime(log.timestamp)}</Text>
        </View>
        <Text style={styles.message}>{log.message}</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Detaylar</Text>
        
        {log.sourceContext && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Kaynak:</Text>
            <Text style={styles.infoValue}>{log.sourceContext}</Text>
          </View>
        )}
        
        {log.requestPath && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Endpoint:</Text>
            <Text style={styles.infoValue}>{log.requestMethod} {log.requestPath}</Text>
          </View>
        )}
        
        {log.statusCode && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Status:</Text>
            <Text style={[styles.infoValue, log.statusCode >= 400 ? styles.errorValue : styles.successValue]}>
              {log.statusCode}
            </Text>
          </View>
        )}
        
        {log.durationMs && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Süre:</Text>
            <Text style={styles.infoValue}>{log.durationMs} ms</Text>
          </View>
        )}
        
        {log.userId && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Kullanıcı:</Text>
            <Text style={styles.infoValue}>{log.userId}</Text>
          </View>
        )}
        
        {log.correlationId && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Correlation ID:</Text>
            <TouchableOpacity onPress={() => copyToClipboard(log.correlationId!)}>
              <Text style={styles.infoValueLink}>{log.correlationId}</Text>
            </TouchableOpacity>
          </View>
        )}
        
        {log.ipAddress && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>IP Adresi:</Text>
            <Text style={styles.infoValue}>{log.ipAddress}</Text>
          </View>
        )}
      </View>

      {log.exception && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Hata Detayı</Text>
          <Text style={[styles.exception, expanded && styles.exceptionExpanded]} numberOfLines={expanded ? undefined : 5}>
            {log.exception}
          </Text>
          <TouchableOpacity onPress={() => setExpanded(!expanded)}>
            <Text style={styles.expandButton}>
              {expanded ? 'Daha az göster' : 'Devamını göster'}
            </Text>
          </TouchableOpacity>
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
  },
  headerRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  time: {
    fontSize: 12,
    color: '#888',
  },
  message: {
    fontSize: 15,
    color: '#333',
    lineHeight: 22,
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
  errorValue: {
    color: '#ef4444',
    fontWeight: 'bold',
  },
  successValue: {
    color: '#10b981',
    fontWeight: 'bold',
  },
  exception: {
    fontSize: 12,
    color: '#666',
    fontFamily: 'monospace',
    lineHeight: 18,
  },
  exceptionExpanded: {
    maxHeight: '100%',
  },
  expandButton: {
    color: '#007AFF',
    fontSize: 12,
    marginTop: 8,
  },
});