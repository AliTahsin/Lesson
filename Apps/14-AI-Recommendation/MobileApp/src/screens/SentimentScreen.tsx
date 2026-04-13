import React, { useState } from 'react';
import {
  View,
  Text,
  TextInput,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  ScrollView
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { aiApi } from '../services/aiApi';
import { SentimentBadge } from '../components/SentimentBadge';
import { SentimentResult, SentimentSummary } from '../types/ai';

export const SentimentScreen = () => {
  const [reviewText, setReviewText] = useState('');
  const [loading, setLoading] = useState(false);
  const [result, setResult] = useState<SentimentResult | null>(null);
  const [summary, setSummary] = useState<SentimentSummary[]>([]);
  const [itemId, setItemId] = useState('');
  const [itemType, setItemType] = useState('Hotel');
  const [showSummary, setShowSummary] = useState(false);

  const handleAnalyze = async () => {
    if (!reviewText.trim()) {
      Alert.alert('Uyarı', 'Lütfen yorum metnini girin');
      return;
    }

    setLoading(true);
    try {
      const data = await aiApi.analyzeSentiment(reviewText);
      setResult(data);
    } catch (error) {
      Alert.alert('Hata', 'Analiz sırasında bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleLoadSummary = async () => {
    if (!itemId) {
      Alert.alert('Uyarı', 'Lütfen otel ID girin');
      return;
    }

    setLoading(true);
    try {
      const data = await aiApi.getSentimentSummary(parseInt(itemId), itemType);
      setSummary(data);
      setShowSummary(true);
    } catch (error) {
      Alert.alert('Hata', 'Özet yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const getSentimentScoreColor = (score: number) => {
    if (score >= 0.7) return '#10b981';
    if (score >= 0.4) return '#f59e0b';
    return '#ef4444';
  };

  const clearForm = () => {
    setReviewText('');
    setResult(null);
    setItemId('');
    setShowSummary(false);
    setSummary([]);
  };

  return (
    <ScrollView style={styles.container}>
      {/* Yorum Analizi Bölümü */}
      <View style={styles.card}>
        <View style={styles.cardHeader}>
          <Icon name="chatbubble-outline" size={24} color="#007AFF" />
          <Text style={styles.cardTitle}>Yorum Duygu Analizi</Text>
        </View>
        
        <TextInput
          style={styles.textArea}
          placeholder="Yorum metnini girin...&#10;&#10;Örnek: 'Otel çok temiz ve personel çok ilgiliydi. Kesinlikle tavsiye ederim.'"
          value={reviewText}
          onChangeText={setReviewText}
          multiline
          numberOfLines={6}
          textAlignVertical="top"
        />
        
        <View style={styles.buttonRow}>
          <TouchableOpacity style={styles.clearButton} onPress={clearForm}>
            <Text style={styles.clearButtonText}>Temizle</Text>
          </TouchableOpacity>
          <TouchableOpacity style={styles.analyzeButton} onPress={handleAnalyze}>
            <Text style={styles.analyzeButtonText}>Duygu Analizi Yap</Text>
          </TouchableOpacity>
        </View>
      </View>

      {loading && (
        <View style={styles.loader}>
          <ActivityIndicator size="large" color="#007AFF" />
          <Text style={styles.loaderText}>Analiz yapılıyor...</Text>
        </View>
      )}

      {/* Analiz Sonucu Bölümü */}
      {result && (
        <View style={styles.resultCard}>
          <View style={styles.resultHeader}>
            <Text style={styles.resultTitle}>Analiz Sonucu</Text>
            <SentimentBadge sentiment={result.sentiment} size="medium" />
          </View>

          <View style={styles.scoreContainer}>
            <View style={styles.scoreItem}>
              <Text style={styles.scoreLabel}>Pozitif</Text>
              <View style={styles.scoreBar}>
                <View style={[styles.scoreFill, { width: `${result.positiveScore * 100}%`, backgroundColor: '#10b981' }]} />
              </View>
              <Text style={styles.scoreValue}>%{(result.positiveScore * 100).toFixed(0)}</Text>
            </View>
            <View style={styles.scoreItem}>
              <Text style={styles.scoreLabel}>Negatif</Text>
              <View style={styles.scoreBar}>
                <View style={[styles.scoreFill, { width: `${result.negativeScore * 100}%`, backgroundColor: '#ef4444' }]} />
              </View>
              <Text style={styles.scoreValue}>%{(result.negativeScore * 100).toFixed(0)}</Text>
            </View>
            <View style={styles.scoreItem}>
              <Text style={styles.scoreLabel}>Nötr</Text>
              <View style={styles.scoreBar}>
                <View style={[styles.scoreFill, { width: `${result.neutralScore * 100}%`, backgroundColor: '#6b7280' }]} />
              </View>
              <Text style={styles.scoreValue}>%{(result.neutralScore * 100).toFixed(0)}</Text>
            </View>
          </View>

          <View style={styles.divider} />

          <View style={styles.confidenceContainer}>
            <Text style={styles.confidenceLabel}>Güven Seviyesi</Text>
            <View style={styles.confidenceBarWrapper}>
              <View style={styles.confidenceBar}>
                <View style={[styles.confidenceFill, { width: `${result.confidence * 100}%`, backgroundColor: getSentimentScoreColor(result.confidence) }]} />
              </View>
              <Text style={styles.confidenceValue}>%{(result.confidence * 100).toFixed(0)}</Text>
            </View>
          </View>

          {result.keywords.length > 0 && (
            <View style={styles.keywordsContainer}>
              <Text style={styles.keywordsTitle}>🔑 Tespit Edilen Anahtar Kelimeler</Text>
              <View style={styles.keywordsList}>
                {result.keywords.map((keyword, index) => (
                  <View key={index} style={styles.keywordBadge}>
                    <Text style={styles.keywordText}>{keyword}</Text>
                  </View>
                ))}
              </View>
            </View>
          )}

          <View style={styles.analysisMeta}>
            <Icon name="time-outline" size={12} color="#aaa" />
            <Text style={styles.analysisTime}>
              Analiz tarihi: {new Date(result.analyzedAt).toLocaleString('tr-TR')}
            </Text>
          </View>
        </View>
      )}

      {/* Otel Duygu Özeti Bölümü */}
      <View style={styles.card}>
        <View style={styles.cardHeader}>
          <Icon name="stats-chart-outline" size={24} color="#007AFF" />
          <Text style={styles.cardTitle}>Otel Duygu Özeti</Text>
        </View>

        <View style={styles.summaryInputRow}>
          <TextInput
            style={styles.summaryInput}
            placeholder="Otel ID (örn: 1, 2, 3)"
            value={itemId}
            onChangeText={setItemId}
            keyboardType="numeric"
          />
          <View style={styles.typeSelector}>
            <TouchableOpacity
              style={[styles.typeOption, itemType === 'Hotel' && styles.typeOptionActive]}
              onPress={() => setItemType('Hotel')}
            >
              <Text style={[styles.typeOptionText, itemType === 'Hotel' && styles.typeOptionTextActive]}>Otel</Text>
            </TouchableOpacity>
            <TouchableOpacity
              style={[styles.typeOption, itemType === 'Restaurant' && styles.typeOptionActive]}
              onPress={() => setItemType('Restaurant')}
            >
              <Text style={[styles.typeOptionText, itemType === 'Restaurant' && styles.typeOptionTextActive]}>Restoran</Text>
            </TouchableOpacity>
          </View>
        </View>

        <TouchableOpacity style={styles.summaryButton} onPress={handleLoadSummary}>
          <Text style={styles.summaryButtonText}>Özet Getir</Text>
        </TouchableOpacity>
      </View>

      {/* Özet Sonuçları */}
      {showSummary && summary.length > 0 && (
        <View style={styles.summaryCard}>
          <Text style={styles.summaryTitle}>📊 Duygu Analizi Özeti</Text>
          {summary.map((item, index) => (
            <View key={index} style={styles.summaryItem}>
              <View style={styles.summaryHeader}>
                <Text style={styles.summaryItemTitle}>
                  {item.itemType} #{item.itemId}
                </Text>
                <Text style={styles.summaryRating}>⭐ {item.averageRating.toFixed(1)}</Text>
              </View>

              <View style={styles.sentimentBars}>
                <View style={styles.sentimentBarItem}>
                  <View style={styles.sentimentBarLabel}>
                    <Text style={styles.sentimentBarText}>😊 Pozitif</Text>
                    <Text style={styles.sentimentBarPercent}>{item.positivePercentage.toFixed(0)}%</Text>
                  </View>
                  <View style={styles.sentimentBarBg}>
                    <View style={[styles.sentimentBarFill, { width: `${item.positivePercentage}%`, backgroundColor: '#10b981' }]} />
                  </View>
                </View>
                <View style={styles.sentimentBarItem}>
                  <View style={styles.sentimentBarLabel}>
                    <Text style={styles.sentimentBarText}>😞 Negatif</Text>
                    <Text style={styles.sentimentBarPercent}>{item.negativePercentage.toFixed(0)}%</Text>
                  </View>
                  <View style={styles.sentimentBarBg}>
                    <View style={[styles.sentimentBarFill, { width: `${item.negativePercentage}%`, backgroundColor: '#ef4444' }]} />
                  </View>
                </View>
              </View>

              <View style={styles.summaryStats}>
                <View style={styles.summaryStat}>
                  <Text style={styles.summaryStatValue}>{item.totalReviews}</Text>
                  <Text style={styles.summaryStatLabel}>Toplam Yorum</Text>
                </View>
                <View style={styles.summaryStat}>
                  <Text style={[styles.summaryStatValue, { color: '#10b981' }]}>{item.positiveCount}</Text>
                  <Text style={styles.summaryStatLabel}>Pozitif</Text>
                </View>
                <View style={styles.summaryStat}>
                  <Text style={[styles.summaryStatValue, { color: '#ef4444' }]}>{item.negativeCount}</Text>
                  <Text style={styles.summaryStatLabel}>Negatif</Text>
                </View>
              </View>
            </View>
          ))}
        </View>
      )}

      {showSummary && summary.length === 0 && !loading && (
        <View style={styles.emptyContainer}>
          <Icon name="document-text-outline" size={48} color="#ccc" />
          <Text style={styles.emptyText}>Bu otel için duygu analizi verisi bulunamadı</Text>
        </View>
      )}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#f5f5f5',
  },
  card: {
    backgroundColor: 'white',
    borderRadius: 16,
    margin: 16,
    padding: 16,
    elevation: 2,
    shadowColor: '#000',
    shadowOffset: { width: 0, height: 2 },
    shadowOpacity: 0.05,
    shadowRadius: 4,
  },
  cardHeader: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
    marginBottom: 16,
  },
  cardTitle: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  textArea: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 12,
    padding: 12,
    fontSize: 14,
    minHeight: 120,
    textAlignVertical: 'top',
    backgroundColor: '#fafafa',
  },
  buttonRow: {
    flexDirection: 'row',
    gap: 12,
    marginTop: 16,
  },
  clearButton: {
    flex: 1,
    paddingVertical: 12,
    borderRadius: 10,
    borderWidth: 1,
    borderColor: '#ddd',
    alignItems: 'center',
  },
  clearButtonText: {
    color: '#666',
    fontWeight: '500',
  },
  analyzeButton: {
    flex: 2,
    paddingVertical: 12,
    borderRadius: 10,
    backgroundColor: '#007AFF',
    alignItems: 'center',
  },
  analyzeButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  loader: {
    alignItems: 'center',
    paddingVertical: 20,
  },
  loaderText: {
    marginTop: 8,
    color: '#666',
  },
  resultCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    marginHorizontal: 16,
    marginBottom: 16,
    padding: 16,
    elevation: 2,
  },
  resultHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 16,
  },
  resultTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  scoreContainer: {
    gap: 12,
  },
  scoreItem: {
    gap: 4,
  },
  scoreLabel: {
    fontSize: 12,
    color: '#666',
  },
  scoreBar: {
    height: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 4,
    overflow: 'hidden',
  },
  scoreFill: {
    height: '100%',
    borderRadius: 4,
  },
  scoreValue: {
    fontSize: 12,
    fontWeight: '500',
    textAlign: 'right',
  },
  divider: {
    height: 1,
    backgroundColor: '#f0f0f0',
    marginVertical: 16,
  },
  confidenceContainer: {
    marginBottom: 16,
  },
  confidenceLabel: {
    fontSize: 12,
    color: '#666',
    marginBottom: 4,
  },
  confidenceBarWrapper: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  confidenceBar: {
    flex: 1,
    height: 6,
    backgroundColor: '#f0f0f0',
    borderRadius: 3,
    overflow: 'hidden',
  },
  confidenceFill: {
    height: '100%',
    borderRadius: 3,
  },
  confidenceValue: {
    fontSize: 12,
    fontWeight: 'bold',
    width: 40,
    textAlign: 'right',
  },
  keywordsContainer: {
    marginTop: 8,
  },
  keywordsTitle: {
    fontSize: 13,
    fontWeight: '500',
    color: '#333',
    marginBottom: 8,
  },
  keywordsList: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 8,
  },
  keywordBadge: {
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 10,
    paddingVertical: 5,
    borderRadius: 16,
  },
  keywordText: {
    fontSize: 12,
    color: '#555',
  },
  analysisMeta: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 4,
    marginTop: 12,
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  analysisTime: {
    fontSize: 10,
    color: '#aaa',
  },
  summaryInputRow: {
    gap: 12,
  },
  summaryInput: {
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 10,
    paddingHorizontal: 12,
    paddingVertical: 10,
    fontSize: 14,
  },
  typeSelector: {
    flexDirection: 'row',
    gap: 8,
  },
  typeOption: {
    flex: 1,
    paddingVertical: 10,
    alignItems: 'center',
    borderRadius: 10,
    backgroundColor: '#f0f0f0',
  },
  typeOptionActive: {
    backgroundColor: '#007AFF',
  },
  typeOptionText: {
    color: '#666',
    fontWeight: '500',
  },
  typeOptionTextActive: {
    color: 'white',
  },
  summaryButton: {
    backgroundColor: '#10b981',
    borderRadius: 10,
    paddingVertical: 12,
    alignItems: 'center',
    marginTop: 16,
  },
  summaryButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  summaryCard: {
    backgroundColor: 'white',
    borderRadius: 16,
    marginHorizontal: 16,
    marginBottom: 20,
    padding: 16,
    elevation: 2,
  },
  summaryTitle: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
    marginBottom: 16,
  },
  summaryItem: {
    backgroundColor: '#f8f9fa',
    borderRadius: 12,
    padding: 12,
    marginBottom: 12,
  },
  summaryHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  summaryItemTitle: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
  },
  summaryRating: {
    fontSize: 13,
    color: '#f59e0b',
  },
  sentimentBars: {
    gap: 8,
    marginBottom: 12,
  },
  sentimentBarItem: {
    gap: 4,
  },
  sentimentBarLabel: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  sentimentBarText: {
    fontSize: 11,
    color: '#666',
  },
  sentimentBarPercent: {
    fontSize: 11,
    fontWeight: '500',
  },
  sentimentBarBg: {
    height: 6,
    backgroundColor: '#e0e0e0',
    borderRadius: 3,
    overflow: 'hidden',
  },
  sentimentBarFill: {
    height: '100%',
    borderRadius: 3,
  },
  summaryStats: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#e0e0e0',
  },
  summaryStat: {
    alignItems: 'center',
  },
  summaryStatValue: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  summaryStatLabel: {
    fontSize: 10,
    color: '#888',
    marginTop: 2,
  },
  emptyContainer: {
    alignItems: 'center',
    paddingVertical: 40,
    marginBottom: 20,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
    marginTop: 12,
    textAlign: 'center',
  },
});