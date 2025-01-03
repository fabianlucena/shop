import { useContext, createContext, useState, useEffect } from 'react';

import useBusiness from '../services/useBusiness';
import useCategory from '../services/useCategory';
import useStore from '../services/useStore';

export const SessionContext = createContext();

export function useSession() {
  return useContext(SessionContext);
}

export function SessionProvider({ children }) {
  const serviceBusiness = useBusiness();
  const serviceCategory = useCategory();
  const serviceStore = useStore();
  const [isInitiated, setIsInitiated] = useState(false);
  const [isLogguedIn, setIsLoggedIn] = useState(false);
  const [permissions, setPermissions] = useState([]);
  const [businesses, setBusinesses] = useState([]);
  const [businessUuid, setBusinessUuid] = useState('');
  const [businessName, setBusinessName] = useState('');
  const [categories, setCategories] = useState([]);
  const [stores, setStores] = useState([]);
  const [categoriesOptions, setCategoriesOptions] = useState([]);
  const [storesOptions, setStoresOptions] = useState([]);

  function loadBussiness() {
    serviceBusiness.get()
      .then(data => {
        const newBusinesses = data.rows;
        setBusinesses(newBusinesses);
        autoSelectBussiness(newBusinesses);
      });
  }

  function loadCategories() {
    serviceCategory.get()
      .then(data => {
        const newCategories = data.rows;
        setCategories(newCategories);
        setCategoriesOptions(newCategories.map(i => ({ value: i.uuid, label: i.name })));
      });
  }

  function loadStores() {
    serviceStore.get()
      .then(data => {
        const newStores  = data.rows;
        setStores(newStores);
        setStoresOptions(newStores.map(i => ({ value: i.uuid, label: i.name })));
      });
  }

  function autoSelectBussiness(businesses) {
    if (!businesses.length) {
      setBusinessUuid('');
      setBusinessName('');
      setStores([]);
    } else if (businesses.length === 1) {
      setBusinessUuid(businesses[0].uuid);
      updateCurrentBusinessName(businesses, businesses[0].uuid);
      loadStores();
    }
  }

  function updateCurrentBusinessName(businesses, businessUuid) {
    var currentBusiness = businesses.find(i => i.uuid === businessUuid);
    if (currentBusiness)
      setBusinessName(currentBusiness.name);
  }

  useEffect(() => {
    if (isLogguedIn) {
      loadBussiness();
      loadCategories();
    }
  }, [isLogguedIn]);

  useEffect(() => {
    autoSelectBussiness(businesses);
  }, [businesses]);

  useEffect(() => {
    updateCurrentBusinessName(businesses, businessUuid);
    loadStores();
  }, [businesses, businessUuid]);

  return (
    <SessionContext.Provider value={{
      isInitiated, setIsInitiated,
      isLogguedIn, setIsLoggedIn,
      permissions, setPermissions,
      loadBussiness,
      businesses, setBusinesses,
      businessUuid, setBusinessUuid,
      businessName,
      categories,
      stores,
      categoriesOptions,
      storesOptions,
    }}>
      {children}
    </SessionContext.Provider>
  );
}