import { useContext, createContext, useState } from 'react';

export const SessionContext = createContext();

export function useSession() {
  return useContext(SessionContext);
}

export function SessionProvider({ children }) {
  const [isInitiated, setIsInitiated] = useState(false);
  const [isLogguedIn, setIsLoggedIn] = useState(false);

  return (
    <SessionContext.Provider value={{
      isInitiated, setIsInitiated,
      isLogguedIn, setIsLoggedIn,
    }}>
      {children}
    </SessionContext.Provider>
  );
}