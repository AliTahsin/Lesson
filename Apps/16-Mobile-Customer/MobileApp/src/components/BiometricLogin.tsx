import React from 'react';
import {
  View,
  Text,
  TouchableOpacity,
  StyleSheet,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useBiometric } from '../hooks/useBiometric';

interface Props {
  onSuccess: () => void;
  onFailure?: () => void;
}

export const BiometricLogin: React.FC<Props> = ({ onSuccess, onFailure }) => {
  const { isAvailable, isEnabled, biometricType, authenticate, enableBiometric, disableBiometric } = useBiometric();

  const handleBiometricLogin = async () => {
    const result = await authenticate();
    if (result.success) {
      onSuccess();
    } else {
      Alert.alert('Doğrulama Başarısız', result.error || 'Lütfen tekrar deneyin');
      onFailure?.();
    }
  };

  const handleToggleBiometric = async () => {
    if (isEnabled) {
      await disableBiometric();
      Alert.alert('Başarılı', 'Biyometrik giriş devre dışı bırakıldı');
    } else {
      const success = await enableBiometric();
      if (success) {
        Alert.alert('Başarılı', 'Biyometrik giriş etkinleştirildi');
      } else {
        Alert.alert('Hata', 'Biyometrik giriş etkinleştirilemedi');
      }
    }
  };

  if (!isAvailable) return null;

  return (
    <View style={styles.container}>
      <TouchableOpacity
        style={styles.biometricButton}
        onPress={handleBiometricLogin}
      >
        <Icon
          name={biometricType === 'Face ID' ? 'scan-outline' : 'finger-print-outline'}
          size={24}
          color="#007AFF"
        />
        <Text style={styles.biometricText}>
          {biometricType} ile Giriş Yap
        </Text>
      </TouchableOpacity>

      <TouchableOpacity
        style={styles.toggleButton}
        onPress={handleToggleBiometric}
      >
        <Text style={styles.toggleText}>
          {isEnabled ? 'Biyometrik Girişi Kapat' : 'Biyometrik Girişi Aç'}
        </Text>
      </TouchableOpacity>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    alignItems: 'center',
    marginTop: 20,
  },
  biometricButton: {
    flexDirection: 'row',
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 20,
    paddingVertical: 12,
    borderRadius: 25,
    gap: 8,
  },
  biometricText: {
    fontSize: 14,
    color: '#007AFF',
    fontWeight: '500',
  },
  toggleButton: {
    marginTop: 12,
  },
  toggleText: {
    fontSize: 12,
    color: '#666',
  },
});