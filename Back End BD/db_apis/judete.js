const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');
const baseQuery = `SELECT id_judet "id",
    nume_judet "nume" from judete
    where 1 = 1`;
const sortableColumns = ['id', 'nume'];
async function find(context) {
  let query = baseQuery;
  const binds = {};
  if (context.nume) {
    binds.nume_judet = context.nume;
    query += '\nand nume_judet = :nume_judet';
  }
  if (context.id) {
    binds.id_judet = context.id;
    query += '\nand id_judet = :id_judet';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_judet asc';
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
const createSql = `insert into judete (
    nume_judet,
    id_judet
  ) values (
    :nume_judet,
    :id_judet
  ) returning id_judet
  into :id_judet1`;
async function create(emp) {
  const judet = Object.assign({}, emp);
  console.log(judet);
  judet.id_judet1 = {
    dir: oracledb.BIND_OUT,
    type: oracledb.VARCHAR,
  };

  const result = await database.simpleExecute(createSql, judet);
  judet.id_judet1 = result.outBinds.id_judet1[0];
  return judet;
}

const updateSql = `update judete
  set nume_judet=:nume_judet,
  id_judet=:id_judet
  where id_judet = :id_newjudet`;

async function update(emp, id) {
  const judet = Object.assign({}, emp);
  emp.id_judet = id;
  const result = await database.simpleExecute(updateSql, judet);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return judet;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from judete
    where id_judet = :id_judet;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_judet: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getjudetFromRec(req) {
  const judet = {
    nume_judet: req.body.nume_judet,
    id_judet: req.body.id_judet,
  };
  return judet;
}

module.exports.getObjFromRec = getjudetFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
