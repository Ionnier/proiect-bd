const database = require('../services/database.js');
const oracledb = require('oracledb');
const baseQuery = `select id_utilizator "id",
    prenume_utilizator "prenume",
    nume_utilizator "nume",
    email "email",
    parola "parola",
    TO_CHAR(data_nastere,'YYYY-MM-DD') "data_nastere"
  from utilizatori
  where 1 = 1`;
const sortableColumns = [
  'id',
  'prenume_utilizator',
  'nume_utilizator',
  'email',
];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_utilizator = context.id;

    query += '\nand id_utilizator = :id_utilizator';
  }

  if (context.oras) {
    binds.oras = context.oras;

    query += '\nand oras = :oras';
  }

  if (context.judet) {
    binds.id_judet = context.id_judet;

    query += '\nand id_judet = :id_judet';
  }
  if (context.skip) {
    binds.row_offset = context.skip;

    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_utilizator asc';
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
    const limit = context.limit > 0 ? context.limit : 3000;
    binds.row_limit = limit;
    query += '\nfetch next :row_limit rows only';
  }
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
const createSql = `insert into utilizatori (
    id_utilizator,
    prenume_utilizator,
    nume_utilizator,
    email,
    parola,
    data_nastere
  ) values (
    id_utilizator_seq.nextval,
    :prenume_utilizator,
    :nume_utilizator,
    :email,
    :parola,
    TO_DATE(:data_nastere,'YYYY-MM-DD')
  ) returning id_utilizator
  into :id_utilizator`;

async function create(emp) {
  const utilizator = Object.assign({}, emp);

  utilizator.id_utilizator = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };
  const result = await database.simpleExecute(createSql, utilizator);

  utilizator.id_utilizator = result.outBinds.id_utilizator[0];

  return utilizator;
}

const updateSql = `update utilizatori
  set prenume_utilizator = :prenume_utilizator,
  nume_utilizator = :nume_utilizator,
  email=:email,
  parola=:parola,
  data_nastere=TO_DATE(:data_nastere,'YYYY-MM-DD')
  where id_utilizator = :id_utilizator`;

async function update(emp, id) {
  emp.id_utilizator = id;
  console.log(emp);
  const utilizator = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, utilizator);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return utilizator;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from utilizatori
    where id_utilizator = :id_utilizator;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_utilizator: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getUtilizatorFromRec(req) {
  const utilizator = {
    prenume_utilizator: req.body.prenume_utilizator,
    nume_utilizator: req.body.nume_utilizator,
    email: req.body.email,
    parola: req.body.parola,
    data_nastere: req.body.data_nastere,
  };
  return utilizator;
}

module.exports.getObjFromRec = getUtilizatorFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
