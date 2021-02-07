const login = require('../db_apis/login.js');

function GetUserFromRec(req) {
  const utilizator = {
    email: req.body.email,
    parola: req.body.parola,
  };
  return utilizator;
}

async function log(req, res, next) {
  try {
    let utilizator = GetUserFromRec(req);
    utilizator = await login.log(utilizator);
    if (utilizator) {
      res.status(200).end(String(utilizator));
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.login = log;
