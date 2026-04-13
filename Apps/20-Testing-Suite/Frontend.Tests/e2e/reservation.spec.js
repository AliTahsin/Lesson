describe('Reservation Flow', () => {
    beforeEach(() => {
        cy.visit('/');
    });

    it('should display hotel list', () => {
        cy.get('[data-testid="hotel-list"]').should('be.visible');
        cy.get('[data-testid="hotel-card"]').should('have.length.at.least', 1);
    });

    it('should search hotels by city', () => {
        cy.get('[data-testid="search-input"]').type('İstanbul');
        cy.get('[data-testid="search-button"]').click();
        cy.get('[data-testid="hotel-card"]').each(($card) => {
            cy.wrap($card).contains('İstanbul');
        });
    });

    it('should create a reservation', () => {
        // Select a hotel
        cy.get('[data-testid="hotel-card"]').first().click();
        
        // Select dates
        cy.get('[data-testid="checkin-date"]').type('2024-12-01');
        cy.get('[data-testid="checkout-date"]').type('2024-12-05');
        
        // Select room
        cy.get('[data-testid="room-card"]').first().click();
        
        // Fill guest info
        cy.get('[data-testid="guest-name"]').type('Test User');
        cy.get('[data-testid="guest-email"]').type('test@email.com');
        
        // Submit
        cy.get('[data-testid="submit-reservation"]').click();
        
        // Verify success
        cy.get('[data-testid="success-message"]').should('be.visible');
        cy.contains('Rezervasyon başarıyla oluşturuldu');
    });

    it('should login successfully', () => {
        cy.visit('/login');
        cy.get('[data-testid="email-input"]').type('admin@hotel.com');
        cy.get('[data-testid="password-input"]').type('Admin123!');
        cy.get('[data-testid="login-button"]').click();
        cy.url().should('include', '/dashboard');
    });
});