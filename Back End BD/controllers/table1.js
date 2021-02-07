const table1 = require('../db_apis/table1.js');

async function get(req, res, next) {
  try {
    const context = {};
    context.id = parseInt(req.params.id, 10);
    const rows = await table1.find(context);
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
    let employee = table1.getObjFromRec(req);

    employee = await table1.create(employee);

    res.status(201).json(employee);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next) {
  try {
    let employee = table1.getObjFromRec(req);
    employee.id_table1 = parseInt(req.params.id, 10);
    console.log(employee);
    employee = await table1.update(employee);

    if (employee !== null) {
      res.status(200).json(employee);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
async function del(req, res, next) {
  try {
    console.log(req.params);
    const id = parseInt(req.params.id, 10);

    const success = await table1.delete(id);

    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}
module.exports.delete = del;
module.exports.put = put;
module.exports.post = post;
module.exports.get = get;
module.exports.model = table1;
