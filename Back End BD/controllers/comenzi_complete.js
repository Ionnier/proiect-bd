const comenzi_complete = require('../db_apis/comenzi_complete.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.id = parseInt(req.params.id, 10);
    context.sort = req.query.sort;
    context.website = req.query.website;
    const rows = await comenzi_complete.find(context);
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
module.exports.model = comenzi_complete;
module.exports.get = get;
