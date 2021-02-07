const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');
const baseQuery = `SELECT id_producator "id",
    nume_producator "nume",
    website "website" from producatori
    where 1 = 1`;
const sortableColumns = ['id', 'nume', 'website'];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.nume) {
    binds.nume_producator = context.nume;
    query += '\nand nume_producator = :nume_producator';
  }

  if (context.id) {
    binds.id_producator = context.id;
    query += '\nand id_producator = :id_producator';
  }

  if (context.website) {
    binds.website = context.website;
    query += '\nand website = :website';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_producator asc';
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
const createSql = `insert into producatori (
    id_producator,
    nume_producator,
    website
  ) values (
    id_producator_seq.nextval,
    :nume_producator,
    :website
  ) returning id_producator
  into :id_producator`;

async function create(emp) {
  const producator = Object.assign({}, emp);

  producator.id_producator = {
    dir: oracledb.BIND_OUT,
    type: oracledb.VARCHAR,
  };

  const result = await database.simpleExecute(createSql, producator);

  producator.id_producator = result.outBinds.id_producator[0];

  return producator;
}

const updateSql = `update producatori
  set nume_producator=:nume_producator,
  website=:website
  where id_producator = :id_producator`;

async function update(emp, id) {
  emp.id_producator = id;
  const producator = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, producator);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return producator;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from producatori
    where id_producator = :id_producator;
    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_producator: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getproducatorFromRec(req) {
  const producator = {
    nume_producator: req.body.nume_producator,
    website: req.body.website,
  };
  console.log(producator);
  return producator;
}

module.exports.getObjFromRec = getproducatorFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
