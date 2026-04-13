import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  TextInput
} from 'react-native';
import { Camera, useCameraDevices } from 'react-native-camera';
import Icon from 'react-native-vector-icons/Ionicons';
import { attendeeApi } from '../services/attendeeApi';
import { Attendee } from '../types/mice';

export const AttendeeCheckInScreen = ({ route }: any) => {
  const { eventId, eventName } = route.params;
  const [attendees, setAttendees] = useState<Attendee[]>([]);
  const [loading, setLoading] = useState(true);
  const [scanning, setScanning] = useState(false);
  const [searchText, setSearchText] = useState('');
  const devices = useCameraDevices();
  const device = devices.back;

  useEffect(() => {
    loadAttendees();
  }, []);

  const loadAttendees = async () => {
    setLoading(true);
    try {
      const data = await attendeeApi.getAttendeesByEvent(eventId);
      setAttendees(data);
    } catch (error) {
      console.error('Error loading attendees:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleCheckIn = async (attendeeId: number) => {
    try {
      await attendeeApi.checkInAttendee(attendeeId);
      Alert.alert('Başarılı', 'Katılımcı giriş yaptı');
      loadAttendees();
    } catch (error) {
      Alert.alert('Hata', 'Giriş yapılamadı');
    }
  };

  const handleQRCodeScan = async (qrCode: string) => {
    setScanning(false);
    try {
      const attendee = await attendeeApi.checkInByQrCode(qrCode);
      Alert.alert('Başarılı', `${attendee.fullName} giriş yaptı`);
      loadAttendees();
    } catch (error) {
      Alert.alert('Hata', 'Geçersiz QR kod');
    }
  };

  const filteredAttendees = attendees.filter(a =>
    a.fullName.toLowerCase().includes(searchText.toLowerCase()) ||
    a.email.toLowerCase().includes(searchText.toLowerCase()) ||
    a.company.toLowerCase().includes(searchText.toLowerCase())
  );

  const checkedInCount = attendees.filter(a => a.hasCheckedIn).length;
  const totalCount = attendees.length;

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  if (scanning && device) {
    return (
      <View style={styles.cameraContainer}>
        <Camera
          style={styles.camera}
          device={device}
          onFrame={async (frame) => {
            // QR code scanning logic would go here
            // For demo, we'll use a mock
          }}
        />
        <TouchableOpacity style={styles.closeCamera} onPress={() => setScanning(false)}>
          <Icon name="close" size={24} color="white" />
        </TouchableOpacity>
      </View>
    );
  }

  return (
    <View style={styles.container}>
      <View style={styles.statsCard}>
        <Text style={styles.eventName}>{eventName}</Text>
        <View style={styles.statsRow}>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{checkedInCount}</Text>
            <Text style={styles.statLabel}>Giriş Yapan</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{totalCount}</Text>
            <Text style={styles.statLabel}>Toplam Kayıtlı</Text>
          </View>
          <View style={styles.statItem}>
            <Text style={styles.statNumber}>{totalCount > 0 ? ((checkedInCount / totalCount) * 100).toFixed(1) : 0}%</Text>
            <Text style={styles.statLabel}>Katılım Oranı</Text>
          </View>
        </View>
      </View>

      <View style={styles.searchContainer}>
        <TextInput
          style={styles.searchInput}
          placeholder="Katılımcı ara..."
          value={searchText}
          onChangeText={setSearchText}
        />
        <TouchableOpacity style={styles.qrButton} onPress={() => setScanning(true)}>
          <Icon name="qr-code" size={24} color="white" />
          <Text style={styles.qrButtonText}>QR Okut</Text>
        </TouchableOpacity>
      </View>

      <FlatList
        data={filteredAttendees}
        keyExtractor={(item) => item.id.toString()}
        renderItem={({ item }) => (
          <View style={styles.attendeeCard}>
            <View style={styles.attendeeInfo}>
              <Text style={styles.attendeeName}>{item.fullName}</Text>
              <Text style={styles.attendeeDetail}>{item.company} • {item.title}</Text>
              <Text style={styles.attendeeDetail}>{item.email}</Text>
            </View>
            {item.hasCheckedIn ? (
              <View style={styles.checkedInBadge}>
                <Icon name="checkmark-circle" size={20} color="#10b981" />
                <Text style={styles.checkedInText}>Giriş Yaptı</Text>
                {item.checkInTime && (
                  <Text style={styles.checkInTime}>
                    {new Date(item.checkInTime).toLocaleTimeString()}
                  </Text>
                )}
              </View>
            ) : (
              <TouchableOpacity
                style={styles.checkInButton}
                onPress={() => handleCheckIn(item.id)}
              >
                <Text style={styles.checkInButtonText}>Giriş Yap</Text>
              </TouchableOpacity>
            )}
          </View>
        )}
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
  statsCard: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  eventName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: 'white',
    marginBottom: 12,
  },
  statsRow: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  statItem: {
    alignItems: 'center',
  },
  statNumber: {
    fontSize: 24,
    fontWeight: 'bold',
    color: 'white',
  },
  statLabel: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  searchContainer: {
    flexDirection: 'row',
    paddingHorizontal: 16,
    marginBottom: 12,
    gap: 12,
  },
  searchInput: {
    flex: 1,
    borderWidth: 1,
    borderColor: '#ddd',
    borderRadius: 8,
    paddingHorizontal: 12,
    paddingVertical: 10,
    backgroundColor: 'white',
  },
  qrButton: {
    backgroundColor: '#007AFF',
    borderRadius: 8,
    paddingHorizontal: 16,
    flexDirection: 'row',
    alignItems: 'center',
    gap: 8,
  },
  qrButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  listContent: {
    paddingBottom: 20,
  },
  attendeeCard: {
    backgroundColor: 'white',
    borderRadius: 12,
    marginHorizontal: 16,
    marginVertical: 6,
    padding: 16,
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
  },
  attendeeInfo: {
    flex: 1,
  },
  attendeeName: {
    fontSize: 16,
    fontWeight: 'bold',
    color: '#333',
  },
  attendeeDetail: {
    fontSize: 12,
    color: '#666',
    marginTop: 2,
  },
  checkedInBadge: {
    alignItems: 'center',
  },
  checkedInText: {
    fontSize: 11,
    color: '#10b981',
    marginTop: 2,
  },
  checkInTime: {
    fontSize: 10,
    color: '#888',
  },
  checkInButton: {
    backgroundColor: '#10b981',
    paddingHorizontal: 16,
    paddingVertical: 8,
    borderRadius: 8,
  },
  checkInButtonText: {
    color: 'white',
    fontWeight: 'bold',
  },
  cameraContainer: {
    flex: 1,
    position: 'relative',
  },
  camera: {
    flex: 1,
  },
  closeCamera: {
    position: 'absolute',
    top: 40,
    right: 20,
    backgroundColor: 'rgba(0,0,0,0.5)',
    borderRadius: 20,
    padding: 8,
  },
});