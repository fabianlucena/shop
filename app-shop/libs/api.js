export class Api {
  static urlBase = process.env.EXPO_PUBLIC_API_URL ?? 'http://localhost:5085/api';
  static headers = {};
  static debug = {
    request: process.env.EXPO_PUBLIC_DEBUG_API_REQUEST,
    response: process.env.EXPO_PUBLIC_DEBUG_API_RESPONSE,
  };

  static async fetch(service, options) {
    let fetchOptions = {...options};
    let url = this.urlBase + service;

    if (fetchOptions.path) {
      url += '/' + fetchOptions.path;
      delete fetchOptions.path;
    }

    if (fetchOptions.query) {
      if (typeof fetchOptions.query === 'function')
        fetchOptions.query = query(fetchOptions);
      
      if (fetchOptions.query) {
        if (typeof fetchOptions.query !== 'string')
          fetchOptions.query = new URLSearchParams(fetchOptions.query).toString();

        if (fetchOptions.query)
          url += '?' + fetchOptions.query;
        
        delete fetchOptions.query;
      }
    }

    if (fetchOptions.body && typeof fetchOptions.body !== 'string') {
      fetchOptions.body = JSON.stringify(fetchOptions.body);
    }

    if (fetchOptions.headers === false) {
      delete fetchOptions.headers;
    } else {
      fetchOptions.headers = {...this.headers, ...fetchOptions.headers};
      if (fetchOptions.json) {
        if (!fetchOptions.headers['Content-Type']) {
          fetchOptions.headers['Content-Type'] = 'application/json';
        }

        if (!fetchOptions.headers.Accept) {
          fetchOptions.headers.Accept = 'application/json';
        }
      }
    }
    delete fetchOptions.json;

    if (this.debug?.request || options.debug === true || options.debug?.request) {
      console.log(`<<< ${fetchOptions.method} ${url}\n${JSON.stringify({...fetchOptions, method: undefined}, null, '  ')}`)
    }

    let res = await fetch(url, fetchOptions);
    if (!res.ok) {
      let errorMessage;
      if (res.headers.get('content-type')?.startsWith('application/json')) {
        try {
          const data = await res.json();
          console.log(data);
          errorMessage = data.message || data.error;
        } catch {}
      }

      errorMessage ||= `Result is not OK. HTTP Status: ${res.status}: ${res.statusText}.`;

      console.error(errorMessage);
      throw new Error(errorMessage);
    }

    let result;
    if (options.json && res.headers.get('content-type')?.startsWith('application/json')) {
      result = await res.json();
    } else {
      result = res;
    }

    if (this.debug?.response || options.debug === true || options.debug?.response) {
      console.log(`>>> ${fetchOptions.method} ${url}\n${JSON.stringify(result, null, '  ')}`)
    }

    return result;
  }

  static get(service, options) {
    return this.fetch(service, {...options, method: 'GET'});
  }

  static post(service, options) {
    return this.fetch(service, {...options, method: 'POST'});
  }

  static patch(service, options) {
    return this.fetch(service, {...options, method: 'PATCH'});
  }

  static delete(service, options) {
    return this.fetch(service, {...options, method: 'DELETE'});
  }

  static getJson(service, options) {
    return this.get(service, {...options, json: true});
  }

  static postJson(service, options) {
    return this.post(service, {...options, json: true});
  }

  static patchJson(service, options) {
    return this.patch(service, {...options, json: true});
  }

  static deleteJson(service, options) {
    return this.delete(service, {...options, json: true});
  }
};