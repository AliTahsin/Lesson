import { render, screen, fireEvent } from '@testing-library/react';
import { HotelCard } from '../../../src/components/HotelCard';

describe('HotelCard Component', () => {
    const mockHotel = {
        id: 1,
        name: 'Test Hotel',
        city: 'İstanbul',
        starRating: 5,
        description: 'Test description',
    };

    it('should render hotel information correctly', () => {
        render(<HotelCard hotel={mockHotel} onPress={() => {}} />);
        
        expect(screen.getByText('Test Hotel')).toBeDefined();
        expect(screen.getByText('İstanbul')).toBeDefined();
        expect(screen.getByText('★★★★★')).toBeDefined();
    });

    it('should call onPress when clicked', () => {
        const handlePress = jest.fn();
        render(<HotelCard hotel={mockHotel} onPress={handlePress} />);
        
        fireEvent.press(screen.getByTestId('hotel-card'));
        expect(handlePress).toHaveBeenCalledWith(1);
    });

    it('should display correct star rating', () => {
        const hotel3Star = { ...mockHotel, starRating: 3 };
        render(<HotelCard hotel={hotel3Star} onPress={() => {}} />);
        
        expect(screen.getByText('★★★☆☆')).toBeDefined();
    });
});