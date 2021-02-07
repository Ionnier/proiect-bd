const database = require('../services/database.js');
const oracledb = require('oracledb');

const baseQuery = `SELECT id_comanda "id_comanda",
    id_produs "id_produs",
    cantitate "cantitate"
    from produse_comandate
    where 1 = 1`;
const sortableColumns = ['id_comanda', 'cantitate', 'id_produs'];
async function find(context) {
  let query = baseQuery;
  const binds = {};
  if (context.id_comanda) {
    binds.id_comanda = context.id_comanda;
    query += '\nand id_comanda = :id_comanda';
  }
  if (context.id_produs) {
    binds.id_produs = context.id_produs;
    query += '\nand id_produs = :id_produs';
  }
  if (context.cantitate) {
    binds.cantitate = context.cantitate;
    query += '\nand cantitate = :cantitate';
  }
  if (context.skip) {
    binds.row_offset = context.skip;
    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by id_comanda asc';
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
const createSql = `insert into produse_comandate (
    id_comanda,
    id_produs,
    cantitate
  ) values (
    :id_comanda,
    :id_produs,
    :cantitate
  )`;
async function create(emp) {
  const comanda = Object.assign({}, emp);
  const result = await database.simpleExecute(createSql, comanda);
  return comanda;
}

const updateSql = `update produse_comandate
  set id_comanda=:id_comanda2,
  id_produs=:id_produs2,
  cantitate=:cantitate
  where id_comanda = :id_comanda and id_produs=:id_produs`;

async function update(emp) {
  const comanda = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, comanda);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return comanda;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from Produse_Comandate
    where id_comanda = :id_comanda and id_produs=:id_produs;

  end;`;

async function del(emp) {
  const comanda = Object.assign({}, emp);
  const result = await database.simpleExecute(deleteSql, comanda);
  return result;
}

function getcomandaFromRec(req) {
  const comanda = {
    id_comanda: req.body.id_comanda,
    id_produs: req.body.id_produs,
    cantitate: req.body.cantitate,
  };
  return comanda;
}
function getcomandaFromRec2(req) {
  const comanda = {
    id_comanda: req.body.id_comanda,
    id_produs: req.body.id_produs,
    cantitate: req.body.cantitate,
    id_comanda2: req.body.id_comanda2,
    id_produs2: req.body.id_produs2,
  };
  return comanda;
}
function getcomandaFromRec3(req) {
  const comanda = {
    id_comanda: req.body.id_comanda,
    id_produs: req.body.id_produs,
  };
  return comanda;
}

module.exports.getObjFromRec = getcomandaFromRec;
module.exports.getObjFromRec2 = getcomandaFromRec2;
module.exports.getObjFromRec3 = getcomandaFromRec3;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;

const addSql =
  'INSERT INTO PRODUSE_COMANDATE VALUES(:id_comanda,:id_produs,:cantitate)';
async function addComanda(id_comanda, id_produs, cantitate) {
  const comanda = {
    id_comanda: id_comanda,
    id_produs: id_produs,
    cantitate: cantitate,
  };
  console.log(comanda);
  await database.simpleExecute(addSql, comanda);
  return;
}

const delSql = 'DELETE FROM Produse_Comandate where id_comanda = :id_comanda';
async function dellComanda(id_comanda) {
  const comanda = {
    id_comanda: id_comanda,
  };
  await database.simpleExecute(delSql, comanda);
  return;
}

const getProdCom =
  'SELECT id_comanda,id_produs,cantitate from Produse_Comandate where id_comanda=:id_comanda';

async function getProducts(id_comanda) {
  const binds = {
    id_comanda: id_comanda,
  };
  const result = await database.simpleExecute(getProdCom, binds);
  console.log(result);
  var vector = [];
  Object.keys(result.rows).forEach(function (key) {
    vector.push(result.rows[key]['ID_PRODUS']);
    vector.push(result.rows[key]['CANTITATE']);
  });
  console.log(vector);
  return vector;
}

module.exports.getProducts = getProducts;
module.exports.addProdComenzi = addComanda;
module.exports.dellComenzi = dellComanda;
