const database = require('../services/database.js');
const oracledb = require('oracledb');

const baseQuery = `select id_comentariu "id_comentariu",  prod.id_producator "id_producator", prod.nume_produs  "nume_produs"
from Comentarii com
join Produse prod on com.id_produs=prod.id_produs
where 1=1`;
const sortableColumns = [];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_producator = context.id;
    query += '\nand prod.id_producator = :id_producator';
  }

  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by com.id_comentariu asc';
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
  const limit = context.limit > 0 ? context.limit : 3000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find = find;

const baseQuery2 = `select prod.nume_produs "nume_produs", pc.id_platforma "id_platforma", prod.id_producator "id_producator" 
from Compatibilitate_Produs pc
join Produse prod on pc.id_produs=prod.id_produs
where 1=1`;
async function find2(context) {
  let query = baseQuery2;
  const binds = {};

  if (context.id) {
    binds.id_producator = context.id;
    query += '\nand prod.id_producator = :id_producator';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by prod.id_produs asc';
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
  const limit = context.limit > 0 ? context.limit : 3000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find2 = find2;

const baseQuery3 = `SELECT id_comanda "id_comanda",prod.id_produs "id_produs",pc.cantitate "cantitate",id_producator "id_producator"
from Produse_comandate pc
join Produse prod on prod.id_produs=pc.id_produs
where 1=1`;
async function find3(context) {
  let query = baseQuery3;
  const binds = {};

  if (context.id) {
    binds.id_producator = context.id;
    query += '\nand prod.id_producator = :id_producator';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by prod.id_produs asc';
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
  const limit = context.limit > 0 ? context.limit : 3000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find3 = find3;

const baseQuery4 = `SELECT ip.id_produs "id_produs",ip.pret "pret",TO_CHAR(data_inceput,'YYYY-MM-DD HH:MM') "data_inceput", prod.id_producator "id_producator"
from Istoricpreturi ip
join Produse prod on ip.id_produs=prod.id_produs
where 1=1`;
async function find4(context) {
  let query = baseQuery4;
  const binds = {};

  if (context.id) {
    binds.id_producator = context.id;
    query += '\nand prod.id_producator = :id_producator';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by prod.id_produs asc';
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
  const limit = context.limit > 0 ? context.limit : 3000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find4 = find4;
