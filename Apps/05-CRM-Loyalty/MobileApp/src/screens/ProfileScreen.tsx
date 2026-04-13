import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert
} from 'react-native';
import { loyaltyApi } from '../services/loyaltyApi';
import { LevelBadge } from '../components/LevelBadge';
import { Customer, LoyaltyInfo } from '../types/loyalty';

export const ProfileScreen = ({ route }: any) => {
  const [email, setEmail] = useState('');
  const [customer, setCustomer] = useState<Customer | null>(null);
  const [loyaltyInfo, setLoyaltyInfo] = useState<LoyaltyInfo | null>(null);
  const [loading, setLoading] = useState(false);
  const [isEditing, setIsEditing] = useState(false);
  const [editedCustomer, setEditedCustomer] = useState<Partial<Customer>>({});

  const loadCustomer = async () => {
    if (!email) {
      Alert.alert('Uyarı', 'Lütfen e-posta adresinizi girin');
      return;
    }

    if (!email.includes('@')) {
      Alert.alert('Hata', 'Geçerli bir e-posta adresi girin');
      return;
    }

    setLoading(true);
    try {
      const customerData = await loyaltyApi.getCustomerByEmail(email);
      setCustomer(customerData);
      setEditedCustomer(customerData);
      
      const loyaltyData = await loyaltyApi.getLoyaltyInfo(customerData.id);
      setLoyaltyInfo(loyaltyData);
    } catch (error) {
      Alert.alert('Hata', 'Müşteri bulunamadı');
      setCustomer(null);
      setLoyaltyInfo(null);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async () => {
    if (!customer) return;
    
    setLoading(true);
    try {
      await loyaltyApi.updateCustomer(customer.id, editedCustomer);
      const updatedCustomer = await loyaltyApi.getCustomerById(customer.id);
      setCustomer(updatedCustomer);
      setIsEditing(false);
      Alert.alert('Başarılı', 'Profil güncellendi');
    } catch (error) {
      Alert.alert('Hata', 'Güncelleme başarısız');
    } finally {
      setLoading(false);
    }
  };

  const getProgressPercentage = () => {
    if (!loyaltyInfo) return 0;
    const currentLevel = loyaltyInfo.currentLevel;
    const points = loyaltyInfo.currentPoints;
    
    let minPoints = 0, maxPoints = 0;
    switch (currentLevel) {
      case 'Bronze': minPoints = 0; maxPoints = 1000; break;
      case 'Silver': minPoints = 1000; maxPoints = 5000; break;
      case 'Gold': minPoints = 5000; maxPoints = 15000; break;
      case 'Platinum': minPoints = 15000; maxPoints = 50000; break;
      default: minPoints = 0; maxPoints = 1000;
    }
    
    const progress = ((points - minPoints) / (maxPoints - minPoints)) * 100;
    return Math.min(100, Math.max(0, progress));
  };

  if (loading && !customer) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.searchContainer}>
        <TextInput
          style={styles.emailInput}
          placeholder="E-posta adresiniz"
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />
        <TouchableOpacity style={styles.searchButton} onPress={loadCustomer}>
          <Text style={styles.searchButtonText}>Getir</Text>
        </TouchableOpacity>
      </View>

      {customer && loyaltyInfo && (
        <>
          <View style={styles.profileCard}>
            <View style={styles.profileHeader}>
              <View>
                <Text style={styles.customerName}>{customer.fullName}</Text>
                <Text style={styles.customerNumber}>{customer.customerNumber}</Text>
              </View>
              <LevelBadge level={customer.membershipLevel} size="large" />
            </View>
            
            <View style={styles.pointsCard}>
              <Text style={styles.pointsLabel}>Toplam Puan</Text>
              <Text style={styles.pointsValue}>{customer.loyaltyPoints.toLocaleString()}</Text>
              
              <View style={styles.progressContainer}>
                <View style={[styles.progressBar, { width: `${getProgressPercentage()}%` }]} />
              </View>
              
              <Text style={styles.nextLevelText}>
                {loyaltyInfo.pointsToNextLevel} puan ile {loyaltyInfo.nextLevel} seviyesine yükselin
              </Text>
            </View>
          </View>

          <View style={styles.statsCard}>
            <Text style={styles.sectionTitle}>İstatistikler</Text>
            <View style={styles.statsGrid}>
              <View style={styles.statItem}>
                <Text style={styles.statValue}>{customer.totalStays}</Text>
                <Text style={styles.statLabel}>Toplam Konaklama</Text>
              </View>
              <View style={styles.statItem}>
                <Text style={styles.statValue}>{customer.totalNights}</Text>
                <Text style={styles.statLabel}>Toplam Gece</Text>
              </View>
              <View style={styles.statItem}>
                <Text style={styles.statValue}>€{customer.totalSpent.toLocaleString()}</Text>
                <Text style={styles.statLabel}>Toplam Harcama</Text>
              </View>
            </View>
          </View>

          <View style={styles.benefitsCard}>
            <Text style={styles.sectionTitle}>Üyelik Avantajları</Text>
            <View style={styles.benefitsGrid}>
              <View style={styles.benefitItem}>
                <Text style={styles.benefitValue}>%{loyaltyInfo.levelBenefits.discountRate}</Text>
                <Text style={styles.benefitLabel}>İndirim</Text>
              </View>
              <View style={styles.benefitItem}>
                <Text style={styles.benefitValue}>x{loyaltyInfo.levelBenefits.pointsMultiplier}</Text>
                <Text style={styles.benefitLabel}>Puan Çarpanı</Text>
              </View>
              <View style={styles.benefitItem}>
                <Text style={styles.benefitValue}>{loyaltyInfo.levelBenefits.freeUpgradePerYear}</Text>
                <Text style={styles.benefitLabel}>Ücretsiz Upgrade</Text>
              </View>
              <View style={styles.benefitItem}>
                <Text style={styles.benefitValue}>{loyaltyInfo.levelBenefits.lateCheckoutHours}</Text>
                <Text style={styles.benefitLabel}>Geç Çıkış</Text>
              </View>
            </View>
            
            <View style={styles.benefitChecks}>
              {loyaltyInfo.levelBenefits.freeBreakfast && (
                <Text style={styles.benefitCheck}>✅ Ücretsiz Kahvaltı</Text>
              )}
              {loyaltyInfo.levelBenefits.airportTransfer && (
                <Text style={styles.benefitCheck}>✅ Havalimanı Transferi</Text>
              )}
              {loyaltyInfo.levelBenefits.loungeAccess && (
                <Text style={styles.benefitCheck}>✅ Lounge Erişimi</Text>
              )}
            </View>
          </View>

          <View style={styles.infoCard}>
            <View style={styles.infoHeader}>
              <Text style={styles.sectionTitle}>Profil Bilgileri</Text>
              <TouchableOpacity onPress={() => setIsEditing(!isEditing)}>
                <Text style={styles.editButton}>{isEditing ? 'İptal' : 'Düzenle'}</Text>
              </TouchableOpacity>
            </View>
            
            {isEditing ? (
              <>
                <TextInput
                  style={styles.input}
                  placeholder="Ad"
                  value={editedCustomer.firstName}
                  onChangeText={(text) => setEditedCustomer({ ...editedCustomer, firstName: text })}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Soyad"
                  value={editedCustomer.lastName}
                  onChangeText={(text) => setEditedCustomer({ ...editedCustomer, lastName: text })}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Telefon"
                  value={editedCustomer.phoneNumber}
                  onChangeText={(text) => setEditedCustomer({ ...editedCustomer, phoneNumber: text })}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Ülke"
                  value={editedCustomer.country}
                  onChangeText={(text) => setEditedCustomer({ ...editedCustomer, country: text })}
                />
                <TextInput
                  style={styles.input}
                  placeholder="Şehir"
                  value={editedCustomer.city}
                  onChangeText={(text) => setEditedCustomer({ ...editedCustomer, city: text })}
                />
                <TouchableOpacity style={styles.saveButton} onPress={handleUpdate}>
                  <Text style={styles.saveButtonText}>Kaydet</Text>
                </TouchableOpacity>
              </>
            ) : (
              <>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Telefon:</Text>
                  <Text style={styles.infoValue}>{customer.phoneNumber}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Ülke:</Text>
                  <Text style={styles.infoValue}>{customer.country}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Şehir:</Text>
                  <Text style={styles.infoValue}>{customer.city}</Text>
                </View>
                <View style={styles.infoRow}>
                  <Text style={styles.infoLabel}>Kayıt Tarihi:</Text>
                  <Text style={styles.infoValue}>{new Date(customer.registrationDate).toLocaleDateString('tr-TR')}</Text>
                </View>
              </>
            )}
          </View>
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
  searchContainer: {
    backgroundColor: 'white',
    padding: 16,
    borderBottomWidth: 1,
    borderBottomColor: '#e0e0e0',
  },
  emailInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 16,
    marginBottom: 12,
  },
  searchButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
  },
  searchButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
  profileCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    elevation: 2,
  },
  profileHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
  },
  customerName: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
  },
  customerNumber: {
    fontSize: 12,
    color: '#888',
    marginTop: 2,
  },
  pointsCard: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
  },
  pointsLabel: {
    fontSize: 14,
    color: 'white',
    opacity: 0.9,
  },
  pointsValue: {
    fontSize: 36,
    fontWeight: 'bold',
    color: 'white',
    marginVertical: 8,
  },
  progressContainer: {
    width: '100%',
    height: 8,
    backgroundColor: 'rgba(255,255,255,0.3)',
    borderRadius: 4,
    overflow: 'hidden',
    marginVertical: 8,
  },
  progressBar: {
    height: '100%',
    backgroundColor: '#fbbf24',
    borderRadius: 4,
  },
  nextLevelText: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 8,
  },
  statsCard: {
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
  statsGrid: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  statItem: {
    alignItems: 'center',
  },
  statValue: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  statLabel: {
    fontSize: 12,
    color: '#888',
    marginTop: 4,
  },
  benefitsCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 12,
    padding: 16,
  },
  benefitsGrid: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 12,
  },
  benefitItem: {
    alignItems: 'center',
  },
  benefitValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#f59e0b',
  },
  benefitLabel: {
    fontSize: 11,
    color: '#666',
    marginTop: 4,
  },
  benefitChecks: {
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
    paddingTop: 12,
  },
  benefitCheck: {
    fontSize: 13,
    color: '#16a34a',
    marginBottom: 4,
  },
  infoCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
  },
  infoHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  editButton: {
    color: '#007AFF',
    fontSize: 14,
    fontWeight: '500',
  },
  infoRow: {
    flexDirection: 'row',
    marginBottom: 8,
  },
  infoLabel: {
    width: 80,
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
  saveButton: {
    backgroundColor: '#16a34a',
    borderRadius: 8,
    paddingVertical: 12,
    alignItems: 'center',
    marginTop: 8,
  },
  saveButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});