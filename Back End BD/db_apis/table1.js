const database = require('../services/database.js');
const oracledb = require('oracledb');
const e = require('express');
const baseQuery = `select id_table1 "id",
    continut "continut"
  from table1
  where 1 = 1`;
async function find(context) {
  let query = baseQuery;
  const binds = {};
  /*
  if (context.id) {
    binds.employee_id = context.id;
    query += '\nand employee_id = :employee_id';
  }
  */
  console.log(query);
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
module.exports.find = find;

const createSql = `insert into table1 (
    id_table1,
    continut
  ) values (
    :table1_id,
    :continut
  ) returning id_table1
  into :table1_id`;

async function create(emp) {
  const employee = Object.assign({}, emp);

  employee.table1_id = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };

  const result = await database.simpleExecute(createSql, employee);

  employee.employee_id = result.outBinds.table1_id[0];

  return employee;
}

const updateSql = `update table1
  set continut = :continut
  where id_table1 = :id_table1`;

async function update(emp, id) {
  emp.id_table1 = id;
  const employee = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, employee);

  if (result.rowsAffected && result.rowsAffected === 1) {
    return employee;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from table1
    where id_table1 = :id_table1;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_table1: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  console.log(binds);
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getObjFromRec(req) {
  const table1 = {
    continut: req.body.continut,
  };
  return table1;
}
module.exports.getObjFromRec = getObjFromRec;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
