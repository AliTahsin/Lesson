import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  ScrollView,
  StyleSheet,
  ActivityIndicator,
  RefreshControl,
  TouchableOpacity
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { staffApi } from '../services/staffApi';
import { signalRService } from '../services/signalRService';
import { useStaffAuthContext } from '../context/StaffAuthContext';
import { DashboardStats } from '../types/staff';

export const DashboardScreen = ({ navigation }: any) => {
  const { staff, role, hotelId } = useStaffAuthContext();
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [unreadCount, setUnreadCount] = useState(0);

  useEffect(() => {
    loadDashboard();
    setupSignalR();
    return () => {
      signalRService.disconnect();
    };
  }, []);

  const loadDashboard = async () => {
    setLoading(true);
    try {
      const [statsData, count] = await Promise.all([
        staffApi.getDashboardStats(),
        staffApi.getUnreadCount()
      ]);
      setStats(statsData);
      setUnreadCount(count);
    } catch (error) {
      console.error('Error loading dashboard:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  const setupSignalR = async () => {
    if (!hotelId || !staff?.id) return;
    
    await signalRService.connect(hotelId, staff.id);
    
    signalRService.on('NewTask', () => {
      loadDashboard();
    });
    
    signalRService.on('CriticalIssue', () => {
      loadDashboard();
    });
    
    signalRService.on('NewNotification', async () => {
      const count = await staffApi.getUnreadCount();
      setUnreadCount(count);
    });
  };

  const onRefresh = () => {
    setRefreshing(true);
    loadDashboard();
  };

  const getRoleColor = (roleName: string) => {
    switch (roleName) {
      case 'Admin': return '#ef4444';
      case 'FrontDesk': return '#3b82f6';
      case 'Housekeeping': return '#10b981';
      case 'Maintenance': return '#f59e0b';
      case 'Restaurant': return '#8b5cf6';
      default: return '#6b7280';
    }
  };

  if (loading && !refreshing) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <ScrollView
      style={styles.container}
      refreshControl={
        <RefreshControl refreshing={refreshing} onRefresh={onRefresh} />
      }
    >
      {/* Welcome Card */}
      <View style={styles.welcomeCard}>
        <View>
          <Text style={styles.welcomeText}>Hoş Geldiniz,</Text>
          <Text style={styles.userName}>{staff?.fullName}</Text>
          <View style={[styles.roleBadge, { backgroundColor: getRoleColor(role || '') }]}>
            <Text style={styles.roleText}>{role}</Text>
          </View>
        </View>
        <TouchableOpacity onPress={() => navigation.navigate('Notifications')}>
          <View style={styles.notificationIcon}>
            <Icon name="notifications-outline" size={24} color="#333" />
            {unreadCount > 0 && (
              <View style={styles.notificationBadge}>
                <Text style={styles.notificationBadgeText}>
                  {unreadCount > 99 ? '99+' : unreadCount}
                </Text>
              </View>
            )}
          </View>
        </TouchableOpacity>
      </View>

      {/* Stats Grid */}
      <View style={styles.statsGrid}>
        <View style={[styles.statCard, styles.blueCard]}>
          <Text style={styles.statValue}>{stats?.pendingTasks || 0}</Text>
          <Text style={styles.statLabel}>Bekleyen Görev</Text>
        </View>
        <View style={[styles.statCard, styles.purpleCard]}>
          <Text style={styles.statValue}>{stats?.inProgressTasks || 0}</Text>
          <Text style={styles.statLabel}>Devam Eden</Text>
        </View>
        <View style={[styles.statCard, styles.greenCard]}>
          <Text style={styles.statValue}>{stats?.completedToday || 0}</Text>
          <Text style={styles.statLabel}>Bugün Tamamlanan</Text>
        </View>
        <View style={[styles.statCard, styles.redCard]}>
          <Text style={styles.statValue}>{stats?.criticalIssues || 0}</Text>
          <Text style={styles.statLabel}>Kritik Arıza</Text>
        </View>
      </View>

      {/* Quick Actions */}
      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Hızlı İşlemler</Text>
        <View style={styles.quickActions}>
          {(role === 'Admin' || role === 'FrontDesk') && (
            <>
              <TouchableOpacity
                style={styles.quickButton}
                onPress={() => navigation.navigate('CheckIn')}
              >
                <Icon name="log-in-outline" size={28} color="#3b82f6" />
                <Text style={styles.quickButtonText}>Check-in</Text>
              </TouchableOpacity>
              <TouchableOpacity
                style={styles.quickButton}
                onPress={() => navigation.navigate('CheckOut')}
              >
                <Icon name="log-out-outline" size={28} color="#f59e0b" />
                <Text style={styles.quickButtonText}>Check-out</Text>
              </TouchableOpacity>
            </>
          )}
          {(role === 'Admin' || role === 'Housekeeping') && (
            <TouchableOpacity
              style={styles.quickButton}
              onPress={() => navigation.navigate('Tasks')}
            >
              <Icon name="checkbox-outline" size={28} color="#10b981" />
              <Text style={styles.quickButtonText}>Görevler</Text>
            </TouchableOpacity>
          )}
          {(role === 'Admin' || role === 'Maintenance') && (
            <TouchableOpacity
              style={styles.quickButton}
              onPress={() => navigation.navigate('Issues')}
            >
              <Icon name="warning-outline" size={28} color="#ef4444" />
              <Text style={styles.quickButtonText}>Arızalar</Text>
            </TouchableOpacity>
          )}
        </View>
      </View>

      {/* Today's Check-ins/outs */}
      {(role === 'Admin' || role === 'FrontDesk') && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Bugün</Text>
          <View style={styles.todayStats}>
            <View style={styles.todayItem}>
              <Icon name="log-in-outline" size={20} color="#3b82f6" />
              <Text style={styles.todayLabel}>Check-in</Text>
              <Text style={styles.todayValue}>{stats?.todayCheckIns || 0}</Text>
            </View>
            <View style={styles.todayDivider} />
            <View style={styles.todayItem}>
              <Icon name="log-out-outline" size={20} color="#f59e0b" />
              <Text style={styles.todayLabel}>Check-out</Text>
              <Text style={styles.todayValue}>{stats?.todayCheckOuts || 0}</Text>
            </View>
          </View>
        </View>
      )}

      {/* Tasks by Type */}
      {stats?.tasksByType && Object.keys(stats.tasksByType).length > 0 && (
        <View style={styles.section}>
          <Text style={styles.sectionTitle}>Görev Dağılımı</Text>
          {Object.entries(stats.tasksByType).map(([type, count]) => (
            <View key={type} style={styles.distributionRow}>
              <Text style={styles.distributionLabel}>{type}</Text>
              <View style={styles.distributionBar}>
                <View style={[styles.distributionFill, { width: `${Math.min(100, count * 10)}%` }]} />
              </View>
              <Text style={styles.distributionCount}>{count}</Text>
            </View>
          ))}
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
  center: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
  },
  welcomeCard: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    backgroundColor: 'white',
    margin: 16,
    padding: 20,
    borderRadius: 16,
    elevation: 2,
  },
  welcomeText: {
    fontSize: 14,
    color: '#666',
  },
  userName: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 4,
  },
  roleBadge: {
    paddingHorizontal: 10,
    paddingVertical: 4,
    borderRadius: 12,
    marginTop: 8,
    alignSelf: 'flex-start',
  },
  roleText: {
    color: 'white',
    fontSize: 11,
    fontWeight: 'bold',
  },
  notificationIcon: {
    position: 'relative',
  },
  notificationBadge: {
    position: 'absolute',
    top: -5,
    right: -10,
    backgroundColor: '#ef4444',
    borderRadius: 10,
    minWidth: 18,
    height: 18,
    justifyContent: 'center',
    alignItems: 'center',
  },
  notificationBadgeText: {
    color: 'white',
    fontSize: 10,
    fontWeight: 'bold',
  },
  statsGrid: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    paddingHorizontal: 12,
    gap: 12,
  },
  statCard: {
    flex: 1,
    minWidth: '45%',
    borderRadius: 12,
    padding: 16,
    alignItems: 'center',
  },
  blueCard: {
    backgroundColor: '#3b82f6',
  },
  purpleCard: {
    backgroundColor: '#8b5cf6',
  },
  greenCard: {
    backgroundColor: '#10b981',
  },
  redCard: {
    backgroundColor: '#ef4444',
  },
  statValue: {
    fontSize: 28,
    fontWeight: 'bold',
    color: 'white',
  },
  statLabel: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  section: {
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
  quickActions: {
    flexDirection: 'row',
    flexWrap: 'wrap',
    gap: 12,
  },
  quickButton: {
    alignItems: 'center',
    backgroundColor: '#f0f0f0',
    padding: 12,
    borderRadius: 12,
    width: '30%',
  },
  quickButtonText: {
    fontSize: 12,
    color: '#333',
    marginTop: 6,
  },
  todayStats: {
    flexDirection: 'row',
    justifyContent: 'space-around',
  },
  todayItem: {
    alignItems: 'center',
  },
  todayLabel: {
    fontSize: 12,
    color: '#666',
    marginTop: 4,
  },
  todayValue: {
    fontSize: 20,
    fontWeight: 'bold',
    color: '#333',
    marginTop: 4,
  },
  todayDivider: {
    width: 1,
    backgroundColor: '#e0e0e0',
  },
  distributionRow: {
    flexDirection: 'row',
    alignItems: 'center',
    marginBottom: 12,
  },
  distributionLabel: {
    width: 80,
    fontSize: 13,
    color: '#666',
  },
  distributionBar: {
    flex: 1,
    height: 8,
    backgroundColor: '#f0f0f0',
    borderRadius: 4,
    marginHorizontal: 12,
    overflow: 'hidden',
  },
  distributionFill: {
    height: '100%',
    backgroundColor: '#007AFF',
    borderRadius: 4,
  },
  distributionCount: {
    width: 30,
    fontSize: 13,
    fontWeight: 'bold',
    color: '#333',
    textAlign: 'right',
  },
});