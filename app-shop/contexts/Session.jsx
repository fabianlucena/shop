import { useContext, createContext, useState, useEffect } from 'react';

import useBusiness from '../services/useBusiness';

export const SessionContext = createContext();

export function useSession() {
  return useContext(SessionContext);
}

export function SessionProvider({ children }) {
  const serviceBusiness = useBusiness();
  const [isInitiated, setIsInitiated] = useState(false);
  const [isLogguedIn, setIsLoggedIn] = useState(false);
  const [permissions, setPermissions] = useState([]);
  const [businesses, setBusinesses] = useState([]);
  const [businessUuid, setBusinessUuid] = useState('');
  const [businessName, setBusinessName] = useState('');

  function loadBussiness() {
    serviceBusiness.get()
      .then(data => {
        const newBusinesses = data.rows;
        setBusinesses(newBusinesses);
        autoSelectBussiness(newBusinesses);
      });
  }

  function autoSelectBussiness(businesses) {
    if (!businesses.length) {
      setBusinessUuid('');
      setBusinessName('');
    } else if (businesses.length === 1) {
      setBusinessUuid(businesses[0].uuid);
      updateCurrentBusinessName(businesses, businesses[0].uuid);
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
    }
  }, [isLogguedIn]);

  useEffect(() => {
    autoSelectBussiness(businesses);
  }, [businesses]);

  useEffect(() => {
    updateCurrentBusinessName(businesses, businessUuid);
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
    }}>
      {children}
    </SessionContext.Provider>
  );
}