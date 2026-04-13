import React, { useState } from 'react';
import {
  View,
  Text,
  ScrollView,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  Modal
} from 'react-native';
import { paymentApi } from '../services/paymentApi';
import { CreditCardInput } from '../components/CreditCardInput';
import { PaymentResponse } from '../types/payment';

export const PaymentScreen = ({ route, navigation }: any) => {
  const { reservationId, customerId, amount, currency, nights } = route.params;
  
  const [cardInfo, setCardInfo] = useState<any>(null);
  const [installment, setInstallment] = useState(1);
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<PaymentResponse | null>(null);
  const [showResult, setShowResult] = useState(false);

  const installments = [1, 2, 3, 6, 9, 12];

  const handlePayment = async () => {
    if (!cardInfo || !cardInfo.cardNumber || cardInfo.cardNumber.length < 16) {
      Alert.alert('Hata', 'Lütfen geçerli bir kart numarası girin');
      return;
    }

    if (!cardInfo.cardHolderName) {
      Alert.alert('Hata', 'Lütfen kart üzerindeki ismi girin');
      return;
    }

    if (!cardInfo.expiryMonth || !cardInfo.expiryYear) {
      Alert.alert('Hata', 'Lütfen son kullanma tarihini girin');
      return;
    }

    if (!cardInfo.cvv || cardInfo.cvv.length < 3) {
      Alert.alert('Hata', 'Lütfen CVV kodunu girin');
      return;
    }

    setLoading(true);
    try {
      const response = await paymentApi.processPayment({
        reservationId,
        customerId,
        amount,
        currency,
        paymentMethod: 'CreditCard',
        cardNumber: cardInfo.cardNumber,
        cardHolderName: cardInfo.cardHolderName,
        expiryMonth: parseInt(cardInfo.expiryMonth),
        expiryYear: parseInt(cardInfo.expiryYear),
        cvv: cardInfo.cvv,
        installment,
        paymentSource: 'Mobile'
      });

      setResult(response);
      setShowResult(true);
    } catch (error: any) {
      Alert.alert('Ödeme Hatası', error.response?.data?.message || 'Ödeme işlemi sırasında bir hata oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleCloseResult = () => {
    setShowResult(false);
    if (result?.isSuccess) {
      navigation.navigate('PaymentHistory', { customerId });
    }
  };

  const ResultModal = () => (
    <Modal visible={showResult} transparent animationType="slide">
      <View style={styles.modalContainer}>
        <View style={[styles.modalContent, result?.isSuccess ? styles.successModal : styles.errorModal]}>
          <Text style={styles.modalIcon}>{result?.isSuccess ? '✅' : '❌'}</Text>
          <Text style={styles.modalTitle}>
            {result?.isSuccess ? 'Ödeme Başarılı!' : 'Ödeme Başarısız!'}
          </Text>
          <Text style={styles.modalMessage}>{result?.message}</Text>
          
          {result?.isSuccess && (
            <>
              <View style={styles.modalDetails}>
                <Text style={styles.modalDetailText}>İşlem No: {result.paymentNumber}</Text>
                <Text style={styles.modalDetailText}>Tutar: {currency} {amount.toLocaleString()}</Text>
                <Text style={styles.modalDetailText}>Taksit: {installment} taksit</Text>
              </View>
              <View style={styles.modalSecurity}>
                <Text style={styles.modalSecurityText}>🔒 3D Secure ile güvenli ödeme</Text>
              </View>
            </>
          )}

          <TouchableOpacity style={styles.modalButton} onPress={handleCloseResult}>
            <Text style={styles.modalButtonText}>Tamam</Text>
          </TouchableOpacity>
        </View>
      </View>
    </Modal>
  );

  return (
    <ScrollView style={styles.container}>
      <View style={styles.amountCard}>
        <Text style={styles.amountLabel}>Ödenecek Tutar</Text>
        <Text style={styles.amountValue}>{currency} {amount.toLocaleString()}</Text>
        <Text style={styles.nightsText}>{nights} gece konaklama</Text>
      </View>

      <CreditCardInput onCardChange={setCardInfo} />

      <View style={styles.installmentCard}>
        <Text style={styles.sectionTitle}>Taksit Seçeneği</Text>
        <View style={styles.installmentContainer}>
          {installments.map((inst) => (
            <TouchableOpacity
              key={inst}
              style={[
                styles.installmentButton,
                installment === inst && styles.installmentButtonActive
              ]}
              onPress={() => setInstallment(inst)}
            >
              <Text style={[
                styles.installmentText,
                installment === inst && styles.installmentTextActive
              ]}>
                {inst} Taksit
              </Text>
              <Text style={styles.installmentPrice}>
                {currency} {(amount / inst).toFixed(2)}
              </Text>
            </TouchableOpacity>
          ))}
        </View>
      </View>

      <View style={styles.infoCard}>
        <Text style={styles.infoTitle}>🔒 Güvenli Ödeme</Text>
        <Text style={styles.infoText}>• 256-bit SSL sertifikası ile şifrelenir</Text>
        <Text style={styles.infoText}>• 3D Secure ile ek güvenlik</Text>
        <Text style={styles.infoText}>• Kredi kartı bilgileriniz saklanmaz</Text>
        <Text style={styles.infoText}>• PCI DSS uyumlu</Text>
      </View>

      <TouchableOpacity
        style={[styles.payButton, loading && styles.payButtonDisabled]}
        onPress={handlePayment}
        disabled={loading}
      >
        {loading ? (
          <ActivityIndicator color="white" />
        ) : (
          <Text style={styles.payButtonText}>
            {currency} {amount.toLocaleString()} Öde
          </Text>
        )}
      </TouchableOpacity>

      <ResultModal />
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  amountCard: {
    backgroundColor: '#007AFF',
    borderRadius: 16,
    margin: 16,
    padding: 20,
    alignItems: 'center',
  },
  amountLabel: {
    color: 'white',
    fontSize: 14,
    opacity: 0.9,
  },
  amountValue: {
    color: 'white',
    fontSize: 36,
    fontWeight: 'bold',
    marginVertical: 8,
  },
  nightsText: {
    color: 'white',
    fontSize: 12,
    opacity: 0.8,
  },
  installmentCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  sectionTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 12,
  },
  installmentContainer: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  installmentButton: {
    width: '30%',
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
    padding: 10,
    alignItems: 'center',
    marginBottom: 8,
  },
  installmentButtonActive: {
    backgroundColor: '#007AFF',
  },
  installmentText: {
    fontSize: 14,
    color: '#333',
  },
  installmentTextActive: {
    color: 'white',
  },
  installmentPrice: {
    fontSize: 11,
    color: '#666',
    marginTop: 2,
  },
  infoCard: {
    backgroundColor: '#f0fdf4',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  infoTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#16a34a',
    marginBottom: 8,
  },
  infoText: {
    fontSize: 12,
    color: '#166534',
    marginBottom: 4,
  },
  payButton: {
    backgroundColor: '#16a34a',
    borderRadius: 12,
    margin: 16,
    paddingVertical: 16,
    alignItems: 'center',
  },
  payButtonDisabled: {
    backgroundColor: '#9ca3af',
  },
  payButtonText: {
    color: 'white',
    fontSize: 18,
    fontWeight: 'bold',
  },
  modalContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: 'rgba(0,0,0,0.5)',
  },
  modalContent: {
    borderRadius: 20,
    padding: 24,
    width: '85%',
    alignItems: 'center',
  },
  successModal: {
    backgroundColor: 'white',
  },
  errorModal: {
    backgroundColor: 'white',
  },
  modalIcon: {
    fontSize: 64,
    marginBottom: 16,
  },
  modalTitle: {
    fontSize: 24,
    fontWeight: 'bold',
    marginBottom: 8,
  },
  modalMessage: {
    fontSize: 14,
    color: '#666',
    textAlign: 'center',
    marginBottom: 16,
  },
  modalDetails: {
    backgroundColor: '#f5f5f5',
    borderRadius: 12,
    padding: 12,
    width: '100%',
    marginBottom: 16,
  },
  modalDetailText: {
    fontSize: 13,
    color: '#333',
    marginBottom: 4,
  },
  modalSecurity: {
    marginBottom: 20,
  },
  modalSecurityText: {
    fontSize: 12,
    color: '#10b981',
  },
  modalButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 32,
    paddingVertical: 12,
  },
  modalButtonText: {
    color: 'white',
    fontSize: 16,
    fontWeight: 'bold',
  },
});