describe('Login Flow', () => {
    beforeEach(async () => {
        await device.reloadReactNative();
    });

    it('should show login screen', async () => {
        await expect(element(by.id('email-input'))).toBeVisible();
        await expect(element(by.id('password-input'))).toBeVisible();
        await expect(element(by.id('login-button'))).toBeVisible();
    });

    it('should login with valid credentials', async () => {
        await element(by.id('email-input')).typeText('admin@hotel.com');
        await element(by.id('password-input')).typeText('Admin123!');
        await element(by.id('login-button')).tap();
        
        await expect(element(by.id('dashboard-screen'))).toBeVisible();
    });

    it('should show error with invalid credentials', async () => {
        await element(by.id('email-input')).typeText('invalid@hotel.com');
        await element(by.id('password-input')).typeText('wrong');
        await element(by.id('login-button')).tap();
        
        await expect(element(by.text('Giriş Hatası'))).toBeVisible();
    });
});