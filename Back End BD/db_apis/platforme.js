const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');
const baseQuery = `SELECT id_platforma "id",
    nume_platforma "nume"
    from platforme
    where 1 = 1`;
const sortableColumns = ['id', 'nume'];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.nume) {
    binds.nume_platforma = context.nume;
    query += '\nand nume_platforma = :nume_platforma';
  }

  if (context.id) {
    binds.id_platforma = context.id;
    query += '\nand id_platforma = :id_platforma';
  }

  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_platforma asc';
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
const createSql = `insert into platforme (
    id_platforma,
    nume_platforma
  ) values (
    id_platforma_seq.nextval,
    :nume_platforma
  ) returning id_platforma
  into :id_platforma`;

async function create(emp) {
  const platforma = Object.assign({}, emp);

  platforma.id_platforma = {
    dir: oracledb.BIND_OUT,
    type: oracledb.VARCHAR,
  };

  const result = await database.simpleExecute(createSql, platforma);

  platforma.id_platforma = result.outBinds.id_platforma[0];

  return platforma;
}

const updateSql = `update platforme
  set nume_platforma=:nume_platforma
  where id_platforma = :id_platforma`;

async function update(emp, id) {
  emp.id_platforma = id;
  const platforma = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, platforma);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return platforma;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from platforme
    where id_platforma = :id_platforma;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_platforma: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };

  const result = await database.simpleExecute(deleteSql, binds);
  console.log(result);
  return result.outBinds.rowcount === 1;
}

function getplatformaFromRec(req) {
  const platforma = {
    nume_platforma: req.body.nume_platforma,
  };
  return platforma;
}

module.exports.getObjFromRec = getplatformaFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
