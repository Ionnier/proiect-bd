const database = require('../services/database.js');
const oracledb = require('oracledb');
const baseQuery = `select id_comentariu "id",
    id_produs "id_produs",
    id_utilizator "id_utilizator",
    continut_comentariu "continut",
    TO_CHAR(data_publicare,'YYYY-MM-DD HH:MM') "data_publicare"
  from Comentarii
  where 1 = 1`;
const sortableColumns = ['id', 'id_utilizator', 'id_produs', 'data_publicare'];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_comentariu = context.id;

    query += '\nand id_comentariu = :id_comentariu';
  }

  if (context.id_utilizator) {
    binds.id_utilizator = context.id_utilizator;

    query += '\nand id_utilizator = :id_utilizator';
  }

  if (context.id_produs) {
    binds.id_produs = context.id_produs;

    query += '\nand id_produs = :id_produs';
  }
  if (context.skip) {
    binds.row_offset = context.skip;

    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_comentariu asc';
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
    const limit = context.limit > 0 ? context.limit : 1000;
    binds.row_limit = limit;
    query += '\nfetch next :row_limit rows only';
  }
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
const createSql = `insert into comentarii (
    id_comentariu,
    id_utilizator,
    id_produs,
    continut_comentariu,
    data_publicare
  ) values (
     id_comentariu_seq.nextval,
    :id_utilizator,
    :id_produs,
    :continut_comentariu,
    sysdate
  ) returning id_comentariu
  into :id_comentariu`;

async function create(emp) {
  const comentariu = Object.assign({}, emp);

  comentariu.id_comentariu = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };
  const result = await database.simpleExecute(createSql, comentariu);

  comentariu.id_comentariu = result.outBinds.id_comentariu[0];

  return comentariu;
}

const updateSql = `update comentarii
  set id_produs = :id_produs,
  id_utilizator = :id_utilizator,
  continut_comentariu=:continut_comentariu
  where id_comentariu= :id_comentariu`;

async function update(emp, id) {
  emp.id_comentariu = id;
  console.log(emp);
  const comentariu = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, comentariu);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return comentariu;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from comentarii
    where id_comentariu = :id_comentariu;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_comentariu: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getComentariuFromRec(req) {
  const comentariu = {
    id_utilizator: req.body.id_utilizator,
    id_produs: req.body.id_produs,
    continut_comentariu: req.body.continut_comentariu,
  };
  return comentariu;
}

module.exports.getObjFromRec = getComentariuFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
