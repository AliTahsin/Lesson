import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { chatApi } from '../services/chatApi';
import { Conversation } from '../types/chat';
import { useAuth } from '../../../12-Auth-RBAC/MobileApp/src/context/AuthContext';

export const ChatHistoryScreen = ({ navigation }: any) => {
  const [conversations, setConversations] = useState<Conversation[]>([]);
  const [loading, setLoading] = useState(true);
  const { user, hasPermission } = useAuth();
  const isAdmin = hasPermission('chat:view');

  useEffect(() => {
    if (isAdmin) {
      loadAllConversations();
    }
  }, []);

  const loadAllConversations = async () => {
    setLoading(true);
    try {
      const stats = await chatApi.getChatStatistics();
      // Mock data for demonstration
      const mockConversations: Conversation[] = [
        {
          id: 1,
          userName: "Ahmet Yılmaz",
          userEmail: "ahmet@email.com",
          status: "Active",
          messageCount: 12,
          startedAt: new Date().toISOString(),
          isBotActive: true
        },
        {
          id: 2,
          userName: "Ayşe Demir",
          userEmail: "ayse@email.com",
          status: "Resolved",
          messageCount: 8,
          startedAt: new Date(Date.now() - 86400000).toISOString(),
          isBotActive: false
        }
      ];
      setConversations(mockConversations);
    } catch (error) {
      console.error('Error loading conversations:', error);
      Alert.alert('Hata', 'Konuşmalar yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Active': return '#10b981';
      case 'Resolved': return '#6b7280';
      case 'Transferred': return '#f59e0b';
      default: return '#6b7280';
    }
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()} ${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!isAdmin) {
    return (
      <View style={styles.center}>
        <Icon name="lock-closed" size={48} color="#ccc" />
        <Text style={styles.permissionText}>Bu sayfaya erişim yetkiniz yok</Text>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <FlatList
        data={conversations}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <TouchableOpacity
            style={styles.conversationCard}
            onPress={() => navigation.navigate('Chat', { conversationId: item.id })}
          >
            <View style={styles.conversationHeader}>
              <View>
                <Text style={styles.userName}>{item.userName}</Text>
                <Text style={styles.userEmail}>{item.userEmail}</Text>
              </View>
              <View style={[styles.statusBadge, { backgroundColor: getStatusColor(item.status) }]}>
                <Text style={styles.statusText}>{item.status}</Text>
              </View>
            </View>
            
            <View style={styles.conversationFooter}>
              <Text style={styles.messageCount}>{item.messageCount} mesaj</Text>
              <Text style={styles.date}>{formatDate(item.startedAt)}</Text>
            </View>
          </TouchableOpacity>
        )}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Icon name="chatbubbles-outline" size={64} color="#ccc" />
            <Text style={styles.emptyText}>Henüz konuşma yok</Text>
          </View>
        }
        contentContainerStyle={styles.listContent}
      />
    </View>
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
  permissionText: {
    fontSize: 14,
    color: '#888',
    marginTop: 12,
  },
  listContent: {
    padding: 16,
  },
  conversationCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    padding: 16,
    marginBottom: 12,
    elevation: 2,
  },
  conversationHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginBottom: 12,
  },
  userName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  userEmail: {
    fontSize: 12,
    color: '#888',
    marginTop: 2,
  },
  statusBadge: {
    paddingHorizontal: 8,
    paddingVertical: 4,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  conversationFooter: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    paddingTop: 12,
    borderTopWidth: 1,
    borderTopColor: '#f0f0f0',
  },
  messageCount: {
    fontSize: 12,
    color: '#888',
  },
  date: {
    fontSize: 12,
    color: '#888',
  },
  emptyContainer: {
    alignItems: 'center',
    paddingTop: 100,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
    marginTop: 16,
  },
});