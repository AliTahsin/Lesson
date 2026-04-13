import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  Image
} from 'react-native';
import * as ImagePicker from 'expo-image-picker';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { customerApi } from '../services/customerApi';
import { CustomerProfile, UpdateProfileRequest } from '../types/customer';
import { storage } from '../utils/storage';

export const ProfileScreen = ({ navigation }: any) => {
  const { t } = useLanguageContext();
  const [profile, setProfile] = useState<CustomerProfile | null>(null);
  const [loading, setLoading] = useState(true);
  const [isEditing, setIsEditing] = useState(false);
  const [editedProfile, setEditedProfile] = useState<UpdateProfileRequest>({});

  useEffect(() => {
    loadProfile();
  }, []);

  const loadProfile = async () => {
    setLoading(true);
    try {
      const data = await customerApi.getProfile();
      setProfile(data);
      setEditedProfile({
        firstName: data.firstName,
        lastName: data.lastName,
        phoneNumber: data.phoneNumber,
        dateOfBirth: data.dateOfBirth,
        country: data.country,
        city: data.city,
        address: data.address,
      });
    } catch (error) {
      console.error('Error loading profile:', error);
      Alert.alert('Hata', 'Profil yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async () => {
    try {
      await customerApi.updateProfile(editedProfile);
      await loadProfile();
      setIsEditing(false);
      Alert.alert('Başarılı', 'Profil güncellendi');
    } catch (error) {
      Alert.alert('Hata', 'Profil güncellenemedi');
    }
  };

  const handlePickImage = async () => {
    const result = await ImagePicker.launchImageLibraryAsync({
      mediaTypes: ImagePicker.MediaTypeOptions.Images,
      allowsEditing: true,
      quality: 0.8,
    });

    if (!result.canceled) {
      // Upload image logic here
      Alert.alert('Başarılı', 'Profil fotoğrafı güncellendi');
    }
  };

  const handleLogout = async () => {
    Alert.alert(
      t('logout'),
      'Oturumunuzu kapatmak istediğinize emin misiniz?',
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Çıkış Yap',
          onPress: async () => {
            await storage.clearTokens();
            navigation.replace('Login');
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

  if (!profile) return null;

  return (
    <ScrollView style={styles.container}>
      <View style={styles.avatarSection}>
        <TouchableOpacity onPress={handlePickImage}>
          <Image
            source={{ uri: profile.profileImageUrl || 'https://randomuser.me/api/portraits/lego/1.jpg' }}
            style={styles.avatar}
          />
          <View style={styles.editAvatar}>
            <Icon name="camera" size={16} color="white" />
          </View>
        </TouchableOpacity>
        <Text style={styles.name}>{profile.fullName}</Text>
        <Text style={styles.email}>{profile.email}</Text>
      </View>

      <View style={styles.section}>
        <View style={styles.sectionHeader}>
          <Text style={styles.sectionTitle}>{t('personal_info')}</Text>
          {!isEditing && (
            <TouchableOpacity onPress={() => setIsEditing(true)}>
              <Text style={styles.editButton}>{t('edit')}</Text>
            </TouchableOpacity>
          )}
        </View>

        {isEditing ? (
          <>
            <TextInput
              style={styles.input}
              placeholder={t('first_name')}
              value={editedProfile.firstName}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, firstName: text })}
            />
            <TextInput
              style={styles.input}
              placeholder={t('last_name')}
              value={editedProfile.lastName}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, lastName: text })}
            />
            <TextInput
              style={styles.input}
              placeholder={t('phone')}
              value={editedProfile.phoneNumber}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, phoneNumber: text })}
              keyboardType="phone-pad"
            />
            <TextInput
              style={styles.input}
              placeholder={t('country')}
              value={editedProfile.country}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, country: text })}
            />
            <TextInput
              style={styles.input}
              placeholder={t('city')}
              value={editedProfile.city}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, city: text })}
            />
            <TextInput
              style={[styles.input, styles.textArea]}
              placeholder={t('address')}
              value={editedProfile.address}
              onChangeText={(text) => setEditedProfile({ ...editedProfile, address: text })}
              multiline
              numberOfLines={3}
            />
            <View style={styles.buttonRow}>
              <TouchableOpacity style={styles.cancelButton} onPress={() => setIsEditing(false)}>
                <Text style={styles.cancelButtonText}>{t('cancel')}</Text>
              </TouchableOpacity>
              <TouchableOpacity style={styles.saveButton} onPress={handleUpdate}>
                <Text style={styles.saveButtonText}>{t('save')}</Text>
              </TouchableOpacity>
            </View>
          </>
        ) : (
          <>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>{t('first_name')}:</Text>
              <Text style={styles.infoValue}>{profile.firstName}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>{t('last_name')}:</Text>
              <Text style={styles.infoValue}>{profile.lastName}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>{t('email')}:</Text>
              <Text style={styles.infoValue}>{profile.email}</Text>
            </View>
            <View style={styles.infoRow}>
              <Text style={styles.infoLabel}>{t('phone')}:</Text>
              <Text style={styles.infoValue}>{profile.phoneNumber}</Text>
            </View>
            {profile.country && (
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>{t('country')}:</Text>
                <Text style={styles.infoValue}>{profile.country}</Text>
              </View>
            )}
            {profile.city && (
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>{t('city')}:</Text>
                <Text style={styles.infoValue}>{profile.city}</Text>
              </View>
            )}
            {profile.address && (
              <View style={styles.infoRow}>
                <Text style={styles.infoLabel}>{t('address')}:</Text>
                <Text style={styles.infoValue}>{profile.address}</Text>
              </View>
            )}
          </>
        )}
      </View>

      <TouchableOpacity style={styles.logoutButton} onPress={handleLogout}>
        <Icon name="log-out-outline" size={20} color="#ef4444" />
        <Text style={styles.logoutButtonText}>{t('logout')}</Text>
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
  avatarSection: {
    alignItems: 'center',
    backgroundColor: 'white',
    paddingVertical: 24,
    marginBottom: 16,
  },
  avatar: {
    width: 100,
    height: 100,
    borderRadius: 50,
  },
  editAvatar: {
    position: 'absolute',
    bottom: 0,
    right: 0,
    backgroundColor: '#007AFF',
    width: 32,
    height: 32,
    borderRadius: 16,
    justifyContent: 'center',
    alignItems: 'center',
    borderWidth: 2,
    borderColor: 'white',
  },
  name: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 12,
  },
  email: {
    fontSize: 14,
    color: '#666',
    marginTop: 4,
  },
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 16,
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
  textArea: {
    minHeight: 80,
    textAlignVertical: 'top',
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
  logoutButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
  },
  logoutButtonText: {
    color: '#ef4444',
    fontWeight: 'bold',
  },
});