import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { useLanguageContext } from '../context/LanguageContext';
import { customerApi } from '../services/customerApi';
import { DigitalKeyCard } from '../components/DigitalKeyCard';
import { DigitalKey } from '../types/customer';

export const DigitalKeyScreen = () => {
  const { t } = useLanguageContext();
  const [keys, setKeys] = useState<DigitalKey[]>([]);
  const [loading, setLoading] = useState(true);
  const [generating, setGenerating] = useState(false);

  useEffect(() => {
    loadKeys();
  }, []);

  const loadKeys = async () => {
    setLoading(true);
    try {
      const data = await customerApi.getDigitalKeys();
      setKeys(data);
    } catch (error) {
      console.error('Error loading digital keys:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleGenerateKey = async () => {
    setGenerating(true);
    try {
      // Mock reservation ID - in real app, user would select a reservation
      const newKey = await customerApi.generateDigitalKey(123);
      setKeys([...keys, newKey]);
      Alert.alert('Başarılı', 'Dijital anahtar oluşturuldu');
    } catch (error) {
      Alert.alert('Hata', 'Anahtar oluşturulamadı');
    } finally {
      setGenerating(false);
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      {keys.length === 0 ? (
        <View style={styles.emptyContainer}>
          <Icon name="key-outline" size={64} color="#ccc" />
          <Text style={styles.emptyText}>Aktif dijital anahtarınız bulunmuyor</Text>
          <Text style={styles.emptySubtext}>
            Aktif bir rezervasyonunuz varsa dijital anahtar oluşturabilirsiniz
          </Text>
          <TouchableOpacity
            style={styles.generateButton}
            onPress={handleGenerateKey}
            disabled={generating}
          >
            <Text style={styles.generateButtonText}>
              {generating ? 'Oluşturuluyor...' : 'Anahtar Oluştur'}
            </Text>
          </TouchableOpacity>
        </View>
      ) : (
        <>
          {keys.map((key) => (
            <DigitalKeyCard key={key.id} digitalKey={key} />
          ))}
          <TouchableOpacity
            style={styles.newKeyButton}
            onPress={handleGenerateKey}
            disabled={generating}
          >
            <Icon name="add-circle-outline" size={20} color="#007AFF" />
            <Text style={styles.newKeyButtonText}>Yeni Anahtar Oluştur</Text>
          </TouchableOpacity>
        </>
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
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 16,
    color: '#888',
    marginTop: 16,
  },
  emptySubtext: {
    fontSize: 13,
    color: '#aaa',
    textAlign: 'center',
    marginTop: 8,
    paddingHorizontal: 40,
  },
  generateButton: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    paddingHorizontal: 24,
    paddingVertical: 12,
    marginTop: 24,
  },
  generateButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  newKeyButton: {
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 14,
  },
  newKeyButtonText: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
});