const preturi = require('../db_apis/preturi.js');
async function get(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await preturi.find(context);
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
    let pret = preturi.getObjFromRec(req);

    pret = await preturi.create(pret);

    res.status(201).json(pret);
  } catch (err) {
    next(err);
  }
}
async function put(req, res, next) {
  try {
    let pret = preturi.getObjFromRec2(req);
    pret = await preturi.update(pret);
    if (pret !== null) {
      res.status(200).json(pret);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    let pret = preturi.getObjFromRec3(req);
    const success = await preturi.delete(pret);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
module.exports.model = preturi;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
