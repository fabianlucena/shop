import { Api } from '../libs/api';

export default function useBusiness() {
  async function add(body, options) {
    options = {...options};
    return await Api.postJson('/v1/business', {...options, body});
  }

  async function get(query, options) {
    options = {...options, query: {...options?.query, ...query}};
    var data = await Api.getJson('/v1/business', options)
      if (!Array.isArray(data.rows)) {
        data.rows = [];
      }
          
    return data;
  }

  async function getSingleForUuid(uuid, options) {
    var data = await get(null, {...options, path: uuid});
    if (!data?.rows?.length) {
      throw new Error('No existe el negocio');
    }

    if (data.rows.length > 1) {
      throw new Error(`Hay muchos necogios ${data.rows.length}`);
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