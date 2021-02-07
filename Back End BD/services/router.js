const express = require('express');
const router = new express.Router();
const employees = require('../controllers/employees.js');
const table1 = require('../controllers/table1.js');
const utilizatori = require('../controllers/utilizatori.js');
const factory = require('./../controllers/factory.js');
const authController = require('./../controllers/authController.js');
const producatori = require('./../controllers/producatori.js');
const platforme = require('./../controllers/platforme.js');
const judete = require('./../controllers/judete.js');
const preturi = require('./../controllers/preturi.js');
const produse = require('./../controllers/produse.js');
const comenzi = require('./../controllers/comenzi.js');
const comentarii = require('./../controllers/comentarii.js');
const produse_comandate = require('./../controllers/produse_comenzi.js');
const comp_produse = require('./../controllers/comp_produse.js');
const front = require('./../controllers/frontController.js');
const join = require('../controllers/join.js');
const group = require('../controllers/group.js');
const ondeletecascade = require('../controllers/ondeletecascade.js');
const produse_complete = require('../controllers/produse_complete.js');
const comenzi_complete = require('../controllers/comenzi_complete.js');
const sumar_comanda = require('../controllers/sumar_comanda.js');
router
  .route('/employees/:id?')
  .get(employees.get)
  .post(employees.post)
  .put(employees.put)
  .delete(employees.delete);
router

  .route('/table1/:id?')
  .get(table1.get)
  .post((req, res, next) => {
    factory.post(req, res, next, table1);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, table1);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, table1);
  });

router
  .route('/utilizatori/:id?')
  .get(utilizatori.get)
  .post((req, res, next) => {
    factory.post(req, res, next, utilizatori);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, utilizatori);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, utilizatori);
  });

router
  .route('/producatori/:id?')
  .get(producatori.get)
  .post((req, res, next) => {
    factory.post(req, res, next, producatori);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, producatori);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, producatori);
  });

router
  .route('/platforme/:id?')
  .get(platforme.get)
  .post((req, res, next) => {
    factory.post(req, res, next, platforme);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, platforme);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, platforme);
  });

router
  .route('/judete/:id?')
  .get(judete.get)
  .post(judete.post)
  .put(judete.put)
  .delete(judete.delete);

router
  .route('/preturi/:id?')
  .get(preturi.get)
  .post(preturi.post)
  .put(preturi.put)
  .delete(preturi.delete);

router
  .route('/produse/:id?')
  .get(produse.get)
  .post(produse.post)
  .put(produse.put)
  .delete(produse.delete);

router.route('/compatibilitateproduse/:id?').get(produse.getCompp);

router.route('/login/').post(authController.login);

router
  .route('/comenzi/:id?')
  .get(comenzi.get)
  .post(comenzi.post)
  .put(comenzi.put)
  .delete(comenzi.delete);

router
  .route('/comentarii/:id?')
  .get(comentarii.get)
  .post(comentarii.post)
  .put(comentarii.put)
  .delete(comentarii.delete);

router.route('/produsecomenzi/:id?').get(comenzi.products);

router
  .route('/produsecomandate/:id?')
  .get(produse_comandate.get)
  .post(produse_comandate.post)
  .put(produse_comandate.put)
  .delete(produse_comandate.delete);

router
  .route('/compproduse/:id?')
  .get(comp_produse.get)
  .post(comp_produse.post)
  .put(comp_produse.put)
  .delete(comp_produse.delete);

router.route('/front/:id?').get(front.get);
router.route('/frontcomm/:id?').get(front.get2);
router.route('/join/:id?').get(join.get);
router.route('/group/:id?').get(group.get);
router.route('/group2/:id?').get(group.get2);
router.route('/ondeletecascadecomentarii/:id?').get(ondeletecascade.get);
router.route('/ondeletecascadecompprod/:id?').get(ondeletecascade.get2);
router.route('/ondeletecascadeip/:id?').get(ondeletecascade.get4);
router.route('/ondeletecascadecomenzi/:id?').get(ondeletecascade.get3);

router
  .route('/produse_complete/:id?')
  .get(produse_complete.get)
  .post((req, res, next) => {
    factory.post(req, res, next, produse_complete);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, produse_complete);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, produse_complete);
  });

router
  .route('/comenzi_complete/:id?')
  .get(comenzi_complete.get)
  .post((req, res, next) => {
    factory.post(req, res, next, comenzi_complete);
  })
  .put((req, res, next) => {
    factory.put(req, res, next, comenzi_complete);
  })
  .delete((req, res, next) => {
    factory.del(req, res, next, comenzi_complete);
  });

router.route('/sumar_comanda/:id?').get(sumar_comanda.get);
module.exports = router;
