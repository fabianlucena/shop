export class Api {
  static urlBase = 'http://192.168.1.35:5085';
  static headers = {};

  static fetch(service, options) {
    options = {...options};
    const url = this.urlBase + service;
    if (options.body && typeof options.body !== 'string') {
      options.body = JSON.stringify(options.body);
    }

    if (options.headers === false) {
      delete options.headers;
    } else {
      options.headers = {...this.headers, ...options.headers};
    }

    console.log(url);
    console.log(options);
    return fetch(url, options);
  }

  static post(service, options) {
    return this.fetch(service, {...options, method: 'POST'});
  }

  static postJson(service, options) {
    return this.fetch(
      service,
      {
        ...options,
        method: 'POST',
        headers: {
          ...options?.headers,
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
      }
    );
  }
};