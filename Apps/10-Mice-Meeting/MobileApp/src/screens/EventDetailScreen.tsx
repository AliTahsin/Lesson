import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  TouchableOpacity,
  Alert,
  Share
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { eventApi } from '../services/eventApi';
import { attendeeApi } from '../services/attendeeApi';
import { Event, EventSchedule, AttendeeStats } from '../types/mice';
import { ScheduleItem } from '../components/ScheduleItem';

export const EventDetailScreen = ({ route, navigation }: any) => {
  const { eventId } = route.params;
  const [event, setEvent] = useState<Event | null>(null);
  const [stats, setStats] = useState<AttendeeStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [showSchedule, setShowSchedule] = useState(true);

  useEffect(() => {
    loadEvent();
  }, []);

  const loadEvent = async () => {
    setLoading(true);
    try {
      const [eventData, statsData] = await Promise.all([
        eventApi.getEventById(eventId),
        attendeeApi.getAttendeeStats(eventId)
      ]);
      setEvent(eventData);
      setStats(statsData);
    } catch (error) {
      console.error('Error loading event:', error);
      Alert.alert('Hata', 'Etkinlik detayı yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const handleShareCalendar = async () => {
    const calendarUrl = await eventApi.downloadCalendar(eventId);
    await Share.share({
      url: calendarUrl,
      message: `${event?.name} etkinliği takvime eklendi`
    });
  };

  const handleStatusChange = async (status: string) => {
    Alert.alert(
      'Durum Değiştir',
      `Etkinlik durumunu "${status}" olarak değiştirmek istediğinize emin misiniz?`,
      [
        { text: 'İptal', style: 'cancel' },
        {
          text: 'Değiştir',
          onPress: async () => {
            await eventApi.updateEventStatus(eventId, status);
            loadEvent();
          }
        }
      ]
    );
  };

  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getDate()} ${getMonthName(date.getMonth())} ${date.getFullYear()}`;
  };

  const formatTime = (dateString: string) => {
    const date = new Date(dateString);
    return `${date.getHours()}:${date.getMinutes().toString().padStart(2, '0')}`;
  };

  const getMonthName = (month: number) => {
    const months = ['Ocak', 'Şubat', 'Mart', 'Nisan', 'Mayıs', 'Haziran', 
                    'Temmuz', 'Ağustos', 'Eylül', 'Ekim', 'Kasım', 'Aralık'];
    return months[month];
  };

  const getStatusColor = (status: string) => {
    switch (status) {
      case 'Planned': return '#f59e0b';
      case 'Confirmed': return '#3b82f6';
      case 'InProgress': return '#8b5cf6';
      case 'Completed': return '#10b981';
      case 'Cancelled': return '#ef4444';
      default: return '#6b7280';
    }
  };

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (!event) {
    return (
      <View style={styles.center}>
        <Text>Etkinlik bulunamadı</Text>
      </View>
    );
  }

  return (
    <ScrollView style={styles.container}>
      <View style={styles.headerCard}>
        <View>
          <Text style={styles.eventName}>{event.name}</Text>
          <Text style={styles.eventNumber}>{event.eventNumber}</Text>
        </View>
        <View style={[styles.statusBadge, { backgroundColor: getStatusColor(event.status) }]}>
          <Text style={styles.statusText}>{event.status}</Text>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Tarih ve Saat</Text>
        <View style={styles.dateRow}>
          <Icon name="calendar-outline" size={20} color="#666" />
          <View>
            <Text style={styles.dateText}>{formatDate(event.startDate)}</Text>
            <Text style={styles.dateSubtext}>
              {formatTime(event.startDate)} - {formatTime(event.endDate)}
            </Text>
          </View>
        </View>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Konum</Text>
        <Text style={styles.locationText}>{event.meetingRoomName}</Text>
      </View>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Katılımcı Bilgileri</Text>
        <View style={styles.statsRow}>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{stats?.totalRegistered || 0}</Text>
            <Text style={styles.statLabel}>Kayıtlı</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{stats?.checkedIn || 0}</Text>
            <Text style={styles.statLabel}>Giriş Yapan</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{stats?.checkedInRate.toFixed(1)}%</Text>
            <Text style={styles.statLabel}>Katılım Oranı</Text>
          </View>
        </View>
        <TouchableOpacity
          style={styles.attendeesButton}
          onPress={() => navigation.navigate('AttendeeCheckIn', { eventId: event.id, eventName: event.name })}
        >
          <Text style={styles.attendeesButtonText}>Katılımcıları Yönet</Text>
        </TouchableOpacity>
      </View>

      <View style={styles.section}>
        <View style={styles.scheduleHeader}>
          <Text style={styles.sectionTitle}>Etkinlik Programı</Text>
          <TouchableOpacity onPress={() => setShowSchedule(!showSchedule)}>
            <Icon name={showSchedule ? "chevron-up" : "chevron-down"} size={20} color="#007AFF" />
          </TouchableOpacity>
        </View>
        {showSchedule && event.schedule?.map((item: EventSchedule) => (
          <ScheduleItem key={item.id} schedule={item} />
        ))}
      </View>

      {event.specialRequests && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Özel İstekler</Text>
          <Text style={styles.specialRequests}>{event.specialRequests}</Text>
        </View>
      )}

      <View style={styles.actionButtons}>
        <TouchableOpacity style={styles.calendarButton} onPress={handleShareCalendar}>
          <Icon name="calendar" size={20} color="white" />
          <Text style={styles.actionButtonText}>Takvime Ekle</Text>
        </TouchableOpacity>
        
        {event.status === 'Planned' && (
          <TouchableOpacity style={styles.confirmButton} onPress={() => handleStatusChange('Confirmed')}>
            <Icon name="checkmark" size={20} color="white" />
            <Text style={styles.actionButtonText}>Onayla</Text>
          </TouchableOpacity>
        )}
        
        {event.status === 'Confirmed' && (
          <TouchableOpacity style={styles.startButton} onPress={() => handleStatusChange('InProgress')}>
            <Icon name="play" size={20} color="white" />
            <Text style={styles.actionButtonText}>Başlat</Text>
          </TouchableOpacity>
        )}
        
        {event.status === 'InProgress' && (
          <TouchableOpacity style={styles.completeButton} onPress={() => handleStatusChange('Completed')}>
            <Icon name="checkmark-done" size={20} color="white" />
            <Text style={styles.actionButtonText}>Tamamla</Text>
          </TouchableOpacity>
        )}
      </View>
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
  headerCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    margin: 16,
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  eventName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: '#333',
  },
  eventNumber: {
    fontSize: 12,
    color: '#888',
    marginTop: 2,
  },
  statusBadge: {
    paddingHorizontal: 12,
    paddingVertical: 6,
    borderRadius: 8,
  },
  statusText: {
    color: 'white',
    fontSize: 12,
    fontWeight: 'bold',
  },
  section: {
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
  dateRow: {
    flexDirection: 'row',
    alignItems: 'center',
    gap: 12,
  },
  dateText: {
    fontSize: 16,
    fontWeight: '500',
    color: '#333',
  },
  dateSubtext: {
    fontSize: 14,
    color: '#666',
    marginTop: 2,
  },
  locationText: {
    fontSize: 16,
    color: '#333',
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
    marginBottom: 16,
  },
  statItem: {
    alignItems: 'center',
  },
  statNumber: {
    fontSize: 24,
    fontWeight: 'bold',
    color: '#007AFF',
  },
  statLabel: {
    fontSize: 12,
    color: '#888',
    marginTop: 4,
  },
  attendeesButton: {
    backgroundColor: '#f0f0f0',
    borderRadius: 8,
    paddingVertical: 10,
    alignItems: 'center',
  },
  attendeesButtonText: {
    color: '#007AFF',
    fontWeight: 'bold',
  },
  scheduleHeader: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  specialRequests: {
    fontSize: 14,
    color: '#666',
    fontStyle: 'italic',
  },
  actionButtons: {
    flexDirection: 'row',
    marginHorizontal: 16,
    marginBottom: 20,
    gap: 12,
  },
  calendarButton: {
    flex: 1,
    backgroundColor: '#007AFF',
    borderRadius: 12,
    paddingVertical: 12,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  confirmButton: {
    flex: 1,
    backgroundColor: '#10b981',
    borderRadius: 12,
    paddingVertical: 12,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  startButton: {
    flex: 1,
    backgroundColor: '#8b5cf6',
    borderRadius: 12,
    paddingVertical: 12,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  completeButton: {
    flex: 1,
    backgroundColor: '#16a34a',
    borderRadius: 12,
    paddingVertical: 12,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'center',
    gap: 8,
  },
  actionButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
});