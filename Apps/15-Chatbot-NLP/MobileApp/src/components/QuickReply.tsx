import React from 'react';
import { View, Text, TouchableOpacity, StyleSheet, ScrollView } from 'react-native';

interface Props {
  onSelect: (text: string) => void;
}

const quickReplies = [
  { text: '🏨 Otel Bilgisi', value: 'Otel hakkında bilgi alabilir miyim?' },
  { text: '💰 Fiyatlar', value: 'Oda fiyatları nedir?' },
  { text: '📅 Rezervasyon', value: 'Rezervasyon yapmak istiyorum' },
  { text: '🏊 Havuz', value: 'Havuzunuz var mı?' },
  { text: '🍽️ Restoran', value: 'Restoranınız var mı?' },
  { text: '📍 Konum', value: 'Otel nerede?' },
];

export const QuickReply: React.FC<Props> = ({ onSelect }) => {
  return (
    <ScrollView horizontal showsHorizontalScrollIndicator={false} style={styles.container}>
      {quickReplies.map((reply, index) => (
        <TouchableOpacity
          key={index}
          style={styles.button}
          onPress={() => onSelect(reply.value)}
        >
          <Text style={styles.buttonText}>{reply.text}</Text>
        </TouchableOpacity>
      ))}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    paddingHorizontal: 8,
    paddingVertical: 8,
    maxHeight: 50,
  },
  button: {
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 20,
    marginHorizontal: 4,
  },
  buttonText: {
    fontSize: 13,
    color: '#333',
  },
});