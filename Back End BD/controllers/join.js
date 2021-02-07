const join = require('../db_apis/join.js');
async function get(req, res, next) {
  try {
    const context = {};
    console.log(req.body);
    context.id = parseInt(req.params.id, 10);
    context.skip = parseInt(req.query.skip, 10);
    context.limit = parseInt(req.query.limit, 10);
    context.sort = req.query.sort;
    context.nume_utilizator = req.query.nume_utilizator;
    context.prenume_utilizator = req.query.prenume_utilizator;
    context.email = req.query.email;
    context.data_nastere = req.query.data_nastere;
    context.id_comanda = req.query.id_comanda;
    context.id_judet = req.query.id_judet;
    context.nume_judet = req.query.nume_judet;
    context.oras = req.query.oras;
    context.strada = req.query.strada;
    context.numar = req.query.numar;
    context.cod_postal = req.query.cod_postal;
    context.scara = req.query.scara;
    context.bloc = req.query.bloc;
    context.apartament = req.query.apartament;
    context.numar_telefon = req.query.numar_telefon;
    context.id_produs = req.query.id_produs;
    context.nume_produs = req.query.nume_produs;
    context.cantitate_comanda = req.query.cantitate_comanda;
    context.pret = req.query.pret;
    context.stoc = req.query.stoc;
    context.categorie = req.query.categorie;
    context.id_producator = req.query.id_producator;
    context.nume_producator = req.query.nume_producator;
    context.website = req.query.website;
    const rows = await join.find(context);
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
