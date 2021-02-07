const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');

const baseQuery = `SELECT id_produs "id_produs",
    nume_produs "nume",
    nume_producator "nume_producator",
    id "id_producator",
    pret "pret",
    cantitate "cantitate",
    categorie "categorie"
    from produse_complete
    where 1 = 1`;
const sortableColumns = ['id', 'nume', 'id_producator'];

async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_produs = context.id;
    query += '\nand id_produs = :id_produs';
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
  const limit = context.limit > 0 ? context.limit : 3000;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}

const createSql = `INSERT INTO produse_complete(
    id_produs,
    nume_produs,
    id,
    pret,
    cantitate,
    categorie) 
    values (
    id_produs_seq.nextval,
    :nume_produs,
    :id_producator,
    :pret,
    :cantitate,
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
  produs.id_produs = result.outBinds.id_produs[0];
  return produs;
}

function getprodusFromRec(req) {
  const produs = {
    nume_produs: req.body.nume_produs,
    id_producator: req.body.id_producator,
    pret: req.body.pret,
    cantitate: req.body.cantitate,
    categorie: req.body.categorie,
  };
  return produs;
}

const updateSql = `update produse_complete
  set nume_produs=:nume_produs,
  id=:id_producator,
  pret=:pret,
  cantitate=:cantitate,
  categorie=:categorie
  where id_produs = :id_produs`;

async function update(emp, id) {
  emp.id_produs = id;
  const produs = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, produs);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return produs;
  } else {
    return null;
  }
}

const deleteSql = `begin
    delete from produse_complete
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

module.exports.update = update;
module.exports.getObjFromRec = getprodusFromRec;
module.exports.create = create;
module.exports.find = find;
module.exports.delete = del;
