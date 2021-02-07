const judete = require('../db_apis/judete.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await judete.find(context);
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
    let judet = judete.getObjFromRec(req);

    judet = await judete.create(judet);

    res.status(201).json(judet);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let judet = judete.getObjFromRec(req);

    judet.id_newjudet = req.params.id;
    judet = await judete.update(judet);

    if (judet !== null) {
      res.status(200).json(judet);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    const id = req.params.id;

    const success = await judete.delete(id);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = judete;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
