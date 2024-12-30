export class Api {
  static urlBase = 'http://localhost:5085/api';
  static headers = {};

  static async fetch(service, options) {
    options = {...options};
    let url = this.urlBase + service;

    if (options.path)
      url += '/' + options.path;

    if (options.body && typeof options.body !== 'string') {
      options.body = JSON.stringify(options.body);
    }

    if (options.headers === false) {
      delete options.headers;
    } else {
      options.headers = {...this.headers, ...options.headers};
      if (options.json) {
        if (!options.headers['Content-Type']) {
          options.headers['Content-Type'] = 'application/json';
        }

        if (!options.headers.Accept) {
          options.headers.Accept = 'application/json';
        }
      }
    }

    if (this.debug) {
      console.log(url);
      console.log(options);
    }

    var res = await fetch(url, options);
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

    if (options.json) {
      if (res.headers.get('content-type')?.startsWith('application/json')) {
        return res.json();
      }
    }

    return res;
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

  static getJson(service, options) {
    return this.get(service, {...options, json: true});
  }

  static postJson(service, options) {
    return this.post(service, {...options, json: true});
  }

  static patchJson(service, options) {
    return this.patch(service, {...options, json: true});
  }
};