async function post(req, res, next, Model) {
  try {
    let obj = Model.model.getObjFromRec(req);

    obj = await Model.model.create(obj);

    res.status(201).json(obj);
  } catch (err) {
    next(err);
  }
}

async function put(req, res, next, Model) {
  try {
    let obj = Model.model.getObjFromRec(req);
    obj = await Model.model.update(obj, req.params.id);
    if (obj !== null) {
      res.status(200).json(obj);
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

async function del(req, res, next, Model) {
  try {
    const id = parseInt(req.params.id, 10);
    console.log(id, Model);
    const success = await Model.model.delete(id);
    if (success) {
      res.status(204).end();
    } else {
      res.status(404).end();
    }
  } catch (err) {
    next(err);
  }
}

module.exports.put = put;
module.exports.del = del;
module.exports.post = post;
