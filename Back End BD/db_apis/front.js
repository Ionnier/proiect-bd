const database = require('../services/database.js');
const comp_produse = require('./comp_produse');
const oracledb = require('oracledb');
const baseQuery = `select id_produs "id",
    nume_producator "nume_producator",
    nume_produs "nume",
    pret "pret",
    descriere "descriere",
    categorie "categorie"
  from produse
  join Producatori using(id_producator)
  where 1 = 1`;
const sortableColumns = ['id', 'nume_producator', 'nume', 'pret', 'cantitate'];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_produs = context.id;

    query += '\nand id_produs = :id_produs';
  }

  if (context.id_producator) {
    binds.id_producator = context.id_producator;

    query += '\nand id_producator = :id_producator';
  }

  if (context.nume) {
    binds.nume = context.nume;

    query += '\nand lower(nume) = lower(:nume)';
  }
  if (context.skip) {
    binds.row_offset = context.skip;

    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_produs asc';
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
  const limit = context.limit > 0 ? context.limit : 10000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

var selectComSql = `select nume_utilizator || ' ' || prenume_utilizator "utilizator", continut_comentariu "continut" , TO_CHAR(data_publicare,'YYYY-MM-DD') "data_publicare" 
from Comentarii com 
join Utilizatori util on com.id_utilizator=util.id_utilizator
join Produse prod on prod.id_produs=com.id_produs
where com.id_produs=:id_produs`;

async function find2(context) {
  let query = selectComSql;
  const binds = {};
  binds.id_produs = context.id;
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by data_publicare desc';
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
  const limit = context.limit > 0 ? context.limit : 10000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

module.exports.find = find;
module.exports.find2 = find2;
