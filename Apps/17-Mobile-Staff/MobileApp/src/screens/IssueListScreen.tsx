import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  Alert,
  Modal,
  TextInput
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { IssueCard } from '../components/IssueCard';
import { useStaffAuthContext } from '../context/StaffAuthContext';
import { MaintenanceIssue } from '../types/staff';

export const IssueListScreen = ({ navigation }: any) => {
  const { role, hotelId, staff } = useStaffAuthContext();
  const [issues, setIssues] = useState<MaintenanceIssue[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [filter, setFilter] = useState<string>('my');
  const [modalVisible, setModalVisible] = useState(false);
  const [newIssue, setNewIssue] = useState({
    category: '',
    description: '',
    priority: 'Medium',
    roomNumber: '',
    roomId: 0
  });

  const isMaintenance = role === 'Maintenance' || role === 'Admin';

  useEffect(() => {
    loadIssues();
  }, [filter]);

  const loadIssues = async () => {
    setLoading(true);
    try {
      let data;
      if (filter === 'my') {
        data = await staffApi.getMyIssues();
      } else if (filter === 'critical') {
        data = await staffApi.getCriticalIssues();
      } else {
        data = await staffApi.getMyIssues();
      }
      setIssues(data);
    } catch (error) {
      console.error('Error loading issues:', error);
      Alert.alert('Hata', 'Arızalar yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadIssues();
  };

  const handleIssueAction = async (issueId: number, action: string) => {
    try {
      if (action === 'start') {
        await staffApi.startIssue(issueId);
        Alert.alert('Başarılı', 'Arıza üzerinde çalışma başlatıldı');
      } else if (action === 'resolve') {
        Alert.prompt(
          'Arıza Çözümü',
          'Çözüm notunu girin:',
          [
            { text: 'İptal', style: 'cancel' },
            {
              text: 'Tamam',
              onPress: async (resolutionNotes) => {
                Alert.prompt(
                  'Maliyet',
                  'Gerçekleşen maliyeti girin (€):',
                  [
                    { text: 'İptal', style: 'cancel' },
                    {
                      text: 'Tamam',
                      onPress: async (cost) => {
                        await staffApi.resolveIssue(issueId, resolutionNotes || '', parseFloat(cost) || 0);
                        Alert.alert('Başarılı', 'Arıza çözüldü olarak işaretlendi');
                        loadIssues();
                      }
                    }
                  ],
                  'plain-text',
                  undefined,
                  'number-pad'
                );
              }
            }
          ],
          'plain-text'
        );
      }
      loadIssues();
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    }
  };

  const handleReportIssue = async () => {
    if (!newIssue.category || !newIssue.description || !newIssue.roomNumber) {
      Alert.alert('Hata', 'Lütfen tüm alanları doldurun');
      return;
    }

    try {
      await staffApi.reportIssue({
        hotelId: hotelId || 0,
        roomId: newIssue.roomId || 1,
        roomNumber: newIssue.roomNumber,
        category: newIssue.category,
        description: newIssue.description,
        priority: newIssue.priority,
        reportedByStaffId: staff?.id,
        reportedByName: staff?.fullName || 'Staff'
      });
      Alert.alert('Başarılı', 'Arıza bildirildi');
      setModalVisible(false);
      setNewIssue({
        category: '',
        description: '',
        priority: 'Medium',
        roomNumber: '',
        roomId: 0
      });
      loadIssues();
    } catch (error) {
      Alert.alert('Hata', 'Arıza bildirilemedi');
    }
  };

  const categories = ['Plumbing', 'Electrical', 'HVAC', 'Furniture', 'Appliance'];
  const priorities = ['Critical', 'High', 'Medium', 'Low'];

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {isMaintenance && (
        <View style={styles.filterBar}>
          <TouchableOpacity
            style={[styles.filterButton, filter === 'my' && styles.filterButtonActive]}
            onPress={() => setFilter('my')}
          >
            <Text style={[styles.filterText, filter === 'my' && styles.filterTextActive]}>
              Arızalarım
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            style={[styles.filterButton, filter === 'critical' && styles.filterButtonActive]}
            onPress={() => setFilter('critical')}
          >
            <Text style={[styles.filterText, filter === 'critical' && styles.filterTextActive]}>
              Kritik Arızalar
            </Text>
          </TouchableOpacity>
        </View>
      )}

      <FlatList
        data={issues}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <IssueCard
            issue={item}
            onPress={(id) => navigation.navigate('IssueDetail', { issueId: id })}
            onAction={handleIssueAction}
            showActions={filter === 'my'}
          />
        )}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="construct-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Arıza bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      <TouchableOpacity
        style={styles.fab}
        onPress={() => setModalVisible(true)}
      >
        <Icon name="add" size={28} color="white" />
      </TouchableOpacity>

      <Modal visible={modalVisible} transparent animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Arıza Bildir</Text>

            <Text style={styles.modalLabel}>Kategori</Text>
            <View style={styles.categoryContainer}>
              {categories.map((cat) => (
                <TouchableOpacity
                  key={cat}
                  style={[styles.categoryButton, newIssue.category === cat && styles.categoryButtonActive]}
                  onPress={() => setNewIssue({ ...newIssue, category: cat })}
                >
                  <Text style={[styles.categoryText, newIssue.category === cat && styles.categoryTextActive]}>
                    {cat}
                  </Text>
                </TouchableOpacity>
              ))}
            </View>

            <Text style={styles.modalLabel}>Öncelik</Text>
            <View style={styles.priorityContainer}>
              {priorities.map((pri) => (
                <TouchableOpacity
                  key={pri}
                  style={[styles.priorityButton, newIssue.priority === pri && styles.priorityButtonActive]}
                  onPress={() => setNewIssue({ ...newIssue, priority: pri })}
                >
                  <Text style={[styles.priorityText, newIssue.priority === pri && styles.priorityTextActive]}>
                    {pri}
                  </Text>
                </TouchableOpacity>
              ))}
            </View>

            <Text style={styles.modalLabel}>Oda No</Text>
            <TextInput
              style={styles.modalInput}
              placeholder="Oda numarası"
              value={newIssue.roomNumber}
              onChangeText={(text) => setNewIssue({ ...newIssue, roomNumber: text })}
            />

            <Text style={styles.modalLabel}>Arıza Açıklaması</Text>
            <TextInput
              style={[styles.modalInput, styles.textArea]}
              placeholder="Arıza detaylarını girin"
              value={newIssue.description}
              onChangeText={(text) => setNewIssue({ ...newIssue, description: text })}
              multiline
              numberOfLines={4}
            />

            <View style={styles.modalButtons}>
              <TouchableOpacity
                style={[styles.modalButton, styles.cancelModalButton]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.cancelModalButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.modalButton, styles.submitModalButton]}
                onPress={handleReportIssue}
              >
                <Text style={styles.submitModalButtonText}>Bildir</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
    </View>
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
  filterBar: {
    flexDirection: 'row',
    backgroundColor: 'white',
    padding: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  filterButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    borderRadius: 8,
  },
  filterButtonActive: {
    backgroundColor: '#007AFF',
  },
  filterText: {
    color: '#666',
    fontWeight: '500',
  },
  filterTextActive: {
    color: 'white',
  },
  listContent: {
    paddingBottom: 80,
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
  },
  fab: {
    position: 'absolute',
    bottom: 20,
    right: 20,
    width: 56,
    height: 56,
    borderRadius: 28,
    backgroundColor: '#007AFF',
    alignItems: 'center',
    justifyContent: 'center',
    elevation: 5,
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'flex-end',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    backgroundColor: 'white',
    borderTopLeftRadius: 20,
    borderTopRightRadius: 20,
    padding: 20,
    maxHeight: '80%',
  },
  modalTitle: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 20,
  },
  modalLabel: {
    fontSize: 14,
    fontWeight: '500',
    color: '#555',
    marginBottom: 8,
    marginTop: 12,
  },
  categoryContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  categoryButton: {
    paddingHorizontal: 16,
    paddingVertical: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  categoryButtonActive: {
    backgroundColor: '#007AFF',
  },
  categoryText: {
    color: '#333',
  },
  categoryTextActive: {
    color: 'white',
  },
  priorityContainer: {
    flexDirection: 'row',
    gap: 8,
  },
  priorityButton: {
    flex: 1,
    paddingVertical: 8,
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
  },
  priorityButtonActive: {
    backgroundColor: '#ef4444',
  },
  priorityText: {
    color: '#333',
  },
  priorityTextActive: {
    color: 'white',
  },
  modalInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
  },
  textArea: {
    minHeight: 80,
    textAlignVertical: 'top',
  },
  modalButtons: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 20,
  },
  modalButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 8,
    alignItems: 'center',
  },
  cancelModalButton: {
    backgroundColor: '#f0f0f0',
  },
  submitModalButton: {
    backgroundColor: '#007AFF',
  },
  cancelModalButtonText: {
    color: '#666',
  },
  submitModalButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});