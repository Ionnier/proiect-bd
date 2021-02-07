const produse_comenzi = require('../db_apis/produse_comenzi.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id_comanda = req.params.id;
    context.id_produs = req.query.id_produs;
    context.cantitate = req.query.cantitate;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await produse_comenzi.find(context);
    if (req.params.id) {
      if (rows.length === 1) {
        res.status(200).json(rows);
      } else {
        res.status(404).end();
      }
    } else {
      res.status(200).json(rows);
    }
  } catch (err) {
    next(err);
  }
}

async function post(req, res, next) {
  try {
    let produs_comandat = produse_comenzi.getObjFromRec(req);

    produs_comandat = await produse_comenzi.create(produs_comandat);

    res.status(201).json(produs_comandat);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let produs_comandat = produse_comenzi.getObjFromRec2(req);
    produs_comandat = await produse_comenzi.update(produs_comandat);
    if (produs_comandat !== null) {
      res.status(200).json(produs_comandat);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    let produs_comandat = produse_comenzi.getObjFromRec3(req);
    const success = await produse_comenzi.delete(produs_comandat);
    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = produse_comenzi;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
