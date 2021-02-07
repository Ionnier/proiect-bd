const group = require('../db_apis/group.js');
async function get(req, res, next) {
  try {
    const context = {};
    console.log(req.body);
    context.id = parseInt(req.params.id, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await group.find(context);
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
    console.log(req.body);
    context.id = parseInt(req.params.id, 10);
    context.avg = parseInt(req.query.avg, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    const rows = await group.find2(context);
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
