const comentarii = require('../db_apis/comentarii.js');

async function get(req, res, next) {
  try {
    const context = {};
    console.log(req.body);
    context.id = parseInt(req.params.id, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    context.id_utilizator = req.query.id_utilizator;
    context.id_produs = req.query.id_produs;
    const rows = await comentarii.find(context);
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
    let comentariu = comentarii.getObjFromRec(req);

    comentariu = await comentarii.create(comentariu);

    res.status(201).json(comentariu);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let comentariu = comentarii.getObjFromRec(req);

    comentariu = await comentarii.update(
      comentariu,
      parseInt(req.params.id, 10)
    );

    if (comentariu !== null) {
      res.status(200).json(comentariu);
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

    const success = await comentarii.delete(id);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.model = comentarii;
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
