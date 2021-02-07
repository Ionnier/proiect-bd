const database = require('../services/database.js');
const comp_produse = require('./comp_produse');
const oracledb = require('oracledb');
const baseQuery = `select id_produs "id",
    id_producator "id_producator",
    nume_produs "nume",
    pret "pret",
    cantitate "cantitate",
    descriere "descriere",
    categorie "categorie"
  from produse
  where 1 = 1`;
const sortableColumns = ['id', 'id_producator', 'nume', 'pret', 'cantitate'];
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
  const limit = context.limit > 0 ? context.limit : 1000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
const createSql = `insert into produse (
    id_produs,
    id_producator,
    nume_produs,
    pret,
    cantitate,
    descriere,
    categorie
  ) values (
    id_produs_seq.nextval,
    :id_producator,
    :nume_produs,
    :pret,
    :cantitate,
    :descriere,
    :categorie
  ) returning id_produs
  into :id_produs`;

async function create(emp) {
  const produs = Object.assign({}, emp);

  produs.id_produs = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };

  const result = await database.simpleExecute(createSql, produs);
  console.log(produs);
  produs.id_produs = result.outBinds.id_produs[0];

  return produs;
}

const updateSql = `update produse
  set id_producator=:id_producator,
  nume_produs=:nume_produs,
  pret=:pret,
  cantitate=:cantitate,
  descriere=:descriere,
  categorie=:categorie
  where id_produs = :id_produs`;

async function update(emp) {
  const produs = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, produs);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return produs;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from Produse
    where id_produs = :id_produs;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_produs: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getprodusFromRec(req) {
  const produs = {
    id_producator: req.body.id_producator,
    nume_produs: req.body.nume_produs,
    pret: req.body.pret,
    cantitate: req.body.cantitate,
    descriere: req.body.descriere,
    categorie: req.body.categorie,
  };
  return produs;
}

function getprodusFromRec2(req) {
  const produs = {
    id_producator: req.body.id_producator,
    nume_produs: req.body.nume_produs,
    pret: req.body.pret,
    cantitate: req.body.cantitate,
    descriere: req.body.descriere,
    categorie: req.body.categorie,
    platforme: req.body.platforme,
  };
  return produs;
}

module.exports.getObjFromRec = getprodusFromRec;
module.exports.getObjFromRec2 = getprodusFromRec2;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
