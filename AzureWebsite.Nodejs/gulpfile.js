var gulp = require('gulp');

var clean = require('gulp-clean');
gulp.task('clean', function () {
    return gulp.src('./public', { read: false, allowEmpty: true })
        .pipe(clean());
});

var pug = require('gulp-pug');
gulp.task('pug', function () {
    return gulp.src('./source/pug/pages/*.pug')
        .pipe(pug({ pretty: true }))
        .pipe(gulp.dest('./public'));
});

gulp.task('scripts', function () {
    return gulp.src('./source/scripts/*.js')
        .pipe(gulp.dest('./public/scripts'));
});

var sass = require('gulp-sass')(require('sass'));
gulp.task('sass', function () {
    return gulp.src('./source/sass/**/*.scss')
        .pipe(sass().on('error', sass.logError))
        .pipe(gulp.dest('./public/styles'));
});


gulp.task('favicon', function () {
    return gulp.src('./source/images/favicon.ico')
        .pipe(gulp.dest('./public'));
});

gulp.task('configurations', function () {
    return gulp.src('./source/configurations/**/*.js')
        .pipe(gulp.dest('./public/configurations'));
});

gulp.task('watch', function () {
    gulp.watch('source/pug/**/*.pug', gulp.series('pug'));
    gulp.watch('source/sass/**/*.scss', gulp.series('sass'));
    gulp.watch('source/scripts/**/*.js', gulp.series('scripts'));
});

gulp.task('default', gulp.series('clean', 'pug', 'sass', 'scripts', 'configurations','favicon', function (done) {
    done();
}));
