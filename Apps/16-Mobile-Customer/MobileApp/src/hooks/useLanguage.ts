import { useState, useEffect } from 'react';
import { storage } from '../utils/storage';
import { getTranslation, translations } from '../utils/translations';
import { customerApi } from '../services/customerApi';

export const useLanguage = () => {
  const [language, setLanguage] = useState('tr');
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadLanguage();
  }, []);

  const loadLanguage = async () => {
    try {
      const savedLanguage = await storage.getLanguage();
      if (savedLanguage) {
        setLanguage(savedLanguage);
      }
    } catch (error) {
      console.error('Error loading language:', error);
    } finally {
      setLoading(false);
    }
  };

  const changeLanguage = async (newLanguage: string) => {
    try {
      await storage.setLanguage(newLanguage);
      await customerApi.updateLanguage(newLanguage);
      setLanguage(newLanguage);
    } catch (error) {
      console.error('Error changing language:', error);
    }
  };

  const t = (key: string): string => {
    return getTranslation(key, language);
  };

  return {
    language,
    loading,
    changeLanguage,
    t,
    translations: translations[language],
  };
};