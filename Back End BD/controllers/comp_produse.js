const comp_produse = require('../db_apis/comp_produse.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id = parseInt(req.params.id, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    context.id_platforma = req.query.id_platforma;
    const rows = await comp_produse.find(context);
    if (req.params.id) {
      res.status(200).json(rows);
    } else {
      res.status(200).json(rows);
    }
  } catch (err) {
    next(err);
  }
}

async function post(req, res, next) {
  try {
    let comp_produs = comp_produse.getObjFromRec2(req);
    console.log(comp_produs);
    comp_produs = await comp_produse.create(comp_produs);
    res.status(201).json(comp_produs);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let comp_produs = comp_produse.getObjFromRec3(req);
    comp_produs = await comp_produse.update(comp_produs);
    if (comp_produs !== null) {
      res.status(200).json(comp_produs);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    let comp_produs = comp_produse.getObjFromRec2(req);
    const success = await comp_produse.delete(comp_produs);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = comp_produse;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
