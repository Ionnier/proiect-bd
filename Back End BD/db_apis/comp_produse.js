const database = require('../services/database.js');
const oracledb = require('oracledb');

const baseQuery = `select id_produs "id_produs",
    id_platforma "id_platforma"
  from compatibilitate_produs
  where 1 = 1`;
const sortableColumns = ['id_produs', 'id_platforma'];

async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_produs = context.id;
    query += '\nand id_produs = :id_produs';
  }

  if (context.id_platforma) {
    binds.id_platforma = context.id_platforma;

    query += '\nand id_platforma = :id_platforma';
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

const createSql = `insert into Compatibilitate_Produs (
    id_produs,
    id_platforma
  ) values (
    :id_produs,
    :id_platforma
  )`;

async function create(emp) {
  const produs = Object.assign({}, emp);
  const result = await database.simpleExecute(createSql, produs);
  return produs;
}

const updateSql = `update Compatibilitate_Produs
  set id_produs=:id_produs2,
  id_platforma=:id_platforma2
  where id_produs = :id_produs and id_platforma=:id_platforma`;

async function update(emp) {
  const produs = Object.assign({}, emp);
  console.log(produs);
  const result = await database.simpleExecute(updateSql, produs);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return produs;
  } else {
    return null;
  }
}

const deleteSql = `begin

    delete from Compatibilitate_Produs
    where id_produs = :id_produs and id_platforma=:id_platforma;

    :rowcount := sql%rowcount;

  end;`;

async function del(emp) {
  const binds = {
    id_produs: emp.id_produs,
    id_platforma: emp.id_platforma,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);
  return result.outBinds.rowcount === 1;
}

const createoneSql = `begin
insert into compatibilitate_produs (
    id_produs,
    id_platforma
  ) values (
    :id_produs,
    :id_platforma
  );
:rowcount := sql%rowcount;
end;`;

async function whileCreate(id_produs, id_platforma) {
  const binds = {
    id_produs: id_produs,
    id_platforma: id_platforma,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(createoneSql, binds);

  return result.outBinds.rowcount;
}

const deleteOneSql = `begin
delete from compatibilitate_produs where id_produs=:id_produs;
:rowcount := sql%rowcount;
end;`;

async function deleteWhileUpdate(id_produs) {
  const binds = {
    id_produs: id_produs,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteOneSql, binds);
  return result.outBinds.rowcount;
}

const compatibilitateSql =
  'select id_platforma from compatibilitate_produs where id_produs=:id_produs';
async function getCompatibility(id_produs) {
  const binds = {
    id_produs: id_produs,
  };
  const result = await database.simpleExecute(compatibilitateSql, binds);
  var vector = [];
  Object.keys(result.rows).forEach(function (key) {
    vector.push(result.rows[key]['ID_PLATFORMA']);
  });
  return vector;
}

function getcompFromRec(req) {
  const comp = {
    id_platforma: req.body.id_producator,
    id_produs: req.body.id_produs,
  };
  return comp;
}

function getcompFromRec2(req) {
  const comp = {
    id_platforma: req.body.id_platforma,
    id_produs: req.body.id_produs,
  };
  return comp;
}

function getcompFromRec3(req) {
  const comp = {
    id_platforma: req.body.id_platforma,
    id_produs: req.body.id_produs,
    id_platforma2: req.body.id_platforma2,
    id_produs2: req.body.id_produs2,
  };
  return comp;
}
module.exports.compatibility = getCompatibility;
module.exports.getObjFromRec = getcompFromRec;
module.exports.getObjFromRec2 = getcompFromRec2;
module.exports.getObjFromRec3 = getcompFromRec3;
module.exports.createCompProd = whileCreate;
module.exports.deleteCompProd = deleteWhileUpdate;
module.exports.find = find;
module.exports.create = create;
module.exports.update = update;
module.exports.delete = del;
