const front = require('../db_apis/front.js');
const comp_produse = require('../db_apis/comp_produse.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await front.find(context);
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

async function get2(req, res, next) {
  try {
    const context = {};
    context.id = req.params.id;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await front.find2(context);
    if (req.params.id) {
      if (rows.length) {
        res.status(200).json(rows);
      } else {
        res.status(404).end();
      }
    }
  } catch (err) {
    next(err);
  }
}

module.exports.get = get;
module.exports.get2 = get2;
