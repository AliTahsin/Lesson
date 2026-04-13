import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert,
  TextInput
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { MaintenanceIssue } from '../types/staff';
import { useStaffAuthContext } from '../context/StaffAuthContext';

export const IssueDetailScreen = ({ route, navigation }: any) => {
  const { issueId } = route.params;
  const { role } = useStaffAuthContext();
  const [issue, setIssue] = useState<MaintenanceIssue | null>(null);
  const [loading, setLoading] = useState(true);
  const [resolutionNotes, setResolutionNotes] = useState('');
  const [actualCost, setActualCost] = useState('');
  const [resolving, setResolving] = useState(false);

  useEffect(() => {
    loadIssue();
  }, []);

  const loadIssue = async () => {
    setLoading(true);
    try {
      const data = await staffApi.getIssueById(issueId);
      setIssue(data);
    } catch (error) {
      console.error('Error loading issue:', error);
      Alert.alert('Hata', 'Arıza detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleStart = async () => {
    try {
      await staffApi.startIssue(issueId);
      Alert.alert('Başarılı', 'Arıza üzerinde çalışma başlatıldı');
      loadIssue();
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    }
  };

  const handleResolve = async () => {
    if (!resolutionNotes) {
      Alert.alert('Uyarı', 'Lütfen çözüm notunu girin');
      return;
    }

    setResolving(true);
    try {
      await staffApi.resolveIssue(issueId, resolutionNotes, parseFloat(actualCost) || 0);
      Alert.alert('Başarılı', 'Arıza çözüldü olarak işaretlendi');
      navigation.goBack();
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    } finally {
      setResolving(false);
    }
  };

  const getPriorityColor = (priority: string) => {
    switch (priority) {
      case 'Critical': return '#ef4444';
      case 'High': return '#f59e0b';
      case 'Medium': return '#3b82f6';
      case 'Low': return '#10b981';
      default: return '#6b7280';
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Reported': return '#f59e0b';
      case 'Assigned': return '#3b82f6';
      case 'InProgress': return '#8b5cf6';
      case 'Resolved': return '#10b981';
      case 'Closed': return '#6b7280';
      default: return '#6b7280';
    }
  };

  const formatDateTime = (dateString?: string) => {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const canStart = issue?.status === 'Assigned' && issue.assignedToStaffName;
  const canResolve = issue?.status === 'InProgress';

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!issue) {
    return (
      <View style={styles.center}>
        <Text>Arıza bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <Text style={styles.issueNumber}>{issue.issueNumber}</Text>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(issue.status) }]}>
          <Text style={styles.statusText}>{issue.status}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Arıza Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Kategori:</Text>
          <Text style={styles.infoValue}>{issue.category}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Öncelik:</Text>
          <Text style={[styles.infoValue, { color: getPriorityColor(issue.priority), fontWeight: 'bold' }]}>
            {issue.priority}
          </Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Oda No:</Text>
          <Text style={styles.infoValue}>{issue.roomNumber}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Açıklama</Text>
        <Text style={styles.description}>{issue.description}</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Zaman Bilgileri</Text>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Bildirilen:</Text>
          <Text style={styles.infoValue}>{formatDateTime(issue.reportedAt)}</Text>
        </View>
        <View style={styles.infoRow}>
          <Text style={styles.infoLabel}>Bildiren:</Text>
          <Text style={styles.infoValue}>{issue.reportedByName}</Text>
        </View>
        {issue.assignedAt && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Atanan:</Text>
            <Text style={styles.infoValue}>{issue.assignedToStaffName}</Text>
          </View>
        )}
        {issue.resolvedAt && (
          <View style={styles.infoRow}>
            <Text style={styles.infoLabel}>Çözüldü:</Text>
            <Text style={styles.infoValue}>{formatDateTime(issue.resolvedAt)}</Text>
          </View>
        )}
      </View>

      {issue.resolutionNotes && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Çözüm Notu</Text>
          <Text style={styles.notes}>{issue.resolutionNotes}</Text>
        </View>
      )}

      {canResolve && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Çözüm Bilgileri</Text>
          <TextInput
            style={styles.notesInput}
            placeholder="Çözüm notu"
            value={resolutionNotes}
            onChangeText={setResolutionNotes}
            multiline
            numberOfLines={3}
          />
          <TextInput
            style={[styles.notesInput, styles.costInput]}
            placeholder="Gerçekleşen maliyet (€)"
            value={actualCost}
            onChangeText={setActualCost}
            keyboardType="numeric"
          />
        </View>
      )}

      {canStart && (
        <TouchableOpacity style={styles.startButton} onPress={handleStart}>
          <Text style={styles.startButtonText}>Arızayı Başlat</Text>
        </TouchableOpacity>
      )}

      {canResolve && (
        <TouchableOpacity
          style={[styles.resolveButton, resolving && styles.disabledButton]}
          onPress={handleResolve}
          disabled={resolving}
        >
          <Text style={styles.resolveButtonText}>
            {resolving ? 'Çözülüyor...' : 'Arızayı Çöz'}
          </Text>
        </TouchableOpacity>
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
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  issueNumber: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
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
    width: 90,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  description: {
    fontSize: 14,
    color: '#555',
    lineHeight: 20,
  },
  notes: {
    fontSize: 14,
    color: '#666',
    fontStyle: 'italic',
  },
  notesInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    padding: 12,
    fontSize: 14,
    textAlignVertical: 'top',
    minHeight: 80,
    marginBottom: 12,
  },
  costInput: {
    minHeight: 45,
  },
  startButton: {
    backgroundColor: '#3b82f6',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    alignItems: 'center',
  },
  startButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  resolveButton: {
    backgroundColor: '#10b981',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    paddingVertical: 14,
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  resolveButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});