import React, { useState, useEffect } from 'react';
import {
  View,
  Text,
  FlatList,
  TouchableOpacity,
  StyleSheet,
  ActivityIndicator,
  Alert,
  SectionList
} from 'react-native';
import Icon from 'react-native-vector-icons/Ionicons';
import { authApi } from '../services/authApi';
import { useAuth } from '../context/AuthContext';
import { Permission, Role } from '../types/auth';

export const PermissionListScreen = ({ route }: any) => {
  const { roleId, roleName } = route.params || {};
  const [role, setRole] = useState<Role | null>(null);
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [rolePermissions, setRolePermissions] = useState<string[]>([]);
  const [loading, setLoading] = useState(true);
  const { hasPermission } = useAuth();

  const canManage = hasPermission('role:manage');

  useEffect(() => {
    loadData();
  }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const [permissionsData, roleData] = await Promise.all([
        authApi.getPermissions(),
        roleId ? authApi.getRoleById(roleId) : Promise.resolve(null)
      ]);
      setPermissions(permissionsData);
      if (roleData) {
        setRole(roleData);
        setRolePermissions(roleData.permissions || []);
      }
    } catch (error) {
      console.error('Error loading permissions:', error);
      Alert.alert('Hata', 'İzinler yüklenirken bir sorun oluştu');
    } finally {
      setLoading(false);
    }
  };

  const togglePermission = async (permissionCode: string, permissionId: number) => {
    if (!roleId || !canManage) return;

    const hasPermission = rolePermissions.includes(permissionCode);
    
    try {
      if (hasPermission) {
        await authApi.removePermissionFromRole(roleId, permissionId);
        setRolePermissions(rolePermissions.filter(p => p !== permissionCode));
      } else {
        await authApi.addPermissionToRole(roleId, permissionId);
        setRolePermissions([...rolePermissions, permissionCode]);
      }
    } catch (error) {
      Alert.alert('Hata', 'İşlem başarısız');
    }
  };

  const groupedPermissions = permissions.reduce((acc, permission) => {
    const category = permission.category;
    if (!acc[category]) {
      acc[category] = [];
    }
    acc[category].push(permission);
    return acc;
  }, {} as Record<string, Permission[]>);

  const sections = Object.entries(groupedPermissions).map(([title, data]) => ({
    title,
    data
  }));

  const renderPermissionItem = ({ item }: { item: Permission }) => {
    const isSelected = rolePermissions.includes(item.code);
    
    return (
      <TouchableOpacity
        style={styles.permissionItem}
        onPress={() => togglePermission(item.code, item.id)}
        disabled={!canManage || !roleId}
      >
        <View style={styles.permissionInfo}>
          <Text style={styles.permissionName}>{item.name}</Text>
          <Text style={styles.permissionCode}>{item.code}</Text>
          <Text style={styles.permissionDescription}>{item.description}</Text>
        </View>
        {roleId && (
          <View style={[styles.checkbox, isSelected && styles.checkboxSelected]}>
            {isSelected && <Icon name="checkmark" size={16} color="white" />}
          </View>
        )}
      </TouchableOpacity>
    );
  };

  const renderSectionHeader = ({ section: { title } }: any) => (
    <View style={styles.sectionHeader}>
      <Text style={styles.sectionHeaderText}>{title}</Text>
    </View>
  );

  if (loading) {
    return (
      <View style={styles.center}>
        <ActivityIndicator size="large" />
      </View>
    );
  }

  return (
    <View style={styles.container}>
      {role && (
        <View style={styles.roleHeader}>
          <Text style={styles.roleName}>{role.name}</Text>
          <Text style={styles.roleDescription}>{role.description}</Text>
        </View>
      )}
      
      <SectionList
        sections={sections}
        keyExtractor={(item) => item.id.toString()}
        renderItem={renderPermissionItem}
        renderSectionHeader={renderSectionHeader}
        contentContainerStyle={styles.listContent}
        ListEmptyComponent={
          <View style={styles.emptyContainer}>
            <Text style={styles.emptyText}>İzin bulunamadı</Text>
          </View>
        }
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
  roleHeader: {
    backgroundColor: '#007AFF',
    borderRadius: 12,
    margin: 16,
    padding: 16,
  },
  roleName: {
    fontSize: 18,
    fontWeight: 'bold',
    color: 'white',
  },
  roleDescription: {
    fontSize: 12,
    color: 'white',
    opacity: 0.9,
    marginTop: 4,
  },
  listContent: {
    paddingBottom: 20,
  },
  sectionHeader: {
    backgroundColor: '#f0f0f0',
    paddingHorizontal: 16,
    paddingVertical: 8,
    marginTop: 8,
  },
  sectionHeaderText: {
    fontSize: 14,
    fontWeight: 'bold',
    color: '#333',
  },
  permissionItem: {
    backgroundColor: 'white',
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    marginHorizontal: 16,
    marginVertical: 4,
    padding: 12,
    borderRadius: 8,
    elevation: 1,
  },
  permissionInfo: {
    flex: 1,
  },
  permissionName: {
    fontSize: 14,
    fontWeight: '500',
    color: '#333',
  },
  permissionCode: {
    fontSize: 11,
    color: '#888',
    marginTop: 2,
  },
  permissionDescription: {
    fontSize: 11,
    color: '#aaa',
    marginTop: 2,
  },
  checkbox: {
    width: 22,
    height: 22,
    borderRadius: 4,
    borderWidth: 2,
    borderColor: '#007AFF',
    justifyContent: 'center',
    alignItems: 'center',
    marginLeft: 12,
  },
  checkboxSelected: {
    backgroundColor: '#007AFF',
  },
  emptyContainer: {
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    paddingTop: 50,
  },
  emptyText: {
    fontSize: 14,
    color: '#888',
  },
});