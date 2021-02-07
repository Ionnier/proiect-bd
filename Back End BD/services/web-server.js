const morgan = require('morgan');
const http = require('http');
const express = require('express');
var bodyParser = require('body-parser');
const webServerConfig = require('../config/web-server.js');
const router = require('./router.js');
var cors = require('cors');
const viewRouter = require('./viewRouter');

let httpServer;
function initialize() {
  return new Promise((resolve, reject) => {
    const app = express();
    app.use(cors());
    app.use(express.urlencoded({ extended: true }));
    app.use(bodyParser.json());
    app.use(morgan('combined'));

    httpServer = http.createServer(app);

    app.use('/api', router);
    app.use('/', viewRouter);
    app.use(
      express.json({
        reviver: reviveJson,
      })
    );
    app.use(function (err, req, res, next) {
      res.status(500).end(err.message);
    });
    httpServer
      .listen(webServerConfig.port)
      .on('listening', () => {
        console.log(
          `Web server listening on localhost:${webServerConfig.port}`
        );

        resolve();
      })
      .on('error', (err) => {
        reject(err);
      });
  });
}

module.exports.initialize = initialize;

// *** previous code above this line ***

function close() {
  return new Promise((resolve, reject) => {
    httpServer.close((err) => {
      if (err) {
        reject(err);
        return;
      }
      resolve();
    });
  });
}
const iso8601RegExp = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(\.\d{3})?Z$/;

function reviveJson(key, value) {
  if (typeof value === 'string' && iso8601RegExp.test(value)) {
    return new Date(value);
  } else {
    return value;
  }
}

module.exports.close = close;
