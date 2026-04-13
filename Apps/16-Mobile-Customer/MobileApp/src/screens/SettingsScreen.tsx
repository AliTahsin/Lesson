import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  Switch,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { useBiometric } from '../hooks/useBiometric';
import { LanguageSelector } from '../components/LanguageSelector';
import { customerApi } from '../services/customerApi';
import { storage } from '../utils/storage';

export const SettingsScreen = ({ navigation }: any) => {
  const { t } = useLanguageContext();
  const { isAvailable: biometricAvailable, isEnabled: biometricEnabled, enableBiometric, disableBiometric } = useBiometric();
  const [pushEnabled, setPushEnabled] = useState(true);
  const [emailEnabled, setEmailEnabled] = useState(true);
  const [smsEnabled, setSmsEnabled] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    loadNotificationSettings();
  }, []);

  const loadNotificationSettings = async () => {
    try {
      const profile = await customerApi.getProfile();
      setPushEnabled(profile.pushEnabled);
      setEmailEnabled(profile.emailEnabled);
      setSmsEnabled(profile.smsEnabled);
    } catch (error) {
      console.error('Error loading settings:', error);
    }
  };

  const handleTogglePush = async (value: boolean) => {
    setPushEnabled(value);
    await customerApi.updateNotificationSettings({ pushEnabled: value, emailEnabled, smsEnabled });
  };

  const handleToggleEmail = async (value: boolean) => {
    setEmailEnabled(value);
    await customerApi.updateNotificationSettings({ pushEnabled, emailEnabled: value, smsEnabled });
  };

  const handleToggleSms = async (value: boolean) => {
    setSmsEnabled(value);
    await customerApi.updateNotificationSettings({ pushEnabled, emailEnabled, smsEnabled: value });
  };

  const handleChangePassword = () => {
    navigation.navigate('ChangePassword');
  };

  const handleLogout = () => {
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

  return (
    <ScrollView style={styles.container}>
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>{t('notifications')}</Text>
        <View style={styles.settingItem}>
          <Text style={styles.settingLabel}>{t('push_notifications')}</Text>
          <Switch value={pushEnabled} onValueChange={handleTogglePush} />
        </View>
        <View style={styles.settingItem}>
          <Text style={styles.settingLabel}>{t('email_notifications')}</Text>
          <Switch value={emailEnabled} onValueChange={handleToggleEmail} />
        </View>
        <View style={styles.settingItem}>
          <Text style={styles.settingLabel}>{t('sms_notifications')}</Text>
          <Switch value={smsEnabled} onValueChange={handleToggleSms} />
        </View>
      </View>

      {biometricAvailable && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Güvenlik</Text>
          <View style={styles.settingItem}>
            <Text style={styles.settingLabel}>{t('biometric_login')}</Text>
            <Switch value={biometricEnabled} onValueChange={biometricEnabled ? disableBiometric : enableBiometric} />
          </View>
        </View>
      )}

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>{t('language')}</Text>
        <View style={styles.settingItem}>
          <LanguageSelector />
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Hesap</Text>
        <TouchableOpacity style={styles.settingItem} onPress={handleChangePassword}>
          <Text style={styles.settingLabel}>{t('change_password')}</Text>
          <Icon name="chevron-forward" size={20} color="#ccc" />
        </TouchableOpacity>
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
  section: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginTop: 16,
    paddingHorizontal: 16,
    overflow: 'hidden',
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    paddingVertical: 12,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  settingItem: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    paddingVertical: 14,
    borderBottomWidth: 1,
    borderBottomColor: '#f0f0f0',
  },
  settingLabel: {
    fontSize: 14,
    color: '#333',
  },
  logoutButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 20,
    padding: 16,
  },
  logoutButtonText: {
    color: '#ef4444',
    fontWeight: 'bold',
  },
});