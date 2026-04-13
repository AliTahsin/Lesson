import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert,
  Modal,
  TextInput,
  Switch
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { authApi } from '../services/authApi';
import { useAuth } from '../context/AuthContext';
import { RoleBadge } from '../components/RoleBadge';
import { User, Role } from '../types/auth';

export const UserDetailScreen = ({ route, navigation }: any) => {
  const { userId } = route.params;
  const [user, setUser] = useState<User | null>(null);
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(true);
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedRoles, setSelectedRoles] = useState<number[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [editedUser, setEditedUser] = useState({
    firstName: '',
    lastName: '',
    phoneNumber: '',
    department: '',
    position: '',
  });
  const { hasPermission, user: currentUser } = useAuth();

  const canManage = hasPermission('user:manage');
  const isOwnProfile = currentUser?.id === userId;

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [userData, rolesData] = await Promise.all([
        authApi.getUserById(userId),
        authApi.getRoles()
      ]);
      setUser(userData);
      setRoles(rolesData);
      setSelectedRoles(userData.roles.map(r => {
        const role = rolesData.find(rl => rl.name === r);
        return role?.id || 0;
      }).filter(id => id !== 0));
      setEditedUser({
        firstName: userData.firstName,
        lastName: userData.lastName,
        phoneNumber: userData.phoneNumber,
        department: userData.department || '',
        position: userData.position || '',
      });
    } catch (error) {
      console.error('Error loading user:', error);
      Alert.alert('Hata', 'Kullanıcı bilgileri yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateProfile = async () => {
    try {
      await authApi.updateUser(userId, editedUser);
      const updatedUser = await authApi.getUserById(userId);
      setUser(updatedUser);
      setIsEditing(false);
      Alert.alert('Başarılı', 'Profil güncellendi');
    } catch (error) {
      Alert.alert('Hata', 'Profil güncellenemedi');
    }
  };

  const handleUpdateRoles = async () => {
    try {
      await authApi.updateUserRoles(userId, selectedRoles);
      const updatedUser = await authApi.getUserById(userId);
      setUser(updatedUser);
      setModalVisible(false);
      Alert.alert('Başarılı', 'Roller güncellendi');
    } catch (error) {
      Alert.alert('Hata', 'Roller güncellenemedi');
    }
  };

  const toggleRole = (roleId: number) => {
    if (selectedRoles.includes(roleId)) {
      setSelectedRoles(selectedRoles.filter(id => id !== roleId));
    } else {
      setSelectedRoles([...selectedRoles, roleId]);
    }
  };

  const handleToggleStatus = async () => {
    if (!user) return;
    
    const action = user.isActive ? 'deactivate' : 'activate';
    Alert.alert(
      `${user.isActive ? 'Devre Dışı Bırak' : 'Aktif Et'}`,
      `${user.fullName} kullanıcısını ${user.isActive ? 'devre dışı bırakmak' : 'aktif etmek'} istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Evet',
          onPress: async () => {
            try {
              if (action === 'activate') {
                await authApi.activateUser(userId);
              } else {
                await authApi.deactivateUser(userId);
              }
              loadData();
            } catch (error) {
              Alert.alert('Hata', 'İşlem başarısız');
            }
          }
        }
      ]
    );
  };

  const handleDeleteUser = async () => {
    Alert.alert(
      'Kullanıcı Sil',
      `${user?.fullName} kullanıcısını silmek istediğinize emin misiniz? Bu işlem geri alınamaz.`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Sil',
          style: 'destructive',
          onPress: async () => {
            try {
              await authApi.deleteUser(userId);
              navigation.goBack();
              Alert.alert('Başarılı', 'Kullanıcı silindi');
            } catch (error) {
              Alert.alert('Hata', 'Kullanıcı silinemedi');
            }
          }
        }
      ]
    );
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!user) {
    return (
      <View style={styles.center}>
        <Text>Kullanıcı bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <View style={styles.avatarContainer}>
          <Text style={styles.avatar}>
            {user.firstName.charAt(0)}{user.lastName.charAt(0)}
          </Text>
        </View>
        <Text style={styles.name}>{user.fullName}</Text>
        <Text style={styles.email}>{user.email}</Text>
        <View style={styles.roleContainer}>
          {user.roles.map((role, index) => (
            <RoleBadge key={index} role={role} />
          ))}
        </View>
        <View style={[styles.statusBadge, { backgroundColor: user.isActive ? '#10b981' : '#ef4444' }]}>
          <Text style={styles.statusText}>{user.isActive ? 'Aktif' : 'Pasif'}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <View style={styles.sectionHeader}>
          <Text style={styles.sectionTitle}>Kişisel Bilgiler</Text>
          {(canManage || isOwnProfile) && !isEditing && (
            <TouchableOpacity onPress={() => setIsEditing(true)}>
              <Text style={styles.editButton}>Düzenle</Text>
            </TouchableOpacity>
          )}
        </View>

        {isEditing ? (
          <>
            <TextInput
              style={styles.input}
              placeholder="Ad"
              value={editedUser.firstName}
              onChangeText={(text) => setEditedUser({ ...editedUser, firstName: text })}
            />
            <TextInput
              style={styles.input}
              placeholder="Soyad"
              value={editedUser.lastName}
              onChangeText={(text) => setEditedUser({ ...editedUser, lastName: text })}
            />
            <TextInput
              style={styles.input}
              placeholder="Telefon"
              value={editedUser.phoneNumber}
              onChangeText={(text) => setEditedUser({ ...editedUser, phoneNumber: text })}
              keyboardType="phone-pad"
            />
            <TextInput
              style={styles.input}
              placeholder="Departman"
              value={editedUser.department}
              onChangeText={(text) => setEditedUser({ ...editedUser, department: text })}
            />
            <TextInput
              style={styles.input}
              placeholder="Pozisyon"
              value={editedUser.position}
              onChangeText={(text) => setEditedUser({ ...editedUser, position: text })}
            />
            <View style={styles.buttonRow}>
              <TouchableOpacity style={styles.cancelButton} onPress={() => setIsEditing(false)}>
                <Text style={styles.cancelButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.saveButton} onPress={handleUpdateProfile}>
                <Text style={styles.saveButtonText}>Kaydet</Text>
              </TouchableOpacity>
            </View>
          </>
        ) : (
          <>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Ad Soyad:</Text>
              <Text style={styles.infoValue}>{user.fullName}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>E-posta:</Text>
              <Text style={styles.infoValue}>{user.email}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Telefon:</Text>
              <Text style={styles.infoValue}>{user.phoneNumber}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Departman:</Text>
              <Text style={styles.infoValue}>{user.department || '-'}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>Pozisyon:</Text>
              <Text style={styles.infoValue}>{user.position || '-'}</Text>
            </View>
          </>
        )}
      </View>

      {canManage && !isOwnProfile && (
        <View style={styles.actionSection}>
          <TouchableOpacity
            style={[styles.actionButton, user.isActive ? styles.deactivateButton : styles.activateButton]}
            onPress={handleToggleStatus}
          >
            <Icon name={user.isActive ? "close-circle-outline" : "checkmark-circle-outline"} size={20} color="white" />
            <Text style={styles.actionButtonText}>
              {user.isActive ? 'Kullanıcıyı Devre Dışı Bırak' : 'Kullanıcıyı Aktif Et'}
            </Text>
          </TouchableOpacity>
          
          <TouchableOpacity
            style={[styles.actionButton, styles.deleteButton]}
            onPress={handleDeleteUser}
          >
            <Icon name="trash-outline" size={20} color="white" />
            <Text style={styles.actionButtonText}>Kullanıcıyı Sil</Text>
          </TouchableOpacity>
        </View>
      )}

      <Modal visible={modalVisible} transparent animationType="slide">
        <View style={styles.modalContainer}>
          <View style={styles.modalContent}>
            <Text style={styles.modalTitle}>Rolleri Düzenle</Text>
            
            <ScrollView style={styles.roleScroll}>
              {roles.map((role) => (
                <TouchableOpacity
                  key={role.id}
                  style={styles.roleOption}
                  onPress={() => toggleRole(role.id)}
                >
                  <View style={styles.roleOptionInfo}>
                    <Text style={styles.roleOptionName}>{role.name}</Text>
                    <Text style={styles.roleOptionDescription}>{role.description}</Text>
                  </View>
                  <View style={[styles.roleCheckbox, selectedRoles.includes(role.id) && styles.roleCheckboxSelected]}>
                    {selectedRoles.includes(role.id) && <Icon name="checkmark" size={16} color="white" />}
                  </View>
                </TouchableOpacity>
              ))}
            </ScrollView>

            <View style={styles.modalButtons}>
              <TouchableOpacity
                style={[styles.modalButton, styles.cancelModalButton]}
                onPress={() => setModalVisible(false)}
              >
                <Text style={styles.cancelModalButtonText}>İptal</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={[styles.modalButton, styles.saveModalButton]}
                onPress={handleUpdateRoles}
              >
                <Text style={styles.saveModalButtonText}>Kaydet</Text>
              </TouchableOpacity>
            </View>
          </View>
        </View>
      </Modal>
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
    backgroundColor: '#007AFF',
    borderRadius: 16,
    margin: 16,
    padding: 24,
    alignItems: 'center',
  },
  avatarContainer: {
    width: 80,
    height: 80,
    borderRadius: 40,
    backgroundColor: 'white',
    justifyContent: 'center',
    alignItems: 'center',
    marginBottom: 16,
  },
  avatar: {
    fontSize: 32,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  name: {
    fontSize: 20,
    fontWeight: 'bold',
    color: 'white',
    marginBottom: 4,
  },
  email: {
    fontSize: 14,
    color: 'white',
    opacity: 0.9,
    marginBottom: 12,
  },
  roleContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    justifyContent: 'center',
    gap: 8,
    marginBottom: 12,
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 20,
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
  sectionHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  editButton: {
    color: '#007AFF',
    fontSize: 14,
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 12,
  },
  infoLabel: {
    width: 100,
    fontSize: 14,
    color: '#666',
  },
  infoValue: {
    flex: 1,
    fontSize: 14,
    color: '#333',
  },
  input: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
    marginBottom: 12,
  },
  buttonRow: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 8,
  },
  cancelButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 8,
    borderWidth: 1,
    borderColor: '#ddd',
    alignItems: 'center',
  },
  cancelButtonText: {
    color: '#666',
  },
  saveButton: {
    flex: 1,
    paddingVertical: 10,
    borderRadius: 8,
    backgroundColor: '#007AFF',
    alignItems: 'center',
  },
  saveButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  actionSection: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
    gap: 12,
  },
  actionButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    paddingVertical: 12,
    borderRadius: 8,
  },
  activateButton: {
    backgroundColor: '#10b981',
  },
  deactivateButton: {
    backgroundColor: '#f59e0b',
  },
  deleteButton: {
    backgroundColor: '#ef4444',
  },
  actionButtonText: {
    color: 'white',
    fontWeight: 'bold',
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
    width: '90%',
    maxHeight: '80%',
  },
  modalTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 16,
  },
  roleScroll: {
    maxHeight: 400,
  },
  roleOption: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  roleOptionInfo: {
    flex: 1,
  },
  roleOptionName: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  roleOptionDescription: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  roleCheckbox: {
    width: 22,
    height: 22,
    borderRadius: 4,
    borderWidth: 2,
    borderColor: '#007AFF',
    justifyContent: 'center',
    alignItems: 'center',
    marginLeft: 12,
  },
  roleCheckboxSelected: {
    backgroundColor: '#007AFF',
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
  saveModalButton: {
    backgroundColor: '#007AFF',
  },
  cancelModalButtonText: {
    color: '#666',
  },
  saveModalButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});