const produse = require('../db_apis/produse.js');
const comp_produse = require('../db_apis/comp_produse.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.id_producator = parseInt(req.query.id_producator, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await produse.find(context);
    if (req.params.id) {
      if (rows.length === 1) {
        res.status(200).json(rows[0]);
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
    let produs = produse.getObjFromRec(req);
    produs = await produse.create(produs);
    let id = produs.id_produs;
    produs = produse.getObjFromRec2(req);
    produs.id_produs = id;
    for (elem in produs.platforme) {
      await comp_produse.createCompProd(id, produs.platforme[elem]);
    }
    res.status(201).json(produs);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let produs = produse.getObjFromRec(req);
    produs.id_produs = parseInt(req.params.id, 10);
    produs = await produse.update(produs);
    if (produs !== null) {
      res.status(200).json(produs);
    } else {
      res.status(404).end();
    }
    produs = produse.getObjFromRec2(req);
    let id = parseInt(req.params.id, 10);
    await comp_produse.deleteCompProd(id);
    for (elem in produs.platforme) {
      await comp_produse.createCompProd(id, produs.platforme[elem]);
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    const id = parseInt(req.params.id, 10);
    const success = await produse.delete(id);
    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

async function getComp(req, res, next) {
  try {
    const id = parseInt(req.params.id, 10);
    const success = await comp_produse.compatibility(id);
    if (success) {
      res.status(200).send(success);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
module.exports.model = produse;
module.exports.getCompp = getComp;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
