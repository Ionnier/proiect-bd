const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');
const baseQuery = `SELECT id_produs "id_produs",
    TO_CHAR(data_inceput,'YYYY-MM-DD HH:MM') "data_inceput",
    pret "pret"
    from IstoricPreturi
    where 1 = 1`;
const sortableColumns = ['id', 'pret', 'data_inceput'];
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
const createSql = `insert into IstoricPreturi (
    id_produs,
    data_inceput,
    pret
  ) values (
    :id_produs,
    TO_DATE(:data_inceput,'YYYY-MM-DD'),
    :pret
  )`;

async function create(emp) {
  const pret = Object.assign({}, emp);
  console.log(pret);
  const result = await database.simpleExecute(createSql, pret);
  return pret;
}

const updateSql = `update IstoricPreturi
  set id_produs=:id_produs2,
  data_inceput=TO_DATE(:data_inceput2,'YYYY-MM-DD'),
  pret=:pret
  where id_produs=:id_produs  and  TO_CHAR(data_inceput,'YYYY-MM-DD HH:MM')=:data_inceput`;

async function update(emp) {
  const pret = Object.assign({}, emp);
  console.log(pret);
  const result = await database.simpleExecute(updateSql, pret);
  console.log(result);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return pret;
  } else {
    return null;
  }
}

const deleteSql = `begin

    delete from IstoricPreturi
    where id_produs=:id_produs  and  TO_CHAR(data_inceput,'YYYY-MM-DD')=:data_inceput;
    :rowcount := sql%rowcount;

  end;`;

async function del(emp) {
  emp.rowcount = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };
  const pret = Object.assign({}, emp);
  console.log(pret);
  const result = await database.simpleExecute(deleteSql, pret);

  return result.outBinds.rowcount === 1;
}

function getpreturiFromRec(req) {
  const pret = {
    id_produs: req.body.id_produs,
    data_inceput: req.body.data_inceput,
    pret: req.body.pret,
  };
  return pret;
}

function getpreturiFromRec2(req) {
  const pret = {
    id_produs: req.body.id_produs,
    data_inceput: req.body.data_inceput,
    id_produs2: req.body.id_produs,
    data_inceput2: req.body.data_inceput2,
    pret: req.body.pret,
  };
  return pret;
}

function getpreturiFromRec3(req) {
  const pret = {
    id_produs: req.body.id_produs,
    data_inceput: req.body.data_inceput,
  };
  return pret;
}

module.exports.getObjFromRec = getpreturiFromRec;
module.exports.getObjFromRec2 = getpreturiFromRec2;
module.exports.getObjFromRec3 = getpreturiFromRec3;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
