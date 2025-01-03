import { Api } from '../libs/api';

export default function useBusiness() {
  async function add(data, options) {
    return await Api.postJson('/v1/business', {...options, body: data});
  }

  async function get(query, options) {
    options = {...options, query: {...options?.query, ...query}};
    var data = await Api.getJson('/v1/business', options)
    if (!Array.isArray(data.rows))
      data.rows = [];
          
    return data;
  }

  async function getSingleForUuid(uuid, options) {
    var data = await get(null, {...options, path: uuid});
    if (!data?.rows?.length) {
      throw new Error('No existe el negocio.');
    }

    if (data.rows.length > 1) {
      throw new Error(`Hay muchos negocios ${data.rows.length}.`);
    }
    
    const row = data.rows[0];
    
    return row;
  }

  async function updateForUuid(uuid, data, options) {
    return await Api.patchJson('/v1/business', {...options, path: uuid, body: data});
  }

  async function deleteForUuid(uuid, options) {
    return await Api.deleteJson('/v1/business', {...options, path: uuid});
  }

  return {
    add,
    get,
    getSingleForUuid,
    updateForUuid,
    deleteForUuid,
  }
}