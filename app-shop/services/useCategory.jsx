import { Api } from '../libs/api';

export default function useCategory() {
  async function get(query, options) {
    options = {...options, query: {...options?.query, ...query}};
    var data = await Api.getJson('/v1/category', options)
      if (!Array.isArray(data.rows)) {
        data.rows = [];
      }
          
    return data;
  }

  return {
    get,
  }
}