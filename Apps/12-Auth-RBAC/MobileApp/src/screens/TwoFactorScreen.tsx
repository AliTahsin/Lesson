import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  KeyboardAvoidingView,
  Platform
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useAuth } from '../context/AuthContext';

export const TwoFactorScreen = ({ route, navigation }: any) => {
  const { userId } = route.params;
  const [code, setCode] = useState('');
  const [loading, setLoading] = useState(false);
  const { loginWith2FA } = useAuth();

  const handleVerify = async () => {
    if (!code || code.length !== 6) {
      Alert.alert('Hata', 'Lütfen 6 haneli kodu girin');
      return;
    }

    setLoading(true);
    try {
      await loginWith2FA({
        userId,
        code,
        deviceId: 'mobile-device',
        deviceName: Platform.OS === 'ios' ? 'iPhone' : 'Android',
        userAgent: Platform.OS
      });
      navigation.replace('Main');
    } catch (error: any) {
      Alert.alert('Doğrulama Hatası', error.response?.data?.error || 'Geçersiz kod');
    } finally {
      setLoading(false);
    }
  };

  return (
    <KeyboardAvoidingView
      style={styles.container}
      behavior={Platform.OS === 'ios' ? 'padding' : 'height'}
    >
      <View style={styles.content}>
        <View style={styles.iconContainer}>
          <Icon name="shield-checkmark" size={80} color="#007AFF" />
        </View>
        
        <Text style={styles.title}>İki Faktörlü Doğrulama</Text>
        <Text style={styles.subtitle}>
          Telefonunuza veya e-posta adresinize gönderilen 6 haneli kodu girin
        </Text>

        <TextInput
          style={styles.codeInput}
          placeholder="000000"
          value={code}
          onChangeText={setCode}
          keyboardType="number-pad"
          maxLength={6}
          textAlign="center"
        />

        <TouchableOpacity
          style={[styles.verifyButton, loading && styles.disabledButton]}
          onPress={handleVerify}
          disabled={loading}
        >
          {loading ? (
            <ActivityIndicator color="white" />
          ) : (
            <Text style={styles.verifyButtonText}>Doğrula</Text>
          )}
        </TouchableOpacity>

        <TouchableOpacity
          style={styles.resendButton}
          onPress={() => Alert.alert('Bilgi', 'Yeni kod gönderildi')}
        >
          <Text style={styles.resendButtonText}>Kodu Tekrar Gönder</Text>
        </TouchableOpacity>
      </View>
    </KeyboardAvoidingView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  content: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    padding: 20,
  },
  iconContainer: {
    marginBottom: 30,
  },
  title: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  subtitle: {
    fontSize: 14,
    color: '#666',
    textAlign: 'center',
    marginBottom: 30,
  },
  codeInput: {
    width: 200,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 12,
    padding: 16,
    fontSize: 24,
    letterSpacing: 8,
    textAlign: 'center',
    backgroundColor: 'white',
    marginBottom: 24,
  },
  verifyButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 14,
    width: '100%',
    alignItems: 'center',
  },
  disabledButton: {
    backgroundColor: '#9ca3af',
  },
  verifyButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  resendButton: {
    marginTop: 16,
  },
  resendButtonText: {
    color: '#007AFF',
    fontSize: 14,
  },
});