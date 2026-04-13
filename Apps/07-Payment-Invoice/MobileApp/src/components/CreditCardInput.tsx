import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  StyleSheet,
  TouchableOpacity
} from 'react-native';

interface Props {
  onCardChange: (card: any) => void;
}

export const CreditCardInput: React.FC<Props> = ({ onCardChange }) => {
  const [cardNumber, setCardNumber] = useState('');
  const [cardHolder, setCardHolder] = useState('');
  const [expiry, setExpiry] = useState('');
  const [cvv, setCvv] = useState('');
  const [isFocused, setIsFocused] = useState(false);

  const formatCardNumber = (text: string) => {
    const cleaned = text.replace(/\s/g, '');
    const groups = cleaned.match(/.{1,4}/g);
    return groups ? groups.join(' ') : cleaned;
  };

  const formatExpiry = (text: string) => {
    const cleaned = text.replace(/\D/g, '');
    if (cleaned.length >= 2) {
      return `${cleaned.slice(0, 2)}/${cleaned.slice(2, 4)}`;
    }
    return cleaned;
  };

  const getCardBrand = (number: string) => {
    const firstDigit = number[0];
    const firstTwo = number.slice(0, 2);
    
    if (firstDigit === '4') return { brand: 'Visa', color: '#1a1f71' };
    if (firstTwo === '51' || firstTwo === '52' || firstTwo === '53' || firstTwo === '54' || firstTwo === '55') {
      return { brand: 'Mastercard', color: '#eb001b' };
    }
    if (firstTwo === '34' || firstTwo === '37') return { brand: 'Amex', color: '#006fcf' };
    return { brand: 'Card', color: '#666' };
  };

  const cardInfo = getCardBrand(cardNumber);

  const handleCardChange = () => {
    onCardChange({
      cardNumber: cardNumber.replace(/\s/g, ''),
      cardHolderName: cardHolder.toUpperCase(),
      expiryMonth: expiry.split('/')[0] || '',
      expiryYear: expiry.split('/')[1] || '',
      cvv,
      cardBrand: cardInfo.brand
    });
  };

  React.useEffect(() => {
    handleCardChange();
  }, [cardNumber, cardHolder, expiry, cvv]);

  return (
    <View style={[styles.container, isFocused && styles.containerFocused]}>
      <View style={styles.cardHeader}>
        <Text style={styles.cardBrand}>{cardInfo.brand}</Text>
        <View style={[styles.cardChip, { backgroundColor: cardInfo.color }]} />
      </View>

      <TextInput
        style={styles.input}
        placeholder="Kart Numarası"
        placeholderTextColor="#999"
        value={cardNumber}
        onChangeText={(text) => setCardNumber(formatCardNumber(text))}
        keyboardType="numeric"
        maxLength={19}
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
      />

      <TextInput
        style={styles.input}
        placeholder="Kart Üzerindeki İsim"
        placeholderTextColor="#999"
        value={cardHolder}
        onChangeText={setCardHolder}
        autoCapitalize="characters"
        onFocus={() => setIsFocused(true)}
        onBlur={() => setIsFocused(false)}
      />

      <View style={styles.row}>
        <TextInput
          style={[styles.input, styles.halfInput]}
          placeholder="SKT (AA/YY)"
          placeholderTextColor="#999"
          value={expiry}
          onChangeText={(text) => setExpiry(formatExpiry(text))}
          keyboardType="numeric"
          maxLength={5}
          onFocus={() => setIsFocused(true)}
          onBlur={() => setIsFocused(false)}
        />

        <TextInput
          style={[styles.input, styles.halfInput]}
          placeholder="CVV"
          placeholderTextColor="#999"
          value={cvv}
          onChangeText={setCvv}
          keyboardType="numeric"
          maxLength={4}
          secureTextEntry
          onFocus={() => setIsFocused(true)}
          onBlur={() => setIsFocused(false)}
        />
      </View>

      <View style={styles.securityBadge}>
        <Text style={styles.securityText}>🔒 PCI DSS Compliant</Text>
        <Text style={styles.securityText}>3D Secure</Text>
      </View>
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    backgroundColor: '#1a1a2e',
    borderRadius: 16,
    padding: 20,
    margin: 16,
    elevation: 5,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.25,
    shadowRadius: 4,
  },
  containerFocused: {
    borderWidth: 2,
    borderColor: '#007AFF',
  },
  cardHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 20,
  },
  cardBrand: {
    color: 'white',
    fontSize: 18,
    fontWeight: 'bold',
  },
  cardChip: {
    width: 40,
    height: 30,
    borderRadius: 6,
    opacity: 0.8,
  },
  input: {
    backgroundColor: 'rgba(255,255,255,0.1)',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 12,
    color: 'white',
    fontSize: 16,
    marginBottom: 12,
  },
  row: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  halfInput: {
    width: '48%',
  },
  securityBadge: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    marginTop: 12,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: 'rgba(255,255,255,0.1)',
  },
  securityText: {
    color: '#10b981',
    fontSize: 11,
  },
});