const platforme = require('../db_apis/platforme.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.nume = req.params.nume;
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    const rows = await platforme.find(context);

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
module.exports.model = platforme;
module.exports.get = get;
