import { useContext, createContext, useState, useEffect } from 'react';

import useCommerce from '../services/useCommerce';
import useCategory from '../services/useCategory';
import useStore from '../services/useStore';

export const SessionContext = createContext();

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
    }}>
      {children}
    </SessionContext.Provider>
  );
}