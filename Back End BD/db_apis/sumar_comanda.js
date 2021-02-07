const database = require('../services/database.js');

const baseQuery = `SELECT id_comanda "id_comanda",
    persoana_contact "persoana_contact",
    nr_produse_comandate "nr_produse_comandate",
    valoare_totala "valoare_totala",
    nume_judet "nume_judet",
    oras "oras",
    strada "strada",
    bloc "bloc",
    cod_postal "cod_postal",
    numar_telefon "numar_telefon",
    TO_CHAR(data_comanda,'YYYY-MM-DD HH:MM') "data_comanda"
    from sumar_comanda
    where 1 = 1`;
const sortableColumns = [
  'id_comanda',
  'nr_produse_comandate',
  'valoare_totala',
  'nume_judet',
  'oras',
  'data_comanda',
];

async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_comanda = context.id;
    query += '\nand id_comanda = :id_comanda';
  }

  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_comanda asc';
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
