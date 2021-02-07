const database = require('../services/database.js');
const oracledb = require('oracledb');
const baseQuery = `SELECT nvl(id_judet,'B') as "id_judet", count(id_comanda) as "numar"
from Comenzi
group by id_judet
having 1=1`;
const sortableColumns = [];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.numar_comenzi = context.id;
    query += '\nand count(id_comanda) > :numar_comenzi';
  }
  if (context.skip) {
    binds.row_offset = context.skip;

    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by count(id_comanda) asc';
  } else {
    let [column, order] = context.sort.split(':');

    if (!sortableColumns.includes(column)) {
      throw new Error('Invalid "sort" column');
    }

    if (order === undefined) {
      order = 'asc';
    }

    if (order !== 'asc' && order !== 'desc') {
      throw new Error('Invalid "sort" order');
    }
    query += `\norder by "${column}" ${order}`;
  }
  if (context.limit > 0) {
    const limit = context.limit > 0 ? context.limit : 30000000;
    binds.row_limit = limit;
    query += '\nfetch next :row_limit rows only';
  }
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

const baseQuery2 = `select p.nume_platforma "nume_platforma",count(pc.id_produs)as "numar_produse",avg(prod.pret) as "pret"
from Compatibilitate_produs pc
right join Platforme p on pc.id_platforma=p.id_platforma
left join Produse prod on pc.id_produs = prod.id_produs
group by pc.id_platforma,p.nume_platforma
having 1=1`;
async function find2(context) {
  let query = baseQuery2;
  const binds = {};

  if (context.id) {
    binds.numar_produse = context.id;
    query += '\nand count(pc.id_produs) > :numar_produse';
  }
  if (context.avg) {
    binds.pret = context.avg;
    query += '\nand avg(prod.pret) > :pret';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by count(pc.id_produs) asc';
  } else {
    let [column, order] = context.sort.split(':');

    if (!sortableColumns.includes(column)) {
      throw new Error('Invalid "sort" column');
    }

    if (order === undefined) {
      order = 'asc';
    }

    if (order !== 'asc' && order !== 'desc') {
      throw new Error('Invalid "sort" order');
    }
    query += `\norder by "${column}" ${order}`;
  }
  if (context.limit > 0) {
    const limit = context.limit > 0 ? context.limit : 30000000;
    binds.row_limit = limit;
    query += '\nfetch next :row_limit rows only';
  }
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find2 = find2;
module.exports.find = find;
