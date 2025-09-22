import { useContext, createContext, useState, useEffect } from 'react';

import useCommerce from '../services/useCommerce';
import useCategory from '../services/useCategory';
import useStore from '../services/useStore';

export const SessionContext = createContext();

export default useSession;

export function useSession() {
  return useContext(SessionContext);
}

export function SessionProvider({ children }) {
  const serviceCommerce = useCommerce();
  const serviceCategory = useCategory();
  const serviceStore = useStore();
  const [isInitiated, setIsInitiated] = useState(false);
  const [isLogguedIn, setIsLoggedIn] = useState(false);
  const [permissions, setPermissions] = useState([]);
  const [commerces, setCommerces] = useState([]);
  const [commerceUuid, setCommerceUuid] = useState('');
  const [commerceName, setCommerceName] = useState('');
  const [categories, setCategories] = useState([]);
  const [stores, setStores] = useState([]);
  const [categoriesOptions, setCategoriesOptions] = useState([]);
  const [storesOptions, setStoresOptions] = useState([]);
  const [messages, setMessages] = useState([]);
  const [, setMessageCounter] = useState(0);

  function loadCommerces() {
    serviceCommerce.get()
      .then(data => {
        const newCommerces = data.rows;
        setCommerces(newCommerces);
        autoSelectCommerce(newCommerces);
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

  function autoSelectCommerce(commerces) {
    if (!commerces.length) {
      setCommerceUuid('');
      setCommerceName('');
      setStores([]);
    } else if (commerces.length === 1) {
      setCommerceUuid(commerces[0].uuid);
      updateCurrentCommerceName(commerces, commerces[0].uuid);
      loadStores();
    }
  }

  function updateCurrentCommerceName(commerces, commerceUuid) {
    var currentCommerce = commerces.find(i => i.uuid === commerceUuid);
    if (currentCommerce)
      setCommerceName(currentCommerce.name);
  }

  useEffect(() => {
    if (isLogguedIn) {
      loadCommerces();
      loadCategories();
    }
  }, [isLogguedIn]);

  useEffect(() => {
    autoSelectCommerce(commerces);
  }, [commerces]);

  useEffect(() => {
    if (isLogguedIn && commerceUuid) {
      updateCurrentCommerceName(commerces, commerceUuid);
      loadStores();
    }
  }, [commerces, commerceUuid]);

  function normalizeMessage(message, options) {
    if (typeof message === 'string')
      message = { message };

    message = {
      variant: 'message',
      ...options,
      ...message,
    };

    return message;
  }

  function addMessage(message, options) {
    message = normalizeMessage(message, options);

    setMessageCounter(c => {
      const newC = c + 1;
      message.id ||= newC;
      return newC;
    });

    if (message.variant === 'error') {
      console.error(message.message);
    } else {
      console.log(message.message);
    }

    setMessages(messages => [...messages, message]);

    return message;
  }

  function addError(message) {
    return addMessage(message, { variant: 'error' });
  }

  function updateMessage(messageId, message, options) {
    message = normalizeMessage(
      message,
      {
        ...message,
        ...options,
        message: undefined,
      }
    );

    setMessages(messages => [
      ...messages.map(m => m.id === messageId ? { ...m, ...message } : m),
    ]);
  }

  return (
    <SessionContext.Provider value={{
      isInitiated, setIsInitiated,
      isLogguedIn, setIsLoggedIn,
      permissions, setPermissions,
      loadCommerces,
      commerces, setCommerces,
      commerceUuid, setCommerceUuid,
      commerceName,
      categories,
      stores,
      categoriesOptions,
      storesOptions,
      addMessage, addError, updateMessage, messages, setMessages
    }}>
      {children}
    </SessionContext.Provider>
  );
}