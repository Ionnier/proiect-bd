const utilizatori = require('../db_apis/utilizatori.js');

async function get(req, res, next) {
  try {
    const context = {};
    console.log(req.body);
    context.id = parseInt(req.params.id, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    context.oras = req.query.oras;
    context.id_judet = req.query.id_judet;
    const rows = await utilizatori.find(context);

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
    let utilizator = getObjFromRec(req);

    utilizator = await utilizatori.create(utilizator);

    res.status(201).json(utilizator);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let utilizator = getObjFromRec(req);

    utilizator.id_utilizator = parseInt(req.params.id, 10);

    utilizator = await utilizatori.update(utilizator);

    if (utilizator !== null) {
      res.status(200).json(utilizator);
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

    const success = await utilizatori.delete(id);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = utilizatori;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
