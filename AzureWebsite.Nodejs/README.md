#NodeJS Static Web Application
A basic framework web site to demonstrate the use of Node.Js, gulp, jade/pug, sass and typescript to build a static easy maintainable source code

## Tools to use
- Node.Js - Ramverk f�r lokal server och import av paket m.m.
- Gulp - S�tta samman och skapa slutresultat
- Pug - Ett b�ttre s�tt att skapa html
- Sass - Ett b�ttre s�tt att skriva css
- Typescript - Ett b�ttre s�tt att skriva javascript
- Express - Basic local http server

## Preparation
- Install node https://nodejs.org/en/download/
- Update node: npm install npm@latest -g
- Installera Gulp: npm install -g gulp
- Installera Gulp: npm install -g live-server
- Initiera nodejs: npm init

## Install dependencies
- npm install express --save
- npm install open --save

## Installed development dependencies 
- npm install gulp --save-dev
- npm install gulp-clean --save-dev
- npm install gulp-pug --save-dev
- npm install node-sass --save-dev
- npm install gulp-sass --save-dev
- npm install typescript --save-dev
- npm install gulp-typescript --save-dev

##Kod f�r paketet
- Gulp script: gulpfile.js
- Http Server: app.js
- Nodejs konfiguration: package.json
- Typescript konfiguration: tsconfig.json

##Kommandon:
- Installation av paket: npm install
- Ren installation av paket: npm clean-install
- Run gulpfile: gulp
- Run script to start: npm run start
- Starta LiveServer
  - cd public
  - live-server --port=9029

##Fungerar fullt ut i Visual Studio