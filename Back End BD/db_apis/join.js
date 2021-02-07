module.exports.find = find;
const database = require('../services/database.js');
const oracledb = require('oracledb');
const baseQuery = `SELECT util.id_utilizator "id_utilizator", util.nume_utilizator "nume_utilizator", util.prenume_utilizator "prenume_utilizator", util.email "email", util.parola "parola", TO_CHAR(util.data_nastere,'YYYY-MM-DD') as "data_nastere", com.id_comanda "id_comanda",
com.id_judet "id_judet", jd.nume_judet "nume_judet", com.oras "oras", com.strada "strada", com.numar "numar", com.cod_postal "cod_postal", com.scara "scara", com.bloc "bloc", com.apartament "apartament",
com.numar_telefon "numar_telefon", pc.id_produs "id_produs", p.nume_produs as "nume_produs", pc.cantitate "cantitate_comandata", p.pret as "pret", p.cantitate "stoc", p.descriere "descriere", p.categorie "categorie", prod.id_producator "id_producator", prod.nume_producator "nume_producator",
prod.website "website" 
from Utilizatori util
join Comenzi com on util.id_utilizator = com.id_utilizator
join Judete jd on com.id_judet = jd.id_judet
join Produse_Comandate pc on com.id_comanda=pc.id_comanda
join Produse p on pc.id_produs = p.id_produs
join Producatori prod on prod.id_producator = p.id_producator
where 1=1`;
const sortableColumns = [
  'id_utilizator',
  'nume_utilizator',
  'prenume_utilizator',
  'email',
  'oras',
  'data_nastere',
  'id_judet',
  'nume_judet',
  'strada',
  'numar',
  'cod_postal',
  'scara',
  'bloc',
  'apartament',
  'numar_telefon',
  'id_produs',
  'nume_produs',
  'cantitate_comandata',
  'pret',
  'stoc',
  'categorie',
  'id_producator',
  'nume_producator',
  'website',
];
async function find(context) {
  let query = baseQuery;
  const binds = {};

  if (context.id) {
    binds.id_utilizator = context.id;
    query += '\nand util.id_utilizator = :id_utilizator';
  }

  if (context.nume_utilizator) {
    binds.nume_utilizator = context.nume_utilizator;
    query += '\nand util.nume_utilizator= :nume_utilizator';
  }

  if (context.prenume_utilizator) {
    binds.prenume_utilizator = context.prenume_utilizator;
    query += '\nand util.prenume_utilizator= :prenume_utilizator';
  }

  if (context.email) {
    binds.email = context.email;
    query += '\nand util.email= :email';
  }

  if (context.data_nastere) {
    binds.data_nastere = context.data_nastere;
    query += "\nand util.data_nastere= TO_DATE(:data_nastere,'YYYY-MM-DD')";
  }

  if (context.id_comanda) {
    binds.id_comanda = context.id_comanda;
    query += '\nand com.id_comanda= :id_comanda';
  }

  if (context.id_judet) {
    binds.id_judet = context.id_judet;
    query += '\nand com.id_judet= :id_judet';
  }

  if (context.nume_judet) {
    binds.nume_judet = context.nume_judet;
    query += '\nand jd.nume_judet= :nume_judet';
  }

  if (context.oras) {
    binds.oras = context.oras;
    query += '\nand com.oras= :oras';
  }

  if (context.strada) {
    binds.strada = context.strada;
    query += '\nand com.strada= :strada';
  }

  if (context.numar) {
    binds.numar = context.numar;
    query += '\nand com.numar= :numar';
  }

  if (context.cod_postal) {
    binds.cod_postal = context.cod_postal;
    query += '\nand com.cod_postal= :cod_postal';
  }

  if (context.scara) {
    binds.scara = context.scara;
    query += '\nand com.scara= :scara';
  }

  if (context.bloc) {
    binds.bloc = context.bloc;
    query += '\nand com.bloc= :bloc';
  }

  if (context.apartament) {
    binds.apartament = context.apartament;
    query += '\nand com.apartament= :apartament';
  }

  if (context.numar_telefon) {
    binds.numar_telefon = context.numar_telefon;
    query += '\nand com.numar_telefon= :numar_telefon';
  }

  if (context.id_produs) {
    binds.id_produs = context.id_produs;
    query += '\nand p.id_produs= :id_produs';
  }

  if (context.nume_produs) {
    binds.nume_produs = context.nume_produs;
    query += '\nand p.nume_produs= :nume_produs';
  }

  if (context.cantitate_comanda) {
    binds.cantitate_comanda = context.cantitate_comanda;
    query += '\nand pc.cantitate= :cantitate_comanda';
  }

  if (context.pret) {
    binds.pret = context.pret;
    query += '\nand prod.pret= :pret';
  }

  if (context.categorie) {
    binds.categorie = context.categorie;
    query += '\nand p.categorie= :categorie';
  }

  if (context.id_producator) {
    binds.id_producator = context.id_producator;
    query += '\nand p.id_producator= :id_producator';
  }

  if (context.nume_producator) {
    binds.nume_producator = context.nume_producator;
    query += '\nand p.nume_producator= :nume_producator';
  }

  if (context.website) {
    binds.website = context.website;
    query += '\nand p.website= :website';
  }

  if (context.skip) {
    binds.row_offset = context.skip;

    query += '\noffset :row_offset rows';
  }
  if (context.sort === undefined) {
    query += '\norder by util.id_utilizator asc';
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
    const limit = context.limit > 0 ? context.limit : 30000000;
    binds.row_limit = limit;
    query += '\nfetch next :row_limit rows only';
  }
  const result = await database.simpleExecute(query, binds);
  return result.rows;
}
module.exports.find = find;
