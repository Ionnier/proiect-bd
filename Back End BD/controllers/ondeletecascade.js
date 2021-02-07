const ondeletecascade = require('../db_apis/ondeletecascade.js');
async function get(req, res, next) {
  try {
    const context = {};
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    const rows = await ondeletecascade.find(context);

    if (req.params.id) {
      if (rows.length) {
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
async function get2(req, res, next) {
  try {
    const context = {};
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    const rows = await ondeletecascade.find2(context);
    if (req.params.id) {
      if (rows.length) {
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
async function get3(req, res, next) {
  try {
    const context = {};
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    const rows = await ondeletecascade.find3(context);
    if (req.params.id) {
      if (rows.length) {
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
async function get4(req, res, next) {
  try {
    const context = {};
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    const rows = await ondeletecascade.find4(context);
    if (req.params.id) {
      if (rows.length) {
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
module.exports.get = get;
module.exports.get2 = get2;
module.exports.get3 = get3;
module.exports.get4 = get4;
