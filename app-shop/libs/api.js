export class Api {
  static urlBase = 'http://localhost:5085/api';
  static headers = {};

  static async fetch(service, options) {
    options = {...options};
    const url = this.urlBase + service;

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
      console.error(res);
      throw new Error('Result is not OK');
    }

    if (options.json) {
      if (res.headers.get('content-type')?.startsWith('application/json')) {
        res = res.json();
      }
    }

    return res;
  }

  static post(service, options) {
    return this.fetch(service, {...options, method: 'POST'});
  }

  static postJson(service, options) {
    return this.post(service, {...options, json: true});
  }
};