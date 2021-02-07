const database = require('../services/database.js');
const oracledb = require('oracledb');
const baseQuery = `select id_comanda "id_comanda",
    id_utilizator "id_utilizator",
    id_judet "id_judet",
    oras "oras",
    strada "strada",
    numar "numar",
    cod_postal "cod_postal",
    scara "scara",
    bloc "bloc",
    apartament "apartament",
    numar_telefon "numar_telefon",
    TO_CHAR(data_comanda,'YYYY-MM-DD HH:MM') "data_comanda"
  from comenzi
  where 1 = 1`;
const sortableColumns = [
  'id_comanda',
  'id_utilizator',
  'id_judet',
  'oras',
  'strada',
  'bloc',
  'apartament',
  'numar_telefon',
  'cod_postal',
];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_comanda = context.id;

    query += '\nand id_comanda = :id_comanda';
  }

  if (context.id_utilizator) {
    binds.id_utilizator = context.id_utilizator;

    query += '\nand id_utilizator = :id_utilizator';
  }

  if (context.id_judet) {
    binds.id_judet = context.id_judet;
    query += '\nand lower(id_judet) = lower(:id_judet)';
  }
  if (context.id_oras) {
    binds.id_oras = context.id_oras;
    query += '\nand lower(id_oras) = lower(:id_oras)';
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
  const limit = context.limit > 0 ? context.limit : 300;
  binds.row_limit = limit;
  query += '\nfetch next :row_limit rows only';

  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
const createSql = `insert into comenzi (
    id_comanda,
    id_utilizator,
    id_judet,
    oras,
    strada,
    numar,
    cod_postal,
    scara,
    bloc,
    apartament,
    numar_telefon,
    data_comanda
  ) values (
    id_comanda_seq.nextval,
    :id_utilizator,
    :id_judet,
    :oras,
    :strada,
    :numar,
    :cod_postal,
    :scara,
    :bloc,
    :apartament,
    :numar_telefon,
    sysdate
  ) returning id_comanda
  into :id_comanda`;

async function create(emp) {
  const comanda = Object.assign({}, emp);
  comanda.id_comanda = {
    dir: oracledb.BIND_OUT,
    type: oracledb.NUMBER,
  };
  const result = await database.simpleExecute(createSql, comanda);
  comanda.id_comanda = result.outBinds.id_comanda[0];
  return comanda;
}

const updateSql = `update comenzi
  set id_utilizator=:id_utilizator,
  id_judet=:id_judet,
  oras=:oras,
  strada=:strada,
  numar=:numar,
  cod_postal=:cod_postal,
  scara=:scara,
  bloc=:bloc,
  apartament=:apartament,
  numar_telefon=:numar_telefon
  where id_comanda = :id_comanda`;

async function update(emp) {
  const produs = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, produs);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return produs;
  } else {
    return null;
  }
}
const deleteSql = `begin

    delete from Comenzi
    where id_comanda = :id_comanda;

    :rowcount := sql%rowcount;

  end;`;

async function del(id) {
  const binds = {
    id_comanda: id,
    rowcount: {
      dir: oracledb.BIND_OUT,
      type: oracledb.NUMBER,
    },
  };
  const result = await database.simpleExecute(deleteSql, binds);

  return result.outBinds.rowcount === 1;
}

function getcomandaFromRec(req) {
  const produs = {
    id_utilizator: req.body.id_utilizator,
    id_judet: req.body.id_judet,
    oras: req.body.oras,
    strada: req.body.strada,
    numar: req.body.numar,
    cod_postal: req.body.cod_postal,
    bloc: req.body.bloc,
    apartament: req.body.apartament,
    scara: req.body.scara,
    numar_telefon: req.body.numar_telefon,
  };
  return produs;
}

function getcomandaFromRec3(req) {
  const produs = {
    produse: req.body.produse,
  };
  return produs;
}

module.exports.getObjFromRec = getcomandaFromRec;
module.exports.getObjFromRec3 = getcomandaFromRec3;
module.exports.delete = del;
module.exports.update = update;
module.exports.create = create;
module.exports.find = find;
