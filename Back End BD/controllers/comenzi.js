const comenzi = require('../db_apis/comenzi.js');
const produse_comenzi = require('../db_apis/produse_comenzi');
async function get(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await comenzi.find(context);
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
    let produs = comenzi.getObjFromRec(req);
    produs = await comenzi.create(produs);
    var id = produs.id_comanda;
    produs.platforme = comenzi.getObjFromRec3(req);
    var array = produs.platforme.produse;
    for (var i = 0; i < array.length; i++) {
      await produse_comenzi.addProdComenzi(id, array[i][0], array[i][1]);
    }
    res.status(201).json(produs);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let produs = comenzi.getObjFromRec(req);
    produs.id_comanda = parseInt(req.params.id, 10);
    produs = await comenzi.update(produs);
    await produse_comenzi.dellComenzi(produs.id_comanda);
    const platforme = comenzi.getObjFromRec3(req);
    var array = platforme.produse;
    if (array != undefined) {
      for (var i = 0; i < array.length; i++) {
        await produse_comenzi.addProdComenzi(
          produs.id_comanda,
          array[i][0],
          array[i][1]
        );
      }
    }
    if (produs !== null) {
      res.status(200).json(produs);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    const id = parseInt(req.params.id, 10);
    const success = await comenzi.delete(id);
    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

async function getProducts(req, res, next) {
  try {
    const id = parseInt(req.params.id, 10);
    const success = await produse_comenzi.getProducts(id);
    if (success) {
      res.status(200).send(success);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = comenzi;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
module.exports.products = getProducts;
