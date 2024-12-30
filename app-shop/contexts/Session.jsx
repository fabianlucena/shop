import { useContext, createContext, useState } from 'react';

export const SessionContext = createContext();

export function useSession() {
  return useContext(SessionContext);
}

export function SessionProvider({ children }) {
  const [isInitiated, setIsInitiated] = useState(false);
  const [isLogguedIn, setIsLoggedIn] = useState(false);
  const [permissions, setPermissions] = useState([]);
  const [business, setBusiness] = useState('');

  return (
    <SessionContext.Provider value={{
      isInitiated, setIsInitiated,
      isLogguedIn, setIsLoggedIn,
      permissions, setPermissions,
      business, setBusiness,
    }}>
      {children}
    </SessionContext.Provider>
  );
}