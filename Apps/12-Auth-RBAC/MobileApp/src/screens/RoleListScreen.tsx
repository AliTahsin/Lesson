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
import { authApi } from '../services/authApi';
import { useAuth } from '../context/AuthContext';
import { Role, Permission } from '../types/auth';

export const RoleListScreen = ({ navigation }: any) => {
  const [roles, setRoles] = useState<Role[]>([]);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedRole, setSelectedRole] = useState<Role | null>(null);
  const [newRoleName, setNewRoleName] = useState('');
  const [newRoleDescription, setNewRoleDescription] = useState('');
  const { hasPermission } = useAuth();

  const canManage = hasPermission('role:manage');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [rolesData, permissionsData] = await Promise.all([
        authApi.getRoles(),
        authApi.getPermissions()
      ]);
      setRoles(rolesData);
      setPermissions(permissionsData);
    } catch (error) {
      console.error('Error loading roles:', error);
      Alert.alert('Hata', 'Roller yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadData();
  };

  const handleCreateRole = async () => {
    if (!newRoleName) {
      Alert.alert('Hata', 'Rol adı gerekli');
      return;
    }

    try {
      await authApi.createRole({
        name: newRoleName,
        description: newRoleDescription,
        level: 'Staff',
        permissionIds: []
      });
      setModalVisible(false);
      setNewRoleName('');
      setNewRoleDescription('');
      loadData();
      Alert.alert('Başarılı', 'Rol oluşturuldu');
    } catch (error) {
      Alert.alert('Hata', 'Rol oluşturulamadı');
    }
  };

  const handleDeleteRole = async (role: Role) => {
    Alert.alert(
      'Rol Sil',
      `${role.name} rolünü silmek istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sil',
          style: 'destructive',
          onPress: async () => {
            try {
              await authApi.deleteRole(role.id);
              loadData();
            } catch (error) {
              Alert.alert('Hata', 'Rol silinemedi');
            }
          }
        }
      ]
    );
  };

  const getLevelColor = (level: string) => {
    switch (level) {
      case 'SuperAdmin': return '#ef4444';
      case 'Admin': return '#f59e0b';
      case 'Manager': return '#3b82f6';
      case 'Staff': return '#10b981';
      case 'Guest': return '#6b7280';
      default: return '#8b5cf6';
    }
  };

  const renderRoleCard = ({ item }: { item: Role }) => (
    <TouchableOpacity
      style={styles.roleCard}
      onPress={() => navigation.navigate('PermissionList', { roleId: item.id, roleName: item.name })}
    >
      <View style={styles.roleHeader}>
        <View>
          <Text style={styles.roleName}>{item.name}</Text>
          <Text style={styles.roleDescription}>{item.description}</Text>
        </View>
        <View style={[styles.levelBadge, { backgroundColor: getLevelColor(item.level) }]}>
          <Text style={styles.levelText}>{item.level}</Text>
        </View>
      </View>

      <View style={styles.roleFooter}>
        <Text style={styles.permissionCount}>
          {item.permissions?.length || 0} izin
        </Text>
        {canManage && !item.isDefault && (
          <TouchableOpacity onPress={() => handleDeleteRole(item)}>
            <Icon name="trash-outline" size={20} color="#ef4444" />
          </TouchableOpacity>
        )}
      </View>
    </TouchableOpacity>
  );

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        data={roles}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderRoleCard}
        refreshControl={
          <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
        }
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>Rol bulunamadı</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />

      {canManage && (
        <TouchableOpacity
          style={styles.fab}
          onPress={() => setModalVisible(true)}
        >
          <Icon name="add" size={28} color="white" />
        </TouchableOpacity>
      )}

      <Modal visible={modalVisible} transparent animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Yeni Rol Oluştur</Text>
            
            <TextInput
              style={styles.modalInput}
              placeholder="Rol Adı"
              value={newRoleName}
              onChangeText={setNewRoleName}
            />
            
            <TextInput
              style={[styles.modalInput, styles.textArea]}
              placeholder="Açıklama"
              value={newRoleDescription}
              onChangeText={setNewRoleDescription}
              multiline
              numberOfLines={3}
            />

            <View style={styles.modalButtons}>
              <TouchableOpacity
                style={[styles.modalButton, styles.cancelModalButton]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.cancelModalButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.modalButton, styles.createModalButton]}
                onPress={handleCreateRole}
              >
                <Text style={styles.createModalButtonText}>Oluştur</Text>
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
  listContent: {
    paddingBottom: 80,
  },
  roleCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    padding: 16,
    elevation: 2,
  },
  roleHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'flex-start',
    marginBottom: 12,
  },
  roleName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  roleDescription: {
    fontSize: 12,
    color: '#666',
    marginTop: 4,
  },
  levelBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  levelText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  roleFooter: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  permissionCount: {
    fontSize: 12,
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
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    backgroundColor: 'white',
    borderRadius: 16,
    padding: 20,
    width: '85%',
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 16,
  },
  modalInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
    marginBottom: 12,
  },
  textArea: {
    minHeight: 80,
    textAlignVertical: 'top',
  },
  modalButtons: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 8,
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
  createModalButton: {
    backgroundColor: '#007AFF',
  },
  cancelModalButtonText: {
    color: '#666',
  },
  createModalButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});