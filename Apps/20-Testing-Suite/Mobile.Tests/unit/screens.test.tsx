import React from 'react';
import { render, fireEvent, waitFor } from '@testing-library/react-native';
import { LoginScreen } from '../../src/screens/LoginScreen';
import { AuthProvider } from '../../src/context/AuthContext';

jest.mock('../../src/services/authApi');

describe('LoginScreen', () => {
    it('should render correctly', () => {
        const { getByPlaceholderText, getByText } = render(
            <AuthProvider>
                <LoginScreen />
            </AuthProvider>
        );
        
        expect(getByPlaceholderText('E-posta veya Kullanıcı Adı')).toBeDefined();
        expect(getByPlaceholderText('Şifre')).toBeDefined();
        expect(getByText('Giriş Yap')).toBeDefined();
    });

    it('should show error when fields are empty', async () => {
        const { getByText } = render(
            <AuthProvider>
                <LoginScreen />
            </AuthProvider>
        );
        
        fireEvent.press(getByText('Giriş Yap'));
        
        await waitFor(() => {
            expect(getByText('Lütfen e-posta/kullanıcı adı ve şifre girin')).toBeDefined();
        });
    });

    it('should navigate on successful login', async () => {
        const mockNavigate = jest.fn();
        const { getByPlaceholderText, getByText } = render(
            <AuthProvider>
                <LoginScreen navigation={{ navigate: mockNavigate }} />
            </AuthProvider>
        );
        
        fireEvent.changeText(getByPlaceholderText('E-posta veya Kullanıcı Adı'), 'admin@hotel.com');
        fireEvent.changeText(getByPlaceholderText('Şifre'), 'Admin123!');
        fireEvent.press(getByText('Giriş Yap'));
        
        await waitFor(() => {
            expect(mockNavigate).toHaveBeenCalledWith('Main');
        });
    });
});