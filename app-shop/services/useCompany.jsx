import { Api } from '../libs/api';

export default function useCompany() {
  async function add(body, options) {
    options = {...options};
    return await Api.postJson('/v1/company', {...options, body});
  }

  async function get(query, options) {
    options = {...options, query: {...options?.query, ...query}};
    var data = await Api.getJson('/v1/company', options)
      if (!Array.isArray(data.rows)) {
        data.rows = [];
      }
          
    return data;
  }

  async function getSingleForUuid(uuid, options) {
    var data = await get(null, {...options, path: uuid});
    if (!data?.rows?.length) {
      throw new Error('No existe la empresa');
    }

    if (data.rows.length > 1) {
      throw new Error(`Hay muchas empresas ${data.rows.length}`);
    }
    
    const row = data.rows[0];
    
    return row;
  }

  return {
    add,
    get,
    getSingleForUuid,
  }
}