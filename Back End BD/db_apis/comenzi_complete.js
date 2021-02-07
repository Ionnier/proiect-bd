const database = require('../services/database.js');
const oracledb = require('oracledb');
const { queueTimeout } = require('oracledb');

const baseQuery = `SELECT id_comanda "id_comanda",
    id_utilizator "id_utilizator",
    persoana_contact "persoana_contact",
    judet_id "id_judet",
    nume_judet "nume_judet",
    oras "oras",
    strada "strada",
    numar "numar",
    cod_postal "cod_postal",
    bloc "bloc",
    scara "scara",
    apartament "apartament",
    numar_telefon "numar_telefon",
    TO_CHAR(data_comanda,'YYYY-MM-DD HH:MM') "data_comanda"
    from comanda_completa
    where 1 = 1`;
const sortableColumns = ['id', 'nume', 'id_producator'];

async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_comanda = context.id;
    query += '\nand id_comanda = :id_comanda';
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

const createSql = `INSERT INTO comanda_completa(
    id_comanda,judet_id,id_utilizator,oras,strada,numar,cod_postal,bloc,scara,apartament,numar_telefon,data_comanda)
    values (
    id_comanda_seq.nextval,
    :id_judet,
    :id_utilizator,
    :oras,
    :strada,
    :numar,
    :cod_postal,
    :bloc,
    :scara,
    :apartament,
    :numar_telefon,
    sysdate)
  returning id_comanda
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

function getcomandaFromRec(req) {
  const comanda = {
    id_judet: req.body.id_judet,
    id_utilizator: req.body.id_utilizator,
    oras: req.body.oras,
    strada: req.body.strada,
    numar: req.body.numar,
    cod_postal: req.body.cod_postal,
    bloc: req.body.bloc,
    scara: req.body.scara,
    apartament: req.body.apartament,
    numar_telefon: req.body.numar_telefon,
  };
  return comanda;
}

const updateSql = `update comanda_completa
  set judet_id=:id_judet,
  id_utilizator=:id_utilizator,
  oras=:oras,
  strada=:strada,
  numar=:numar,
  cod_postal=:cod_postal,
  bloc=:bloc,
  scara=:scara,
  apartament=:apartament,
  numar_telefon=:numar_telefon
  where id_comanda = :id_comanda`;

async function update(emp, id) {
  emp.id_comanda = id;
  const comanda = Object.assign({}, emp);
  const result = await database.simpleExecute(updateSql, comanda);
  if (result.rowsAffected && result.rowsAffected === 1) {
    return comanda;
  } else {
    return null;
  }
}

const deleteSql = `begin
    delete from comanda_completa
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

module.exports.update = update;
module.exports.getObjFromRec = getcomandaFromRec;
module.exports.create = create;
module.exports.find = find;
module.exports.delete = del;
