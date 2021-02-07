const database = require('../services/database.js');
const oracledb = require('oracledb');
const loginSql = `select id_utilizator
  from Utilizatori
  where email = :email and parola=:parola`;
async function login(user) {
  const utilizator = Object.assign({}, user);
  const result = await database.simpleExecute(loginSql, utilizator);
  if (result.rows[0]) {
    return result.rows[0].ID_UTILIZATOR;
  } else {
    return null;
  }
}
module.exports.log = login;
