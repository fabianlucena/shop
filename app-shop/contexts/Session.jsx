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
  const [business, setBusiness] = useState('');

  function loadBussiness() {
    serviceBusiness.get()
      .then(data => {
        const newBusinesses = data.rows.map(i => ({value: i.uuid, label: i.name}))
        setBusinesses(newBusinesses);
        autoSelectBussiness(newBusinesses);
      });
  }

  function autoSelectBussiness(businesses) {
    if (!businesses.length) {
      setBusiness('');
    } else if (businesses.length === 1) {
      setBusiness(businesses[0].value);
    }
  }

  useEffect(() => {
    if (isLogguedIn) {
      loadBussiness();
    }
  }, [isLogguedIn]);

  useEffect(() => {
    autoSelectBussiness(businesses);
  }, [businesses]);

  return (
    <SessionContext.Provider value={{
      isInitiated, setIsInitiated,
      isLogguedIn, setIsLoggedIn,
      permissions, setPermissions,
      loadBussiness,
      businesses, setBusinesses,
      business, setBusiness,
    }}>
      {children}
    </SessionContext.Provider>
  );
}